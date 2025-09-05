Imports System.Data.OleDb
Public Class TagPurhcaseDetail
    Dim strSql As String
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        objGPack.Validator_Object(Me)
        gridView.RowTemplate.Height = 21
        gridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect
        'gridView.GridColor = Color.White
    End Sub
    Public Sub LoadPurchaseDetail(ByVal itemId As Integer, ByVal tagNo As String)
        strSql = " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPTAGCHECK')> 0 DROP TABLE TEMPTAGCHECK"
        strSql += " CREATE TABLE TEMPTAGCHECK"
        strSql += " ("
        strSql += " COL1 VARCHAR(300)"
        strSql += " ,COL2 VARCHAR(20)"
        strSql += " ,COL3 VARCHAR(20)"
        strSql += " ,COL4 VARCHAR(20)"
        strSql += " ,COL5 VARCHAR(20)"
        strSql += " ,RESULT INT"
        strSql += " ,ORD INT"
        strSql += " ,COLHEAD VARCHAR(2)"
        strSql += " )"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPTAGCHECKPUR') > 0 DROP TABLE TEMPTAGCHECKPUR"
        strSql += " SELECT PURRATE,PURGRSNET,PURWASTAGE,PURTOUCH,PURMC,PURVALUE "
        strSql += " INTO TEMPTAGCHECKPUR"
        strSql += " FROM " & cnAdminDb & "..ITEMTAG"
        strSql += " WHERE itemid = " & itemId & " AND TAGNO = '" & tagNo & "'"
        'strSql += " WHERE SNO = 'TN112'"

        strSql += " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPTAGCHECKSTONE') > 0 DROP TABLE TEMPTAGCHECKSTONE"
        strSql += " SELECT CASE WHEN STNSUBITEMID <> 0 THEN (SELECT SUBITEMNAME FROM " & CNADMINDB & "..SUBITEMMAST WHERE SUBITEMID = T.STNSUBITEMID)"
        strSql += " ELSE (sELECT ITEMNAME FROM " & CNADMINDB & "..ITEMMAST WHERE ITEMID = T.STNITEMID) END AS PARTICULAR"
        strSql += " ,STNPCS,STNWT,PURRATE,PURAMT"
        strSql += " INTO TEMPTAGCHECKSTONE"
        strSql += " FROM " & cnAdminDb & "..ITEMTAGSTONE T"
        strSql += " WHERE TAGSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO = '" & tagNo & "' AND ITEMID = " & itemId & ")"
        strSql += " "
        strSql += " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPTAGCHECKMISC') > 0 DROP TABLE TEMPTAGCHECKMISC"
        strSql += " SELECT "
        strSql += " (SELECT MISCNAME FROM " & cnAdminDb & "..MISCCHARGES WHERE MISCID = T.MISCID)AS PARTICULAR"
        strSql += " ,PURAMOUNT"
        strSql += " INTO TEMPTAGCHECKMISC"
        strSql += " FROM " & cnAdminDb & "..ITEMTAGMISCCHAR AS T"
        strSql += " WHERE TAGSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO = '" & tagNo & "' AND ITEMID = " & itemId & ")"
        strSql += " "
        strSql += " IF (SELECT TOP 1 1 FROM SYSOBJECTS WHERE NAME = 'TEMPTAGCHECKMETAL') > 0 DROP TABLE TEMPTAGCHECKMETAL"
        strSql += " SELECT "
        strSql += " (sELECT CATNAME FROM " & cnAdminDb & "..CATEGORY WHERE CATCODE = T.CATCODE)aS CATNAME"
        strSql += " ,GRSWT,PURRATE,AMOUNT"
        strSql += " INTO TEMPTAGCHECKMETAL"
        strSql += " FROM " & cnAdminDb & "..ITEMTAGMETAL AS T"
        strSql += " WHERE TAGSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO = '" & tagNo & "' AND ITEMID = " & itemId & ")"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()


        strSql = " IF (SELECT COUNT(*) FROM TEMPTAGCHECKPUR)>0"
        strSql += " BEGIN"
        strSql += " INSERT INTO TEMPTAGCHECK(COL1,COL2,RESULT,ORD,COLHEAD)"
        strSql += " SELECT 'RATE',CONVERT(VARCHAR,PURRATE)"
        strSql += " ,0 RESULT,2 ORD,'M' COLHEAD FROM TEMPTAGCHECKPUR"
        strSql += " INSERT INTO TEMPTAGCHECK(COL1,COL2,RESULT,ORD,COLHEAD)"
        strSql += " SELECT 'WASTAGE',CONVERT(VARCHAR,PURWASTAGE)"
        strSql += " ,0 RESULT,2 ORD,'M' COLHEAD FROM TEMPTAGCHECKPUR"
        strSql += " INSERT INTO TEMPTAGCHECK(COL1,COL2,RESULT,ORD,COLHEAD)"
        strSql += " SELECT 'TOUCH',CONVERT(VARCHAR,PURTOUCH)"
        strSql += " ,0 RESULT,2 ORD,'M' COLHEAD FROM TEMPTAGCHECKPUR"
        strSql += " INSERT INTO TEMPTAGCHECK(COL1,COL2,RESULT,ORD,COLHEAD)"
        strSql += " SELECT 'MC',CONVERT(VARCHAR,PURMC)"
        strSql += " ,0 RESULT,2 ORD,'M' COLHEAD FROM TEMPTAGCHECKPUR"
        strSql += " INSERT INTO TEMPTAGCHECK(COL1,COL2,RESULT,ORD,COLHEAD)"
        strSql += " SELECT 'VALUE',CONVERT(VARCHAR,PURVALUE)"
        strSql += " ,0 RESULT,2 ORD,'M' COLHEAD FROM TEMPTAGCHECKPUR"
        strSql += " END"

        strSql += " IF (SELECT COUNT(*) FROM TEMPTAGCHECKSTONE)>0"
        strSql += " BEGIN"
        strSql += " INSERT INTO TEMPTAGCHECK(COL1,RESULT,ORD,COLHEAD)"
        strSql += " SELECT 'STONE DETAILS',1 RESULT,1 ORD,'T' COLHEAD"
        strSql += " INSERT INTO TEMPTAGCHECK(COL1,COL2,COL3,COL4,COL5,RESULT,ORD,COLHEAD)"
        strSql += " SELECT '','PCS','WEIGHT','RATE','AMOUNT',1 RESULT,2 ORD,'H' COLHEAD"
        strSql += " INSERT INTO TEMPTAGCHECK(COL1,COL2,COL3,COL4,COL5,RESULT,ORD,COLHEAD)"
        strSql += " SELECT PARTICULAR,STNPCS,STNWT,PURRATE,PURAMT,1 RESULT,3 ORD,' 'COLHEAD FROM TEMPTAGCHECKSTONE"
        strSql += " END"
        strSql += " "
        strSql += " IF (SELECT COUNT(*) FROM TEMPTAGCHECKMISC)>0"
        strSql += " BEGIN"
        strSql += " INSERT INTO TEMPTAGCHECK(COL1,RESULT,ORD,COLHEAD)"
        strSql += " SELECT 'MISCELLANEOUS DETAILS',2 RESULT,1 ORD,'T' COLHEAD"
        strSql += " INSERT INTO TEMPTAGCHECK(COL1,COL2,RESULT,ORD,COLHEAD)"
        strSql += " SELECT '','AMOUNT',2 RESULT,2 ORD,'H' COLHEAD"
        strSql += " INSERT INTO TEMPTAGCHECK(COL1,COL2,RESULT,ORD,COLHEAD)"
        strSql += " SELECT PARTICULAR,PURAMOUNT,2 RESULT,3 ORD,' 'COLHEAD FROM TEMPTAGCHECKMISC"
        strSql += " END"
        strSql += " "
        strSql += " IF (SELECT COUNT(*) FROM TEMPTAGCHECKMETAL)>0"
        strSql += " BEGIN"
        strSql += " INSERT INTO TEMPTAGCHECK(COL1,RESULT,ORD,COLHEAD)"
        strSql += " SELECT 'MULTI METAL DETAILS',3 RESULT,1 ORD,'T' COLHEAD"
        strSql += " INSERT INTO TEMPTAGCHECK(COL1,COL2,COL3,COL4,RESULT,ORD,COLHEAD)"
        strSql += " SELECT '','GRSWT','PURRATE','AMOUNT',3 RESULT,2 ORD,'H' COLHEAD"
        strSql += " INSERT INTO TEMPTAGCHECK(COL1,COL2,COL3,COL4,RESULT,ORD,COLHEAD)"
        strSql += " SELECT CATNAME,GRSWT,PURRATE,AMOUNT,3 RESULT,3 ORD,' 'COLHEAD FROM TEMPTAGCHECKMETAL"
        strSql += " END"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " SELECT * FROM TEMPTAGCHECK ORDER BY RESULT,ORD"
        Dim dtPurchase As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtPurchase)
        gridView.DataSource = Nothing
        gridView.DataSource = dtPurchase
        gridView.ColumnHeadersVisible = False
        With gridView
            .Columns("COL1").Width = 306
            .Columns("COL2").Width = 100
            .Columns("COL3").Width = 100
            .Columns("COL4").Width = 100
            .Columns("COL5").Width = 100

            .Columns("COL2").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("COL3").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("COL4").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("COL5").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            .Columns("COLHEAD").Visible = False
            .Columns("RESULT").Visible = False
            .Columns("ORD").Visible = False
        End With
    End Sub

    Private Sub TagPurhcaseDetail_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub TagPurhcaseDetail_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        For cnt As Integer = 0 To gridView.RowCount - 1
            With gridView.Rows(cnt)
                If .Cells("COLHEAD").Value.ToString = "T" Then
                    .Cells("COL1").Style = reportHeadStyle
                ElseIf .Cells("COLHEAD").Value.ToString = "H" Then
                    .DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
                    For Each cel As DataGridViewCell In .Cells
                        cel.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                    Next
                End If
            End With
        Next
    End Sub
End Class