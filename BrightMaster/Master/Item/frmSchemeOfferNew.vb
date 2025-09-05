Imports System.Data.OleDb
Public Class frmSchemeOfferNew
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim flagUpdate As Boolean
    Dim strSql As String
    Dim dt As New DataTable
    Dim dtcost As New DataTable
    Dim da1 As OleDbDataAdapter
    Dim CSchemeid As Integer
    Dim CItemid As Integer
    Dim CSubItemid As Integer
    Dim flper As Integer
    Dim vasl As Integer
    Dim CCostid As String
    Dim dtCostCentre As New DataTable
    Dim Syncdb As String = cnAdminDb
    Dim INSBASEOFFERSLAB As Boolean = IIf(GetAdmindbSoftValue("INSBASEOFFERSLAB", "N") = "Y", True, False)
    Dim DiscVatPer As Decimal = Val(GetAdmindbSoftValue("VBCFORSCHEME", "0").ToString)
    Dim SPECIFICFORMAT As String = GetAdmindbSoftValue("SPECIFICFORMAT", "")


    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        'CmbItem.Text = "ALL"
        strSql = " SELECT NAME FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE='C' ORDER BY NAME"
        objGPack.FillCombo(strSql, cmbScheme_MAN)
        objGPack.FillCombo(strSql, cmbFromScheme_MAN)
        objGPack.FillCombo(strSql, cmbToScheme_MAN)
        funcNew()
    End Sub
    Function funcExit() As Integer
        Me.Close()
    End Function

    Private Sub frmSchemeOfferNew_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            If cmbVbc.Focused Then Exit Sub
            If gridView.Focused Then Exit Sub
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmSchemeOffer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcNew()
        cmbScheme_MAN.Select()
        Dim uprefix As String = Mid(cnAdminDb, 1, 3)
        If GetAdmindbSoftValue("SYNC-SEPDATADB", "N", tran) = "Y" Then
            Dim usuffix As String = "UTILDB"
            If DbChecker(uprefix + usuffix) <> 0 Then Syncdb = uprefix + usuffix
        End If
    End Sub
    Function funcClear()
        cmbScheme_MAN.Text = ""
        Return 0
    End Function
    Function funcNew()
        flagUpdate = False
        funcClear()
        cmbScheme_MAN.Focus()
        If GetAdmindbSoftValue("COSTCENTRE", "N") = "Y" Then
            strSql = " SELECT 'ALL' AS COSTNAME UNION ALL "
            strSql += " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE C"
            strSql += " JOIN " & cnAdminDb & "..SYNCCOSTCENTRE S ON S.COSTID=C.COSTID"
            strSql += " WHERE ISNULL(C.ACTIVE,'Y')<>'N' "
            strSql += " ORDER BY COSTNAME"
            objGPack.FillCombo(strSql, cmbCostcentre_MAN, True, False)
            cmbCostcentre_MAN.Enabled = True
        Else
            cmbCostcentre_MAN.Enabled = False
        End If
        cmbVbc.Items.Clear()
        cmbVbc.Items.Add("YES")
        cmbVbc.Items.Add("NO")
        cmbVbc.Items.Add("RANGE")
        cmbVbc.Text = ""
        Return 0
    End Function

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim SchemeName As String = cmbScheme_MAN.Text
        If SchemeName = "" Then MsgBox("Scheme Should not Empty", MsgBoxStyle.Information) : cmbScheme_MAN.Focus() : Exit Sub
        strSql = vbCrLf + "DECLARE @SCHMID AS INTEGER "
        strSql += vbCrLf + "DECLARE @COSTID AS VARCHAR(10) "
        strSql += vbCrLf + " SELECT @SCHMID=(SELECT DISTINCT(SCHEMEID) FROM " & cnAdminDb & "..SCHEMEOFFERMAST AS S, " & cnAdminDb & "..CREDITCARD AS C "
        strSql += vbCrLf + " WHERE C.CARDCODE=S.SCHEMEID AND C.NAME IN('" & SchemeName & "'))"
        strSql += vbCrLf + " SELECT @COSTID=(SELECT DISTINCT(COSTID) FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME ='" & cmbCostcentre_MAN.Text & "')"
        strSql += vbCrLf + "  SELECT * FROM "
        strSql += vbCrLf + "  ("
        strSql += vbCrLf + "  SELECT DISTINCT I.ITEMNAME AS PARTICULAR,I.ITEMNAME,"
        strSql += vbCrLf + "  CASE WHEN S.FLATPER<>0 THEN S.FLATPER ELSE NULL END FLATPER,"
        strSql += vbCrLf + "  CASE WHEN S.VA_SLAB<>0 THEN S.VA_SLAB ELSE NULL END VA_SLAB,"
        strSql += vbCrLf + "  CASE WHEN S.WAST_SLAB<>0 THEN S.WAST_SLAB ELSE NULL END WAST_SLAB,"
        strSql += vbCrLf + "  CASE WHEN S.MC_SLAB<>0 THEN S.MC_SLAB ELSE NULL END MC_SLAB,"
        If SPECIFICFORMAT = "1" Then
            strSql += vbCrLf + "  CASE WHEN S.EXTRAWAST_PER<>0 THEN S.EXTRAWAST_PER ELSE NULL END EXTRAWAST_PER,"
        End If
        strSql += vbCrLf + "  CASE WHEN S.SCHEMEID<>0 THEN S.SCHEMEID ELSE NULL END SCHEMEID,"
        strSql += vbCrLf + "  CASE WHEN @SCHMID<>0 "
        If cmbCostcentre_MAN.Text <> "" And cmbCostcentre_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND @COSTID=S.COSTID "
        strSql += vbCrLf + "  THEN CASE WHEN ISNULL(S.VBC,'')='Y' THEN 'YES' WHEN ISNULL(S.VBC,'')='N' THEN 'NO'WHEN ISNULL(S.VBC,'')='R' THEN 'RANGE' END ELSE NULL END VBC,"
        strSql += vbCrLf + "  CASE WHEN S.VATPER<>0 THEN S.VATPER ELSE NULL END VATPER,"
        strSql += vbCrLf + "  ISNULL(INSFROM,0) INSFROM,ISNULL(INSTO,0) INSTO,"
        strSql += vbCrLf + "  'T'COLHEAD,SUBITEM,I.ITEMID,CONVERT(INT,NULL)SUBITEMID "
        strSql += vbCrLf + "  FROM  " & cnAdminDb & "..ITEMMAST AS I  "
        strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SCHEMEOFFERMAST AS S ON S.ITEMID=I.ITEMID AND S.SUBITEMID=0"
        'strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..COSTCENTRE AS C ON ISNULL(S.COSTID,'')=ISNULL(C.COSTID,'')"
        'strSql += vbCrLf + "  WHERE 1=1"
        If cmbCostcentre_MAN.Text <> "" And cmbCostcentre_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND (S.COSTID = @COSTID OR ISNULL(S.COSTID,'')='')"
        If SchemeName <> "" Then
            strSql += vbCrLf + " AND (S.SCHEMEID IN(SELECT DISTINCT(SCHEMEID) FROM " & cnAdminDb & "..SCHEMEOFFERMAST AS S, " & cnAdminDb & "..CREDITCARD AS C "
            strSql += vbCrLf + " WHERE C.CARDCODE=S.SCHEMEID AND C.NAME IN('" & SchemeName & "')) OR ISNULL(S.SCHEMEID,0)=0) "
        End If
        strSql += vbCrLf + "  WHERE ISNULL(I.ACTIVE,'Y')='Y' "
        strSql += vbCrLf + "  UNION ALL"
        strSql += vbCrLf + "  SELECT DISTINCT '   ' + SUBITEMNAME,(SELECT DISTINCT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.ITEMID)AS ITEMNAME,"
        strSql += vbCrLf + "  CASE WHEN SS.FLATPER<>0 THEN SS.FLATPER ELSE NULL END FLATPER,"
        strSql += vbCrLf + "  CASE WHEN SS.VA_SLAB<>0 THEN SS.VA_SLAB ELSE NULL END VA_SLAB,"
        strSql += vbCrLf + "  CASE WHEN SS.WAST_SLAB<>0 THEN SS.WAST_SLAB ELSE NULL END WAST_SLAB,"
        strSql += vbCrLf + "  CASE WHEN SS.MC_SLAB<>0 THEN SS.MC_SLAB ELSE NULL END MC_SLAB,"
        If SPECIFICFORMAT = "1" Then
            strSql += vbCrLf + "  CASE WHEN SS.EXTRAWAST_PER<>0 THEN SS.EXTRAWAST_PER ELSE NULL END EXTRAWAST_PER,"
        End If
        strSql += vbCrLf + "  CASE WHEN SS.SCHEMEID<>0 THEN SS.SCHEMEID ELSE NULL END SCHEMEID,"
        strSql += vbCrLf + "  CASE WHEN @SCHMID<>0 "
        If cmbCostcentre_MAN.Text <> "" And cmbCostcentre_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND @COSTID=SS.COSTID "
        strSql += vbCrLf + "  THEN CASE WHEN ISNULL(SS.VBC,'')='Y' THEN 'YES' WHEN ISNULL(SS.VBC,'')='N' THEN 'NO'WHEN ISNULL(SS.VBC,'')='R' THEN 'RANGE' END ELSE NULL END VBC,"
        strSql += vbCrLf + "  CASE WHEN SS.VATPER<>0 THEN SS.VATPER ELSE NULL END VATPER,"
        strSql += vbCrLf + "  ISNULL(INSFROM,0) INSFROM,ISNULL(INSTO,0) INSTO,"
        strSql += vbCrLf + "  '' COLHEAD,'N' AS SUBITEM,S.ITEMID,S.SUBITEMID"
        strSql += vbCrLf + "  FROM " & cnAdminDb & "..SUBITEMMAST AS S"
        strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..SCHEMEOFFERMAST AS SS ON ISNULL(S.ITEMID,0)=ISNULL(SS.ITEMID,0) AND ISNULL(S.SUBITEMID,0)=ISNULL(SS.SUBITEMID,0)"
        'strSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..COSTCENTRE AS C ON ISNULL(SS.COSTID,'')=ISNULL(C.COSTID,'')"
        'strSql += vbCrLf + "  WHERE 1=1"
        If cmbCostcentre_MAN.Text <> "" And cmbCostcentre_MAN.Text <> "ALL" Then strSql += vbCrLf + " AND (SS.COSTID = @COSTID OR ISNULL(SS.COSTID,'')='')"
        If SchemeName <> "" Then
            strSql += vbCrLf + " AND (SS.SCHEMEID IN(SELECT DISTINCT(SCHEMEID) FROM " & cnAdminDb & "..SCHEMEOFFERMAST AS S, " & cnAdminDb & "..CREDITCARD AS C "
            strSql += vbCrLf + " WHERE C.CARDCODE=S.SCHEMEID AND C.NAME IN('" & SchemeName & "')) OR ISNULL(SS.SCHEMEID,0)=0) "
        End If
        strSql += vbCrLf + " WHERE ISNULL(S.ACTIVE,'Y')='Y' )X"
        strSql += vbCrLf + "  ORDER BY ITEMNAME,COLHEAD DESC"

        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtGrid)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        gridView.DataSource = dtGrid
        BrighttechPack.GlobalMethods.FormatGridColumns(gridView, True, )
        FillGridGroupStyle_KeyNoWise(gridView)
        gridView.Select()
        With gridView
            .SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
            .Columns("KEYNO").Visible = False
            .Columns("COLHEAD").Visible = False
            .Columns("SUBITEM").Visible = False
            .Columns("ITEMNAME").Visible = False
            .Columns("ITEMID").Visible = False
            .Columns("SUBITEMID").Visible = False
            .Columns("SCHEMEID").Visible = False
            If INSBASEOFFERSLAB Then
                .Columns("INSFROM").HeaderText = "INS.FROM"
                .Columns("INSFROM").Width = 70
                .Columns("INSTO").HeaderText = "INS.TO"
                .Columns("INSTO").Width = 70
                .Columns("INSFROM").ReadOnly = False
                .Columns("INSTO").ReadOnly = False
            Else
                .Columns("INSFROM").Visible = False
                .Columns("INSTO").Visible = False
            End If

            .Columns("PARTICULAR").Width = 300
            .Columns("FLATPER").HeaderText = "FLAT %"
            .Columns("VA_SLAB").HeaderText = "VA_SLAB %"
            .Columns("WAST_SLAB").HeaderText = "WAST_SLAB %"
            .Columns("MC_SLAB").HeaderText = "MC_SLAB %"
            .Columns("VATPER").HeaderText = "VATOFF %"
            .Columns("FLATPER").Width = 70
            .Columns("VA_SLAB").Width = 95
            .Columns("WAST_SLAB").Width = 95
            .Columns("MC_SLAB").Width = 95
            .Columns("VBC").Width = 80
            .Columns("FLATPER").ReadOnly = False
            .Columns("VA_SLAB").ReadOnly = False
            .Columns("WAST_SLAB").ReadOnly = False
            .Columns("MC_SLAB").ReadOnly = False
            .Columns("VBC").ReadOnly = False
            .Columns("VATPER").ReadOnly = False
            .Columns("FLATPER").DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .Columns("VA_SLAB").DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .Columns("WAST_SLAB").DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .Columns("MC_SLAB").DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .Columns("VBC").DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .Columns("FLATPER").DefaultCellStyle.Format = "0.00"
            .Columns("VA_SLAB").DefaultCellStyle.Format = "0.00"
            .Columns("WAST_SLAB").DefaultCellStyle.Format = "0.00"
            .Columns("MC_SLAB").DefaultCellStyle.Format = "0.00"
            .Columns("VATPER").DefaultCellStyle.Format = "0.00"
            .Columns("VATPER").Visible = False
            If .Columns.Contains("EXTRAWAST_PER") And SPECIFICFORMAT = "1" Then .Columns("EXTRAWAST_PER").HeaderText = "EXTRA WAST %"
            If .Columns.Contains("EXTRAWAST_PER") And SPECIFICFORMAT = "1" Then .Columns("EXTRAWAST_PER").ReadOnly = False
            If .Columns.Contains("EXTRAWAST_PER") And SPECIFICFORMAT = "1" Then .Columns("EXTRAWAST_PER").DefaultCellStyle.Format = "0.00"
            If .Columns.Contains("EXTRAWAST_PER") And SPECIFICFORMAT = "1" Then .Columns("EXTRAWAST_PER").DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            If DiscVatPer > 0 Then
                .Columns("VATPER").Visible = True
            End If
        End With
        gridView.Rows(0).Cells("FLATPER").Selected = True
    End Sub

    Private Sub gridView_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellEndEdit
        If Not gridView.RowCount > 0 Then Exit Sub
        If gridView.CurrentRow Is Nothing Then Exit Sub
        Dim schemeid As Integer
        Dim Costid As String = ""
        Dim dtscheme As New DataTable
        strSql = " SELECT CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE NAME ='" & cmbScheme_MAN.Text & "' AND CARDTYPE='C'"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtscheme)
        If dtscheme.Rows.Count > 0 Then
            schemeid = Val(dtscheme.Rows(0).Item("CARDCODE").ToString)
            strSql = "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE "
            If cmbCostcentre_MAN.Text <> "" And cmbCostcentre_MAN.Text <> "ALL" Then
                strSql += " WHERE COSTNAME='" & cmbCostcentre_MAN.Text & "'"
                Costid = objGPack.GetSqlValue(strSql, "")
            Else
                Costid = ""
            End If
            strSql = ""
            Try
                Dim dtCost As New DataTable
                strSql = " SELECT C.COSTID,COSTNAME FROM " & cnAdminDb & "..COSTCENTRE C "
                strSql += " JOIN " & cnAdminDb & "..SYNCCOSTCENTRE S ON S.COSTID=C.COSTID "
                strSql += " WHERE ISNULL(C.ACTIVE,'Y')<>'N' ORDER BY COSTNAME"
                da = New OleDbDataAdapter(strSql, cn)
                da.Fill(dtCost)
                For I As Integer = 0 To IIf(Costid = "" And cmbCostcentre_MAN.Enabled = True, dtCost.Rows.Count - 1, 0)
                    If Costid = "" Then
                        If cmbCostcentre_MAN.Enabled = True Then Costid = dtCost.Rows(I).Item("COSTID").ToString
                    End If
                    ''TO INCLUDE SINGLE VALUE FOR SUB ITEM
                    strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..SCHEMEOFFERMAST"
                    strSql += " WHERE SCHEMEID = " & schemeid & ""
                    strSql += " AND ITEMID = " & Val(gridView.CurrentRow.Cells("ITEMID").Value.ToString) & ""
                    strSql += " AND SUBITEMID = " & Val(gridView.CurrentRow.Cells("SUBITEMID").Value.ToString) & ""
                    strSql += " AND COSTID='" & Costid & "'"
                    If INSBASEOFFERSLAB Then strSql += " AND INSFROM=" & Val(gridView.CurrentRow.Cells("INSFROM").Value.ToString) & " AND INSTO = " & Val(gridView.CurrentRow.Cells("INSTO").Value.ToString) & ""
                    If objGPack.GetSqlValue(strSql).Length > 0 Then
                        strSql = " UPDATE " & cnAdminDb & "..SCHEMEOFFERMAST SET FLATPER = " & Val(gridView.CurrentRow.Cells("FLATPER").Value.ToString) & ""
                        strSql += " ,VA_SLAB = " & Val(gridView.CurrentRow.Cells("VA_SLAB").Value.ToString) & ""
                        strSql += " ,WAST_SLAB = " & Val(gridView.CurrentRow.Cells("WAST_SLAB").Value.ToString) & ""
                        strSql += " ,MC_SLAB = " & Val(gridView.CurrentRow.Cells("MC_SLAB").Value.ToString) & ""
                        If SPECIFICFORMAT = "1" Then
                            strSql += " ,EXTRAWAST_PER = " & Val(gridView.CurrentRow.Cells("EXTRAWAST_PER").Value.ToString) & ""
                        End If
                        strSql += " ,VBC = '" & Mid(gridView.CurrentRow.Cells("VBC").Value.ToString, 1, 1) & "'"
                        strSql += " ,COSTID='" & Costid & "'"
                        strSql += " ,VATPER = " & Val(gridView.CurrentRow.Cells("VATPER").Value.ToString) & ""
                        strSql += " WHERE ITEMID = " & Val(gridView.CurrentRow.Cells("ITEMID").Value.ToString) & ""
                        strSql += " AND SUBITEMID = " & Val(gridView.CurrentRow.Cells("SUBITEMID").Value.ToString) & ""
                        strSql += " AND SCHEMEID = " & Val(gridView.CurrentRow.Cells("SCHEMEID").Value.ToString) & ""
                        strSql += " AND COSTID='" & Costid & "'"
                        If INSBASEOFFERSLAB Then strSql += " AND INSFROM=" & Val(gridView.CurrentRow.Cells("INSFROM").Value.ToString) & " AND INSTO = " & Val(gridView.CurrentRow.Cells("INSTO").Value.ToString) & ""
                    Else
                        strSql = " INSERT INTO " & cnAdminDb & "..SCHEMEOFFERMAST(SCHEMEID,ITEMID,SUBITEMID,FLATPER,VA_SLAB,WAST_SLAB,VATPER,MC_SLAB,VBC,ACTIVE,USERID,UPDATED,UPTIME,COSTID"
                        If INSBASEOFFERSLAB Then strSql += " ,INSFROM,INSTO"
                        If SPECIFICFORMAT = "1" Then strSql += " ,EXTRAWAST_PER"
                        strSql += " )"
                        strSql += " VALUES( '" & schemeid & "','" & Val(gridView.CurrentRow.Cells("ITEMID").Value.ToString) & "','" & Val(gridView.CurrentRow.Cells("SUBITEMID").Value.ToString) & "',"
                        strSql += " '" & Val(gridView.CurrentRow.Cells("FLATPER").Value.ToString) & "',"
                        strSql += " '" & Val(gridView.CurrentRow.Cells("VA_SLAB").Value.ToString) & "',"
                        strSql += " '" & Val(gridView.CurrentRow.Cells("WAST_SLAB").Value.ToString) & "',"
                        strSql += " '" & Val(gridView.CurrentRow.Cells("VATPER").Value.ToString) & "',"
                        'strSql += " '" & Val(gridView.CurrentRow.Cells("MC_SLAB").Value.ToString) & "','Y','Y',"
                        strSql += " '" & Val(gridView.CurrentRow.Cells("MC_SLAB").Value.ToString) & "',"
                        'strSql += " '" & gridView.CurrentRow.Cells("VBC").Value.ToString() & "', 'Y',"
                        strSql += " '" & Mid(gridView.CurrentRow.Cells("VBC").Value.ToString, 1, 1) & "', 'Y',"
                        strSql += " "
                        strSql += "'" & userId & "','" & Today.Date.ToString("yyyy-MM-dd") & "','" & Date.Now.ToLongTimeString & "','" & Costid & "'"
                        If INSBASEOFFERSLAB Then strSql += " ," & Val(gridView.CurrentRow.Cells("INSFROM").Value.ToString) & "," & Val(gridView.CurrentRow.Cells("INSTO").Value.ToString)
                        If SPECIFICFORMAT = "1" Then strSql += " ,'" & Val(gridView.CurrentRow.Cells("EXTRAWAST_PER").Value.ToString) & "'"
                        strSql += " )"
                        If gridView.CurrentRow.Cells("SCHEMEID").Value.ToString = "" Then gridView.CurrentRow.Cells("SCHEMEID").Value = schemeid
                    End If

                    tran = Nothing
                    tran = cn.BeginTransaction
                    ExecQuery(SyncMode.Stock, strSql, cn, tran, Costid)
                    tran.Commit()
                    tran = Nothing
                    If gridView.CurrentRow.Cells("SUBITEM").Value.ToString = "Y" Then
                        For cnt As Integer = e.RowIndex + 1 To gridView.RowCount - 1
                            If gridView.Rows(cnt).Cells("ITEMNAME").Value.ToString <> gridView.CurrentRow.Cells("ITEMNAME").Value.ToString Then Exit For
                            gridView.Rows(cnt).Cells("FLATPER").Value = gridView.CurrentRow.Cells("FLATPER").Value
                            gridView.Rows(cnt).Cells("VA_SLAB").Value = gridView.CurrentRow.Cells("VA_SLAB").Value
                            gridView.Rows(cnt).Cells("WAST_SLAB").Value = gridView.CurrentRow.Cells("WAST_SLAB").Value
                            gridView.Rows(cnt).Cells("MC_SLAB").Value = gridView.CurrentRow.Cells("MC_SLAB").Value
                            gridView.Rows(cnt).Cells("VATPER").Value = gridView.CurrentRow.Cells("VATPER").Value
                            gridView.Rows(cnt).Cells("SCHEMEID").Value = gridView.CurrentRow.Cells("SCHEMEID").Value
                            gridView.Rows(cnt).Cells("VBC").Value = gridView.CurrentRow.Cells("VBC").Value
                            If INSBASEOFFERSLAB Then
                                gridView.Rows(cnt).Cells("INSFROM").Value = gridView.CurrentRow.Cells("INSFROM").Value
                                gridView.Rows(cnt).Cells("INSTO").Value = gridView.CurrentRow.Cells("INSTO").Value
                            End If
                            If SPECIFICFORMAT = "1" Then gridView.Rows(cnt).Cells("EXTRAWAST_PER").Value = gridView.CurrentRow.Cells("EXTRAWAST_PER").Value
                            strSql = " SELECT 'CHECK' FROM " & cnAdminDb & "..SCHEMEOFFERMAST"
                            strSql += " WHERE SCHEMEID = " & schemeid & ""
                            strSql += " AND ITEMID = " & Val(gridView.Rows(cnt).Cells("ITEMID").Value.ToString) & ""
                            strSql += " AND SUBITEMID = " & Val(gridView.Rows(cnt).Cells("SUBITEMID").Value.ToString) & ""
                            strSql += " AND COSTID='" & Costid & "'"
                            If INSBASEOFFERSLAB Then strSql += " AND INSFROM=" & Val(gridView.CurrentRow.Cells("INSFROM").Value.ToString) & " AND INSTO = " & Val(gridView.CurrentRow.Cells("INSTO").Value.ToString) & ""
                            If objGPack.GetSqlValue(strSql, "", "", tran).Length > 0 Then
                                strSql = " UPDATE " & cnAdminDb & "..SCHEMEOFFERMAST SET FLATPER = " & Val(gridView.CurrentRow.Cells("FLATPER").Value.ToString) & ""
                                strSql += " ,VA_SLAB = " & Val(gridView.Rows(cnt).Cells("VA_SLAB").Value.ToString) & ""
                                strSql += " ,WAST_SLAB = " & Val(gridView.Rows(cnt).Cells("WAST_SLAB").Value.ToString) & ""
                                strSql += " ,MC_SLAB = " & Val(gridView.Rows(cnt).Cells("MC_SLAB").Value.ToString) & ""
                                strSql += " ,VBC = '" & Mid(gridView.CurrentRow.Cells("VBC").Value.ToString, 1, 1) & "'"
                                strSql += " ,VATPER = " & Val(gridView.CurrentRow.Cells("VATPER").Value.ToString) & ""
                                strSql += " ,COSTID='" & Costid & "'"
                                If SPECIFICFORMAT = "1" Then strSql += " ,EXTRAWAST_PER = " & Val(gridView.Rows(cnt).Cells("EXTRAWAST_PER").Value.ToString) & ""
                                strSql += " WHERE ITEMID = " & Val(gridView.Rows(cnt).Cells("ITEMID").Value.ToString) & ""
                                strSql += " AND SUBITEMID = " & Val(gridView.Rows(cnt).Cells("SUBITEMID").Value.ToString) & ""
                                strSql += " AND SCHEMEID = " & Val(gridView.Rows(cnt).Cells("SCHEMEID").Value.ToString) & ""
                                strSql += " AND COSTID='" & Costid & "'"
                                If INSBASEOFFERSLAB Then strSql += " AND INSFROM=" & Val(gridView.Rows(cnt).Cells("INSFROM").Value.ToString) & " AND INSTO = " & Val(gridView.Rows(cnt).Cells("INSTO").Value.ToString) & ""
                            Else
                                strSql = " INSERT INTO " & cnAdminDb & "..SCHEMEOFFERMAST(SCHEMEID,ITEMID,SUBITEMID,FLATPER,VA_SLAB,WAST_SLAB,VATPER,MC_SLAB,VBC,ACTIVE,USERID,UPDATED,UPTIME,COSTID"
                                If INSBASEOFFERSLAB Then strSql += " ,INSFROM,INSTO"
                                If SPECIFICFORMAT = "1" Then strSql += " ,EXTRAWAST_PER"
                                strSql += ")"
                                strSql += " VALUES( '" & schemeid & "','" & Val(gridView.Rows(cnt).Cells("ITEMID").Value.ToString) & "','" & Val(gridView.Rows(cnt).Cells("SUBITEMID").Value.ToString) & "',"
                                strSql += " '" & Val(gridView.Rows(cnt).Cells("FLATPER").Value.ToString) & "',"
                                strSql += " '" & Val(gridView.Rows(cnt).Cells("VA_SLAB").Value.ToString) & "',"
                                strSql += " '" & Val(gridView.Rows(cnt).Cells("WAST_SLAB").Value.ToString) & "',"
                                strSql += " '" & Val(gridView.CurrentRow.Cells("VATPER").Value.ToString) & "',"
                                strSql += " '" & Val(gridView.Rows(cnt).Cells("MC_SLAB").Value.ToString) & "','Y','Y',"
                                strSql += " '" & userId & "','" & Today.Date.ToString("yyyy-MM-dd") & "','" & Date.Now.ToLongTimeString & "','" & Costid & "'"
                                If INSBASEOFFERSLAB Then strSql += " ," & Val(gridView.Rows(cnt).Cells("INSFROM").Value.ToString) & "," & Val(gridView.Rows(cnt).Cells("INSTO").Value.ToString)
                                If SPECIFICFORMAT = "1" Then strSql += " ,'" & Val(gridView.Rows(cnt).Cells("EXTRAWAST_PER").Value.ToString) & "'"
                                strSql += ")"

                            End If
                            tran = Nothing
                            tran = cn.BeginTransaction
                            ExecQuery(SyncMode.Stock, strSql, cn, tran, Costid)
                            tran.Commit()
                            tran = Nothing
                        Next
                    End If
                    Costid = ""
                Next
            Catch ex As Exception
                If Not tran Is Nothing Then tran.Rollback() : tran = Nothing
                MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            End Try
        End If
    End Sub

    Private Sub gridView_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellEnter
        Dim pt As Point = gridView.Location
        Select Case gridView.Columns(e.ColumnIndex).Name
            Case "VBC"
                cmbVbc.Size = New Size(gridView.Columns(e.ColumnIndex).Width, cmbVbc.Height)
                cmbVbc.Location = pt + gridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, False).Location
                cmbVbc.Text = gridView.CurrentCell.Value.ToString
                cmbVbc.Visible = True
                cmbVbc.Select()
        End Select
    End Sub

    Private Sub gridView_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellLeave
        Select Case gridView.Columns(e.ColumnIndex).Name
            Case "VBC"
                gridView.CurrentRow.Cells("VBC").Value = cmbVbc.Text
                cmbVbc.Text = ""
                cmbVbc.Visible = False
        End Select
    End Sub

    Private Sub gridView_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles gridView.EditingControlShowing
        If Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("VA_SLAB").Index And Not e.Control Is Nothing Then
            Dim tb As TextBox = CType(e.Control, TextBox)
            tb.Tag = "PER"
            AddHandler tb.KeyPress, AddressOf TextKeyPressEvent
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("WAST_SLAB").Index And Not e.Control Is Nothing Then
            Dim tb As TextBox = CType(e.Control, TextBox)
            tb.Tag = "PER"
            AddHandler tb.KeyPress, AddressOf TextKeyPressEvent
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("MC_SLAB").Index And Not e.Control Is Nothing Then
            Dim tb As TextBox = CType(e.Control, TextBox)
            tb.Tag = "PER"
            AddHandler tb.KeyPress, AddressOf TextKeyPressEvent
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("FLATPER").Index And Not e.Control Is Nothing Then
            Dim tb As TextBox = CType(e.Control, TextBox)
            tb.Tag = "PER"
            AddHandler tb.KeyPress, AddressOf TextKeyPressEvent
        End If
        If SPECIFICFORMAT = "1" Then
            If Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("EXTRAWAST_PER").Index And Not e.Control Is Nothing Then
                Dim tb As TextBox = CType(e.Control, TextBox)
                tb.Tag = "PER"
                AddHandler tb.KeyPress, AddressOf TextKeyPressEvent
            End If
        End If
    End Sub
    Private Sub TextKeyPressEvent(ByVal sender As Object, ByVal e As KeyPressEventArgs)
        If CType(sender, TextBox).Tag = "AMOUNT" Then
            textBoxKeyPressValidator(CType(sender, TextBox), e, Datatype.Amount)
        ElseIf CType(sender, TextBox).Tag = "PER" Then
            textBoxKeyPressValidator(CType(sender, TextBox), e, Datatype.Percentage)
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        funcNew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        funcExit()
    End Sub

    Private Sub NewToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        funcNew()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        funcExit()
    End Sub

    Private Sub cmbCostcentre_MAN_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCostcentre_MAN.SelectedIndexChanged
        'If cmbCostcentre_MAN.Text <> "" And gridView.Rows.Count > 0 Then
        '    btnSearch_Click(Me, New KeyEventArgs(Keys.Enter))
        'End If
    End Sub


    Private Function GetControlLocation(ByVal ctrl As Control, ByRef pt As Point) As Point
        If TypeOf ctrl Is Form Then
            Return pt
        Else
            pt += ctrl.Location
            Return GetControlLocation(ctrl.Parent, pt)
        End If
        Return pt
    End Function

    Private Sub cmbVbc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbVbc.KeyDown
        If e.KeyCode = Keys.Enter Then
            With gridView
                If Not .RowCount > 0 Then
                    Exit Sub
                End If
                Select Case .Columns(.CurrentCell.ColumnIndex).Name
                    Case "VBC"
                        gridView.CurrentRow.Cells("VBC").Value = cmbVbc.Text
                        cmbVbc.Visible = False
                        gridView_CellEndEdit(Me, New DataGridViewCellEventArgs(.CurrentCell.ColumnIndex, .CurrentCell.RowIndex))
                        'gridView.Rows(.CurrentCell.RowIndex + 1).Cells("MC_SLAB").Selected = True
                End Select

            End With
        End If
    End Sub

    Private Sub btnCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopy.Click
        If cmbFromScheme_MAN.Text = "" Then MsgBox("Scheme Should not Empty", MsgBoxStyle.Information) : cmbFromScheme_MAN.Focus() : Exit Sub
        If cmbToScheme_MAN.Text = "" Then MsgBox("Scheme Should not Empty", MsgBoxStyle.Information) : cmbToScheme_MAN.Focus() : Exit Sub
        If cmbFromScheme_MAN.Text = cmbToScheme_MAN.Text Then MsgBox("Scheme Should not Same", MsgBoxStyle.Information) : cmbToScheme_MAN.Focus() : Exit Sub
        Dim Costid As String = ""
        Dim Sql As String
        Dim SchemeId, NewSchemeId As Integer
        SchemeId = Val(objGPack.GetSqlValue("SELECT CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE='C' AND NAME='" & cmbFromScheme_MAN.Text & "'", ""))
        NewSchemeId = Val(objGPack.GetSqlValue("SELECT CARDCODE FROM " & cnAdminDb & "..CREDITCARD WHERE CARDTYPE='C' AND NAME='" & cmbToScheme_MAN.Text & "'", ""))
        If cmbCostcentre_MAN.Text <> "ALL" And cmbCostcentre_MAN.Text <> "" Then
            Costid = objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME='" & cmbCostcentre_MAN.Text & "'", "")
        End If

        Dim dtCost As New DataTable
        strSql = " SELECT C.COSTID,COSTNAME FROM " & cnAdminDb & "..COSTCENTRE C "
        strSql += " JOIN " & cnAdminDb & "..SYNCCOSTCENTRE S ON S.COSTID=C.COSTID "
        strSql += " WHERE ISNULL(C.ACTIVE,'Y')<>'N' "
        strSql += " AND S.COSTID<>'" & cnCostId & "'"
        strSql += " ORDER BY COSTNAME"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCost)

        strSql = "DELETE FROM " & cnAdminDb & "..SCHEMEOFFERMAST "
        strSql += " WHERE SCHEMEID=" & NewSchemeId
        If Costid <> "" Then
            strSql += " AND COSTID='" & Costid & "'"
        End If

        Sql = "SELECT * FROM " & cnAdminDb & "..SCHEMEOFFERMAST "
        Sql += " WHERE SCHEMEID=" & SchemeId
        If Costid <> "" Then
            Sql += " AND COSTID='" & Costid & "'"
        End If
        Dim dtSchOffer As New DataTable
        da = New OleDbDataAdapter(Sql, cn)
        da.Fill(dtSchOffer)
        If dtSchOffer.Rows.Count > 0 Then
            Try
                tran = Nothing
                tran = cn.BeginTransaction
                If Costid = "" Then
                    ExecQuery(SyncMode.Master, strSql, cn, tran)
                Else
                    ExecQuery(SyncMode.Stock, strSql, cn, tran, Costid)
                End If

                Sql = " INSERT INTO " & cnAdminDb & "..SCHEMEOFFERMAST("
                Sql += " SCHEMEID,ITEMID,SUBITEMID,FLATPER,VA_SLAB,VBC,ACTIVE,USERID,UPDATED"
                Sql += " ,UPTIME, COSTID, WAST_SLAB, MC_SLAB, INSFROM, INSTO"
                If SPECIFICFORMAT = "1" Then
                    Sql += " ,EXTRAWAST_PER"
                End If
                Sql += " )"
                Sql += " SELECT " & NewSchemeId & ", ITEMID, SUBITEMID, FLATPER, VA_SLAB, VBC, ACTIVE, " & userId & ", '" & Today.Date.ToString("yyyy-MM-dd") & "'"
                Sql += " ,'" & Date.Now.ToLongTimeString & "', COSTID, WAST_SLAB, MC_SLAB, INSFROM, INSTO"
                If SPECIFICFORMAT = "1" Then
                    Sql += " ,EXTRAWAST_PER"
                End If
                Sql += " FROM " & cnAdminDb & "..SCHEMEOFFERMAST "
                Sql += " WHERE SCHEMEID=" & SchemeId
                If Costid <> "" Then
                    Sql += " AND COSTID='" & Costid & "'"
                End If
                cmd = New OleDbCommand(Sql, cn, tran)
                cmd.ExecuteNonQuery()
                For I As Integer = 0 To IIf(Costid = "" And cmbCostcentre_MAN.Enabled = True, dtCost.Rows.Count - 1, 0)
                    If Costid = "" Then
                        If cmbCostcentre_MAN.Enabled = True Then Costid = dtCost.Rows(I).Item("COSTID").ToString
                    End If
                    If Costid <> "" Then InsertQry(cnAdminDb, "SCHEMEOFFERMAST", cnCostId, Costid, cn, tran, "SCHEMEID", NewSchemeId, "COSTID", Costid)
                    Costid = ""
                Next
                tran.Commit()
                tran = Nothing
                MsgBox("Copied..", MsgBoxStyle.Information)
            Catch ex As Exception
                If Not tran Is Nothing Then tran.Rollback() : tran = Nothing
                MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            End Try
        End If
    End Sub
    Private Sub InsertQry(ByVal dbname As String, ByVal TableName As String, ByVal FromCostId As String, ByVal ToCostId As String _
                            , ByVal _Cn As OleDbConnection, ByVal _Tran As OleDbTransaction _
                            , ByVal CondColName1 As String, ByVal CondColVal1 As String _
                            , ByVal CondColName2 As String, ByVal CondColVal2 As String)
        Dim Qry As String = Nothing
        Dim dtTemp As New DataTable

        strSql = " IF OBJECT_ID('" & cnStockDb & "..INS_" & TableName & "', 'U') IS NOT NULL DROP TABLE " & cnStockDb & "..INS_" & TableName & ""
        strSql += vbCrLf + " SELECT * INTO " & cnStockDb & "..INS_" & TableName & " "
        strSql += vbCrLf + " FROM " & dbname & ".." & TableName & " WHERE 1=1 "
        strSql += vbCrLf + " AND " & CondColName1 & " ='" & CondColVal1 & "'"
        If CondColName2 <> "" And CondColVal2 <> "" Then strSql += vbCrLf + " AND " & CondColName2 & " ='" & CondColVal2 & "'"
        cmd = New OleDbCommand(strSql, _Cn, _Tran)
        cmd.ExecuteNonQuery()

        Dim mtempqrytb As String = "TEMPQRYTB"
        strSql = " EXEC " & cnStockDb & "..INSERTQRYGENERATOR_TABLENEW "
        strSql += vbCrLf + " @DBNAME = '" & dbname & "',@TABLENAME = 'INS_" & TableName & "',@MASK_TABLENAME = '" & TableName & "',@TEMPTABLE='" & mtempqrytb & "'"
        cmd = New OleDbCommand(strSql, _Cn, _Tran)
        cmd.ExecuteNonQuery()

        strSql = " INSERT INTO " & Syncdb & "..SENDSYNC(FROMID,TOID,SQLTEXT,STATUS,UPDFILE)    "
        strSql += vbCrLf + " SELECT '" & FromCostId & "','" & ToCostId & "', SQLTEXT,'N' AS STATUS,'' UPDFILE FROM " & cnStockDb & ".." & mtempqrytb
        cmd = New OleDbCommand(strSql, _Cn, _Tran)
        cmd.ExecuteNonQuery()


        strSql = " DROP TABLE " & cnStockDb & "..INS_" & TableName & ""
        cmd = New OleDbCommand(strSql, _Cn, _Tran)
        cmd.ExecuteNonQuery()
        strSql = " DROP TABLE " & cnStockDb & ".." & mtempqrytb & ""
        cmd = New OleDbCommand(strSql, _Cn, _Tran)
        cmd.ExecuteNonQuery()
    End Sub
End Class