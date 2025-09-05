Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.IO
Public Class frmMain

#Region "Variable"
    Dim da As OleDbDataAdapter
    Dim tran As OleDbTransaction
    Dim strSql As String = Nothing
    Dim AppStartupPath As String = Application.StartupPath
    Dim dt_OrderRemainder As DataTable
    Dim dt_OrderRemainder_Today As DataTable
    Dim dt_OutStandingSummary As DataTable
    Dim dt_OutStandingSummary_Today As DataTable
    Dim dt_StockReorder As DataTable
    Dim dt_StockSummary As DataTable
    Dim dt_InstantCashDetail As DataTable
    Dim dt_BirthRemainder As DataTable
    Dim flag_Order As Boolean = False
    Dim flag_Order_Today As Boolean = False
    Dim flag_Outstanding As Boolean = False
    Dim flag_Outstanding_Today As Boolean = False
    Dim flag_stock As Boolean = False
    Dim flag_StockReorder As Boolean = False
    Dim flag_InstantCashDetail As Boolean = False
    Dim flag_BirthRemainder As Boolean = False
    Dim flag_ratepop As Boolean = False
    Dim strParam As String
#End Region

#Region "Class Variable"
    Private WithEvents taskbarNotifier1 As TaskBarNotifier
#End Region

#Region "Constructor"

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.ShowInTaskbar = False
        Me.WindowState = FormWindowState.Minimized
        strParam = Command()
        If strParam = "ORDER" Then
            flag_Order = True
        ElseIf strParam = "ORDER_TODAY" Then
            flag_Order_Today = True
        ElseIf strParam = "OUTSTANDING" Then
            flag_Outstanding = True
        ElseIf strParam = "OUTSTANDING_TODAY" Then
            flag_Outstanding_Today = True
        ElseIf strParam = "STOCK" Then
            flag_stock = True
        ElseIf strParam = "REORDER" Then
            flag_StockReorder = True
        ElseIf strParam = "INSTANTCASHDETAIL" Then
            flag_InstantCashDetail = True
        ElseIf strParam = "BIRTHREMAINDER" Then
            flag_BirthRemainder = True
        Else
            flag_ratepop = True
            'Else
            '    'Temporary
            '    'flag_Outstanding = True
            '    'flag_stock = True
            '    strParam = "ORDER"
            '    flag_Order = True
        End If

    End Sub
#End Region

#Region "Form Load"

    Private Sub frmMain_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If IO.File.Exists(Application.StartupPath & "\Coninfo.ini") = True Then
                Dim file As New FileStream(Application.StartupPath + "\ConInfo.ini", FileMode.Open)
                Dim fstream As New StreamReader(file, System.Text.Encoding.Default)
                Dim DbType As String
                Dim dbPath As String
                Dim password As String
                dbName = Mid(fstream.ReadLine, 21)
                dbName = Mid(dbName.ToUpper, 1, 3) & "ADMINDB"
                compName = Mid(fstream.ReadLine, 21)
                dbSourceName = Mid(fstream.ReadLine, 21)
                dbPath = Mid(fstream.ReadLine, 21)
                password = Mid(fstream.ReadLine, 21).Trim
                If Trim(password) <> "" Then
                    password = Decrypt(password)
                End If
                DbType = UCase(Mid(fstream.ReadLine, 21))
                fstream.Close()
                'flag_stock = True
                'flag_Outstanding = False
                If DbType = "W" Then
                    cn = New OleDbConnection("Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=" & dbName & ";Data Source=" & dbSourceName & "")
                Else
                    cn = New OleDbConnection(String.Format("PROVIDER = SQLOLEDB.1;Persist Security Info=False; Initial Catalog=" & dbName & ";Data Source={0};User Id=" & IIf(DbType <> "S", DbType, "SA") & ";password=" & password & ";", dbSourceName))
                End If

            Else
                MsgBox("Coninfo Not Found", MsgBoxStyle.Information)
                Exit Sub
            End If
        Catch ex As Exception

        End Try

        Try
            If cn.State = ConnectionState.Closed Then cn.Open()
        Catch ex As Exception
            'MsgBox("Connection Problem", MsgBoxStyle.Information)
            MessageBox.Show(ex.ToString)
        End Try

        dt_OrderRemainder = New DataTable
        dt_OrderRemainder_Today = New DataTable
        dt_OutStandingSummary = New DataTable
        dt_OutStandingSummary_Today = New DataTable
        dt_StockSummary = New DataTable
        dt_StockReorder = New DataTable
        dt_InstantCashDetail = New DataTable
        dt_BirthRemainder = New DataTable
        If flag_Order = True Then
            'Checking Other Form
            dt_OrderRemainder = OrderReminder()
        ElseIf flag_Order_Today = True Then
            'Checking Other Form
            dt_OrderRemainder_Today = OrderReminder_Today()
        ElseIf flag_Outstanding = True Then
            dt_OutStandingSummary = OutstandingSummary()
        ElseIf flag_Outstanding_Today = True Then
            dt_OutStandingSummary_Today = OutstandingSummary_Today()
        ElseIf flag_stock = True Then
            dt_StockSummary = StockSummary()
        ElseIf flag_StockReorder = True Then
            dt_StockReorder = ReOrder()
        ElseIf flag_InstantCashDetail = True Then
            dt_InstantCashDetail = InstantCashReport()
        ElseIf flag_BirthRemainder = True Then
            DT_BirthRemainder = BirthRemainder()
        Else
            ''Checking In form
            'dt_OrderRemainder = OrderReminder()
            'dt_OutStandingSummary = OutstandingSummary()
            'dt_StockSummary = StockSummary()
        End If
        'funRemainder()
        ' Add any initialization after the InitializeComponent() call.
        taskbarNotifier1 = New TaskBarNotifier()
        taskbarNotifier1.SetBackgroundBitmap(New Bitmap(My.Resources.skin), Color.FromArgb(255, 0, 255))
        taskbarNotifier1.SetCloseBitmap(New Bitmap(My.Resources.close), Color.FromArgb(255, 0, 255), New Point(465, 17))

        'If flag_Order = True Then
        '    'Order Reaminder
        '    taskbarNotifier1.AddControlToListbox1(New Bitmap(My.Resources.close), Color.FromArgb(255, 0, 255), New Point(17, 145), dt_OrderRemainder, flag_Order)
        '    taskbarNotifier1.AddControlToLabel1(New Bitmap(My.Resources.close), Color.FromArgb(255, 0, 255), New Point(17, 130), flag_Order)

        '    taskbarNotifier1.AddControlToListbox2(New Bitmap(My.Resources.close), Color.FromArgb(255, 0, 255), New Point(172, 145), dt_OrderRemainder, flag_Order)
        '    taskbarNotifier1.AddControlToLabel2(New Bitmap(My.Resources.close), Color.FromArgb(255, 0, 255), New Point(170, 130), flag_Order)

        '    taskbarNotifier1.AddControlToListbox3(New Bitmap(My.Resources.close), Color.FromArgb(255, 0, 255), New Point(328, 145), dt_OrderRemainder, flag_Order)
        '    taskbarNotifier1.AddControlToLabel3(New Bitmap(My.Resources.close), Color.FromArgb(255, 0, 255), New Point(325, 130), flag_Order)
        'End If

        If flag_Order = True Then
            'Order Reaminder
            taskbarNotifier1.AddControlToGridView3(New Bitmap(My.Resources.close), Color.FromArgb(255, 0, 255), New Point(18, 141), dt_OrderRemainder, flag_Order)
        End If
        If flag_Order_Today = True Then
            'Order Reaminder
            taskbarNotifier1.AddControlToGridView3(New Bitmap(My.Resources.close), Color.FromArgb(255, 0, 255), New Point(18, 141), dt_OrderRemainder_Today, flag_Order_Today)
        End If
        If flag_Outstanding = True Then
            'Outstanding Summary
            taskbarNotifier1.AddControlToGridView1(New Bitmap(My.Resources.close), Color.FromArgb(255, 0, 255), New Point(18, 141), dt_OutStandingSummary, flag_Outstanding)
        End If
        If flag_Outstanding_Today = True Then
            'Outstanding Summary Today
            taskbarNotifier1.AddControlToGridView1(New Bitmap(My.Resources.close), Color.FromArgb(255, 0, 255), New Point(18, 141), dt_OutStandingSummary_Today, flag_Outstanding_Today)
        End If
        If flag_stock = True Then
            'Stock Summary
            taskbarNotifier1.AddControlToGridView2(New Bitmap(My.Resources.close), Color.FromArgb(255, 0, 255), New Point(17, 141), dt_StockSummary, flag_stock)
        End If
        If flag_StockReorder = True Then
            'Stock Reorder
            taskbarNotifier1.AddControlToGridView4(New Bitmap(My.Resources.close), Color.FromArgb(255, 0, 255), New Point(17, 141), dt_StockReorder, flag_StockReorder)
        End If

        If flag_InstantCashDetail = True Then
            'Instant Cash Detail
            taskbarNotifier1.AddControlToGridView4(New Bitmap(My.Resources.close), Color.FromArgb(255, 0, 255), New Point(17, 141), dt_InstantCashDetail, flag_InstantCashDetail)
        End If
        If flag_BirthRemainder = True Then
            'Birth Remainder
            taskbarNotifier1.AddControlToGridView5(New Bitmap(My.Resources.close), Color.FromArgb(255, 0, 255), New Point(17, 141), dt_BirthRemainder, flag_BirthRemainder)
        End If

        'Common Name
        taskbarNotifier1.TitleRectangle = New Rectangle(150, 57, 400, 28)
        taskbarNotifier1.TextRectangle = New Rectangle(75, 92, 400, 55)

        With taskbarNotifier1
            .NormalTitleColor = Color.Black
            .HoverTitleColor = Color.Blue
            .NormalContentColor = Color.Yellow
            .HoverContentColor = Color.White
            .CloseButtonClickEnabled = True 'checkBoxCloseClickable.Checked
            .TitleClickEnabled = False 'checkBoxTitleClickable.Checked
            .TextClickEnabled = True 'checkBoxContentClickable.Checked
            .DrawTextFocusRect = True 'checkBoxSelectionRectangle.Checked
            .KeepVisibleOnMouseOver = True 'checkBoxKeepVisibleOnMouseOver.Checked
            .ReShowOnMouseOver = False 'checkBoxReShowOnMouseOver.Checked

            If flag_Order = True Then
                'Remainder
                .Show("Order", _
                  "", _
                Integer.Parse("1500"), _
                Integer.Parse("43200000"), _
                Integer.Parse("1500"))
            End If
            If flag_Order_Today = True Then
                'Remainder
                .Show("Order_Today", _
                  "", _
                Integer.Parse("1500"), _
                Integer.Parse("43200000"), _
                Integer.Parse("1500"))
            End If
            If flag_Outstanding = True Then
                .Show("Outstanding", _
                "", _
                Integer.Parse("1500"), _
                Integer.Parse("43200000"), _
                Integer.Parse("1500"))
            End If
            If flag_Outstanding_Today = True Then
                .Show("Outstanding_Today", _
                "", _
                Integer.Parse("1500"), _
                Integer.Parse("43200000"), _
                Integer.Parse("1500"))
            End If
            If flag_stock = True Then
                .Show("Stock", _
                "", _
                Integer.Parse("1500"), _
                Integer.Parse("43200000"), _
                Integer.Parse("1500"))
            End If
            If flag_StockReorder = True Then
                .Show("Stock Reorder", _
                "", _
                Integer.Parse("1500"), _
                Integer.Parse("43200000"), _
                Integer.Parse("1500"))
            End If

            If flag_InstantCashDetail = True Then
                .Show("InstantcashDetail", _
                            "", _
                            Integer.Parse("1500"), _
                            Integer.Parse("43200000"), _
                            Integer.Parse("1500"))
            End If

            If flag_ratepop = True Then
                .Show("Remainder Rate Update ", _
                                            "", _
                                            Integer.Parse("1500"), _
                                            Integer.Parse("43200000"), _
                                            Integer.Parse("1500"))
            End If
            If flag_BirthRemainder = True Then
                .Show("BirthDay Remainder ", _
                                            "", _
                                            Integer.Parse("1500"), _
                                            Integer.Parse("43200000"), _
                                            Integer.Parse("1500"))
            End If
        End With

    End Sub
#End Region

#Region "DataTable Function"

    Function OrderReminder() As DataTable
        Dim dt As New DataTable
        strSql = " SELECT DISTINCT "
        strSql += vbCrLf + "  (SELECT DESIGNERNAME FROM " & dbName & "..DESIGNER WHERE DESIGNERID="
        strSql += vbCrLf + "  (SELECT TOP 1 DESIGNERID FROM " & dbName & "..ORIRDETAIL WHERE ORNO=O.ORNO AND ORSTATUS='I'))DESIGNERNAME"
        strSql += vbCrLf + "  ,O.ORNO, O.ORDATE,O.REMDATE,O.DUEDATE, P.PNAME, P.MOBILE FROM " & dbName & "..ORMAST AS O"
        strsql += VBCRLF + "  INNER JOIN " & dbName & "..CUSTOMERINFO AS CU ON CU.BATCHNO = O.BATCHNO"
        strsql += VBCRLF + "  INNER JOIN " & dbName & "..PERSONALINFO AS P ON P.SNO = CU.PSNO"
        'strsql += VBCRLF + "  WHERE O.DUEDATE BETWEEN GETDATE() AND GETDATE() + 3 "
        strSql += vbCrLf + " WHERE O.DUEDATE  <='" & Date.Now.ToString("yyyy-MM-dd") & "' "
        strSql += vbCrLf + " AND O.ODBATCHNO IS NULL "
        strSql += vbCrLf + " AND O.SNO IN (SELECT ORSNO FROM " & dbName & "..ORIRDETAIL WHERE ORNO=O.ORNO AND ORSTATUS='I')"
        strSql += vbCrLf + " AND O.SNO NOT IN (SELECT ORSNO FROM " & dbName & "..ORIRDETAIL WHERE ORNO=O.ORNO AND ORSTATUS='R')"
        strSql += vbCrLf + " AND ISNULL(O.CANCEL,'')=''"
        strSql += vbCrLf + " AND ISNULL(O.ORDCANCEL,'')=''"
        strSql += vbCrLf + "  ORDER BY O.DUEDATE "


        'strSql = " SELECT O.ORNO, O.ORDATE,O.REMDATE,O.DUEDATE, P.PNAME, P.MOBILE FROM " & dbName & "..ORMAST AS O"
        'strSql += vbCrLf + " LEFT OUTER JOIN " & dbName & "..CUSTOMERINFO AS CU ON CU.BATCHNO = O.BATCHNO"
        'strSql += vbCrLf + " LEFT OUTER JOIN " & dbName & "..PERSONALINFO AS P ON P.SNO = CU.PSNO"
        'strSql += vbCrLf + " WHERE O.DUEDATE BETWEEN '" & Date.Now.ToString("yyyy-MM-dd") & "' AND '" & Date.Now.ToString("yyyy-MM-dd") & "' "
        'strSql += vbCrLf + " AND O.SNO IN (SELECT ORSNO FROM " & dbName & "..ORIRDETAIL WHERE ORNO=O.ORNO AND ORSTATUS='I')"
        'strSql += vbCrLf + " AND O.SNO NOT IN (SELECT ORSNO FROM " & dbName & "..ORIRDETAIL WHERE ORNO=O.ORNO AND ORSTATUS='R')"
        'strSql += vbCrLf + " ORDER BY O.DUEDATE"
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        Return dt
    End Function
    Function OrderReminder_Today() As DataTable
        Dim dt As New DataTable
        strSql = " SELECT DISTINCT "
        strSql += vbCrLf + "  (SELECT DESIGNERNAME FROM " & dbName & "..DESIGNER WHERE DESIGNERID="
        strSql += vbCrLf + "  (SELECT TOP 1 DESIGNERID FROM " & dbName & "..ORIRDETAIL WHERE ORNO=O.ORNO AND ORSTATUS='I'))DESIGNERNAME"
        strSql += vbCrLf + "  ,O.ORNO, O.ORDATE,O.REMDATE,O.DUEDATE, P.PNAME, P.MOBILE FROM " & dbName & "..ORMAST AS O"
        strSql += vbCrLf + "  INNER JOIN " & dbName & "..CUSTOMERINFO AS CU ON CU.BATCHNO = O.BATCHNO"
        strSql += vbCrLf + "  INNER JOIN " & dbName & "..PERSONALINFO AS P ON P.SNO = CU.PSNO"
        'strsql += VBCRLF + "  WHERE O.DUEDATE BETWEEN GETDATE() AND GETDATE() + 3 "
        strSql += vbCrLf + " WHERE O.DUEDATE  ='" & Date.Now.ToString("yyyy-MM-dd") & "' "
        strSql += vbCrLf + " AND O.ODBATCHNO IS NULL "
        strSql += vbCrLf + " AND O.SNO IN (SELECT ORSNO FROM " & dbName & "..ORIRDETAIL WHERE ORNO=O.ORNO AND ORSTATUS='I')"
        strSql += vbCrLf + " AND O.SNO NOT IN (SELECT ORSNO FROM " & dbName & "..ORIRDETAIL WHERE ORNO=O.ORNO AND ORSTATUS='R')"
        strSql += vbCrLf + " AND ISNULL(O.CANCEL,'')=''"
        strSql += vbCrLf + " AND ISNULL(O.ORDCANCEL,'')=''"
        strSql += vbCrLf + "  ORDER BY O.DUEDATE "


        'strSql = " SELECT O.ORNO, O.ORDATE,O.REMDATE,O.DUEDATE, P.PNAME, P.MOBILE FROM " & dbName & "..ORMAST AS O"
        'strSql += vbCrLf + " LEFT OUTER JOIN " & dbName & "..CUSTOMERINFO AS CU ON CU.BATCHNO = O.BATCHNO"
        'strSql += vbCrLf + " LEFT OUTER JOIN " & dbName & "..PERSONALINFO AS P ON P.SNO = CU.PSNO"
        'strSql += vbCrLf + " WHERE O.DUEDATE BETWEEN '" & Date.Now.ToString("yyyy-MM-dd") & "' AND '" & Date.Now.ToString("yyyy-MM-dd") & "' "
        'strSql += vbCrLf + " AND O.SNO IN (SELECT ORSNO FROM " & dbName & "..ORIRDETAIL WHERE ORNO=O.ORNO AND ORSTATUS='I')"
        'strSql += vbCrLf + " AND O.SNO NOT IN (SELECT ORSNO FROM " & dbName & "..ORIRDETAIL WHERE ORNO=O.ORNO AND ORSTATUS='R')"
        'strSql += vbCrLf + " ORDER BY O.DUEDATE"
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        Return dt
    End Function
    Function BirthRemainder() As DataTable
        Dim dt As New DataTable
        strSql = "SELECT CUSTOMERNAME,WISHDATE,WISHES,MOBILENO FROM "
        strSql += vbCrLf + "(SELECT DISTINCT PNAME AS CUSTOMERNAME,CONVERT (NVARCHAR(50),DATEADD(YY,DATEDIFF(YY,DOB,GETDATE()),DOB),103) AS WISHDATE"
        strSql += vbCrLf + ",'BIRTHDAY' AS WISHES,MOBILE AS MOBILENO  FROM " & dbName & "..PERSONALINFO WHERE CONVERT(NVARCHAR(50),DATEDIFF(DD,GETDATE(),DATEADD(YY,DATEDIFF(YY,DOB,GETDATE()),DOB)),103) BETWEEN 0 AND 2 "
        strSql += vbCrLf + "UNION ALL "
        strSql += vbCrLf + "SELECT DISTINCT ACNAME AS CUSTOMERNAME ,CONVERT (NVARCHAR(50),DATEADD(YY,DATEDIFF(YY,DOBIRTH,GETDATE()),DOBIRTH),103) AS WISHDATE ,'BIRTHDAY' AS WISHES,MOBILE AS MOBILENO  "
        strSql += vbCrLf + " From " & dbName & "..ACHEAD WHERE CONVERT(NVARCHAR(50),DATEDIFF(DD,GETDATE(),DATEADD(YY,DATEDIFF(YY,DOBIRTH,GETDATE()),DOBIRTH)),103)  BETWEEN 0 AND 2"
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT DISTINCT PNAME AS CUSTOMERNAME ,CONVERT (NVARCHAR(50),DATEADD(YY,DATEDIFF(YY,ANNIVERSARY,GETDATE()),ANNIVERSARY),103) AS WISHDATE,'ANNIVERSARY' AS WISHES,MOBILE AS MOBILENO  FROM " & dbName & "..PERSONALINFO"
        strSql += vbCrLf + "WHERE CONVERT(NVARCHAR(50),DATEDIFF(DD,GETDATE(),DATEADD(YY,DATEDIFF(YY,ANNIVERSARY,GETDATE()),ANNIVERSARY)),103) BETWEEN 0 AND 2 "
        strSql += vbCrLf + "UNION ALL"
        strSql += vbCrLf + "SELECT DISTINCT ACNAME AS CUSTOMERNAME , CONVERT (NVARCHAR(50),DATEADD(YY,DATEDIFF(YY,ANNIVERSARY,GETDATE()),ANNIVERSARY),103) AS ANNIVERSARYDATE,'ANNIVERSARY' AS WISHES,"
        strSql += vbCrLf + "MOBILE AS MOBILENO  FROM " & dbName & "..ACHEAD WHERE CONVERT(NVARCHAR(50),DATEDIFF(DD,GETDATE(),DATEADD(YY,DATEDIFF(YY,ANNIVERSARY,GETDATE()),ANNIVERSARY)),103)  BETWEEN 0 AND 2"
        strSql += vbCrLf + ")X GROUP BY CUSTOMERNAME,WISHDATE,WISHES,MOBILENO ORDER BY CUSTOMERNAME ASC"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        Return dt
    End Function
    Function OutstandingSummary() As DataTable
        Dim dt As New DataTable
        'strSql = " IF OBJECT_ID('TEMPTABLEDB..OUTSTANDINGREMAINDER') IS NOT NULL DROP TABLE TEMPTABLEDB..OUTSTANDINGREMAINDER"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()

        'strSql = "SELECT ROW_NUMBER() OVER (ORDER BY DUEDATE DESC) AS SNO,* INTO TEMPTABLEDB..OUTSTANDINGREMAINDER "
        'strsql += VBCRLF + "   FROM"
        'strsql += VBCRLF + "  (SELECT O.DUEDATE, P.PNAME AS NAME, P.MOBILE AS MOBILENO, SUBSTRING(RUNNO, 6,20) AS RUNNO"
        'strsql += VBCRLF + "  ,SUM(CASE WHEN RECPAY='P' THEN AMOUNT ELSE -1*AMOUNT END)AMOUNT, RUNNO AS RUNNO1"
        'strsql += VBCRLF + "  , '1' AS RESULT"
        'strsql += VBCRLF + "  FROM  " & dbName & "..OUTSTANDING AS O"
        'strsql += VBCRLF + "  INNER JOIN  " & dbName & "..CUSTOMERINFO AS C ON C.BATCHNO = O.BATCHNO"
        'strsql += VBCRLF + "  INNER JOIN  " & dbName & "..PERSONALINFO AS P ON P.SNO = C.PSNO"
        'strsql += VBCRLF + "  WHERE 1 = 1 "
        'strsql += VBCRLF + "  AND ISNULL(O.CANCEL,'') = '' AND O.RECPAY = 'P' "
        'strsql += VBCRLF + "  AND O.PAYMODE IN('DU','DP','DR')"
        'strsql += VBCRLF + "  AND O.FROMFLAG='P' AND O.AMOUNT <> 0.00"
        'strsql += VBCRLF + "  GROUP BY PNAME,MOBILE,RUNNO, O.DUEDATE)X"
        'cmd = New OleDbCommand(strSql, cn)
        'cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()

        'strSql = "INSERT INTO TEMPTABLEDB..OUTSTANDINGREMAINDER(RUNNO,AMOUNT,RESULT) "
        'strsql += VBCRLF + " SELECT 'TOTAL', SUM(AMOUNT)TOTAL, '2'  FROM TEMPTABLEDB..OUTSTANDINGREMAINDER WHERE RESULT = 1"
        'cmd = New OleDbCommand(strSql, cn)
        'cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()
        'strSql = "SELECT NAME, MOBILENO, RUNNO, AMOUNT FROM TEMPTABLEDB..OUTSTANDINGREMAINDER ORDER BY RESULT"
        'da = New OleDbDataAdapter(strSql, cn)

        strSql = " SELECT 1 FROM SYSOBJECTS WHERE NAME='REM_RPT_CUSTOMEROUTSTANDING_MONTH'"
        If Val(GetSqlValue(strSql).ToString()) = 0 Then
            strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME='REM_RPT_CUSTOMEROUTSTANDING_MONTH')>0"
            strSql += vbCrLf + "  	DROP PROCEDURE REM_RPT_CUSTOMEROUTSTANDING_MONTH"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            strSql = vbCrLf + "CREATE PROCEDURE REM_RPT_CUSTOMEROUTSTANDING_MONTH"
            strSql += vbCrLf + " (        "
            strSql += vbCrLf + " @FROMDATE VARCHAR(12)        "
            strSql += vbCrLf + " ,@TODATE VARCHAR(12)    "
            strSql += vbCrLf + " ,@DISPLAY VARCHAR(1)        "
            strSql += vbCrLf + " ,@SUMMARY VARCHAR(1)        "
            strSql += vbCrLf + " ,@ORDERBY VARCHAR(1)        "
            strSql += vbCrLf + " ,@FILTERBY VARCHAR(50)        "
            strSql += vbCrLf + " ,@FILTERCAPTION VARCHAR(50)        "
            strSql += vbCrLf + " ,@COSTNAME VARCHAR(50)        "
            strSql += vbCrLf + " ,@COMPANYID VARCHAR(5000)        "
            strSql += vbCrLf + " ,@TYPEID VARCHAR(50)    "
            strSql += vbCrLf + " ,@CRPUR VARCHAR(1)        "
            strSql += vbCrLf + " ,@ACCODE VARCHAR(25)        "
            strSql += vbCrLf + " ,@GRPAREA VARCHAR(1)"
            strSql += vbCrLf + " ,@GRPCC VARCHAR(1)    "
            strSql += vbCrLf + " )        "
            strSql += vbCrLf + " AS        "
            strSql += vbCrLf + " BEGIN      "
            strSql += vbCrLf + " 	DECLARE @QRY VARCHAR(4000)    "
            strSql += vbCrLf + " 	DECLARE @TYPE VARCHAR(50)"
            strSql += vbCrLf + " 	SET @TYPE = @TYPEID"
            strSql += vbCrLf + " 	SET @TYPEID = REPLACE(@TYPEID,'DP','D')    "
            strSql += vbCrLf + " 	SET @ACCODE= REPLACE(@ACCODE,',',''',''')    "
            strSql += vbCrLf + " 	IF OBJECT_ID('TEMPTABLEDB..COSTCENTRE', 'U') IS NOT NULL DROP TABLE TEMPTABLEDB..COSTCENTRE          "
            strSql += vbCrLf + " 	CREATE TABLE TEMPTABLEDB..COSTCENTRE(COSTID VARCHAR(3))        "
            strSql += vbCrLf + " 	IF @COSTNAME = 'ALL' OR @COSTNAME = ''        "
            strSql += vbCrLf + " 	INSERT INTO TEMPTABLEDB..COSTCENTRE SELECT COSTID FROM COSTCENTRE UNION ALL SELECT ''        "
            strSql += vbCrLf + " 	ELSE        "
            strSql += vbCrLf + " 	INSERT INTO TEMPTABLEDB..COSTCENTRE SELECT COSTID FROM COSTCENTRE WHERE COSTNAME = @COSTNAME        "
            strSql += vbCrLf + " 	IF OBJECT_ID('TEMPTABLEDB..COMPANY', 'U') IS NOT NULL DROP TABLE  TEMPTABLEDB..COMPANY          "
            strSql += vbCrLf + " 	CREATE TABLE TEMPTABLEDB..COMPANY (COMPANYID VARCHAR(3))          "
            strSql += vbCrLf + " 	DECLARE @TCOMPANY VARCHAR(50)          "
            strSql += vbCrLf + " 	IF CHARINDEX(',',@COMPANYID)=0          "
            strSql += vbCrLf + " 	INSERT INTO TEMPTABLEDB..COMPANY (COMPANYID) SELECT @COMPANYID          "
            strSql += vbCrLf + " 	WHILE CHARINDEX(',',@COMPANYID)>0          "
            strSql += vbCrLf + " 	BEGIN          "
            strSql += vbCrLf + " 	SET @TCOMPANY=LEFT(@COMPANYID,CHARINDEX(',',@COMPANYID)-1)          "
            strSql += vbCrLf + " 	SET @COMPANYID=RIGHT(@COMPANYID,LEN(@COMPANYID)-CHARINDEX(',',@COMPANYID))          "
            strSql += vbCrLf + " 	IF @COMPANYID <>'' AND CHARINDEX(',',@COMPANYID)=0          "
            strSql += vbCrLf + " 	SET @COMPANYID= @COMPANYID + ','          "
            strSql += vbCrLf + " 	INSERT INTO TEMPTABLEDB..COMPANY (COMPANYID) SELECT @TCOMPANY          "
            strSql += vbCrLf + " 	END          "
            strSql += vbCrLf + " 	IF OBJECT_ID('TEMPTABLEDB..TYPE', 'U') IS NOT NULL DROP TABLE TEMPTABLEDB..TYPE         "
            strSql += vbCrLf + " 	CREATE TABLE TEMPTABLEDB..TYPE (TYPEID VARCHAR(3))            "
            strSql += vbCrLf + " 	DECLARE @TEMPID VARCHAR(5)            "
            strSql += vbCrLf + " 	IF CHARINDEX(',',@TYPEID)=0            "
            strSql += vbCrLf + " 	INSERT INTO TEMPTABLEDB..TYPE (TYPEID) SELECT @TYPEID            "
            strSql += vbCrLf + " 	WHILE CHARINDEX(',',@TYPEID)>0            "
            strSql += vbCrLf + " 	BEGIN            "
            strSql += vbCrLf + " 	SET @TEMPID=LEFT(@TYPEID,CHARINDEX(',',@TYPEID)-1)            "
            strSql += vbCrLf + " 	SET @TYPEID=RIGHT(@TYPEID,LEN(@TYPEID)-CHARINDEX(',',@TYPEID))            "
            strSql += vbCrLf + " 	IF @TYPEID <>'' AND CHARINDEX(',',@TYPEID)=0            "
            strSql += vbCrLf + " 	SET @TYPEID= @TYPEID + ','            "
            strSql += vbCrLf + " 	INSERT INTO TEMPTABLEDB..TYPE (TYPEID) SELECT @TEMPID            "
            strSql += vbCrLf + " 	END            "
            strSql += vbCrLf + " 	IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME='TEMP_OUTSTANDING') > 0        "
            strSql += vbCrLf + " 	DROP TABLE TEMPTABLEDB..TEMP_OUTSTANDING        "
            strSql += vbCrLf + " 	PRINT @TYPEID "
            strSql += vbCrLf + " 	IF @SUMMARY = 'N'        "
            strSql += vbCrLf + " 	BEGIN        "
            strSql += vbCrLf + " 		SELECT @QRY = 'SELECT CONVERT(VARCHAR(200),NULL)PARTICULAR,O.RUNNO,CONVERT(VARCHAR(50),O.TRANTYPE)TRANTYPE,O.TRANNO,O.TRANDATE,O.DUEDATE'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,CASE WHEN RECPAY = ''P'' THEN AMOUNT ELSE 0 END AS DEBIT'        "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,CASE WHEN RECPAY = ''R'' THEN AMOUNT ELSE 0 END AS CREDIT'    "
            strSql += vbCrLf + "		SELECT @QRY = @QRY + CHAR(13)+' ,CONVERT(NUMERIC(15,2),NULL)BALANCE'"
            strSql += vbCrLf + "		SELECT @QRY = @QRY + CHAR(13)+' ,CASE WHEN RECPAY = ''P'' THEN GRSWT ELSE 0 END AS DEBIT_WT'        "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,CASE WHEN RECPAY = ''R'' THEN GRSWT ELSE 0 END AS CREDIT_WT'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,CONVERT(NUMERIC(15,2),NULL)BALANCE_WT,P.PNAME NAME'"
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,CASE WHEN ISNULL(P.MOBILE,'''')='''' THEN P.PHONERES ELSE P.MOBILE END AS MOBILE'"
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,P.ADDRESS1,P.ADDRESS2,P.AREA,P.CITY'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,(SELECT EMPNAME FROM EMPMASTER WHERE EMPID = O.EMPID)AS SALESPERSON'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,(SELECT COSTNAME FROM COSTCENTRE WHERE COSTID = O.COSTID)AS COSTNAME'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,CONVERT(INT,2)RESULT,CONVERT(VARCHAR,NULL)COLHEAD,CONVERT(INT,1)AREAORD,O.PAYMODE'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,O.COSTID,O.COMPANYID,CONVERT(VARCHAR(12),NULL)LASTTRANDATE'"
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,O.REMARK1,O.REMARK2'"
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,IDENTITY(INT,1,1)KNO'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' INTO TEMPTABLEDB..TEMP_OUTSTANDING'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' FROM  OUTSTANDING AS O'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' INNER JOIN CUSTOMERINFO AS CU ON CU.BATCHNO = O.BATCHNO'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' INNER JOIN PERSONALINFO AS P ON P.SNO = CU.PSNO'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' WHERE O.TRANDATE BETWEEN '''+@FROMDATE+''' AND '''+@TODATE+''''    "
            strSql += vbCrLf + " 		IF @COSTNAME <> 'ALL' AND @COSTNAME <> '' SELECT @QRY = @QRY + CHAR(13)+' AND ISNULL(O.COSTID,'''') IN (SELECT COSTID FROM TEMPTABLEDB..COSTCENTRE)'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' AND ISNULL(O.CANCEL,'''') = ''''  AND ISNULL(O.COMPANYID,'''') IN (SELECT COMPANYID FROM TEMPTABLEDB..COMPANY)'    "
            strSql += vbCrLf + " 		IF @ACCODE <> ''      "
            strSql += vbCrLf + " 		BEGIN"
            strSql += vbCrLf + " 			IF @TYPE='D,'"
            strSql += vbCrLf + " 				SELECT @QRY = @QRY + CHAR(13)+' AND O.FROMFLAG=''P'''"
            strSql += vbCrLf + " 			ELSE"
            strSql += vbCrLf + " 				SELECT @QRY = @QRY + CHAR(13)+' AND O.ACCODE IN('''+@ACCODE+''')'"
            strSql += vbCrLf + " 		END"
            strSql += vbCrLf + " 		ELSE"
            strSql += vbCrLf + " 		BEGIN"
            strSql += vbCrLf + " 			SELECT @QRY = @QRY + CHAR(13)+' AND O.ACCODE IN(SELECT ACCODE FROM ACHEAD WHERE ACTYPE=''C'')'"
            strSql += vbCrLf + " 		END"
            strSql += vbCrLf + " 		/*IF @CRPUR ='N' SELECT @QRY = @QRY + CHAR(13)+' AND O.PAYMODE<>''DP'''    "
            strSql += vbCrLf + " 		IF @CRPUR ='Y' SELECT @QRY = @QRY + CHAR(13)+'AND O.PAYMODE=''DP'' ' */      "
            strSql += vbCrLf + " 		PRINT  @QRY"
            strSql += vbCrLf + " 		EXEC (@QRY)    "
            strSql += vbCrLf + " 	END        "
            strSql += vbCrLf + " 	ELSE IF @SUMMARY = 'Y'        "
            strSql += vbCrLf + " 	BEGIN      "
            strSql += vbCrLf + " 		SELECT @QRY = ' SELECT PARTICULAR,RUNNO,TRANTYPE,TRANNO,CONVERT(VARCHAR,TRANDATE,105)TRANDATE1,CONVERT(VARCHAR,DUEDATE,105)DUEDATE,TRANDATE,DEBIT,CREDIT,BALANCE'  "
            strSql += vbCrLf + "		SELECT @QRY = @QRY + CHAR(13)+' ,DEBIT_WT,CREDIT_WT,BALANCE_WT'"
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,NAME,MOBILE,ADDRESS1,ADDRESS2,AREA,CITY'"
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,PAYMODE,SALESPERSON,COSTNAME, '  "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' AREAORD,COSTID,COMPANYID,RESULT,COLHEAD'"
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,CONVERT(VARCHAR,(SELECT TOP 1 TRANDATE FROM OUTSTANDING WHERE RUNNO = X.RUNNO AND COSTID = X.COSTID AND COMPANYID = X.COMPANYID '  "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ORDER BY TRANDATE DESC),103)AS LASTTRANDATE '    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,REMARK1,REMARK2'"
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,IDENTITY(INT,1,1)KNO'"
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' INTO TEMPTABLEDB..TEMP_OUTSTANDING '    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' FROM'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ('      "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' SELECT CONVERT(VARCHAR(200),NULL)PARTICULAR,O.RUNNO,CONVERT(VARCHAR(50),O.TRANTYPE)TRANTYPE'  "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,(SELECT TOP 1 TRANNO FROM OUTSTANDING WHERE RUNNO = O.RUNNO AND COSTID = O.COSTID AND COMPANYID = O.COMPANYID ORDER BY TRANDATE,TRANNO) TRANNO'  "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,(SELECT TOP 1 TRANDATE FROM OUTSTANDING WHERE RUNNO = O.RUNNO AND COSTID = O.COSTID AND COMPANYID = O.COMPANYID ORDER BY TRANDATE,TRANNO) TRANDATE'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,(SELECT TOP 1 DUEDATE FROM OUTSTANDING WHERE RUNNO = O.RUNNO AND COSTID = O.COSTID AND COMPANYID = O.COMPANYID ORDER BY TRANDATE,TRANNO) DUEDATE'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,SUM(CASE WHEN RECPAY = ''P'' THEN AMOUNT ELSE 0 END) AS DEBIT'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,SUM(CASE WHEN RECPAY = ''R'' THEN AMOUNT ELSE 0 END) AS CREDIT'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,CASE WHEN TRANTYPE=''D'' THEN SUM(CASE WHEN RECPAY = ''P'' THEN AMOUNT ELSE -1*AMOUNT END) '"
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ELSE SUM(CASE WHEN RECPAY = ''R'' THEN AMOUNT ELSE -1*AMOUNT END) END AS BALANCE'    "
            strSql += vbCrLf + "		SELECT @QRY = @QRY + CHAR(13)+' ,SUM(CASE WHEN RECPAY = ''P'' THEN GRSWT ELSE 0 END) AS DEBIT_WT'        "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,SUM(CASE WHEN RECPAY = ''R'' THEN GRSWT ELSE 0 END) AS CREDIT_WT'    "
            strSql += vbCrLf + "		SELECT @QRY = @QRY + CHAR(13)+' ,CASE WHEN TRANTYPE=''D'' THEN SUM(CASE WHEN RECPAY = ''P'' THEN GRSWT ELSE -1*GRSWT END) '"
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ELSE SUM(CASE WHEN RECPAY = ''R'' THEN GRSWT ELSE -1*GRSWT END) END AS BALANCE_WT'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,P.PNAME NAME'"
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,CASE WHEN ISNULL(P.MOBILE,'''')='''' THEN P.PHONERES ELSE P.MOBILE END AS MOBILE'"
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,P.AREA,P.ADDRESS1,P.ADDRESS2,P.CITY,CONVERT(VARCHAR(10),NULL) PAYMODE'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,CONVERT(VARCHAR(40),NULL) SALESPERSON'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,(SELECT COSTNAME FROM COSTCENTRE WHERE COSTID = O.COSTID)AS COSTNAME'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,CONVERT(INT,2)RESULT,CONVERT(VARCHAR,NULL)COLHEAD,CONVERT(INT,1)AREAORD'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,O.COSTID,O.COMPANYID'  "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,(SELECT TOP 1 REMARK1 FROM OUTSTANDING WHERE RUNNO = O.RUNNO AND COSTID = O.COSTID AND COMPANYID = O.COMPANYID ORDER BY TRANDATE,TRANNO) REMARK1'  "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,(SELECT TOP 1 REMARK2 FROM OUTSTANDING WHERE RUNNO = O.RUNNO AND COSTID = O.COSTID AND COMPANYID = O.COMPANYID ORDER BY TRANDATE,TRANNO) REMARK2'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' FROM OUTSTANDING AS O'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' INNER JOIN CUSTOMERINFO AS CU ON CU.BATCHNO = '"
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' (SELECT TOP 1 BATCHNO FROM OUTSTANDING WHERE RUNNO = O.RUNNO AND COSTID = O.COSTID AND COMPANYID = O.COMPANYID ORDER BY TRANDATE DESC)'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' INNER JOIN PERSONALINFO AS P ON P.SNO = CU.PSNO'        "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' WHERE O.TRANDATE BETWEEN '''+@FROMDATE+''' AND '''+@TODATE+''''        "
            strSql += vbCrLf + " 		IF @COSTNAME <> 'ALL' AND @COSTNAME <> '' SELECT @QRY = @QRY + CHAR(13)+' AND ISNULL(O.COSTID,'''') IN (SELECT COSTID FROM TEMPTABLEDB..COSTCENTRE)'        "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' AND ISNULL(O.CANCEL,'''') = '''''        "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' AND ISNULL(O.COMPANYID,'''') IN (SELECT COMPANYID FROM TEMPTABLEDB..COMPANY)'      "
            strSql += vbCrLf + " 		IF @ACCODE <> ''      "
            strSql += vbCrLf + " 		BEGIN"
            strSql += vbCrLf + " 			IF @TYPE='D,'"
            strSql += vbCrLf + " 				SELECT @QRY = @QRY + CHAR(13)+' AND O.FROMFLAG=''P''  '"
            strSql += vbCrLf + " 			ELSE"
            strSql += vbCrLf + " 				SELECT @QRY = @QRY + CHAR(13)+' AND O.ACCODE IN('''+@ACCODE+''')'"
            strSql += vbCrLf + " 		END"
            strSql += vbCrLf + " 		ELSE"
            strSql += vbCrLf + " 		BEGIN"
            strSql += vbCrLf + " 			SELECT @QRY = @QRY + CHAR(13)+' AND O.ACCODE IN(SELECT ACCODE FROM ACHEAD WHERE ACTYPE=''C'')'"
            strSql += vbCrLf + " 		END"
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' GROUP BY O.RUNNO,O.TRANTYPE,P.PNAME,P.MOBILE,P.PHONERES'"
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,O.COSTID,O.COMPANYID,P.AREA,P.ADDRESS1,P.ADDRESS2,P.CITY'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' )X'    "
            strSql += vbCrLf + " 		PRINT @QRY    "
            strSql += vbCrLf + " 		EXEC (@QRY)   "
            strSql += vbCrLf + " 	END"
            strSql += vbCrLf + " 	IF @TYPE='D,'"
            strSql += vbCrLf + " 		DELETE FROM TEMPTABLEDB..TEMP_OUTSTANDING WHERE RUNNO IN (SELECT RUNNO FROM  OUTSTANDING WHERE RECPAY='R' AND PAYMODE IN('DU','DP')) 	"
            strSql += vbCrLf + "	UPDATE TEMPTABLEDB..TEMP_OUTSTANDING SET PAYMODE = 'DP'"
            strSql += vbCrLf + "	WHERE RUNNO IN (SELECT RUNNO FROM  OUTSTANDING WHERE RECPAY='R' AND PAYMODE='DU') "
            strSql += vbCrLf + " 	UPDATE TEMPTABLEDB..TEMP_OUTSTANDING SET TRANTYPE = 'J'        "
            strSql += vbCrLf + " 	WHERE RUNNO IN (SELECT RUNNO FROM  OUTSTANDING O WHERE EXISTS (SELECT 1 FROM  ITEMDETAIL WHERE BATCHNO = O.BATCHNO AND RUNNO=O.RUNNO))        "
            strSql += vbCrLf + " 	UPDATE TEMPTABLEDB..TEMP_OUTSTANDING SET TRANTYPE = 'O' WHERE SUBSTRING(RUNNO,6,1) = 'O'        "
            strSql += vbCrLf + " 	DELETE FROM TEMPTABLEDB..TEMP_OUTSTANDING WHERE TRANTYPE NOT IN (SELECT TYPEID FROM TEMPTABLEDB..TYPE)        "
            strSql += vbCrLf + " 	IF @DISPLAY = 'C'        "
            strSql += vbCrLf + " 	BEGIN        "
            strSql += vbCrLf + " 		DELETE FROM TEMPTABLEDB..TEMP_OUTSTANDING WHERE KNO NOT IN (      "
            strSql += vbCrLf + " 		SELECT KNO FROM TEMPTABLEDB..TEMP_OUTSTANDING AS O      "
            strSql += vbCrLf + " 		INNER JOIN      "
            strSql += vbCrLf + " 		(      "
            strSql += vbCrLf + " 		SELECT RUNNO,COSTID,COMPANYID FROM TEMPTABLEDB..TEMP_OUTSTANDING      "
            strSql += vbCrLf + " 		GROUP BY RUNNO,COSTID,COMPANYID        "
            strSql += vbCrLf + " 		HAVING SUM(ISNULL(DEBIT,0))-SUM(ISNULL(CREDIT,0))  = 0      "
            strSql += vbCrLf + " 		)X ON X.RUNNO = O.RUNNO AND X.COSTID = O.COSTID AND X.COMPANYID = O.COMPANYID)      "
            strSql += vbCrLf + " 	END        "
            strSql += vbCrLf + " 	IF @DISPLAY = 'P'        "
            strSql += vbCrLf + " 	BEGIN        "
            strSql += vbCrLf + " 		DELETE FROM TEMPTABLEDB..TEMP_OUTSTANDING WHERE KNO NOT IN (      "
            strSql += vbCrLf + " 		SELECT KNO FROM TEMPTABLEDB..TEMP_OUTSTANDING AS O      "
            strSql += vbCrLf + " 		INNER JOIN      "
            strSql += vbCrLf + " 		(      "
            strSql += vbCrLf + " 		SELECT RUNNO,COSTID,COMPANYID FROM TEMPTABLEDB..TEMP_OUTSTANDING      "
            strSql += vbCrLf + " 		GROUP BY RUNNO,COSTID,COMPANYID        "
            strSql += vbCrLf + " 		HAVING SUM(ISNULL(DEBIT,0))-SUM(ISNULL(CREDIT,0))  <> 0      "
            strSql += vbCrLf + " 		)X ON X.RUNNO = O.RUNNO AND X.COSTID = O.COSTID AND X.COMPANYID = O.COMPANYID)      "
            strSql += vbCrLf + " 	END    "
            strSql += vbCrLf + " 	IF @CRPUR ='Y'     "
            strSql += vbCrLf + " 	BEGIN     "
            strSql += vbCrLf + " 		DELETE FROM TEMPTABLEDB..TEMP_OUTSTANDING WHERE KNO NOT IN (      "
            strSql += vbCrLf + " 		SELECT KNO FROM TEMPTABLEDB..TEMP_OUTSTANDING AS O      "
            strSql += vbCrLf + " 		INNER JOIN      "
            strSql += vbCrLf + " 		(      "
            strSql += vbCrLf + " 		SELECT RUNNO,COSTID,COMPANYID FROM TEMPTABLEDB..TEMP_OUTSTANDING      "
            strSql += vbCrLf + " 		GROUP BY RUNNO,COSTID,COMPANYID       "
            strSql += vbCrLf + " 		HAVING SUM(ISNULL(DEBIT,0))-SUM(ISNULL(CREDIT,0))  < 0      "
            strSql += vbCrLf + " 		)X ON X.RUNNO = O.RUNNO AND X.COSTID = O.COSTID AND X.COMPANYID = O.COMPANYID)      "
            strSql += vbCrLf + " 		UPDATE TEMPTABLEDB..TEMP_OUTSTANDING SET PAYMODE='DP'    "
            strSql += vbCrLf + " 	END    "
            strSql += vbCrLf + " 	ELSE IF @CRPUR ='B'     "
            strSql += vbCrLf + " 	BEGIN    "
            strSql += vbCrLf + " 		UPDATE TEMPTABLEDB..TEMP_OUTSTANDING SET PAYMODE='DP' WHERE KNO IN (      "
            strSql += vbCrLf + " 		SELECT KNO FROM TEMPTABLEDB..TEMP_OUTSTANDING AS O      "
            strSql += vbCrLf + " 		INNER JOIN      "
            strSql += vbCrLf + " 		(      "
            strSql += vbCrLf + " 		SELECT RUNNO,COSTID,COMPANYID FROM TEMPTABLEDB..TEMP_OUTSTANDING      "
            strSql += vbCrLf + " 		GROUP BY RUNNO,COSTID,COMPANYID      "
            strSql += vbCrLf + " 		HAVING SUM(ISNULL(DEBIT,0))-SUM(ISNULL(CREDIT,0))< 0      "
            strSql += vbCrLf + " 		)X ON X.RUNNO = O.RUNNO AND X.COSTID = O.COSTID AND X.COMPANYID = O.COMPANYID)      "
            strSql += vbCrLf + " 	END    "
            strSql += vbCrLf + " 	IF @FILTERBY <> 'NONE' AND @FILTERBY <> ''        "
            strSql += vbCrLf + " 	BEGIN        "
            strSql += vbCrLf + " 		SET @QRY = ' DELETE FROM TEMPTABLEDB..TEMP_OUTSTANDING WHERE '+@FILTERBY+' NOT LIKE ''%'+@FILTERCAPTION+'%'''      "
            strSql += vbCrLf + " 		PRINT  @QRY"
            strSql += vbCrLf + " 		EXEC(@QRY)        "
            strSql += vbCrLf + " 	END        "
            strSql += vbCrLf + " 	UPDATE TEMPTABLEDB..TEMP_OUTSTANDING SET TRANTYPE =         "
            strSql += vbCrLf + " 	CASE WHEN TRANTYPE = 'A' THEN 'ADVANCE'         "
            strSql += vbCrLf + " 	WHEN TRANTYPE = 'D' AND ISNULL(PAYMODE,'') ='DP' THEN 'CR PURCHASE'    "
            strSql += vbCrLf + " 	WHEN TRANTYPE = 'D' AND ISNULL(PAYMODE,'') <>'DP' THEN 'CREDIT'        "
            strSql += vbCrLf + " 	WHEN TRANTYPE = 'T' THEN 'OTHER'        "
            strSql += vbCrLf + " 	WHEN TRANTYPE = 'O' THEN 'ORDER'        "
            strSql += vbCrLf + " 	WHEN TRANTYPE = 'J' THEN 'JND'        "
            strSql += vbCrLf + " 	ELSE TRANTYPE END        "
            strSql += vbCrLf + " 	FROM TEMPTABLEDB..TEMP_OUTSTANDING    "


            strSql += vbCrLf + " 	IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP_OUTSTANDING)>0        "
            strSql += vbCrLf + " 	BEGIN       "
            strSql += vbCrLf + " 		IF @GRPCC ='Y'"
            strSql += vbCrLf + " 		BEGIN"
            strSql += vbCrLf + " 		/** Inserting Title **/     "
            strSql += vbCrLf + " 		INSERT INTO TEMPTABLEDB..TEMP_OUTSTANDING(TRANTYPE,COSTNAME,RESULT,AREAORD,COLHEAD)        "
            strSql += vbCrLf + " 		SELECT DISTINCT NULL AS TRANTYPE,COSTNAME,0 RESULT,0 AREAORD,'T' COLHEAD FROM TEMPTABLEDB..TEMP_OUTSTANDING WHERE RESULT = 2   "
            strSql += vbCrLf + " 		INSERT INTO TEMPTABLEDB..TEMP_OUTSTANDING(TRANTYPE,COSTNAME,RESULT,AREAORD,COLHEAD)        "
            strSql += vbCrLf + " 		SELECT DISTINCT TRANTYPE,COSTNAME,1 RESULT,0 AREAORD,'T' COLHEAD FROM TEMPTABLEDB..TEMP_OUTSTANDING WHERE RESULT = 2   "
            strSql += vbCrLf + " 			/** Inserting SubTotal **/        "
            strSql += vbCrLf + " 		INSERT INTO TEMPTABLEDB..TEMP_OUTSTANDING(TRANTYPE,COSTNAME,RESULT,AREAORD,COLHEAD,DEBIT,CREDIT,BALANCE,DEBIT_WT,CREDIT_WT,BALANCE_WT)"
            strSql += vbCrLf + " 		SELECT TRANTYPE,COSTNAME,3 RESULT,2 AREAORD,'S' COLHEAD,SUM(DEBIT),SUM(CREDIT),SUM(BALANCE),SUM(DEBIT_WT),SUM(CREDIT_WT),SUM(BALANCE_WT)"
            strSql += vbCrLf + " 		FROM TEMPTABLEDB..TEMP_OUTSTANDING WHERE RESULT = 2 GROUP BY TRANTYPE ,COSTNAME"
            strSql += vbCrLf + " 		/** Inserting SubTotal Difference**/          "
            strSql += vbCrLf + " 		INSERT INTO TEMPTABLEDB..TEMP_OUTSTANDING(TRANTYPE,COSTNAME,RESULT,AREAORD,COLHEAD,DEBIT,CREDIT,DEBIT_WT,CREDIT_WT)          "
            strSql += vbCrLf + " 		SELECT TRANTYPE + '->DIFF',COSTNAME,4 RESULT,3 AREAORD,'S2' COLHEAD,    "
            strSql += vbCrLf + " 		SUM(CASE WHEN ISNULL(DEBIT,0)>ISNULL(CREDIT,0) THEN ISNULL(DEBIT,0)-ISNULL(CREDIT,0) END)DEBIT,    "
            strSql += vbCrLf + " 		SUM(CASE WHEN ISNULL(DEBIT,0)<ISNULL(CREDIT,0) THEN ISNULL(CREDIT,0)-ISNULL(DEBIT,0) END)CREDIT,  "
            strSql += vbCrLf + "		SUM(CASE WHEN ISNULL(DEBIT_WT,0)>ISNULL(CREDIT_WT,0) THEN ISNULL(DEBIT_WT,0)-ISNULL(CREDIT_WT,0) END)DEBIT_WT,    "
            strSql += vbCrLf + " 		SUM(CASE WHEN ISNULL(DEBIT_WT,0)<ISNULL(CREDIT_WT,0) THEN ISNULL(CREDIT_WT,0)-ISNULL(DEBIT_WT,0) END)CREDIT_WT      "
            strSql += vbCrLf + " 		FROM TEMPTABLEDB..TEMP_OUTSTANDING WHERE RESULT = 3 GROUP BY TRANTYPE ,COSTNAME"
            strSql += vbCrLf + " 		END"
            strSql += vbCrLf + " 		IF @GRPCC ='N'"
            strSql += vbCrLf + " 		BEGIN"
            strSql += vbCrLf + " 				/** Inserting Title **/        "
            strSql += vbCrLf + " 			INSERT INTO TEMPTABLEDB..TEMP_OUTSTANDING(TRANTYPE,RESULT,AREAORD,COLHEAD)        "
            strSql += vbCrLf + " 			SELECT DISTINCT TRANTYPE,1 RESULT,0 AREAORD,'T' COLHEAD FROM TEMPTABLEDB..TEMP_OUTSTANDING WHERE RESULT = 2        "
            strSql += vbCrLf + " 			/** Inserting SubTotal **/        "
            strSql += vbCrLf + " 			INSERT INTO TEMPTABLEDB..TEMP_OUTSTANDING(TRANTYPE,RESULT,AREAORD,COLHEAD,DEBIT,CREDIT,BALANCE,DEBIT_WT,CREDIT_WT,BALANCE_WT)        "
            strSql += vbCrLf + " 			SELECT TRANTYPE,3 RESULT,3 AREAORD,'S' COLHEAD,SUM(DEBIT),SUM(CREDIT),SUM(BALANCE),SUM(DEBIT_WT),SUM(CREDIT_WT),SUM(BALANCE_WT)"
            strSql += vbCrLf + " 			FROM TEMPTABLEDB..TEMP_OUTSTANDING WHERE RESULT = 2 GROUP BY TRANTYPE         "
            strSql += vbCrLf + " 			/** Inserting SubTotal Difference**/          "
            strSql += vbCrLf + " 			INSERT INTO TEMPTABLEDB..TEMP_OUTSTANDING(TRANTYPE,RESULT,AREAORD,COLHEAD,DEBIT,CREDIT,DEBIT_WT,CREDIT_WT)          "
            strSql += vbCrLf + " 			SELECT TRANTYPE + '->DIFF',4 RESULT,3 AREAORD,'S2' COLHEAD,    "
            strSql += vbCrLf + " 			SUM(CASE WHEN ISNULL(DEBIT,0)>ISNULL(CREDIT,0) THEN ISNULL(DEBIT,0)-ISNULL(CREDIT,0) END)DEBIT,    "
            strSql += vbCrLf + " 			SUM(CASE WHEN ISNULL(DEBIT,0)<ISNULL(CREDIT,0) THEN ISNULL(CREDIT,0)-ISNULL(DEBIT,0) END)CREDIT,    "
            strSql += vbCrLf + "			SUM(CASE WHEN ISNULL(DEBIT_WT,0)>ISNULL(CREDIT_WT,0) THEN ISNULL(DEBIT_WT,0)-ISNULL(CREDIT_WT,0) END)DEBIT_WT,    "
            strSql += vbCrLf + " 			SUM(CASE WHEN ISNULL(DEBIT_WT,0)<ISNULL(CREDIT_WT,0) THEN ISNULL(CREDIT_WT,0)-ISNULL(DEBIT_WT,0) END)CREDIT_WT    "
            strSql += vbCrLf + " 			FROM TEMPTABLEDB..TEMP_OUTSTANDING WHERE RESULT = 3 GROUP BY TRANTYPE    "
            strSql += vbCrLf + " 		END"
            strSql += vbCrLf + " 		IF @GRPAREA = 'Y'    "
            strSql += vbCrLf + " 		BEGIN    "
            strSql += vbCrLf + " 			/** Inserting Title **/        "
            strSql += vbCrLf + " 			INSERT INTO TEMPTABLEDB..TEMP_OUTSTANDING(TRANTYPE,AREA,AREAORD,RESULT,COLHEAD)        "
            strSql += vbCrLf + " 			SELECT DISTINCT TRANTYPE,AREA,0 AREAORD,2 RESULT,'T1' COLHEAD FROM TEMPTABLEDB..TEMP_OUTSTANDING WHERE RESULT = 2   "
            strSql += vbCrLf + " 			 /** Inserting SubTotal **/        "
            strSql += vbCrLf + " 			INSERT INTO TEMPTABLEDB..TEMP_OUTSTANDING(TRANTYPE,AREA,AREAORD,RESULT,COLHEAD,DEBIT,CREDIT,BALANCE,DEBIT_WT,CREDIT_WT,BALANCE_WT)        "
            strSql += vbCrLf + " 			SELECT TRANTYPE,AREA,2 AREAORD,2 RESULT,'S1' COLHEAD,SUM(DEBIT),SUM(CREDIT),SUM(BALANCE),SUM(DEBIT_WT),SUM(CREDIT_WT),SUM(BALANCE_WT)     "
            strSql += vbCrLf + " 			FROM TEMPTABLEDB..TEMP_OUTSTANDING WHERE RESULT = 2 GROUP BY TRANTYPE,AREA "
            strSql += vbCrLf + " 		END    "
            strSql += vbCrLf + " 		/** Inserting Grand Total **/        "
            strSql += vbCrLf + " 		INSERT INTO TEMPTABLEDB..TEMP_OUTSTANDING(TRANTYPE,COSTNAME,RESULT,COLHEAD,DEBIT,CREDIT,BALANCE,DEBIT_WT,CREDIT_WT,BALANCE_WT)        "
            strSql += vbCrLf + " 		SELECT 'ZZZZZ','ZZZZZ',4 RESULT,'G' COLHEAD,SUM(DEBIT),SUM(CREDIT),SUM(BALANCE),SUM(DEBIT_WT),SUM(CREDIT_WT),SUM(BALANCE_WT)"
            strSql += vbCrLf + " 		FROM TEMPTABLEDB..TEMP_OUTSTANDING WHERE RESULT = 2        "
            strSql += vbCrLf + " 		/** Inserting Grand Total Difference**/        "
            strSql += vbCrLf + " 		INSERT INTO TEMPTABLEDB..TEMP_OUTSTANDING(TRANTYPE,COSTNAME,RESULT,COLHEAD,DEBIT,CREDIT,DEBIT_WT,CREDIT_WT)          "
            strSql += vbCrLf + " 		SELECT 'ZZZZZZ','ZZZZZZ',5 RESULT,'G1' COLHEAD,    "
            strSql += vbCrLf + " 		SUM(CASE WHEN ISNULL(DEBIT,0)>ISNULL(CREDIT,0) THEN ISNULL(DEBIT,0)-ISNULL(CREDIT,0) END)DEBIT,      "
            strSql += vbCrLf + " 		SUM(CASE WHEN ISNULL(DEBIT,0)<ISNULL(CREDIT,0) THEN ISNULL(CREDIT,0)-ISNULL(DEBIT,0) END)CREDIT,    "
            strSql += vbCrLf + "		SUM(CASE WHEN ISNULL(DEBIT_WT,0)>ISNULL(CREDIT_WT,0) THEN ISNULL(DEBIT_WT,0)-ISNULL(CREDIT_WT,0) END)DEBIT_WT,      "
            strSql += vbCrLf + " 		SUM(CASE WHEN ISNULL(DEBIT_WT,0)<ISNULL(CREDIT_WT,0) THEN ISNULL(CREDIT_WT,0)-ISNULL(DEBIT_WT,0) END)CREDIT_WT    "
            strSql += vbCrLf + " 		FROM TEMPTABLEDB..TEMP_OUTSTANDING WHERE RESULT = 4       "
            strSql += vbCrLf + " 	END        "
            strSql += vbCrLf + " 	DECLARE @SPACE VARCHAR(10)    "
            strSql += vbCrLf + " 	SET @SPACE = CASE WHEN @GRPAREA = 'Y' THEN ' ' ELSE '' END "
            strSql += vbCrLf + " 	IF @GRPCC ='Y'   "
            strSql += vbCrLf + " 	BEGIN"
            strSql += vbCrLf + " 		UPDATE TEMPTABLEDB..TEMP_OUTSTANDING SET PARTICULAR =     "
            strSql += vbCrLf + " 		CASE     "
            strSql += vbCrLf + " 		WHEN COLHEAD = 'T1' THEN @SPACE + AREA     "
            strSql += vbCrLf + " 		WHEN COLHEAD = 'S1' THEN @SPACE + ISNULL(AREA,'') + ' ->TOT'    "
            strSql += vbCrLf + " 		WHEN RESULT = 0 THEN COSTNAME    "
            strSql += vbCrLf + " 		WHEN RESULT = 1 THEN TRANTYPE   "
            strSql += vbCrLf + " 		WHEN RESULT = 2 THEN @SPACE + '  ' + SUBSTRING(RUNNO,6,20)     "
            strSql += vbCrLf + " 		WHEN RESULT = 3 THEN ISNULL(TRANTYPE,'') + ' ->TOT'    "
            strSql += vbCrLf + " 		WHEN RESULT = 4 THEN 'DIFF'    "
            strSql += vbCrLf + " 		WHEN RESULT = 6 THEN 'GRAND DIFF'    "
            strSql += vbCrLf + " 		ELSE 'GRAND TOTAL' END  "
            strSql += vbCrLf + " 	END"
            strSql += vbCrLf + " 	IF @GRPCC='N'"
            strSql += vbCrLf + " 	BEGIN"
            strSql += vbCrLf + " 		UPDATE TEMPTABLEDB..TEMP_OUTSTANDING SET PARTICULAR =     "
            strSql += vbCrLf + " 		CASE     "
            strSql += vbCrLf + " 		WHEN COLHEAD = 'T1' THEN @SPACE + AREA     "
            strSql += vbCrLf + " 		WHEN COLHEAD = 'S1' THEN @SPACE + ISNULL(AREA,'') + ' ->TOT'    "
            strSql += vbCrLf + " 		WHEN RESULT = 1 THEN TRANTYPE   "
            strSql += vbCrLf + " 		WHEN RESULT = 2 THEN @SPACE + '  ' + SUBSTRING(RUNNO,6,20)     "
            strSql += vbCrLf + " 		WHEN RESULT = 3 THEN ISNULL(TRANTYPE,'') + ' ->TOT'    "
            strSql += vbCrLf + " 		WHEN RESULT = 4 THEN 'DIFF'    "
            strSql += vbCrLf + " 		WHEN RESULT = 6 THEN 'GRAND DIFF'    "
            strSql += vbCrLf + " 		ELSE 'GRAND TOTAL' END  "
            strSql += vbCrLf + " 	END	      "
            strSql += vbCrLf + " 	UPDATE TEMPTABLEDB..TEMP_OUTSTANDING SET DEBIT = CASE WHEN ISNULL(DEBIT,0) <> 0 THEN DEBIT ELSE NULL END        "
            strSql += vbCrLf + " 	,CREDIT = CASE WHEN ISNULL(cREDIT,0) <> 0 THEN CREDIT ELSE NULL END        "
            strSql += vbCrLf + " 	,BALANCE = CASE WHEN ISNULL(BALANCE,0) <> 0 THEN BALANCE ELSE NULL END      "
            strSql += vbCrLf + "	UPDATE TEMPTABLEDB..TEMP_OUTSTANDING SET DEBIT_WT = CASE WHEN ISNULL(DEBIT_WT,0) <> 0 THEN DEBIT_WT ELSE NULL END        "
            strSql += vbCrLf + " 	,CREDIT_WT = CASE WHEN ISNULL(CREDIT_WT,0) <> 0 THEN CREDIT_WT ELSE NULL END        "
            strSql += vbCrLf + " 	,BALANCE_WT = CASE WHEN ISNULL(BALANCE_WT,0) <> 0 THEN BALANCE_WT ELSE NULL END      "
            strSql += vbCrLf + " 	IF @GRPCC ='Y' "
            strSql += vbCrLf + " 	BEGIN"
            strSql += vbCrLf + " 		IF @ORDERBY = 'N'    "
            strSql += vbCrLf + " 			SELECT * FROM TEMPTABLEDB..TEMP_OUTSTANDING ORDER BY COSTNAME,TRANTYPE,RESULT,NAME    "
            strSql += vbCrLf + " 		ELSE IF @ORDERBY = 'B'    "
            strSql += vbCrLf + " 			SELECT * FROM TEMPTABLEDB..TEMP_OUTSTANDING ORDER BY COSTNAME,TRANTYPE,RESULT,TRANDATE    "
            strSql += vbCrLf + " 		ELSE   "
            strSql += vbCrLf + " 			SELECT * FROM TEMPTABLEDB..TEMP_OUTSTANDING ORDER BY COSTNAME ,TRANTYPE,RESULT,TRANNO "
            strSql += vbCrLf + " 	END"
            strSql += vbCrLf + " 	IF @GRPCC ='N' "
            strSql += vbCrLf + " 	BEGIN"
            strSql += vbCrLf + " 		IF @ORDERBY = 'N'    "
            strSql += vbCrLf + " 			SELECT * FROM TEMPTABLEDB..TEMP_OUTSTANDING ORDER BY TRANTYPE,RESULT,NAME    "
            strSql += vbCrLf + " 		ELSE IF @ORDERBY = 'B'    "
            strSql += vbCrLf + " 			SELECT * FROM TEMPTABLEDB..TEMP_OUTSTANDING ORDER BY TRANTYPE,RESULT,TRANDATE    "
            strSql += vbCrLf + " 		ELSE   "
            strSql += vbCrLf + " 			SELECT * FROM TEMPTABLEDB..TEMP_OUTSTANDING ORDER BY TRANTYPE,RESULT,TRANNO "
            strSql += vbCrLf + " 	END"
            strSql += vbCrLf + " END"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
        End If


        strSql = " EXEC " & dbName & "..REM_RPT_CUSTOMEROUTSTANDING_MONTH"
        strSql += vbCrLf + " @GRPCC='N',"
        strSql += vbCrLf + " @FROMDATE = '1900-01-01'"
        strSql += vbCrLf + " ,@TODATE = '" & Date.Now.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@DISPLAY = 'A'"
        strSql += vbCrLf + " ,@SUMMARY = 'Y'"
        strSql += vbCrLf + " ,@ORDERBY = 'R'"
        strSql += vbCrLf + " ,@FILTERBY = 'NONE'"
        strSql += vbCrLf + " ,@FILTERCAPTION = ''"
        strSql += vbCrLf + " ,@COSTNAME = ''"
        strSql += vbCrLf + " ,@COMPANYID = '" & dbName.Replace("ADMINDB", "") & "'"
        strSql += vbCrLf + " ,@TYPEID = 'D,'"
        strSql += vbCrLf + " ,@CRPUR = 'N'"
        strSql += vbCrLf + " ,@ACCODE='DRS'"
        strSql += vbCrLf + " ,@GRPAREA = 'N'"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        strSql = " SELECT "
        strSql += vbCrLf + " ROW_NUMBER() OVER(ORDER BY TRANTYPE,RESULT,TRANNO ) SNO,DUEDATE,TRANDATE,TRANNO,NAME,MOBILE AS MOBILENO,"
        strSql += vbCrLf + " SUBSTRING(RUNNO,6,10)RUNNO,BALANCE AS AMOUNT,RUNNO AS RUNNO1,1 AS RESULT "
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP_OUTSTANDING WHERE RESULT=2 AND ISNULL(BALANCE,0)<>0 ORDER BY TRANDATE DESC, TRANNO  DESC "
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        Return dt
    End Function
    Function OutstandingSummary_Today() As DataTable
        Dim dt As New DataTable

        strSql = " SELECT 1 FROM SYSOBJECTS WHERE NAME='REM_RPT_CUSTOMEROUTSTANDING_TODAY'"
        If Val(GetSqlValue(strSql).ToString()) = 0 Then
            strSql = " IF (SELECT 1 FROM SYSOBJECTS WHERE NAME='REM_RPT_CUSTOMEROUTSTANDING_TODAY')>0"
            strSql += vbCrLf + "  	DROP PROCEDURE REM_RPT_CUSTOMEROUTSTANDING_TODAY"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
            strSql = vbCrLf + "CREATE PROCEDURE REM_RPT_CUSTOMEROUTSTANDING_TODAY"
            strSql += vbCrLf + " (        "
            strSql += vbCrLf + " @FROMDATE VARCHAR(12)        "
            strSql += vbCrLf + " ,@TODATE VARCHAR(12)    "
            strSql += vbCrLf + " ,@DISPLAY VARCHAR(1)        "
            strSql += vbCrLf + " ,@SUMMARY VARCHAR(1)        "
            strSql += vbCrLf + " ,@ORDERBY VARCHAR(1)        "
            strSql += vbCrLf + " ,@FILTERBY VARCHAR(50)        "
            strSql += vbCrLf + " ,@FILTERCAPTION VARCHAR(50)        "
            strSql += vbCrLf + " ,@COSTNAME VARCHAR(50)        "
            strSql += vbCrLf + " ,@COMPANYID VARCHAR(5000)        "
            strSql += vbCrLf + " ,@TYPEID VARCHAR(50)    "
            strSql += vbCrLf + " ,@CRPUR VARCHAR(1)        "
            strSql += vbCrLf + " ,@ACCODE VARCHAR(25)        "
            strSql += vbCrLf + " ,@GRPAREA VARCHAR(1)"
            strSql += vbCrLf + " ,@GRPCC VARCHAR(1)    "
            strSql += vbCrLf + " )        "
            strSql += vbCrLf + " AS        "
            strSql += vbCrLf + " BEGIN      "
            strSql += vbCrLf + " 	DECLARE @QRY VARCHAR(4000)    "
            strSql += vbCrLf + " 	DECLARE @TYPE VARCHAR(50)"
            strSql += vbCrLf + " 	SET @TYPE = @TYPEID"
            strSql += vbCrLf + " 	SET @TYPEID = REPLACE(@TYPEID,'DP','D')    "
            strSql += vbCrLf + " 	SET @ACCODE= REPLACE(@ACCODE,',',''',''')    "
            strSql += vbCrLf + " 	IF OBJECT_ID('TEMPTABLEDB..COSTCENTRE', 'U') IS NOT NULL DROP TABLE TEMPTABLEDB..COSTCENTRE          "
            strSql += vbCrLf + " 	CREATE TABLE TEMPTABLEDB..COSTCENTRE(COSTID VARCHAR(3))        "
            strSql += vbCrLf + " 	IF @COSTNAME = 'ALL' OR @COSTNAME = ''        "
            strSql += vbCrLf + " 	INSERT INTO TEMPTABLEDB..COSTCENTRE SELECT COSTID FROM COSTCENTRE UNION ALL SELECT ''        "
            strSql += vbCrLf + " 	ELSE        "
            strSql += vbCrLf + " 	INSERT INTO TEMPTABLEDB..COSTCENTRE SELECT COSTID FROM COSTCENTRE WHERE COSTNAME = @COSTNAME        "
            strSql += vbCrLf + " 	IF OBJECT_ID('TEMPTABLEDB..COMPANY', 'U') IS NOT NULL DROP TABLE  TEMPTABLEDB..COMPANY          "
            strSql += vbCrLf + " 	CREATE TABLE TEMPTABLEDB..COMPANY (COMPANYID VARCHAR(3))          "
            strSql += vbCrLf + " 	DECLARE @TCOMPANY VARCHAR(50)          "
            strSql += vbCrLf + " 	IF CHARINDEX(',',@COMPANYID)=0          "
            strSql += vbCrLf + " 	INSERT INTO TEMPTABLEDB..COMPANY (COMPANYID) SELECT @COMPANYID          "
            strSql += vbCrLf + " 	WHILE CHARINDEX(',',@COMPANYID)>0          "
            strSql += vbCrLf + " 	BEGIN          "
            strSql += vbCrLf + " 	SET @TCOMPANY=LEFT(@COMPANYID,CHARINDEX(',',@COMPANYID)-1)          "
            strSql += vbCrLf + " 	SET @COMPANYID=RIGHT(@COMPANYID,LEN(@COMPANYID)-CHARINDEX(',',@COMPANYID))          "
            strSql += vbCrLf + " 	IF @COMPANYID <>'' AND CHARINDEX(',',@COMPANYID)=0          "
            strSql += vbCrLf + " 	SET @COMPANYID= @COMPANYID + ','          "
            strSql += vbCrLf + " 	INSERT INTO TEMPTABLEDB..COMPANY (COMPANYID) SELECT @TCOMPANY          "
            strSql += vbCrLf + " 	END          "
            strSql += vbCrLf + " 	IF OBJECT_ID('TEMPTABLEDB..TYPE', 'U') IS NOT NULL DROP TABLE TEMPTABLEDB..TYPE         "
            strSql += vbCrLf + " 	CREATE TABLE TEMPTABLEDB..TYPE (TYPEID VARCHAR(3))            "
            strSql += vbCrLf + " 	DECLARE @TEMPID VARCHAR(5)            "
            strSql += vbCrLf + " 	IF CHARINDEX(',',@TYPEID)=0            "
            strSql += vbCrLf + " 	INSERT INTO TEMPTABLEDB..TYPE (TYPEID) SELECT @TYPEID            "
            strSql += vbCrLf + " 	WHILE CHARINDEX(',',@TYPEID)>0            "
            strSql += vbCrLf + " 	BEGIN            "
            strSql += vbCrLf + " 	SET @TEMPID=LEFT(@TYPEID,CHARINDEX(',',@TYPEID)-1)            "
            strSql += vbCrLf + " 	SET @TYPEID=RIGHT(@TYPEID,LEN(@TYPEID)-CHARINDEX(',',@TYPEID))            "
            strSql += vbCrLf + " 	IF @TYPEID <>'' AND CHARINDEX(',',@TYPEID)=0            "
            strSql += vbCrLf + " 	SET @TYPEID= @TYPEID + ','            "
            strSql += vbCrLf + " 	INSERT INTO TEMPTABLEDB..TYPE (TYPEID) SELECT @TEMPID            "
            strSql += vbCrLf + " 	END            "
            strSql += vbCrLf + " 	IF (SELECT 1 FROM TEMPTABLEDB..SYSOBJECTS WHERE NAME='TEMP_OUTSTANDING') > 0        "
            strSql += vbCrLf + " 	DROP TABLE TEMPTABLEDB..TEMP_OUTSTANDING        "
            strSql += vbCrLf + " 	PRINT @TYPEID "
            strSql += vbCrLf + " 	IF @SUMMARY = 'N'        "
            strSql += vbCrLf + " 	BEGIN        "
            strSql += vbCrLf + " 		SELECT @QRY = 'SELECT CONVERT(VARCHAR(200),NULL)PARTICULAR,O.RUNNO,CONVERT(VARCHAR(50),O.TRANTYPE)TRANTYPE,O.TRANNO,O.TRANDATE,O.DUEDATE'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,CASE WHEN RECPAY = ''P'' THEN AMOUNT ELSE 0 END AS DEBIT'        "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,CASE WHEN RECPAY = ''R'' THEN AMOUNT ELSE 0 END AS CREDIT'    "
            strSql += vbCrLf + "		SELECT @QRY = @QRY + CHAR(13)+' ,CONVERT(NUMERIC(15,2),NULL)BALANCE'"
            strSql += vbCrLf + "		SELECT @QRY = @QRY + CHAR(13)+' ,CASE WHEN RECPAY = ''P'' THEN GRSWT ELSE 0 END AS DEBIT_WT'        "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,CASE WHEN RECPAY = ''R'' THEN GRSWT ELSE 0 END AS CREDIT_WT'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,CONVERT(NUMERIC(15,2),NULL)BALANCE_WT,P.PNAME NAME'"
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,CASE WHEN ISNULL(P.MOBILE,'''')='''' THEN P.PHONERES ELSE P.MOBILE END AS MOBILE'"
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,P.ADDRESS1,P.ADDRESS2,P.AREA,P.CITY'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,(SELECT EMPNAME FROM EMPMASTER WHERE EMPID = O.EMPID)AS SALESPERSON'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,(SELECT COSTNAME FROM COSTCENTRE WHERE COSTID = O.COSTID)AS COSTNAME'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,CONVERT(INT,2)RESULT,CONVERT(VARCHAR,NULL)COLHEAD,CONVERT(INT,1)AREAORD,O.PAYMODE'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,O.COSTID,O.COMPANYID,CONVERT(VARCHAR(12),NULL)LASTTRANDATE'"
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,O.REMARK1,O.REMARK2'"
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,IDENTITY(INT,1,1)KNO'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' INTO TEMPTABLEDB..TEMP_OUTSTANDING'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' FROM  OUTSTANDING AS O'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' INNER JOIN CUSTOMERINFO AS CU ON CU.BATCHNO = O.BATCHNO'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' INNER JOIN PERSONALINFO AS P ON P.SNO = CU.PSNO'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' WHERE O.TRANDATE BETWEEN '''+@FROMDATE+''' AND '''+@TODATE+''''    "
            strSql += vbCrLf + " 		IF @COSTNAME <> 'ALL' AND @COSTNAME <> '' SELECT @QRY = @QRY + CHAR(13)+' AND ISNULL(O.COSTID,'''') IN (SELECT COSTID FROM TEMPTABLEDB..COSTCENTRE)'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' AND ISNULL(O.CANCEL,'''') = ''''  AND ISNULL(O.COMPANYID,'''') IN (SELECT COMPANYID FROM TEMPTABLEDB..COMPANY)'    "
            strSql += vbCrLf + " 		IF @ACCODE <> ''      "
            strSql += vbCrLf + " 		BEGIN"
            strSql += vbCrLf + " 			IF @TYPE='D,'"
            strSql += vbCrLf + " 				SELECT @QRY = @QRY + CHAR(13)+' AND O.FROMFLAG=''P'''"
            strSql += vbCrLf + " 			ELSE"
            strSql += vbCrLf + " 				SELECT @QRY = @QRY + CHAR(13)+' AND O.ACCODE IN('''+@ACCODE+''')'"
            strSql += vbCrLf + " 		END"
            strSql += vbCrLf + " 		ELSE"
            strSql += vbCrLf + " 		BEGIN"
            strSql += vbCrLf + " 			SELECT @QRY = @QRY + CHAR(13)+' AND O.ACCODE IN(SELECT ACCODE FROM ACHEAD WHERE ACTYPE=''C'')'"
            strSql += vbCrLf + " 		END"
            strSql += vbCrLf + " 		/*IF @CRPUR ='N' SELECT @QRY = @QRY + CHAR(13)+' AND O.PAYMODE<>''DP'''    "
            strSql += vbCrLf + " 		IF @CRPUR ='Y' SELECT @QRY = @QRY + CHAR(13)+'AND O.PAYMODE=''DP'' ' */      "
            strSql += vbCrLf + " 		PRINT  @QRY"
            strSql += vbCrLf + " 		EXEC (@QRY)    "
            strSql += vbCrLf + " 	END        "
            strSql += vbCrLf + " 	ELSE IF @SUMMARY = 'Y'        "
            strSql += vbCrLf + " 	BEGIN      "
            strSql += vbCrLf + " 		SELECT @QRY = ' SELECT PARTICULAR,RUNNO,TRANTYPE,TRANNO,CONVERT(VARCHAR,TRANDATE,105)TRANDATE1,CONVERT(VARCHAR,DUEDATE,105)DUEDATE,TRANDATE,DEBIT,CREDIT,BALANCE'  "
            strSql += vbCrLf + "		SELECT @QRY = @QRY + CHAR(13)+' ,DEBIT_WT,CREDIT_WT,BALANCE_WT'"
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,NAME,MOBILE,ADDRESS1,ADDRESS2,AREA,CITY'"
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,PAYMODE,SALESPERSON,COSTNAME, '  "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' AREAORD,COSTID,COMPANYID,RESULT,COLHEAD'"
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,CONVERT(VARCHAR,(SELECT TOP 1 TRANDATE FROM OUTSTANDING WHERE RUNNO = X.RUNNO AND COSTID = X.COSTID AND COMPANYID = X.COMPANYID '  "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ORDER BY TRANDATE DESC),103)AS LASTTRANDATE '    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,REMARK1,REMARK2'"
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,IDENTITY(INT,1,1)KNO'"
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' INTO TEMPTABLEDB..TEMP_OUTSTANDING '    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' FROM'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ('      "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' SELECT CONVERT(VARCHAR(200),NULL)PARTICULAR,O.RUNNO,CONVERT(VARCHAR(50),O.TRANTYPE)TRANTYPE'  "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,(SELECT TOP 1 TRANNO FROM OUTSTANDING WHERE RUNNO = O.RUNNO AND COSTID = O.COSTID AND COMPANYID = O.COMPANYID ORDER BY TRANDATE,TRANNO) TRANNO'  "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,(SELECT TOP 1 TRANDATE FROM OUTSTANDING WHERE RUNNO = O.RUNNO AND COSTID = O.COSTID AND COMPANYID = O.COMPANYID ORDER BY TRANDATE,TRANNO) TRANDATE'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,(SELECT TOP 1 DUEDATE FROM OUTSTANDING WHERE RUNNO = O.RUNNO AND COSTID = O.COSTID AND COMPANYID = O.COMPANYID ORDER BY TRANDATE,TRANNO) DUEDATE'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,SUM(CASE WHEN RECPAY = ''P'' THEN AMOUNT ELSE 0 END) AS DEBIT'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,SUM(CASE WHEN RECPAY = ''R'' THEN AMOUNT ELSE 0 END) AS CREDIT'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,CASE WHEN TRANTYPE=''D'' THEN SUM(CASE WHEN RECPAY = ''P'' THEN AMOUNT ELSE -1*AMOUNT END) '"
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ELSE SUM(CASE WHEN RECPAY = ''R'' THEN AMOUNT ELSE -1*AMOUNT END) END AS BALANCE'    "
            strSql += vbCrLf + "		SELECT @QRY = @QRY + CHAR(13)+' ,SUM(CASE WHEN RECPAY = ''P'' THEN GRSWT ELSE 0 END) AS DEBIT_WT'        "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,SUM(CASE WHEN RECPAY = ''R'' THEN GRSWT ELSE 0 END) AS CREDIT_WT'    "
            strSql += vbCrLf + "		SELECT @QRY = @QRY + CHAR(13)+' ,CASE WHEN TRANTYPE=''D'' THEN SUM(CASE WHEN RECPAY = ''P'' THEN GRSWT ELSE -1*GRSWT END) '"
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ELSE SUM(CASE WHEN RECPAY = ''R'' THEN GRSWT ELSE -1*GRSWT END) END AS BALANCE_WT'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,P.PNAME NAME'"
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,CASE WHEN ISNULL(P.MOBILE,'''')='''' THEN P.PHONERES ELSE P.MOBILE END AS MOBILE'"
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,P.AREA,P.ADDRESS1,P.ADDRESS2,P.CITY,CONVERT(VARCHAR(10),NULL) PAYMODE'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,CONVERT(VARCHAR(40),NULL) SALESPERSON'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,(SELECT COSTNAME FROM COSTCENTRE WHERE COSTID = O.COSTID)AS COSTNAME'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,CONVERT(INT,2)RESULT,CONVERT(VARCHAR,NULL)COLHEAD,CONVERT(INT,1)AREAORD'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,O.COSTID,O.COMPANYID'  "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,(SELECT TOP 1 REMARK1 FROM OUTSTANDING WHERE RUNNO = O.RUNNO AND COSTID = O.COSTID AND COMPANYID = O.COMPANYID ORDER BY TRANDATE,TRANNO) REMARK1'  "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,(SELECT TOP 1 REMARK2 FROM OUTSTANDING WHERE RUNNO = O.RUNNO AND COSTID = O.COSTID AND COMPANYID = O.COMPANYID ORDER BY TRANDATE,TRANNO) REMARK2'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' FROM OUTSTANDING AS O'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' INNER JOIN CUSTOMERINFO AS CU ON CU.BATCHNO = '"
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' (SELECT TOP 1 BATCHNO FROM OUTSTANDING WHERE RUNNO = O.RUNNO AND COSTID = O.COSTID AND COMPANYID = O.COMPANYID ORDER BY TRANDATE DESC)'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' INNER JOIN PERSONALINFO AS P ON P.SNO = CU.PSNO'        "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' WHERE O.TRANDATE BETWEEN '''+@FROMDATE+''' AND '''+@TODATE+''' AND O.DUEDATE ='''+@TODATE+''' '        "
            strSql += vbCrLf + " 		IF @COSTNAME <> 'ALL' AND @COSTNAME <> '' SELECT @QRY = @QRY + CHAR(13)+' AND ISNULL(O.COSTID,'''') IN (SELECT COSTID FROM TEMPTABLEDB..COSTCENTRE)'        "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' AND ISNULL(O.CANCEL,'''') = '''''        "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' AND ISNULL(O.COMPANYID,'''') IN (SELECT COMPANYID FROM TEMPTABLEDB..COMPANY)'      "
            strSql += vbCrLf + " 		IF @ACCODE <> ''      "
            strSql += vbCrLf + " 		BEGIN"
            strSql += vbCrLf + " 			IF @TYPE='D,'"
            strSql += vbCrLf + " 				SELECT @QRY = @QRY + CHAR(13)+' AND O.FROMFLAG=''P''  '"
            strSql += vbCrLf + " 			ELSE"
            strSql += vbCrLf + " 				SELECT @QRY = @QRY + CHAR(13)+' AND O.ACCODE IN('''+@ACCODE+''')'"
            strSql += vbCrLf + " 		END"
            strSql += vbCrLf + " 		ELSE"
            strSql += vbCrLf + " 		BEGIN"
            strSql += vbCrLf + " 			SELECT @QRY = @QRY + CHAR(13)+' AND O.ACCODE IN(SELECT ACCODE FROM ACHEAD WHERE ACTYPE=''C'')'"
            strSql += vbCrLf + " 		END"
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' GROUP BY O.RUNNO,O.TRANTYPE,P.PNAME,P.MOBILE,P.PHONERES'"
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' ,O.COSTID,O.COMPANYID,P.AREA,P.ADDRESS1,P.ADDRESS2,P.CITY'    "
            strSql += vbCrLf + " 		SELECT @QRY = @QRY + CHAR(13)+' )X'    "
            strSql += vbCrLf + " 		PRINT @QRY    "
            strSql += vbCrLf + " 		EXEC (@QRY)   "
            strSql += vbCrLf + " 	END"
            strSql += vbCrLf + " 	IF @TYPE='D,'"
            strSql += vbCrLf + " 		DELETE FROM TEMPTABLEDB..TEMP_OUTSTANDING WHERE RUNNO IN (SELECT RUNNO FROM  OUTSTANDING WHERE RECPAY='R' AND PAYMODE IN('DU','DP')) 	"
            strSql += vbCrLf + "	UPDATE TEMPTABLEDB..TEMP_OUTSTANDING SET PAYMODE = 'DP'"
            strSql += vbCrLf + "	WHERE RUNNO IN (SELECT RUNNO FROM  OUTSTANDING WHERE RECPAY='R' AND PAYMODE='DU') "
            strSql += vbCrLf + " 	UPDATE TEMPTABLEDB..TEMP_OUTSTANDING SET TRANTYPE = 'J'        "
            strSql += vbCrLf + " 	WHERE RUNNO IN (SELECT RUNNO FROM  OUTSTANDING O WHERE EXISTS (SELECT 1 FROM  ITEMDETAIL WHERE BATCHNO = O.BATCHNO AND RUNNO=O.RUNNO))        "
            strSql += vbCrLf + " 	UPDATE TEMPTABLEDB..TEMP_OUTSTANDING SET TRANTYPE = 'O' WHERE SUBSTRING(RUNNO,6,1) = 'O'        "
            strSql += vbCrLf + " 	DELETE FROM TEMPTABLEDB..TEMP_OUTSTANDING WHERE TRANTYPE NOT IN (SELECT TYPEID FROM TEMPTABLEDB..TYPE)        "
            strSql += vbCrLf + " 	IF @DISPLAY = 'C'        "
            strSql += vbCrLf + " 	BEGIN        "
            strSql += vbCrLf + " 		DELETE FROM TEMPTABLEDB..TEMP_OUTSTANDING WHERE KNO NOT IN (      "
            strSql += vbCrLf + " 		SELECT KNO FROM TEMPTABLEDB..TEMP_OUTSTANDING AS O      "
            strSql += vbCrLf + " 		INNER JOIN      "
            strSql += vbCrLf + " 		(      "
            strSql += vbCrLf + " 		SELECT RUNNO,COSTID,COMPANYID FROM TEMPTABLEDB..TEMP_OUTSTANDING      "
            strSql += vbCrLf + " 		GROUP BY RUNNO,COSTID,COMPANYID        "
            strSql += vbCrLf + " 		HAVING SUM(ISNULL(DEBIT,0))-SUM(ISNULL(CREDIT,0))  = 0      "
            strSql += vbCrLf + " 		)X ON X.RUNNO = O.RUNNO AND X.COSTID = O.COSTID AND X.COMPANYID = O.COMPANYID)      "
            strSql += vbCrLf + " 	END        "
            strSql += vbCrLf + " 	IF @DISPLAY = 'P'        "
            strSql += vbCrLf + " 	BEGIN        "
            strSql += vbCrLf + " 		DELETE FROM TEMPTABLEDB..TEMP_OUTSTANDING WHERE KNO NOT IN (      "
            strSql += vbCrLf + " 		SELECT KNO FROM TEMPTABLEDB..TEMP_OUTSTANDING AS O      "
            strSql += vbCrLf + " 		INNER JOIN      "
            strSql += vbCrLf + " 		(      "
            strSql += vbCrLf + " 		SELECT RUNNO,COSTID,COMPANYID FROM TEMPTABLEDB..TEMP_OUTSTANDING      "
            strSql += vbCrLf + " 		GROUP BY RUNNO,COSTID,COMPANYID        "
            strSql += vbCrLf + " 		HAVING SUM(ISNULL(DEBIT,0))-SUM(ISNULL(CREDIT,0))  <> 0      "
            strSql += vbCrLf + " 		)X ON X.RUNNO = O.RUNNO AND X.COSTID = O.COSTID AND X.COMPANYID = O.COMPANYID)      "
            strSql += vbCrLf + " 	END    "
            strSql += vbCrLf + " 	IF @CRPUR ='Y'     "
            strSql += vbCrLf + " 	BEGIN     "
            strSql += vbCrLf + " 		DELETE FROM TEMPTABLEDB..TEMP_OUTSTANDING WHERE KNO NOT IN (      "
            strSql += vbCrLf + " 		SELECT KNO FROM TEMPTABLEDB..TEMP_OUTSTANDING AS O      "
            strSql += vbCrLf + " 		INNER JOIN      "
            strSql += vbCrLf + " 		(      "
            strSql += vbCrLf + " 		SELECT RUNNO,COSTID,COMPANYID FROM TEMPTABLEDB..TEMP_OUTSTANDING      "
            strSql += vbCrLf + " 		GROUP BY RUNNO,COSTID,COMPANYID       "
            strSql += vbCrLf + " 		HAVING SUM(ISNULL(DEBIT,0))-SUM(ISNULL(CREDIT,0))  < 0      "
            strSql += vbCrLf + " 		)X ON X.RUNNO = O.RUNNO AND X.COSTID = O.COSTID AND X.COMPANYID = O.COMPANYID)      "
            strSql += vbCrLf + " 		UPDATE TEMPTABLEDB..TEMP_OUTSTANDING SET PAYMODE='DP'    "
            strSql += vbCrLf + " 	END    "
            strSql += vbCrLf + " 	ELSE IF @CRPUR ='B'     "
            strSql += vbCrLf + " 	BEGIN    "
            strSql += vbCrLf + " 		UPDATE TEMPTABLEDB..TEMP_OUTSTANDING SET PAYMODE='DP' WHERE KNO IN (      "
            strSql += vbCrLf + " 		SELECT KNO FROM TEMPTABLEDB..TEMP_OUTSTANDING AS O      "
            strSql += vbCrLf + " 		INNER JOIN      "
            strSql += vbCrLf + " 		(      "
            strSql += vbCrLf + " 		SELECT RUNNO,COSTID,COMPANYID FROM TEMPTABLEDB..TEMP_OUTSTANDING      "
            strSql += vbCrLf + " 		GROUP BY RUNNO,COSTID,COMPANYID      "
            strSql += vbCrLf + " 		HAVING SUM(ISNULL(DEBIT,0))-SUM(ISNULL(CREDIT,0))< 0      "
            strSql += vbCrLf + " 		)X ON X.RUNNO = O.RUNNO AND X.COSTID = O.COSTID AND X.COMPANYID = O.COMPANYID)      "
            strSql += vbCrLf + " 	END    "
            strSql += vbCrLf + " 	IF @FILTERBY <> 'NONE' AND @FILTERBY <> ''        "
            strSql += vbCrLf + " 	BEGIN        "
            strSql += vbCrLf + " 		SET @QRY = ' DELETE FROM TEMPTABLEDB..TEMP_OUTSTANDING WHERE '+@FILTERBY+' NOT LIKE ''%'+@FILTERCAPTION+'%'''      "
            strSql += vbCrLf + " 		PRINT  @QRY"
            strSql += vbCrLf + " 		EXEC(@QRY)        "
            strSql += vbCrLf + " 	END        "
            strSql += vbCrLf + " 	UPDATE TEMPTABLEDB..TEMP_OUTSTANDING SET TRANTYPE =         "
            strSql += vbCrLf + " 	CASE WHEN TRANTYPE = 'A' THEN 'ADVANCE'         "
            strSql += vbCrLf + " 	WHEN TRANTYPE = 'D' AND ISNULL(PAYMODE,'') ='DP' THEN 'CR PURCHASE'    "
            strSql += vbCrLf + " 	WHEN TRANTYPE = 'D' AND ISNULL(PAYMODE,'') <>'DP' THEN 'CREDIT'        "
            strSql += vbCrLf + " 	WHEN TRANTYPE = 'T' THEN 'OTHER'        "
            strSql += vbCrLf + " 	WHEN TRANTYPE = 'O' THEN 'ORDER'        "
            strSql += vbCrLf + " 	WHEN TRANTYPE = 'J' THEN 'JND'        "
            strSql += vbCrLf + " 	ELSE TRANTYPE END        "
            strSql += vbCrLf + " 	FROM TEMPTABLEDB..TEMP_OUTSTANDING    "


            strSql += vbCrLf + " 	IF (SELECT COUNT(*) FROM TEMPTABLEDB..TEMP_OUTSTANDING)>0        "
            strSql += vbCrLf + " 	BEGIN       "
            strSql += vbCrLf + " 		IF @GRPCC ='Y'"
            strSql += vbCrLf + " 		BEGIN"
            strSql += vbCrLf + " 		/** Inserting Title **/     "
            strSql += vbCrLf + " 		INSERT INTO TEMPTABLEDB..TEMP_OUTSTANDING(TRANTYPE,COSTNAME,RESULT,AREAORD,COLHEAD)        "
            strSql += vbCrLf + " 		SELECT DISTINCT NULL AS TRANTYPE,COSTNAME,0 RESULT,0 AREAORD,'T' COLHEAD FROM TEMPTABLEDB..TEMP_OUTSTANDING WHERE RESULT = 2   "
            strSql += vbCrLf + " 		INSERT INTO TEMPTABLEDB..TEMP_OUTSTANDING(TRANTYPE,COSTNAME,RESULT,AREAORD,COLHEAD)        "
            strSql += vbCrLf + " 		SELECT DISTINCT TRANTYPE,COSTNAME,1 RESULT,0 AREAORD,'T' COLHEAD FROM TEMPTABLEDB..TEMP_OUTSTANDING WHERE RESULT = 2   "
            strSql += vbCrLf + " 			/** Inserting SubTotal **/        "
            strSql += vbCrLf + " 		INSERT INTO TEMPTABLEDB..TEMP_OUTSTANDING(TRANTYPE,COSTNAME,RESULT,AREAORD,COLHEAD,DEBIT,CREDIT,BALANCE,DEBIT_WT,CREDIT_WT,BALANCE_WT)"
            strSql += vbCrLf + " 		SELECT TRANTYPE,COSTNAME,3 RESULT,2 AREAORD,'S' COLHEAD,SUM(DEBIT),SUM(CREDIT),SUM(BALANCE),SUM(DEBIT_WT),SUM(CREDIT_WT),SUM(BALANCE_WT)"
            strSql += vbCrLf + " 		FROM TEMPTABLEDB..TEMP_OUTSTANDING WHERE RESULT = 2 GROUP BY TRANTYPE ,COSTNAME"
            strSql += vbCrLf + " 		/** Inserting SubTotal Difference**/          "
            strSql += vbCrLf + " 		INSERT INTO TEMPTABLEDB..TEMP_OUTSTANDING(TRANTYPE,COSTNAME,RESULT,AREAORD,COLHEAD,DEBIT,CREDIT,DEBIT_WT,CREDIT_WT)          "
            strSql += vbCrLf + " 		SELECT TRANTYPE + '->DIFF',COSTNAME,4 RESULT,3 AREAORD,'S2' COLHEAD,    "
            strSql += vbCrLf + " 		SUM(CASE WHEN ISNULL(DEBIT,0)>ISNULL(CREDIT,0) THEN ISNULL(DEBIT,0)-ISNULL(CREDIT,0) END)DEBIT,    "
            strSql += vbCrLf + " 		SUM(CASE WHEN ISNULL(DEBIT,0)<ISNULL(CREDIT,0) THEN ISNULL(CREDIT,0)-ISNULL(DEBIT,0) END)CREDIT,  "
            strSql += vbCrLf + "		SUM(CASE WHEN ISNULL(DEBIT_WT,0)>ISNULL(CREDIT_WT,0) THEN ISNULL(DEBIT_WT,0)-ISNULL(CREDIT_WT,0) END)DEBIT_WT,    "
            strSql += vbCrLf + " 		SUM(CASE WHEN ISNULL(DEBIT_WT,0)<ISNULL(CREDIT_WT,0) THEN ISNULL(CREDIT_WT,0)-ISNULL(DEBIT_WT,0) END)CREDIT_WT      "
            strSql += vbCrLf + " 		FROM TEMPTABLEDB..TEMP_OUTSTANDING WHERE RESULT = 3 GROUP BY TRANTYPE ,COSTNAME"
            strSql += vbCrLf + " 		END"
            strSql += vbCrLf + " 		IF @GRPCC ='N'"
            strSql += vbCrLf + " 		BEGIN"
            strSql += vbCrLf + " 				/** Inserting Title **/        "
            strSql += vbCrLf + " 			INSERT INTO TEMPTABLEDB..TEMP_OUTSTANDING(TRANTYPE,RESULT,AREAORD,COLHEAD)        "
            strSql += vbCrLf + " 			SELECT DISTINCT TRANTYPE,1 RESULT,0 AREAORD,'T' COLHEAD FROM TEMPTABLEDB..TEMP_OUTSTANDING WHERE RESULT = 2        "
            strSql += vbCrLf + " 			/** Inserting SubTotal **/        "
            strSql += vbCrLf + " 			INSERT INTO TEMPTABLEDB..TEMP_OUTSTANDING(TRANTYPE,RESULT,AREAORD,COLHEAD,DEBIT,CREDIT,BALANCE,DEBIT_WT,CREDIT_WT,BALANCE_WT)        "
            strSql += vbCrLf + " 			SELECT TRANTYPE,3 RESULT,3 AREAORD,'S' COLHEAD,SUM(DEBIT),SUM(CREDIT),SUM(BALANCE),SUM(DEBIT_WT),SUM(CREDIT_WT),SUM(BALANCE_WT)"
            strSql += vbCrLf + " 			FROM TEMPTABLEDB..TEMP_OUTSTANDING WHERE RESULT = 2 GROUP BY TRANTYPE         "
            strSql += vbCrLf + " 			/** Inserting SubTotal Difference**/          "
            strSql += vbCrLf + " 			INSERT INTO TEMPTABLEDB..TEMP_OUTSTANDING(TRANTYPE,RESULT,AREAORD,COLHEAD,DEBIT,CREDIT,DEBIT_WT,CREDIT_WT)          "
            strSql += vbCrLf + " 			SELECT TRANTYPE + '->DIFF',4 RESULT,3 AREAORD,'S2' COLHEAD,    "
            strSql += vbCrLf + " 			SUM(CASE WHEN ISNULL(DEBIT,0)>ISNULL(CREDIT,0) THEN ISNULL(DEBIT,0)-ISNULL(CREDIT,0) END)DEBIT,    "
            strSql += vbCrLf + " 			SUM(CASE WHEN ISNULL(DEBIT,0)<ISNULL(CREDIT,0) THEN ISNULL(CREDIT,0)-ISNULL(DEBIT,0) END)CREDIT,    "
            strSql += vbCrLf + "			SUM(CASE WHEN ISNULL(DEBIT_WT,0)>ISNULL(CREDIT_WT,0) THEN ISNULL(DEBIT_WT,0)-ISNULL(CREDIT_WT,0) END)DEBIT_WT,    "
            strSql += vbCrLf + " 			SUM(CASE WHEN ISNULL(DEBIT_WT,0)<ISNULL(CREDIT_WT,0) THEN ISNULL(CREDIT_WT,0)-ISNULL(DEBIT_WT,0) END)CREDIT_WT    "
            strSql += vbCrLf + " 			FROM TEMPTABLEDB..TEMP_OUTSTANDING WHERE RESULT = 3 GROUP BY TRANTYPE    "
            strSql += vbCrLf + " 		END"
            strSql += vbCrLf + " 		IF @GRPAREA = 'Y'    "
            strSql += vbCrLf + " 		BEGIN    "
            strSql += vbCrLf + " 			/** Inserting Title **/        "
            strSql += vbCrLf + " 			INSERT INTO TEMPTABLEDB..TEMP_OUTSTANDING(TRANTYPE,AREA,AREAORD,RESULT,COLHEAD)        "
            strSql += vbCrLf + " 			SELECT DISTINCT TRANTYPE,AREA,0 AREAORD,2 RESULT,'T1' COLHEAD FROM TEMPTABLEDB..TEMP_OUTSTANDING WHERE RESULT = 2   "
            strSql += vbCrLf + " 			 /** Inserting SubTotal **/        "
            strSql += vbCrLf + " 			INSERT INTO TEMPTABLEDB..TEMP_OUTSTANDING(TRANTYPE,AREA,AREAORD,RESULT,COLHEAD,DEBIT,CREDIT,BALANCE,DEBIT_WT,CREDIT_WT,BALANCE_WT)        "
            strSql += vbCrLf + " 			SELECT TRANTYPE,AREA,2 AREAORD,2 RESULT,'S1' COLHEAD,SUM(DEBIT),SUM(CREDIT),SUM(BALANCE),SUM(DEBIT_WT),SUM(CREDIT_WT),SUM(BALANCE_WT)     "
            strSql += vbCrLf + " 			FROM TEMPTABLEDB..TEMP_OUTSTANDING WHERE RESULT = 2 GROUP BY TRANTYPE,AREA "
            strSql += vbCrLf + " 		END    "
            strSql += vbCrLf + " 		/** Inserting Grand Total **/        "
            strSql += vbCrLf + " 		INSERT INTO TEMPTABLEDB..TEMP_OUTSTANDING(TRANTYPE,COSTNAME,RESULT,COLHEAD,DEBIT,CREDIT,BALANCE,DEBIT_WT,CREDIT_WT,BALANCE_WT)        "
            strSql += vbCrLf + " 		SELECT 'ZZZZZ','ZZZZZ',4 RESULT,'G' COLHEAD,SUM(DEBIT),SUM(CREDIT),SUM(BALANCE),SUM(DEBIT_WT),SUM(CREDIT_WT),SUM(BALANCE_WT)"
            strSql += vbCrLf + " 		FROM TEMPTABLEDB..TEMP_OUTSTANDING WHERE RESULT = 2        "
            strSql += vbCrLf + " 		/** Inserting Grand Total Difference**/        "
            strSql += vbCrLf + " 		INSERT INTO TEMPTABLEDB..TEMP_OUTSTANDING(TRANTYPE,COSTNAME,RESULT,COLHEAD,DEBIT,CREDIT,DEBIT_WT,CREDIT_WT)          "
            strSql += vbCrLf + " 		SELECT 'ZZZZZZ','ZZZZZZ',5 RESULT,'G1' COLHEAD,    "
            strSql += vbCrLf + " 		SUM(CASE WHEN ISNULL(DEBIT,0)>ISNULL(CREDIT,0) THEN ISNULL(DEBIT,0)-ISNULL(CREDIT,0) END)DEBIT,      "
            strSql += vbCrLf + " 		SUM(CASE WHEN ISNULL(DEBIT,0)<ISNULL(CREDIT,0) THEN ISNULL(CREDIT,0)-ISNULL(DEBIT,0) END)CREDIT,    "
            strSql += vbCrLf + "		SUM(CASE WHEN ISNULL(DEBIT_WT,0)>ISNULL(CREDIT_WT,0) THEN ISNULL(DEBIT_WT,0)-ISNULL(CREDIT_WT,0) END)DEBIT_WT,      "
            strSql += vbCrLf + " 		SUM(CASE WHEN ISNULL(DEBIT_WT,0)<ISNULL(CREDIT_WT,0) THEN ISNULL(CREDIT_WT,0)-ISNULL(DEBIT_WT,0) END)CREDIT_WT    "
            strSql += vbCrLf + " 		FROM TEMPTABLEDB..TEMP_OUTSTANDING WHERE RESULT = 4       "
            strSql += vbCrLf + " 	END        "
            strSql += vbCrLf + " 	DECLARE @SPACE VARCHAR(10)    "
            strSql += vbCrLf + " 	SET @SPACE = CASE WHEN @GRPAREA = 'Y' THEN ' ' ELSE '' END "
            strSql += vbCrLf + " 	IF @GRPCC ='Y'   "
            strSql += vbCrLf + " 	BEGIN"
            strSql += vbCrLf + " 		UPDATE TEMPTABLEDB..TEMP_OUTSTANDING SET PARTICULAR =     "
            strSql += vbCrLf + " 		CASE     "
            strSql += vbCrLf + " 		WHEN COLHEAD = 'T1' THEN @SPACE + AREA     "
            strSql += vbCrLf + " 		WHEN COLHEAD = 'S1' THEN @SPACE + ISNULL(AREA,'') + ' ->TOT'    "
            strSql += vbCrLf + " 		WHEN RESULT = 0 THEN COSTNAME    "
            strSql += vbCrLf + " 		WHEN RESULT = 1 THEN TRANTYPE   "
            strSql += vbCrLf + " 		WHEN RESULT = 2 THEN @SPACE + '  ' + SUBSTRING(RUNNO,6,20)     "
            strSql += vbCrLf + " 		WHEN RESULT = 3 THEN ISNULL(TRANTYPE,'') + ' ->TOT'    "
            strSql += vbCrLf + " 		WHEN RESULT = 4 THEN 'DIFF'    "
            strSql += vbCrLf + " 		WHEN RESULT = 6 THEN 'GRAND DIFF'    "
            strSql += vbCrLf + " 		ELSE 'GRAND TOTAL' END  "
            strSql += vbCrLf + " 	END"
            strSql += vbCrLf + " 	IF @GRPCC='N'"
            strSql += vbCrLf + " 	BEGIN"
            strSql += vbCrLf + " 		UPDATE TEMPTABLEDB..TEMP_OUTSTANDING SET PARTICULAR =     "
            strSql += vbCrLf + " 		CASE     "
            strSql += vbCrLf + " 		WHEN COLHEAD = 'T1' THEN @SPACE + AREA     "
            strSql += vbCrLf + " 		WHEN COLHEAD = 'S1' THEN @SPACE + ISNULL(AREA,'') + ' ->TOT'    "
            strSql += vbCrLf + " 		WHEN RESULT = 1 THEN TRANTYPE   "
            strSql += vbCrLf + " 		WHEN RESULT = 2 THEN @SPACE + '  ' + SUBSTRING(RUNNO,6,20)     "
            strSql += vbCrLf + " 		WHEN RESULT = 3 THEN ISNULL(TRANTYPE,'') + ' ->TOT'    "
            strSql += vbCrLf + " 		WHEN RESULT = 4 THEN 'DIFF'    "
            strSql += vbCrLf + " 		WHEN RESULT = 6 THEN 'GRAND DIFF'    "
            strSql += vbCrLf + " 		ELSE 'GRAND TOTAL' END  "
            strSql += vbCrLf + " 	END	      "
            strSql += vbCrLf + " 	UPDATE TEMPTABLEDB..TEMP_OUTSTANDING SET DEBIT = CASE WHEN ISNULL(DEBIT,0) <> 0 THEN DEBIT ELSE NULL END        "
            strSql += vbCrLf + " 	,CREDIT = CASE WHEN ISNULL(cREDIT,0) <> 0 THEN CREDIT ELSE NULL END        "
            strSql += vbCrLf + " 	,BALANCE = CASE WHEN ISNULL(BALANCE,0) <> 0 THEN BALANCE ELSE NULL END      "
            strSql += vbCrLf + "	UPDATE TEMPTABLEDB..TEMP_OUTSTANDING SET DEBIT_WT = CASE WHEN ISNULL(DEBIT_WT,0) <> 0 THEN DEBIT_WT ELSE NULL END        "
            strSql += vbCrLf + " 	,CREDIT_WT = CASE WHEN ISNULL(CREDIT_WT,0) <> 0 THEN CREDIT_WT ELSE NULL END        "
            strSql += vbCrLf + " 	,BALANCE_WT = CASE WHEN ISNULL(BALANCE_WT,0) <> 0 THEN BALANCE_WT ELSE NULL END      "
            strSql += vbCrLf + " 	IF @GRPCC ='Y' "
            strSql += vbCrLf + " 	BEGIN"
            strSql += vbCrLf + " 		IF @ORDERBY = 'N'    "
            strSql += vbCrLf + " 			SELECT * FROM TEMPTABLEDB..TEMP_OUTSTANDING ORDER BY COSTNAME,TRANTYPE,RESULT,NAME    "
            strSql += vbCrLf + " 		ELSE IF @ORDERBY = 'B'    "
            strSql += vbCrLf + " 			SELECT * FROM TEMPTABLEDB..TEMP_OUTSTANDING ORDER BY COSTNAME,TRANTYPE,RESULT,TRANDATE    "
            strSql += vbCrLf + " 		ELSE   "
            strSql += vbCrLf + " 			SELECT * FROM TEMPTABLEDB..TEMP_OUTSTANDING ORDER BY COSTNAME ,TRANTYPE,RESULT,TRANNO "
            strSql += vbCrLf + " 	END"
            strSql += vbCrLf + " 	IF @GRPCC ='N' "
            strSql += vbCrLf + " 	BEGIN"
            strSql += vbCrLf + " 		IF @ORDERBY = 'N'    "
            strSql += vbCrLf + " 			SELECT * FROM TEMPTABLEDB..TEMP_OUTSTANDING ORDER BY TRANTYPE,RESULT,NAME    "
            strSql += vbCrLf + " 		ELSE IF @ORDERBY = 'B'    "
            strSql += vbCrLf + " 			SELECT * FROM TEMPTABLEDB..TEMP_OUTSTANDING ORDER BY TRANTYPE,RESULT,TRANDATE    "
            strSql += vbCrLf + " 		ELSE   "
            strSql += vbCrLf + " 			SELECT * FROM TEMPTABLEDB..TEMP_OUTSTANDING ORDER BY TRANTYPE,RESULT,TRANNO "
            strSql += vbCrLf + " 	END"
            strSql += vbCrLf + " END"
            cmd = New OleDbCommand(strSql, cn)
            cmd.ExecuteNonQuery()
        End If


        strSql = " EXEC " & dbName & "..REM_RPT_CUSTOMEROUTSTANDING_TODAY"
        strSql += vbCrLf + " @GRPCC='N',"
        strSql += vbCrLf + " @FROMDATE = '1900-01-01'"
        strSql += vbCrLf + " ,@TODATE = '" & Date.Now.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@DISPLAY = 'A'"
        strSql += vbCrLf + " ,@SUMMARY = 'Y'"
        strSql += vbCrLf + " ,@ORDERBY = 'R'"
        strSql += vbCrLf + " ,@FILTERBY = 'NONE'"
        strSql += vbCrLf + " ,@FILTERCAPTION = ''"
        strSql += vbCrLf + " ,@COSTNAME = ''"
        strSql += vbCrLf + " ,@COMPANYID = '" & dbName.Replace("ADMINDB", "") & "'"
        strSql += vbCrLf + " ,@TYPEID = 'D,'"
        strSql += vbCrLf + " ,@CRPUR = 'N'"
        strSql += vbCrLf + " ,@ACCODE='DRS'"
        strSql += vbCrLf + " ,@GRPAREA = 'N'"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()
        strSql = " SELECT "
        strSql += vbCrLf + " ROW_NUMBER() OVER(ORDER BY TRANTYPE,RESULT,TRANNO ) SNO,DUEDATE,TRANDATE,TRANNO,NAME,MOBILE AS MOBILENO,"
        strSql += vbCrLf + " SUBSTRING(RUNNO,6,10)RUNNO,BALANCE AS AMOUNT,RUNNO AS RUNNO1,1 AS RESULT "
        strSql += vbCrLf + " FROM TEMPTABLEDB..TEMP_OUTSTANDING WHERE RESULT=2 AND ISNULL(BALANCE,0)<>0 ORDER BY TRANDATE DESC, TRANNO  DESC "
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        Return dt
    End Function

    Function InstantCashReport() As DataTable
        Dim stockdb As String
        Dim globaldateval As String
        Dim dt As DataTable
        strSql = " SELECT REPLACE(CONVERT(varchar(15),CASE WHEN  CTLTEXT = '' THEN GETDATE() ELSE CTLTEXT END,102),'.','-') CTLTEXT "
        strSql += vbCrLf + " FROM " & dbName & "..SOFTCONTROL WHERE CTLID = 'GLOBALDATEVAL'"
        globaldateval = GetSqlValue(strSql)
        strSql = "select DBNAME from " & dbName & "..DBMASTER where GETDATE() between STARTDATE and ENDDATE"
        stockdb = GetSqlValue(strSql).ToString
        strSql = vbCrLf + "  SELECT 'INSTANT CASH' PARTICULAR, SUM(AMOUNT) AMOUNT FROM ("
        strSql += vbCrLf + "  SELECT CASE WHEN TRANMODE = 'C' THEN  -1 * SUM(AMOUNT) "
        strSql += vbCrLf + "  ELSE SUM(AMOUNT) END AMOUNT FROM " & stockdb & "..ACCTRAN WHERE TRANDATE = '" & globaldateval & "' "
        strSql += vbCrLf + "  AND ACCODE = 'CASH' AND PAYMODE = 'CA' "
        strSql += vbCrLf + "  AND ISNULL(CANCEL,'') = ''"
        strSql += vbCrLf + "  GROUP BY TRANMODE"
        strSql += vbCrLf + "  )X"
        da = New OleDbDataAdapter(strSql, cn)
        dt = New DataTable
        da.Fill(dt)
        Return dt
    End Function

    Function ReOrder() As DataTable
        Dim dt As New DataTable
        strSql = vbCrLf + "SELECT "
        strSql += vbCrLf + " (SELECT ITEMNAME FROM " & dbName & "..ITEMMAST WHERE ITEMID = I.ITEMID) ITEMNAME"
        strSql += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & dbName & "..SUBITEMMAST WHERE SUBITEMID = I.SUBITEMID AND ITEMID = I.ITEMID) SUBITEMNAME"
        strSql += vbCrLf + " ,S.RANGECAPTION "
        strSql += vbCrLf + " ,S.PIECE REORDERLEVEL  "
        strSql += vbCrLf + " ,SUM(I.PCS)AVILABLE_PCS "
        strSql += vbCrLf + " ,S.PIECE-SUM(I.PCS) WANTED_PCS"
        strSql += vbCrLf + " FROM " & dbName & "..ITEMTAG I  "
        strSql += vbCrLf + " INNER JOIN " & dbName & "..STKREORDER AS S ON S.ITEMID = I.ITEMID AND S.SUBITEMID = I.SUBITEMID AND I.COSTID = S.COSTID"
        strSql += vbCrLf + " AND I.GRSWT BETWEEN S.FROMWEIGHT AND S.TOWEIGHT "
        strSql += vbCrLf + " WHERE ISSDATE IS NULL"
        strSql += vbCrLf + " GROUP BY I.ITEMID, I.SUBITEMID,S.RANGECAPTION,S.PIECE"
        strSql += vbCrLf + " HAVING S.PIECE >0 AND S.PIECE -SUM(I.PCS) > 0"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        Return dt
    End Function
    Function StockSummary() As DataTable
        Dim dt As New DataTable
        'strSql = "SELECT (SELECT TOP 1 COSTNAME FROM " & dbName & "..COSTCENTRE WHERE COSTID=T.COSTID)COSTNAME"
        'strSql += vbCrLf + "  ,SUM(PCS)PCS,SUM(GRSWT)GRSWT FROM " & dbName & "..ITEMTAG T"
        'strSql += vbCrLf + "  GROUP BY COSTID"

        strSql = " SELECT 1 FROM SYSOBJECTS WHERE NAME='REM_SP_RPT_STOCKREORDERNEW'"
        If Val(GetSqlValue(strSql).ToString()) = 0 Then

            strSql = " IF(SELECT 1 FROM SYSOBJECTS WHERE NAME='REM_SP_RPT_STOCKREORDERNEW')>0"
            strSql += vbCrLf + "  	DROP PROCEDURE REM_SP_RPT_STOCKREORDERNEW"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()

            strSql = " CREATE PROCEDURE REM_SP_RPT_STOCKREORDERNEW"
            strSql += vbCrLf + " (  "
            strSql += vbCrLf + " @METALID VARCHAR(1000)  "
            strSql += vbCrLf + " ,@ITEMID VARCHAR(8000)  "
            strSql += vbCrLf + " ,@SUBITEMID VARCHAR(8000)  "
            strSql += vbCrLf + " ,@COSTID VARCHAR(4000)  "
            strSql += vbCrLf + " ,@COUNTER VARCHAR(8000)  "
            strSql += vbCrLf + " ,@SIZEID VARCHAR(8000)  "
            strSql += vbCrLf + " ,@ASONDATE VARCHAR(25)  "
            strSql += vbCrLf + " ,@COMPANYID VARCHAR(3)  "
            strSql += vbCrLf + " ,@RTYPE VARCHAR(1) "
            strSql += vbCrLf + " ,@RMODE VARCHAR(1)"
            strSql += vbCrLf + " ,@SIZE VARCHAR(1)"
            strSql += vbCrLf + " ,@WITHORDER VARCHAR(1)"
            strSql += vbCrLf + " ,@DESIGNREORDER VARCHAR(1)"
            strSql += vbCrLf + " )  "
            strSql += vbCrLf + " AS  "
            strSql += vbCrLf + " SET NOCOUNT ON  "
            strSql += vbCrLf + " DECLARE @QRY AS NVARCHAR(4000)"
            strSql += vbCrLf + " DECLARE @STRQRY AS VARCHAR(8000)"
            strSql += vbCrLf + " DECLARE @TCOSTID AS VARCHAR(4000)"
            strSql += vbCrLf + " EXEC  SP_SPLITVALUES @CHK_DB = '" & dbName & "',@CHK_TBL = 'ITEMCOUNTER',@CHK_FEILD = 'ITEMCTRID',@RESULT_TBL = 'TEMPCOUNTER',@INPUTVALUES = @COUNTER "
            strSql += vbCrLf + " EXEC  SP_SPLITVALUES @CHK_DB = '" & dbName & "',@CHK_TBL = 'SUBITEMMAST',@CHK_FEILD = 'SUBITEMID',@RESULT_TBL = 'TEMPSUBITEMMAST',@INPUTVALUES = @SUBITEMID  "
            strSql += vbCrLf + " EXEC  SP_SPLITVALUES @CHK_DB = '" & dbName & "',@CHK_TBL = 'ITEMSIZE',@CHK_FEILD = 'SIZEID',@RESULT_TBL = 'TEMPITEMSIZE',@INPUTVALUES = @SIZEID  "
            strSql += vbCrLf + " IF  @COSTID<>'ALL' OR @COSTID=''  SET @TCOSTID = REPLACE(@COSTID,',',''',''') SET @TCOSTID = ''''+ @TCOSTID +''''"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..TEMPCOSTCENTRE', 'U') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPCOSTCENTRE          "
            strSql += vbCrLf + " BEGIN"
            strSql += vbCrLf + " 		SELECT @QRY = 'CREATE TABLE TEMPTABLEDB..TEMPCOSTCENTRE(COSTID VARCHAR(3))'        		"
            strSql += vbCrLf + " 		EXECUTE SP_EXECUTESQL @QRY"
            strSql += vbCrLf + " 		IF @COSTID = 'ALL' OR @COSTID = ''  "
            strSql += vbCrLf + " 		BEGIN  		"
            strSql += vbCrLf + " 			SELECT @QRY = 'INSERT INTO TEMPTABLEDB..TEMPCOSTCENTRE SELECT COSTID FROM COSTCENTRE'    "
            strSql += vbCrLf + " 		END    "
            strSql += vbCrLf + " 		ELSE "
            strSql += vbCrLf + " 		BEGIN       		"
            strSql += vbCrLf + " 			SELECT @QRY = 'INSERT INTO TEMPTABLEDB..TEMPCOSTCENTRE SELECT COSTID FROM COSTCENTRE WHERE COSTID IN('+ @TCOSTID +')'"
            strSql += vbCrLf + " 		END  		"
            strSql += vbCrLf + " 		EXECUTE SP_EXECUTESQL @QRY"
            strSql += vbCrLf + " 		/*ITEM FILTER AND METAL FILTER*/"
            strSql += vbCrLf + " 		IF OBJECT_ID('TEMPTABLEDB..TEMPITEMMAST') IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPITEMMAST"
            strSql += vbCrLf + " 		IF @ITEMID<>'ALL'"
            strSql += vbCrLf + " 		BEGIN "
            strSql += vbCrLf + " 			SELECT @QRY = 'SELECT ITEMID AS ITEMIDS INTO TEMPTABLEDB..TEMPITEMMAST FROM ITEMMAST WHERE ISNULL(ACTIVE,'''')<>''N'''"
            strSql += vbCrLf + " 			SELECT @QRY = @QRY + ' AND ISNULL(STOCKREPORT,'''')<>''N'' AND ITEMID IN('+ @ITEMID +')'"
            strSql += vbCrLf + " 			IF @METALID<>'ALL' SELECT @QRY = @QRY + ' AND METALID='''+ @METALID +''''"
            strSql += vbCrLf + " 			EXECUTE SP_EXECUTESQL @QRY"
            strSql += vbCrLf + " 		END"
            strSql += vbCrLf + " 		ELSE"
            strSql += vbCrLf + " 		BEGIN "
            strSql += vbCrLf + " 			SELECT @QRY = 'SELECT ITEMID AS ITEMIDS INTO TEMPTABLEDB..TEMPITEMMAST FROM ITEMMAST WHERE ISNULL(ACTIVE,'''')<>''N'''"
            strSql += vbCrLf + " 			SELECT @QRY = @QRY + ' AND ISNULL(STOCKREPORT,'''')<>''N'''"
            strSql += vbCrLf + " 			IF @METALID<>'ALL' SELECT @QRY = @QRY + ' AND METALID='''+ @METALID +''''"
            strSql += vbCrLf + " 			PRINT @qry"
            strSql += vbCrLf + " 			EXECUTE SP_EXECUTESQL @QRY"
            strSql += vbCrLf + " 		END"
            strSql += vbCrLf + " 		IF @DESIGNREORDER='N'"
            strSql += vbCrLf + " 		BEGIN"
            strSql += vbCrLf + " 			PRINT '@DESIGNREORDER -N === '+@COSTID"
            strSql += vbCrLf + " 			PRINT @ASONDATE"
            strSql += vbCrLf + " 			SELECT @STRQRY=' IF OBJECT_ID(''TEMPTABLEDB..NONSTKORDER'') IS NOT NULL DROP TABLE TEMPTABLEDB..NONSTKORDER'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT SM.SUBITEMNAME,RE.PIECE AS REORDPCS,RE.WEIGHT AS REORDWT' "
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,T.PCS,T.GRSWT,IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM/*,SI.SIZENAME*/'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,2 RESULT'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INTO TEMPTABLEDB..NONSTKORDER'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' FROM  ITEMTAG AS T'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INNER JOIN  ITEMMAST AS IM ON IM.ITEMID = T.ITEMID     '"
            strSql += vbCrLf + " 			IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INNER JOIN  COSTCENTRE AS CC ON CC.COSTID = T.COSTID    '"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' LEFT OUTER JOIN  SUBITEMMAST AS SM ON SM.ITEMID = T.ITEMID AND SM.SUBITEMID = T.SUBITEMID'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' LEFT OUTER JOIN  ITEMSIZE AS SI ON SI.SIZEID = T.SIZEID'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' LEFT OUTER JOIN  STKREORDER AS RE ON RE.ITEMID=T.ITEMID'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' WHERE T.RECDATE <= '''+@ASONDATE+''' AND (T.ISSDATE IS NULL OR T.ISSDATE >'''+@ASONDATE+''''"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' )AND NOT EXISTS (SELECT 1 FROM  STKREORDER RE WHERE T.ITEMID = RE.ITEMID AND T.SUBITEMID = RE.SUBITEMID AND T.GRSWT BETWEEN RE.FROMWEIGHT AND RE.TOWEIGHT )/*AND T.SIZEID = RE.SIZEID)*/'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND ISNULL(T.COMPANYID,'''') = '''+@COMPANYID+''''"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND T.ITEMID IN (SELECT ITEMIDS FROM TEMPTABLEDB..TEMPITEMMAST)'  "
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND T.SUBITEMID IN (SELECT COMPANYID FROM TEMPSUBITEMMAST)'  "
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND ISNULL(T.SIZEID,0) IN (SELECT COMPANYID FROM TEMPITEMSIZE)'      "
            strSql += vbCrLf + " 			IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND ISNULL(T.COSTID,0) IN (SELECT COSTID FROM TEMPTABLEDB..TEMPCOSTCENTRE)'     "
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND ISNULL(T.ITEMCTRID,0) IN (SELECT COMPANYID FROM TEMPCOUNTER)' "
            strSql += vbCrLf + " 			IF  @WITHORDER<>'Y' SELECT @STRQRY= @STRQRY+ CHAR(13) + '   AND ISNULL(T.ORSNO,'''')='''''"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ORDER BY ITEM'"
            strSql += vbCrLf + " 			PRINT @STRQRY"
            strSql += vbCrLf + " 			EXEC (@STRQRY)"
            strSql += vbCrLf + " 			IF @RMODE = 'A'"
            strSql += vbCrLf + " 			BEGIN"
            strSql += vbCrLf + " 				SELECT @STRQRY = ' UPDATE TEMPTABLEDB..NONSTKORDER SET SUBITEM = ''OTHERS'''"
            strSql += vbCrLf + " 				EXEC (@STRQRY)"
            strSql += vbCrLf + " 			END"
            strSql += vbCrLf + " 			IF @SIZE ='Y'"
            strSql += vbCrLf + " 				BEGIN"
            strSql += vbCrLf + " 				PRINT 'SIZE -Y'"
            strSql += vbCrLf + " 				SELECT @STRQRY=' IF OBJECT_ID(''TEMPTABLEDB..STKORDER'') IS NOT NULL DROP TABLE TEMPTABLEDB..STKORDER'  "
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT SUBITEMNAME,CONVERT(VARCHAR(200),RANGE)RANGE,SUM(REPCS)REORDPCS,SUM(REWT)REORDWT,SUM(STKPCS)STKPCS,SUM(STKWT)STKWT'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CONVERT(INT,0)SDIFFPCS,CONVERT(NUMERIC(15,3),0)SDIFFWT'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CONVERT(INT,0)EDIFFPCS,CONVERT(NUMERIC(15,3),0)EDIFFWT'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,ITEM,SUBITEM,SIZENAME,FROMWEIGHT, TOWEIGHT, RESULT, COLHEAD'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INTO TEMPTABLEDB..STKORDER  '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' FROM'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' (SELECT SM.SUBITEMNAME'"
            strSql += vbCrLf + " 				IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CC.COSTNAME'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,(CONVERT(VARCHAR(100),FROMWEIGHT) + '' TO '' + CONVERT(VARCHAR(100),TOWEIGHT) + '' ['' + CONVERT(VARCHAR(100),(CONVERT(DECIMAL (15,3),(CONVERT(VARCHAR(100),((FROMWEIGHT+TOWEIGHT)/2)))))) + '']'')AS RANGE,CONVERT(INT,SUM(T.PCS))STKPCS,CONVERT(NUMERIC(15,3),SUM(T.GRSWT))STKWT'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,(RE.PIECE) AS REPCS,(RE.WEIGHT) AS REWT,CONVERT(VARCHAR(100),SI.SIZENAME)SIZENAME  '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CONVERT(VARCHAR(100),IM.ITEMNAME) AS ITEM,CONVERT(VARCHAR(100),SM.SUBITEMNAME)AS SUBITEM '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,RE.FROMWEIGHT,RE.TOWEIGHT,CONVERT(INT,1) AS RESULT,CONVERT(VARCHAR(3),NULL)COLHEAD     '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' FROM  STKREORDER AS RE  '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INNER JOIN  ITEMMAST IM ON IM.ITEMID = RE.ITEMID  '"
            strSql += vbCrLf + " 				IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INNER JOIN  COSTCENTRE CC ON CC.COSTID = RE.COSTID'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' LEFT OUTER JOIN  SUBITEMMAST SM ON SM.ITEMID = RE.ITEMID AND SM.SUBITEMID = RE.SUBITEMID  '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' LEFT OUTER JOIN  ITEMTAG AS T ON T.RECDATE <= '''+@ASONDATE+''' AND (T.ISSDATE IS NULL OR T.ISSDATE > '''+@ASONDATE+''''"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' )AND T.ITEMID = RE.ITEMID AND T.SUBITEMID = RE.SUBITEMID AND T.GRSWT BETWEEN FROMWEIGHT AND TOWEIGHT AND ISNULL(T.SIZEID,0)= (CASE WHEN ISNULL(RE.SIZEID,0) <>0 THEN RE.SIZEID ELSE 0 END) /*AND ISNULL(T.SIZEID,0)=ISNULL(RE.SIZEID,0)*/ '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND T.COMPANYID = '''+@COMPANYID+''' AND T.COSTID = RE.COSTID LEFT OUTER JOIN  ITEMSIZE AS SI ON SI.SIZEID = RE.SIZEID  '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' WHERE RE.ITEMID IN (SELECT ITEMIDS FROM TEMPTABLEDB..TEMPITEMMAST)    '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND RE.SUBITEMID IN (SELECT COMPANYID FROM TEMPSUBITEMMAST)    '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND ISNULL(RE.SIZEID,0) IN (SELECT COMPANYID FROM TEMPITEMSIZE)    '"
            strSql += vbCrLf + " 				IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND ISNULL(RE.COSTID,0) IN (SELECT COSTID FROM TEMPTABLEDB..TEMPCOSTCENTRE) '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND ISNULL(T.ITEMCTRID,0) IN (SELECT COMPANYID FROM TEMPCOUNTER) '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND RE.RANGEMODE=''W'''"
            strSql += vbCrLf + " 				IF  @WITHORDER<>'Y' SELECT @STRQRY= @STRQRY+ CHAR(13) + '   AND ISNULL(T.ORSNO,'''')='''''"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' GROUP BY ITEMNAME'"
            strSql += vbCrLf + " 				IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ',CC.COSTNAME'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,SM.SUBITEMNAME,RE.FROMWEIGHT,RE.TOWEIGHT,SI.SIZENAME,RE.PIECE,RE.WEIGHT'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' UNION ALL'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT DISTINCT SM.SUBITEMNAME'"
            strSql += vbCrLf + " 				IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CC.COSTNAME'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,''OTHER RANGE'' RANGE,SUM(T.PCS)STKPCS ,SUM(T.GRSWT)STKWT,0 PIECE,0 WEIGHT'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CONVERT(VARCHAR(100),SI.SIZENAME)SIZENAME'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CONVERT(VARCHAR(100),IM.ITEMNAME) AS ITEM,CONVERT(VARCHAR(100),SM.SUBITEMNAME)AS SUBITEM '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,0 FROMWEIGHT,0 TOWEIGHT ,CONVERT(INT,1) AS RESULT,CONVERT(VARCHAR(3),NULL)COLHEAD   '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' FROM ITEMTAG AS T    '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' LEFT OUTER JOIN  SUBITEMMAST SM ON SM.ITEMID=T.ITEMID AND SM.SUBITEMID=T.SUBITEMID'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' LEFT OUTER JOIN  ITEMMAST IM ON IM.ITEMID=T.ITEMID '"
            strSql += vbCrLf + " 				IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' LEFT OUTER JOIN  COSTCENTRE CC ON CC.COSTID = T.COSTID'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' LEFT OUTER JOIN  ITEMSIZE AS SI ON SI.SIZEID=T.SIZEID'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' WHERE NOT EXISTS (SELECT 1 FROM  STKREORDER RE WHERE T.ITEMID = RE.ITEMID AND T.SUBITEMID = RE.SUBITEMID '"
            strSql += vbCrLf + " 				IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND ISNULL(T.COSTID,'''')=ISNULL(RE.COSTID,'''') '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + 'and  T.GRSWT between RE.FROMWEIGHT and RE.TOWEIGHT AND ISNULL(T.SIZEID,0)= (CASE WHEN ISNULL(RE.SIZEID,0) <>0 THEN RE.SIZEID ELSE 0 END) /*AND ISNULL(T.SIZEID,0)=ISNULL(RE.SIZEID,0)*/)'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND T.RECDATE <= '''+@ASONDATE+'''  AND (T.ISSDATE IS NULL OR T.ISSDATE > '''+@ASONDATE+''''"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' )AND T.ITEMID IN (SELECT ITEMIDS FROM TEMPTABLEDB..TEMPITEMMAST)    '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND T.SUBITEMID IN (SELECT COMPANYID FROM TEMPSUBITEMMAST)    '"
            strSql += vbCrLf + " 				IF  @WITHORDER<>'Y' SELECT @STRQRY= @STRQRY+ CHAR(13) + '   AND ISNULL(T.ORSNO,'''')='''''"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND ISNULL(T.SIZEID,0) IN (SELECT COMPANYID FROM TEMPITEMSIZE)    '"
            strSql += vbCrLf + " 				IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND ISNULL(T.COSTID,0) IN (SELECT COSTID FROM TEMPTABLEDB..TEMPCOSTCENTRE) '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND ISNULL(T.ITEMCTRID,0) IN (SELECT COMPANYID FROM TEMPCOUNTER) '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' GROUP BY ITEMNAME'"
            strSql += vbCrLf + " 				IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ',CC.COSTNAME'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ',SM.SUBITEMNAME,SI.SIZENAME'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' )X GROUP BY RANGE,SUBITEMNAME,FROMWEIGHT,TOWEIGHT,ITEM,SUBITEM,SIZENAME,'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' RESULT, COLHEAD'"
            strSql += vbCrLf + " 				PRINT @STRQRY"
            strSql += vbCrLf + " 				EXEC (@STRQRY)"
            strSql += vbCrLf + " 				/*UPDATE #STKORDER SET DIFFPCS =STKPCS-REORDPCS,DIFFWT = STKWT-REORDWT*/"
            strSql += vbCrLf + " 				SELECT @STRQRY = 'UPDATE TEMPTABLEDB..STKORDER SET SDIFFPCS =(CASE WHEN ISNULL(STKPCS,0)-ISNULL(REORDPCS,0)<0 THEN ISNULL(STKPCS,0)-ISNULL(REORDPCS,0) ELSE 0 END)'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,SDIFFWT = (CASE WHEN ISNULL(STKWT,0)-ISNULL(REORDWT,0)<0 THEN ISNULL(STKWT,0)-ISNULL(REORDWT,0) ELSE 0 END)'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,EDIFFPCS =(CASE WHEN ISNULL(STKPCS,0)-ISNULL(REORDPCS,0)>0 THEN ISNULL(STKPCS,0)-ISNULL(REORDPCS,0) ELSE 0 END)'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,EDIFFWT = (CASE WHEN ISNULL(STKWT,0)-ISNULL(REORDWT,0)>0 THEN ISNULL(STKWT,0)-ISNULL(REORDWT,0) ELSE 0 END)'"
            strSql += vbCrLf + " 				PRINT @STRQRY"
            strSql += vbCrLf + " 				EXEC (@STRQRY)"
            strSql += vbCrLf + " 				SELECT @STRQRY = 'DECLARE @LEN AS INT'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' DECLARE @LEN_FWT AS INT'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' DECLARE @LEN_TWT AS INT'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT @LEN = MAX(LEN(SUBITEMNAME))+1 FROM TEMPTABLEDB..STKORDER WHERE RESULT = 1'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT @LEN_FWT = MAX(LEN(FROMWEIGHT)) FROM TEMPTABLEDB..STKORDER'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT @LEN_TWT = MAX(LEN(TOWEIGHT)) FROM TEMPTABLEDB..STKORDER'"
            strSql += vbCrLf + " 				PRINT @STRQRY"
            strSql += vbCrLf + " 				EXEC (@STRQRY)"
            strSql += vbCrLf + " 				IF @RTYPE = 'S'"
            strSql += vbCrLf + " 				SELECT @STRQRY =   ' DELETE FROM TEMPTABLEDB..STKORDER WHERE NOT (SDIFFPCS < 0 OR SDIFFWT < 0)'"
            strSql += vbCrLf + " 				PRINT @STRQRY"
            strSql += vbCrLf + " 				EXEC (@STRQRY)"
            strSql += vbCrLf + " 				IF @RTYPE = 'E'"
            strSql += vbCrLf + " 				SELECT @STRQRY =  ' DELETE FROM TEMPTABLEDB..STKORDER WHERE NOT (EDIFFPCS > 0 OR EDIFFWT > 0)'"
            strSql += vbCrLf + " 				PRINT @STRQRY"
            strSql += vbCrLf + " 				EXEC (@STRQRY)"
            strSql += vbCrLf + " 				IF (SELECT COUNT(*) FROM TEMPTABLEDB..STKORDER)>0  "
            strSql += vbCrLf + " 				BEGIN"
            strSql += vbCrLf + " 					SELECT @STRQRY =   ' UPDATE TEMPTABLEDB..STKORDER SET SDIFFPCS=(SDIFFPCS*(-1)) WHERE SDIFFPCS < 0'"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' UPDATE TEMPTABLEDB..STKORDER SET SDIFFWT=(SDIFFWT*(-1)) WHERE SDIFFWT < 0'"
            strSql += vbCrLf + " 					PRINT @STRQRY"
            strSql += vbCrLf + " 					EXEC (@STRQRY)"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INSERT INTO TEMPTABLEDB..STKORDER(ITEM,SUBITEMNAME,RESULT,COLHEAD)'"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT DISTINCT ITEM,ITEM,0 RESULT,''T''COLHEAD FROM TEMPTABLEDB..STKORDER'"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INSERT INTO TEMPTABLEDB..STKORDER (ITEM,SUBITEMNAME,RESULT,COLHEAD)'"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT DISTINCT ITEM,SUBITEMNAME,3 RESULT,''E''COLHEAD FROM TEMPTABLEDB..STKORDER '"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INSERT INTO TEMPTABLEDB..STKORDER (ITEM,SUBITEMNAME,RESULT,COLHEAD,REORDPCS,REORDWT,STKPCS,STKWT,SDIFFPCS,SDIFFWT,EDIFFPCS,EDIFFWT,SUBITEM)'"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT DISTINCT ITEM,''SUB TOTAL'',2 RESULT,''S''COLHEAD,'     		"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SUM(REORDPCS),SUM(REORDWT),SUM(STKPCS),SUM(STKWT),SUM(SDIFFPCS),SUM(SDIFFWT),SUM(EDIFFPCS),SUM(EDIFFWT),SUBITEM'"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' FROM TEMPTABLEDB..STKORDER WHERE RESULT<>3 AND RESULT<>0 GROUP BY SUBITEM,ITEM'"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INSERT INTO TEMPTABLEDB..STKORDER (ITEM,SUBITEMNAME,RESULT,COLHEAD,REORDPCS,REORDWT,STKPCS,STKWT,SDIFFPCS,SDIFFWT,EDIFFPCS,EDIFFWT,SUBITEM)'"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT DISTINCT ITEM,''SUB TOTAL'',4 RESULT,''S''COLHEAD,'"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SUM(REORDPCS),SUM(REORDWT),SUM(STKPCS),SUM(STKWT),SUM(SDIFFPCS),SUM(SDIFFWT),SUM(EDIFFPCS),SUM(EDIFFWT),''ZZ.SUB TOTAL'''"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' FROM TEMPTABLEDB..STKORDER WHERE RESULT<>2 AND RESULT<>3 GROUP BY ITEM'"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INSERT INTO TEMPTABLEDB..STKORDER(ITEM,SUBITEMNAME,RESULT,COLHEAD,REORDPCS,REORDWT,STKPCS,STKWT,SDIFFPCS,SDIFFWT,EDIFFPCS,EDIFFWT,SUBITEM)  '"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' Select DISTINCT ''ZZZZ'',''GRAND TOTAL'',5 RESULT,''G'' COLHEAD'"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,SUM(REORDPCS),SUM(REORDWT),SUM(STKPCS),SUM(STKWT),SUM(SDIFFPCS),SUM(SDIFFWT),SUM(EDIFFPCS),SUM(EDIFFWT),''GRAND TOTAL'''	 "
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' FROM TEMPTABLEDB..STKORDER WHERE RESULT IN (1)'"
            strSql += vbCrLf + " 					PRINT @STRQRY"
            strSql += vbCrLf + " 					EXEC (@STRQRY)"
            strSql += vbCrLf + " 					SELECT @STRQRY =  ' UPDATE TEMPTABLEDB..STKORDER SET SUBITEM = SUBITEMNAME WHERE COLHEAD = ''E'''"
            strSql += vbCrLf + " 					PRINT @STRQRY"
            strSql += vbCrLf + " 					EXEC (@STRQRY)"
            strSql += vbCrLf + " 				END"
            strSql += vbCrLf + " 			END "
            strSql += vbCrLf + " 			IF @SIZE='N'"
            strSql += vbCrLf + " 			BEGIN"
            strSql += vbCrLf + " 				PRINT 'SIZE -N'"
            strSql += vbCrLf + " 				SELECT @STRQRY = 'IF OBJECT_ID(''TEMPTABLEDB..STKORDER1'') IS NOT NULL DROP TABLE TEMPTABLEDB..STKORDER1' "
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT SUBITEMNAME,CONVERT(VARCHAR(200),RANGE)RANGE,SUM(REPCS)REORDPCS,SUM(REWT)REORDWT,SUM(STKPCS)STKPCS,SUM(STKWT)STKWT'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CONVERT(INT,0)SDIFFPCS,CONVERT(NUMERIC(15,3),0)SDIFFWT'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CONVERT(INT,0)EDIFFPCS,CONVERT(NUMERIC(15,3),0)EDIFFWT'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,ITEM,SUBITEM,FROMWEIGHT,TOWEIGHT,RESULT,COLHEAD'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INTO TEMPTABLEDB..STKORDER1  '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' FROM ('"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT  '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SM.SUBITEMNAME'"
            strSql += vbCrLf + " 				IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + '  ,CC.COSTNAME'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,(CONVERT(VARCHAR(100),FROMWEIGHT) + '' TO '' + CONVERT(VARCHAR(100),TOWEIGHT) + '' ['' + CONVERT(VARCHAR(100),(CONVERT(DECIMAL (15,3),(CONVERT(VARCHAR(100),((FROMWEIGHT+TOWEIGHT)/2)))))) + '']'')AS RANGE,CONVERT(INT,SUM(T.PCS))STKPCS,CONVERT(NUMERIC(15,3),SUM(T.GRSWT))STKWT'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,(RE.PIECE) AS REPCS,(RE.WEIGHT) AS REWT  '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CONVERT(VARCHAR(100),IM.ITEMNAME) AS ITEM,CONVERT(VARCHAR(100),SM.SUBITEMNAME)AS SUBITEM '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,RE.FROMWEIGHT,RE.TOWEIGHT    '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CONVERT(INT,1) AS RESULT    '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CONVERT(VARCHAR(3),NULL)COLHEAD     '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' FROM  STKREORDER AS RE  '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INNER JOIN  ITEMMAST IM ON IM.ITEMID = RE.ITEMID  '"
            strSql += vbCrLf + " 				IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INNER JOIN  COSTCENTRE CC ON CC.COSTID = RE.COSTID'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' LEFT OUTER JOIN  SUBITEMMAST SM ON SM.ITEMID = RE.ITEMID AND SM.SUBITEMID = RE.SUBITEMID  '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' LEFT OUTER JOIN  ITEMTAG AS T ON T.RECDATE <= '''+@ASONDATE+''' AND (T.ISSDATE IS NULL OR T.ISSDATE >'''+@ASONDATE+ ''''"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' )AND T.ITEMID = RE.ITEMID '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND T.SUBITEMID = RE.SUBITEMID '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND T.GRSWT BETWEEN FROMWEIGHT AND TOWEIGHT'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND T.COMPANYID = '''+@COMPANYID+''' AND T.COSTID = RE.COSTID   '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' WHERE RE.ITEMID IN (SELECT ITEMIDS FROM TEMPTABLEDB..TEMPITEMMAST)    '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND RE.SUBITEMID IN (SELECT COMPANYID FROM TEMPSUBITEMMAST)    '"
            strSql += vbCrLf + " 				IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND ISNULL(RE.COSTID,0) IN (SELECT COSTID FROM TEMPTABLEDB..TEMPCOSTCENTRE)'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND RE.RANGEMODE=''W'''"
            strSql += vbCrLf + " 				IF  @WITHORDER<>'Y' SELECT @STRQRY= @STRQRY+ CHAR(13) + '   AND ISNULL(T.ORSNO,'''')='''''"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' GROUP BY ITEMNAME'"
            strSql += vbCrLf + " 				IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CC.COSTNAME'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,SM.SUBITEMNAME,RE.FROMWEIGHT,RE.TOWEIGHT,RE.PIECE,RE.WEIGHT'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' UNION ALL'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT DISTINCT SM.SUBITEMNAME'"
            strSql += vbCrLf + " 				IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CC.COSTNAME'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,''OTHER RANGE'' RANGE,SUM(T.PCS)STKPCS ,SUM(T.GRSWT)STKWT,0 PIECE,0 WEIGHT'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CONVERT(VARCHAR(100),IM.ITEMNAME) AS ITEM,CONVERT(VARCHAR(100),SM.SUBITEMNAME)AS SUBITEM '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,0 FROMWEIGHT,0 TOWEIGHT    '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CONVERT(INT,1) AS RESULT,CONVERT(VARCHAR(3),NULL)COLHEAD   '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' FROM ITEMTAG AS T    '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' LEFT OUTER jOIN  SUBITEMMAST SM ON SM.ITEMID=T.ITEMID AND SM.SUBITEMID=T.SUBITEMID'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' LEFT OUTER JOIN  ITEMMAST IM ON IM.ITEMID=T.ITEMID '"
            strSql += vbCrLf + " 				IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INNER JOIN  COSTCENTRE CC ON CC.COSTID = T.COSTID'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' WHERE NOT EXISTS (Select 1 from  STKREORDER RE WHERE T.ITEMID = RE.ITEMID AND T.SUBITEMID = RE.SUBITEMID '"
            strSql += vbCrLf + " 				IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND ISNULL(T.COSTID,'''')=ISNULL(RE.COSTID,'''') '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' and  T.GRSWT between RE.FROMWEIGHT and RE.TOWEIGHT)'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND T.RECDATE <= '''+@ASONDATE+'''  AND (T.ISSDATE IS NULL OR T.ISSDATE >'''+ @ASONDATE+ ''''"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' )AND T.ITEMID IN (SELECT ITEMIDS FROM TEMPTABLEDB..TEMPITEMMAST)'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND T.SUBITEMID IN (SELECT COMPANYID FROM TEMPSUBITEMMAST)'"
            strSql += vbCrLf + " 				IF  @WITHORDER<>'Y' SELECT @STRQRY= @STRQRY+ CHAR(13) + '   AND ISNULL(T.ORSNO,'''')='''''"
            strSql += vbCrLf + " 				IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND ISNULL(T.COSTID,0) IN (SELECT COSTID FROM TEMPTABLEDB..TEMPCOSTCENTRE)'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' GROUP BY ITEMNAME,SM.SUBITEMNAME'"
            strSql += vbCrLf + " 				IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CC.COSTNAME'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' )X GROUP BY RANGE,SUBITEMNAME,FROMWEIGHT,TOWEIGHT,ITEM,SUBITEM,RESULT,COLHEAD'"
            strSql += vbCrLf + " 				PRINT @STRQRY"
            strSql += vbCrLf + " 				EXEC (@STRQRY)"
            strSql += vbCrLf + " 				SELECT @STRQRY =  ' UPDATE TEMPTABLEDB..STKORDER1  SET SDIFFPCS =(CASE WHEN ISNULL(STKPCS,0)-ISNULL(REORDPCS,0)<0 THEN ISNULL(STKPCS,0)-ISNULL(REORDPCS,0) ELSE 0 END)'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,SDIFFWT = (CASE WHEN ISNULL(STKWT,0)-ISNULL(REORDWT,0)<0 THEN ISNULL(STKWT,0)-ISNULL(REORDWT,0) ELSE 0 END)'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,EDIFFPCS =(CASE WHEN ISNULL(STKPCS,0)-ISNULL(REORDPCS,0)>0 THEN ISNULL(STKPCS,0)-ISNULL(REORDPCS,0) ELSE 0 END)'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,EDIFFWT = (CASE WHEN ISNULL(STKWT,0)-ISNULL(REORDWT,0)>0 THEN ISNULL(STKWT,0)-ISNULL(REORDWT,0) ELSE 0 END)'"
            strSql += vbCrLf + " 				PRINT @STRQRY"
            strSql += vbCrLf + " 				EXEC (@STRQRY)"
            strSql += vbCrLf + " 				SELECT @STRQRY =  ' DECLARE @LEN1 AS INT'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' DECLARE @LEN_FWT1 AS INT'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' DECLARE @LEN_TWT1 AS INT'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT @LEN1 = MAX(LEN(SUBITEMNAME))+1 FROM TEMPTABLEDB..STKORDER1  WHERE RESULT = 1'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT @LEN_FWT1 = MAX(LEN(FROMWEIGHT)) FROM TEMPTABLEDB..STKORDER1 '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT @LEN_TWT1 = MAX(LEN(TOWEIGHT)) FROM TEMPTABLEDB..STKORDER1 '"
            strSql += vbCrLf + " 				PRINT @STRQRY"
            strSql += vbCrLf + " 				EXEC (@STRQRY)"
            strSql += vbCrLf + " 				/*UPDATE #STKORDER SET RANGE =  RANGE + REPLICATE(' ',@LEN-LEN(RANGE)) + '[' + REPLICATE(' ',@LEN_FWT - LEN(FROMWEIGHT)) + CONVERT(VARCHAR,FROMWEIGHT) + ' TO ' + REPLICATE(' ',@LEN_TWT - LEN(TOWEIGHT)) + CONVERT(VARCHAR,TOWEIGHT) + ']' WHERE RESULT = 1*/"
            strSql += vbCrLf + " 				IF @RTYPE = 'S'"
            strSql += vbCrLf + " 				SELECT @STRQRY =  ' DELETE FROM TEMPTABLEDB..STKORDER1  WHERE NOT (SDIFFPCS < 0 OR SDIFFWT < 0)'"
            strSql += vbCrLf + " 				PRINT @STRQRY"
            strSql += vbCrLf + " 				EXEC (@STRQRY)"
            strSql += vbCrLf + " 				IF @RTYPE = 'E'"
            strSql += vbCrLf + " 				SELECT @STRQRY =  ' DELETE FROM TEMPTABLEDB..STKORDER1  WHERE NOT (EDIFFPCS > 0 OR EDIFFWT > 0)'"
            strSql += vbCrLf + " 				PRINT @STRQRY"
            strSql += vbCrLf + " 				EXEC (@STRQRY)"
            strSql += vbCrLf + " 				IF (SELECT COUNT(*) FROM TEMPTABLEDB..STKORDER1 )>0  "
            strSql += vbCrLf + " 				BEGIN"
            strSql += vbCrLf + " 					SELECT @STRQRY =  ' UPDATE TEMPTABLEDB..STKORDER1  SET SDIFFPCS=(SDIFFPCS*(-1)) WHERE SDIFFPCS < 0'"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' UPDATE TEMPTABLEDB..STKORDER1  SET SDIFFWT=(SDIFFWT*(-1)) WHERE SDIFFWT < 0 '"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INSERT INTO TEMPTABLEDB..STKORDER1 (ITEM,SUBITEMNAME,RESULT,COLHEAD)'"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT DISTINCT ITEM,ITEM,0 RESULT,''T''COLHEAD FROM TEMPTABLEDB..STKORDER1 '"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INSERT INTO TEMPTABLEDB..STKORDER1  (ITEM,SUBITEMNAME,RESULT,COLHEAD)'"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT DISTINCT ITEM,SUBITEMNAME,3 RESULT,''E''COLHEAD FROM TEMPTABLEDB..STKORDER1  '"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INSERT INTO TEMPTABLEDB..STKORDER1  (ITEM,SUBITEMNAME,RESULT,COLHEAD,REORDPCS,REORDWT,STKPCS,STKWT,SDIFFPCS,SDIFFWT,EDIFFPCS,EDIFFWT,SUBITEM)'"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT DISTINCT ITEM,''SUB TOTAL'',2 RESULT,''S''COLHEAD,'"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SUM(REORDPCS),SUM(REORDWT),SUM(STKPCS),SUM(STKWT),SUM(SDIFFPCS),SUM(SDIFFWT),SUM(EDIFFPCS),SUM(EDIFFWT),SUBITEM'"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' FROM TEMPTABLEDB..STKORDER1  WHERE RESULT<>3 AND RESULT<>0 GROUP BY SUBITEM,ITEM'"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INSERT INTO TEMPTABLEDB..STKORDER1  (ITEM,SUBITEMNAME,RESULT,COLHEAD,REORDPCS,REORDWT,STKPCS,STKWT,SDIFFPCS,SDIFFWT,EDIFFPCS,EDIFFWT,SUBITEM)'"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT DISTINCT ITEM,''SUB TOTAL'',4 RESULT,''S''COLHEAD,'"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SUM(REORDPCS),SUM(REORDWT),SUM(STKPCS),SUM(STKWT),SUM(SDIFFPCS),SUM(SDIFFWT),SUM(EDIFFPCS),SUM(EDIFFWT),''ZZ.SUB TOTAL'''"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' FROM TEMPTABLEDB..STKORDER1  WHERE RESULT<>2 AND RESULT<>3 GROUP BY ITEM'"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INSERT INTO TEMPTABLEDB..STKORDER1 (ITEM,SUBITEMNAME,RESULT,COLHEAD,REORDPCS,REORDWT,STKPCS,STKWT,SDIFFPCS,SDIFFWT,EDIFFPCS,EDIFFWT,SUBITEM)  '"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' Select DISTINCT ''ZZZZ'',''GRAND TOTAL'',5 RESULT,''G'' COLHEAD	'"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,SUM(REORDPCS),SUM(REORDWT),SUM(STKPCS),SUM(STKWT),SUM(SDIFFPCS),SUM(SDIFFWT),SUM(EDIFFPCS),SUM(EDIFFWT),''GRAND TOTAL'''"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' FROM TEMPTABLEDB..STKORDER1  WHERE RESULT IN (1)'"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' UPDATE TEMPTABLEDB..STKORDER1  SET SUBITEM = SUBITEMNAME WHERE COLHEAD = ''E'''"
            strSql += vbCrLf + " 					PRINT @STRQRY"
            strSql += vbCrLf + " 					EXEC (@STRQRY)"
            strSql += vbCrLf + " 				END"
            strSql += vbCrLf + " 			END"
            strSql += vbCrLf + " 			IF @SIZE='Y'"
            strSql += vbCrLf + " 			BEGIN"
            strSql += vbCrLf + " 				SELECT @STRQRY =  ' DELETE FROM  TEMPTABLEDB..STKORDER  WHERE COLHEAD=''E'''"
            strSql += vbCrLf + " 				PRINT @STRQRY"
            strSql += vbCrLf + " 				EXEC (@STRQRY)"
            strSql += vbCrLf + " 				SELECT @STRQRY =  ' SELECT CASE WHEN COLHEAD = ''E'' THEN '''' ELSE SUBITEMNAME END SUBITEMNAME,'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' [RANGE],REORDPCS,REORDWT,STKPCS,STKWT,SDIFFPCS,SDIFFWT,EDIFFPCS,EDIFFWT,ITEM,SUBITEM,FROMWEIGHT,TOWEIGHT,RESULT,COLHEAD,SIZENAME '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' FROM TEMPTABLEDB..STKORDER  T ORDER BY ITEM,SUBITEM,RESULT,SIZENAME,[RANGE],FROMWEIGHT,TOWEIGHT'"
            strSql += vbCrLf + " 				PRINT @STRQRY"
            strSql += vbCrLf + " 				EXEC (@STRQRY)"
            strSql += vbCrLf + " 			END"
            strSql += vbCrLf + " 			IF @SIZE='N'"
            strSql += vbCrLf + " 			BEGIN"
            strSql += vbCrLf + " 				SELECT @STRQRY =  ' DELETE FROM  TEMPTABLEDB..STKORDER1  WHERE COLHEAD=''E'''"
            strSql += vbCrLf + " 				PRINT @STRQRY"
            strSql += vbCrLf + " 				EXEC (@STRQRY)"
            strSql += vbCrLf + " 				SELECT @STRQRY =  ' SELECT CASE WHEN COLHEAD = ''E'' THEN '''' ELSE SUBITEMNAME END SUBITEMNAME,'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + '[RANGE],REORDPCS,REORDWT,STKPCS,STKWT,SDIFFPCS,SDIFFWT,EDIFFPCS,EDIFFWT,ITEM,SUBITEM,FROMWEIGHT,TOWEIGHT,RESULT,COLHEAD FROM TEMPTABLEDB..STKORDER1  T '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + 'ORDER BY ITEM,SUBITEM,RESULT,[RANGE],FROMWEIGHT,TOWEIGHT'"
            strSql += vbCrLf + " 				PRINT @STRQRY"
            strSql += vbCrLf + " 				EXEC (@STRQRY)"
            strSql += vbCrLf + " 			END"
            strSql += vbCrLf + " 		END"
            strSql += vbCrLf + " 		IF @DESIGNREORDER='Y'"
            strSql += vbCrLf + " 		BEGIN"
            strSql += vbCrLf + " 			PRINT '@DESIGNREORDER -Y'"
            strSql += vbCrLf + " 			SELECT @STRQRY =  ' IF OBJECT_ID(''TEMPTABLEDB..DESIGNNONSTKORDER'') IS NOT NULL DROP TABLE TEMPTABLEDB..DESIGNNONSTKORDER'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT SM.SUBITEMNAME'"
            strSql += vbCrLf + " 			IF LEN(@COSTID)>0SELECT @STRQRY= @STRQRY+ CHAR(13) + ',CC.COSTNAME'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ',RE.PIECE AS REORDPCS,RE.WEIGHT AS REORDWT '"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ',T.PCS,T.GRSWT,IM.ITEMNAME AS ITEM,SM.SUBITEMNAME AS SUBITEM/*,SI.SIZENAME*/'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ',2 RESULT,DE.DESIGNERNAME '"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + 'INTO TEMPTABLEDB..DESIGNNONSTKORDER'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' FROM  ITEMTAG AS T'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INNER JOIN  ITEMMAST AS IM ON IM.ITEMID = T.ITEMID'"
            strSql += vbCrLf + " 			IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INNER JOIN  COSTCENTRE CC ON CC.COSTID = T.COSTID'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' LEFT OUTER JOIN  DESIGNER AS DE ON DE.DESIGNERID=T.DESIGNERID'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' LEFT OUTER JOIN  SUBITEMMAST AS SM ON SM.ITEMID = T.ITEMID AND SM.SUBITEMID = T.SUBITEMID'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' LEFT OUTER JOIN  ITEMSIZE AS SI ON SI.SIZEID = T.SIZEID'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' LEFT OUTER JOIN  STKREORDER AS RE ON RE.ITEMID=T.ITEMID'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' WHERE T.RECDATE <= '''+@ASONDATE+''' AND (T.ISSDATE IS NULL OR T.ISSDATE >'''+ @ASONDATE+''''"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' )AND NOT EXISTS (SELECT 1 FROM  STKREORDER RE WHERE T.ITEMID = RE.ITEMID AND T.SUBITEMID = RE.SUBITEMID '"
            strSql += vbCrLf + " 			IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND ISNULL(T.COSTID,'''')=ISNULL(RE.COSTID,'''') '"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND T.GRSWT BETWEEN RE.FROMWEIGHT AND RE.TOWEIGHT )/*AND T.SIZEID = RE.SIZEID)*/'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND ISNULL(T.COMPANYID,'''') = '''+@COMPANYID  +''''"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND T.ITEMID IN (SELECT ITEMIDS FROM TEMPTABLEDB..TEMPITEMMAST)'  "
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND T.SUBITEMID IN (SELECT COMPANYID FROM TEMPSUBITEMMAST) ' "
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND ISNULL(T.SIZEID,0) IN (SELECT COMPANYID FROM TEMPITEMSIZE) ' "
            strSql += vbCrLf + " 			IF  @WITHORDER<>'Y' SELECT @STRQRY= @STRQRY+ CHAR(13) + '   AND ISNULL(T.ORSNO,'''')='''''"
            strSql += vbCrLf + " 			IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND ISNULL(T.COSTID,0) IN (SELECT COSTID FROM TEMPTABLEDB..TEMPCOSTCENTRE)'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ORDER BY DESIGNERNAME,ITEM'"
            strSql += vbCrLf + " 			IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,COSTNAME'"
            strSql += vbCrLf + " 			PRINT @STRQRY"
            strSql += vbCrLf + " 			EXEC (@STRQRY)"
            strSql += vbCrLf + " 			IF @RMODE = 'A'"
            strSql += vbCrLf + " 			BEGIN"
            strSql += vbCrLf + " 			SELECT @STRQRY =  ' UPDATE TEMPTABLEDB..DESIGNNONSTKORDER SET SUBITEM = ''OTHERS'''"
            strSql += vbCrLf + " 			PRINT @STRQRY"
            strSql += vbCrLf + " 			EXEC (@STRQRY)"
            strSql += vbCrLf + " 			END"
            strSql += vbCrLf + " 			IF @SIZE ='Y'"
            strSql += vbCrLf + " 			BEGIN"
            strSql += vbCrLf + " 			SELECT @STRQRY =  ' IF OBJECT_ID(''TEMPDB..DESIGNSTKORDER'') IS NOT NULL DROP TABLE DESIGNSTKORDER  '"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT SUBITEMNAME'"
            strSql += vbCrLf + " 			IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,COSTNAME'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CONVERT(VARCHAR(200),RANGE)RANGE,SUM(REPCS)REORDPCS,SUM(REWT)REORDWT,SUM(STKPCS)STKPCS,SUM(STKWT)STKWT'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CONVERT(NUMERIC(15,3),0)SDIFFPCS,CONVERT(NUMERIC(15,3),0)SDIFFWT'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CONVERT(NUMERIC(15,3),0)EDIFFPCS,CONVERT(NUMERIC(15,3),0)EDIFFWT'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,ITEM,SUBITEM,SIZENAME,'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' FROMWEIGHT, TOWEIGHT, RESULT,DESIGNER, COLHEAD'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INTO DESIGNSTKORDER  '"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' FROM('"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT'  "
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SM.SUBITEMNAME'"
            strSql += vbCrLf + " 			IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CC.COSTNAME'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,(CONVERT(VARCHAR(100),FROMWEIGHT) + '' TO '' + CONVERT(VARCHAR(100),TOWEIGHT) + '' ['' + CONVERT(VARCHAR(100),(CONVERT(DECIMAL (15,3),(CONVERT(VARCHAR(100),((FROMWEIGHT+TOWEIGHT)/2)))))) + '']'')AS RANGE,CONVERT(INT,SUM(T.PCS))STKPCS,CONVERT(NUMERIC(15,3),SUM(T.GRSWT))STKWT'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,(RE.PIECE) AS REPCS,(RE.WEIGHT) AS REWT'  "
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CONVERT(VARCHAR(100),SI.SIZENAME)SIZENAME'  "
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CONVERT(VARCHAR(100),IM.ITEMNAME) AS ITEM,CONVERT(VARCHAR(100),SM.SUBITEMNAME)AS SUBITEM '"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,RE.FROMWEIGHT,RE.TOWEIGHT'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CONVERT(INT,2) AS RESULT'  "
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CONVERT(VARCHAR(100),DE.DESIGNERNAME) AS DESIGNER'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CONVERT(VARCHAR(3),NULL)COLHEAD'     "
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' FROM  STKREORDER AS RE'  "
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INNER JOIN  ITEMMAST IM ON IM.ITEMID = RE.ITEMID '"
            strSql += vbCrLf + " 			IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INNER JOIN  COSTCENTRE CC ON CC.COSTID = RE.COSTID '"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' LEFT OUTER JOIN  DESIGNER DE ON DE.DESIGNERID=RE.DESIGNID'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' LEFT OUTER JOIN  SUBITEMMAST SM ON SM.ITEMID = RE.ITEMID AND SM.SUBITEMID = RE.SUBITEMID  '"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' LEFT OUTER JOIN ITEMTAG AS T ON T.RECDATE <= '''+@ASONDATE+''' AND (T.ISSDATE IS NULL OR T.ISSDATE > '''+@ASONDATE+''''"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' )AND T.ITEMID = RE.ITEMID '"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND T.SUBITEMID = RE.SUBITEMID '"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND T.GRSWT BETWEEN FROMWEIGHT AND TOWEIGHT'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND T.COMPANYID = '''+@COMPANYID+''' AND T.COSTID = RE.COSTID AND ISNULL(T.SIZEID,0)=ISNULL(RE.SIZEID,0)  '"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' LEFT OUTER JOIN  ITEMSIZE AS SI ON SI.SIZEID = RE.SIZEID  '"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' WHERE RE.ITEMID IN (SELECT ITEMIDS FROM TEMPTABLEDB..TEMPITEMMAST)    '"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND RE.SUBITEMID IN (SELECT COMPANYID FROM TEMPSUBITEMMAST)'    "
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND ISNULL(RE.SIZEID,0) IN (SELECT COMPANYID FROM TEMPITEMSIZE)    '"
            strSql += vbCrLf + " 			IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND ISNULL(RE.COSTID,0) IN (SELECT COSTID FROM TEMPTABLEDB..TEMPCOSTCENTRE)'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND IM.CALTYPE=''W'''"
            strSql += vbCrLf + " 			IF  @WITHORDER<>'Y' SELECT @STRQRY= @STRQRY+ CHAR(13) + '   AND ISNULL(T.ORSNO,'''')='''''"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' GROUP BY DESIGNERNAME'"
            strSql += vbCrLf + " 			IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,COSTNAME'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,ITEMNAME,SM.SUBITEMNAME,RE.FROMWEIGHT,RE.TOWEIGHT,SI.SIZENAME,RE.PIECE,RE.WEIGHT'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' UNION ALL'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT DISTINCT SM.SUBITEMNAME'"
            strSql += vbCrLf + " 			IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CC.COSTNAME'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,''OTHER RANGE'' RANGE,SUM(T.PCS)STKPCS ,SUM(T.GRSWT)STKWT,0 PIECE,0 WEIGHT'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CONVERT(VARCHAR(100),SI.SIZENAME)SIZENAME'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CONVERT(VARCHAR(100),IM.ITEMNAME) AS ITEM,CONVERT(VARCHAR(100),SM.SUBITEMNAME)AS SUBITEM '"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,0 FROMWEIGHT,0 TOWEIGHT    '"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CONVERT(INT,2) AS RESULT   '"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CONVERT(VARCHAR(100),DE.DESIGNERNAME) AS DESIGNER'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CONVERT(VARCHAR(3),NULL)COLHEAD'   "
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' FROM ITEMTAG AS T'    "
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' LEFT OUTER JOIN  DESIGNER DE ON DE.DESIGNERID=T.DESIGNERID'"
            strSql += vbCrLf + " 			IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INNER JOIN  COSTCENTRE CC ON CC.COSTID = T.COSTID'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' LEFT OUTER JOIN  SUBITEMMAST SM ON SM.ITEMID=T.ITEMID AND SM.SUBITEMID=T.SUBITEMID'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' LEFT OUTER JOIN  ITEMMAST IM ON IM.ITEMID=T.ITEMID '"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' LEFT OUTER JOIN  ITEMSIZE AS SI ON SI.SIZEID=T.SIZEID'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' WHERE NOT EXISTS (SELECT 1 FROM  STKREORDER RE WHERE T.ITEMID = RE.ITEMID AND T.SUBITEMID = RE.SUBITEMID '"
            strSql += vbCrLf + " 			IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND ISNULL(T.COSTID,'''')=ISNULL(RE.COSTID,'''') '"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' and  T.GRSWT between RE.FROMWEIGHT and RE.TOWEIGHT AND ISNULL(T.SIZEID,0)=ISNULL(RE.SIZEID,0))'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND T.RECDATE <= '''+@ASONDATE+'''  AND (T.ISSDATE IS NULL OR T.ISSDATE > '''+@ASONDATE+ ''''  "
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' )AND T.ITEMID IN (SELECT ITEMIDS FROM TEMPTABLEDB..TEMPITEMMAST)'    "
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND T.SUBITEMID IN (SELECT COMPANYID FROM TEMPSUBITEMMAST)'    "
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND ISNULL(T.SIZEID,0) IN (SELECT COMPANYID FROM TEMPITEMSIZE)'  "
            strSql += vbCrLf + " 			IF  @WITHORDER<>'Y' SELECT @STRQRY= @STRQRY+ CHAR(13) + '   AND ISNULL(T.ORSNO,'''')='''''  "
            strSql += vbCrLf + " 			IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND ISNULL(T.COSTID,0) IN (SELECT COSTID FROM TEMPTABLEDB..TEMPCOSTCENTRE)'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND IM.CALTYPE=''W'''"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' GROUP BY DESIGNERNAME'"
            strSql += vbCrLf + " 			IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CC.COSTNAME'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,ITEMNAME,SM.SUBITEMNAME,SI.SIZENAME'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' )X GROUP BY SUBITEMNAME'"
            strSql += vbCrLf + " 			IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,COSTNAME'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,FROMWEIGHT,TOWEIGHT,RANGE,DESIGNER,ITEM,SUBITEM,SIZENAME,'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' RESULT, COLHEAD'"
            strSql += vbCrLf + " 			PRINT @STRQRY"
            strSql += vbCrLf + " 			EXEC (@STRQRY)"
            strSql += vbCrLf + " 			/*UPDATE DESIGNSTKORDER SET DIFFPCS =STKPCS-REORDPCS,DIFFWT = STKWT-REORDWT*/"
            strSql += vbCrLf + " 			SELECT @STRQRY =   ' UPDATE DESIGNSTKORDER SET SDIFFPCS =(CASE WHEN ISNULL(STKPCS,0)-ISNULL(REORDPCS,0)<0 THEN ISNULL(STKPCS,0)-ISNULL(REORDPCS,0) ELSE 0 END)'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,SDIFFWT = (CASE WHEN ISNULL(STKWT,0)-ISNULL(REORDWT,0)<0 THEN ISNULL(STKWT,0)-ISNULL(REORDWT,0) ELSE 0 END)'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,EDIFFPCS =(CASE WHEN ISNULL(STKPCS,0)-ISNULL(REORDPCS,0)>0 THEN ISNULL(STKPCS,0)-ISNULL(REORDPCS,0) ELSE 0 END)'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,EDIFFWT = (CASE WHEN ISNULL(STKWT,0)-ISNULL(REORDWT,0)>0 THEN ISNULL(STKWT,0)-ISNULL(REORDWT,0) ELSE 0 END)'"
            strSql += vbCrLf + " 			PRINT @STRQRY"
            strSql += vbCrLf + " 			EXEC (@STRQRY)"
            strSql += vbCrLf + " 			SELECT @STRQRY = ' DECLARE @DESIGNLEN AS INT'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' DECLARE @DESIGNLEN_FWT AS INT'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' DECLARE @DESIGNLEN_TWT AS INT'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT @DESIGNLEN = MAX(LEN(SUBITEMNAME))+1 FROM DESIGNSTKORDER WHERE RESULT = 2'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT @DESIGNLEN_FWT = MAX(LEN(FROMWEIGHT)) FROM DESIGNSTKORDER'"
            strSql += vbCrLf + " 			SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT @DESIGNLEN_TWT = MAX(LEN(TOWEIGHT)) FROM DESIGNSTKORDER'"
            strSql += vbCrLf + " 			PRINT @STRQRY"
            strSql += vbCrLf + " 			EXEC (@STRQRY)"
            strSql += vbCrLf + " 			IF @RTYPE = 'S'"
            strSql += vbCrLf + " 			SELECT @STRQRY = ' DELETE FROM DESIGNSTKORDER WHERE NOT (SDIFFPCS < 0 OR SDIFFWT < 0)'"
            strSql += vbCrLf + " 			PRINT @STRQRY"
            strSql += vbCrLf + " 			EXEC (@STRQRY)"
            strSql += vbCrLf + " 			IF @RTYPE = 'E'"
            strSql += vbCrLf + " 			SELECT @STRQRY = ' DELETE FROM DESIGNSTKORDER WHERE NOT (SDIFFPCS > 0 OR SDIFFWT > 0)'"
            strSql += vbCrLf + " 			PRINT @STRQRY"
            strSql += vbCrLf + " 			EXEC (@STRQRY)"
            strSql += vbCrLf + " 			IF (SELECT COUNT(*) FROM DESIGNSTKORDER)>0  "
            strSql += vbCrLf + " 			BEGIN"
            strSql += vbCrLf + " 				SELECT @STRQRY =  ' UPDATE DESIGNSTKORDER SET SDIFFPCS=(SDIFFPCS*(-1)) WHERE SDIFFPCS < 0'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' UPDATE DESIGNSTKORDER SET SDIFFWT=(SDIFFWT*(-1)) WHERE SDIFFWT < 0 '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INSERT INTO DESIGNSTKORDER(DESIGNER,ITEM,SUBITEMNAME,RESULT,COLHEAD)'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT DISTINCT DESIGNER,ITEM,DESIGNER,0 RESULT,''T''COLHEAD FROM DESIGNSTKORDER'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INSERT INTO DESIGNSTKORDER(DESIGNER,ITEM,SUBITEMNAME,RESULT,COLHEAD)'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT DISTINCT DESIGNER,ITEM,ITEM,1 RESULT,''N''COLHEAD FROM DESIGNSTKORDER'"
            strSql += vbCrLf + " 				/*INSERT INTO DESIGNSTKORDER1 (DESIGNER,ITEM,SUBITEMNAME,RESULT,COLHEAD)"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT DISTINCT DESIGNER,ITEM,SUBITEMNAME,4 RESULT,'E'COLHEAD FROM DESIGNSTKORDER1*/"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INSERT INTO DESIGNSTKORDER (DESIGNER,ITEM,SUBITEMNAME,RESULT,COLHEAD,REORDPCS,REORDWT,STKPCS,STKWT,SDIFFPCS,SDIFFWT,EDIFFPCS,EDIFFWT,SUBITEM)'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT DISTINCT DESIGNER,ITEM,''SUB TOTAL'',3 RESULT,''S''COLHEAD,'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SUM(REORDPCS),SUM(REORDWT),SUM(STKPCS),SUM(STKWT),SUM(SDIFFPCS),SUM(SDIFFWT),SUM(EDIFFPCS),SUM(EDIFFWT),''ZZ.SUB TOTAL'''"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' FROM DESIGNSTKORDER GROUP BY DESIGNER,ITEM'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INSERT INTO DESIGNSTKORDER(DESIGNER,ITEM,SUBITEMNAME,RESULT,COLHEAD,REORDPCS,REORDWT,STKPCS,STKWT,SDIFFPCS,SDIFFWT,EDIFFPCS,EDIFFWT,SUBITEM)'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' Select DISTINCT ''ZZZZZZZ'',''ZZZZZZZZZZZZ'',''GRAND TOTAL'',5 RESULT,''G'' COLHEAD'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,SUM(REORDPCS),SUM(REORDWT),SUM(STKPCS),SUM(STKWT),SUM(SDIFFPCS),SUM(SDIFFWT),SUM(EDIFFPCS),SUM(EDIFFWT),''GRAND TOTAL'''"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' FROM DESIGNSTKORDER WHERE RESULT IN (2) '"
            strSql += vbCrLf + " 				PRINT @STRQRY"
            strSql += vbCrLf + " 				EXEC (@STRQRY)"
            strSql += vbCrLf + " 				/*UPDATE DESIGNSTKORDER SET SUBITEM = SUBITEMNAME WHERE COLHEAD = 'E'*/"
            strSql += vbCrLf + " 			END"
            strSql += vbCrLf + " 			END "
            strSql += vbCrLf + " 			IF @SIZE='N'"
            strSql += vbCrLf + " 			BEGIN"
            strSql += vbCrLf + " 				SELECT @STRQRY =  ' IF OBJECT_ID(''TEMPTABLEDB..DESIGNSTKORDER1'') IS NOT NULL DROP TABLE TEMPTABLEDB..DESIGNSTKORDER1 '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT SUBITEMNAME'"
            strSql += vbCrLf + " 				IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,COSTNAME'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CONVERT(VARCHAR(200),RANGE)RANGE,SUM(REPCS)REORDPCS,SUM(REWT)REORDWT,SUM(STKPCS)STKPCS,SUM(STKWT)STKWT'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CONVERT(NUMERIC(15,3),0)SDIFFPCS,CONVERT(NUMERIC(15,3),0)SDIFFWT'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CONVERT(NUMERIC(15,3),0)EDIFFPCS,CONVERT(NUMERIC(15,3),0)EDIFFWT'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,ITEM,SUBITEM,FROMWEIGHT,TOWEIGHT,RESULT,DESIGNER,COLHEAD'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INTO TEMPTABLEDB..DESIGNSTKORDER1'  "
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' FROM'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ('"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT ' "
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SM.SUBITEMNAME'"
            strSql += vbCrLf + " 				IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CC.COSTNAME'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,(CONVERT(VARCHAR(100),FROMWEIGHT) + '' TO '' + CONVERT(VARCHAR(100),TOWEIGHT) + '' ['' + CONVERT(VARCHAR(100),(CONVERT(DECIMAL (15,3),(CONVERT(VARCHAR(100),((FROMWEIGHT+TOWEIGHT)/2)))))) + '']'')AS RANGE,CONVERT(INT,SUM(T.PCS))STKPCS,CONVERT(NUMERIC(15,3),SUM(T.GRSWT))STKWT'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,(RE.PIECE) AS REPCS,(RE.WEIGHT) AS REWT ' "
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CONVERT(VARCHAR(100),IM.ITEMNAME) AS ITEM,CONVERT(VARCHAR(100),SM.SUBITEMNAME)AS SUBITEM' "
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,RE.FROMWEIGHT,RE.TOWEIGHT'    "
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CONVERT(INT,2) AS RESULT '   "
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CONVERT(VARCHAR(100),DE.DESIGNERNAME) AS DESIGNER'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CONVERT(VARCHAR(3),NULL)COLHEAD'     "
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' FROM  STKREORDER AS RE ' "
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INNER JOIN  ITEMMAST IM ON IM.ITEMID = RE.ITEMID'  "
            strSql += vbCrLf + " 				IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INNER JOIN  COSTCENTRE CC ON CC.COSTID = RE.COSTID'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' LEFT OUTER JOIN  DESIGNER DE ON DE.DESIGNERID=RE.DESIGNID'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' LEFT OUTER JOIN  SUBITEMMAST SM ON SM.ITEMID = RE.ITEMID AND SM.SUBITEMID = RE.SUBITEMID'  "
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' LEFT OUTER JOIN  ITEMTAG AS T ON T.RECDATE <= '''+@ASONDATE+''' AND (T.ISSDATE IS NULL OR T.ISSDATE > '''+@ASONDATE+ ''''"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' )AND T.ITEMID = RE.ITEMID '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND T.SUBITEMID = RE.SUBITEMID '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND T.GRSWT BETWEEN FROMWEIGHT AND TOWEIGHT'"
            strSql += vbCrLf + " 				IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND T.COMPANYID = '''+@COMPANYID+''' AND T.COSTID = RE.COSTID   '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' WHERE RE.ITEMID IN (SELECT ITEMIDS FROM TEMPTABLEDB..TEMPITEMMAST)'    "
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND RE.SUBITEMID IN (SELECT COMPANYID FROM TEMPSUBITEMMAST)'    "
            strSql += vbCrLf + " 				IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND ISNULL(RE.COSTID,0) IN (SELECT COSTID FROM TEMPTABLEDB..TEMPCOSTCENTRE)'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND RE.RANGEMODE=''W'''"
            strSql += vbCrLf + " 				IF  @WITHORDER<>'Y' SELECT @STRQRY= @STRQRY+ CHAR(13) + '   AND ISNULL(T.ORSNO,'''')='''''"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' GROUP BY DESIGNERNAME'"
            strSql += vbCrLf + " 				IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CC.COSTNAME'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,ITEMNAME,SM.SUBITEMNAME,RE.FROMWEIGHT,RE.TOWEIGHT,RE.PIECE,RE.WEIGHT'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' UNION ALL'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT DISTINCT SM.SUBITEMNAME'"
            strSql += vbCrLf + " 				IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CC.COSTNAME'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,''OTHER RANGE'' RANGE,SUM(T.PCS)STKPCS ,SUM(T.GRSWT)STKWT,0 PIECE,0 WEIGHT'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CONVERT(VARCHAR(100),IM.ITEMNAME) AS ITEM,CONVERT(VARCHAR(100),SM.SUBITEMNAME)AS SUBITEM '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,0 FROMWEIGHT,0 TOWEIGHT '   "
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CONVERT(INT,2) AS RESULT '   "
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CONVERT(VARCHAR(100),DE.DESIGNERNAME) AS DESIGNER'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CONVERT(VARCHAR(3),NULL)COLHEAD   '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' FROM  ITEMTAG AS T'    "
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' LEFT OUTER JOIN  DESIGNER DE ON DE.DESIGNERID=T.DESIGNERID'"
            strSql += vbCrLf + " 				IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INNER JOIN  COSTCENTRE CC ON CC.COSTID = T.COSTID'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' LEFT outer jOIN  SUBITEMMAST SM ON SM.ITEMID=T.ITEMID AND SM.SUBITEMID=T.SUBITEMID'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' LEFT OUTER JOIN  ITEMMAST IM ON IM.ITEMID=T.ITEMID' "
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' WHERE NOT EXISTS (Select 1 from  STKREORDER RE WHERE T.ITEMID = RE.ITEMID AND T.SUBITEMID = RE.SUBITEMID'"
            strSql += vbCrLf + " 				IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + '  AND ISNULL(T.COSTID,'''')=ISNULL(RE.COSTID,'''') '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' and  T.GRSWT between RE.FROMWEIGHT and RE.TOWEIGHT)'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND T.RECDATE <= '''+@ASONDATE+'''  AND (T.ISSDATE IS NULL OR T.ISSDATE >'''+ @ASONDATE+ ''''  "
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' )AND T.ITEMID IN (SELECT ITEMIDS FROM TEMPTABLEDB..TEMPITEMMAST)    '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND T.SUBITEMID IN (SELECT COMPANYID FROM TEMPSUBITEMMAST)' "
            strSql += vbCrLf + " 				IF  @WITHORDER<>'Y' SELECT @STRQRY= @STRQRY+ CHAR(13) + '   AND ISNULL(T.ORSNO,'''')='''''   "
            strSql += vbCrLf + " 				IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' AND ISNULL(T.COSTID,0) IN (SELECT COSTID FROM TEMPTABLEDB..TEMPCOSTCENTRE)'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' GROUP BY DESIGNERNAME'"
            strSql += vbCrLf + " 				IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,CC.COSTNAME'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,ITEMNAME,SM.SUBITEMNAME'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' )X GROUP BY RANGE,SUBITEMNAME'"
            strSql += vbCrLf + " 				IF LEN(@COSTID)>0 SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,COSTNAME'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,FROMWEIGHT,TOWEIGHT,DESIGNER,ITEM,SUBITEM,RESULT,COLHEAD'"
            strSql += vbCrLf + " 				PRINT @STRQRY"
            strSql += vbCrLf + " 				EXEC (@STRQRY)"
            strSql += vbCrLf + " 				/*UPDATE DESIGNSTKORDER1 SET DIFFPCS =isnull(STKPCS,0)-isnull(REORDPCS,0),DIFFWT = Isnull(STKWT,0)-isnull(REORDWT,0)*/"
            strSql += vbCrLf + " 				SELECT @STRQRY =  ' UPDATE TEMPTABLEDB..DESIGNSTKORDER1 SET SDIFFPCS =(CASE WHEN ISNULL(STKPCS,0)-ISNULL(REORDPCS,0)<0 THEN ISNULL(STKPCS,0)-ISNULL(REORDPCS,0) ELSE 0 END)'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,SDIFFWT = (CASE WHEN ISNULL(STKWT,0)-ISNULL(REORDWT,0)<0 THEN ISNULL(STKWT,0)-ISNULL(REORDWT,0) ELSE 0 END)'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,EDIFFPCS =(CASE WHEN ISNULL(STKPCS,0)-ISNULL(REORDPCS,0)>0 THEN ISNULL(STKPCS,0)-ISNULL(REORDPCS,0) ELSE 0 END)'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,EDIFFWT = (CASE WHEN ISNULL(STKWT,0)-ISNULL(REORDWT,0)>0 THEN ISNULL(STKWT,0)-ISNULL(REORDWT,0) ELSE 0 END)'"
            strSql += vbCrLf + " 				PRINT @STRQRY"
            strSql += vbCrLf + " 				EXEC (@STRQRY)"
            strSql += vbCrLf + " 				SELECT @STRQRY =  ' DECLARE @DESIGNLEN1 AS INT'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' DECLARE @DESIGNLEN_FWT1 AS INT'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' DECLARE @DESIGNLEN_TWT1 AS INT'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT @DESIGNLEN1 = MAX(LEN(SUBITEMNAME))+1 FROM TEMPTABLEDB..DESIGNSTKORDER1 WHERE RESULT = 2'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT @DESIGNLEN_FWT1 = MAX(LEN(FROMWEIGHT)) FROM TEMPTABLEDB..DESIGNSTKORDER1'"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT @DESIGNLEN_TWT1 = MAX(LEN(TOWEIGHT)) FROM TEMPTABLEDB..DESIGNSTKORDER1'"
            strSql += vbCrLf + " 				PRINT @STRQRY"
            strSql += vbCrLf + " 				EXEC (@STRQRY)"
            strSql += vbCrLf + " 				/*UPDATE #STKORDER SET RANGE =  RANGE + REPLICATE(' ',@LEN-LEN(RANGE)) + '[' + REPLICATE(' ',@LEN_FWT - LEN(FROMWEIGHT)) + CONVERT(VARCHAR,FROMWEIGHT) + ' TO ' + REPLICATE(' ',@LEN_TWT - LEN(TOWEIGHT)) + CONVERT(VARCHAR,TOWEIGHT) + ']' WHERE RESULT = 1*/"
            strSql += vbCrLf + " 				IF @RTYPE = 'S'"
            strSql += vbCrLf + " 				SELECT @STRQRY =  ' DELETE FROM TEMPTABLEDB..DESIGNSTKORDER1 WHERE NOT (SDIFFPCS < 0 OR SDIFFWT < 0)'"
            strSql += vbCrLf + " 				PRINT @STRQRY"
            strSql += vbCrLf + " 				EXEC (@STRQRY)"
            strSql += vbCrLf + " 				IF @RTYPE = 'E'"
            strSql += vbCrLf + " 				SELECT @STRQRY =  ' DELETE FROM TEMPTABLEDB..DESIGNSTKORDER1 WHERE NOT (EDIFFPCS > 0 OR EDIFFWT > 0)'"
            strSql += vbCrLf + " 				PRINT @STRQRY"
            strSql += vbCrLf + " 				EXEC (@STRQRY)"
            strSql += vbCrLf + " 				IF (SELECT COUNT(*) FROM TEMPTABLEDB..DESIGNSTKORDER1)>0  "
            strSql += vbCrLf + " 				BEGIN"
            strSql += vbCrLf + " 					SELECT @STRQRY =  ' UPDATE TEMPTABLEDB..DESIGNSTKORDER1 SET SDIFFPCS=(SDIFFPCS*(-1)) WHERE SDIFFPCS < 0'"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' UPDATE TEMPTABLEDB..DESIGNSTKORDER1 SET SDIFFWT=(SDIFFWT*(-1)) WHERE SDIFFWT < 0 '"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INSERT INTO TEMPTABLEDB..DESIGNSTKORDER1(DESIGNER,ITEM,SUBITEMNAME,RESULT,COLHEAD)'"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT DISTINCT DESIGNER,ITEM,DESIGNER,0 RESULT,''T''COLHEAD FROM TEMPTABLEDB..DESIGNSTKORDER1'"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INSERT INTO TEMPTABLEDB..DESIGNSTKORDER1(DESIGNER,ITEM,SUBITEMNAME,RESULT,COLHEAD)'"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT DISTINCT DESIGNER,ITEM,ITEM,1 RESULT,''N''COLHEAD FROM TEMPTABLEDB..DESIGNSTKORDER1'"
            strSql += vbCrLf + " 					/*INSERT INTO DESIGNSTKORDER1 (DESIGNER,ITEM,SUBITEMNAME,RESULT,COLHEAD)"
            strSql += vbCrLf + " 					SELECT DISTINCT DESIGNER,ITEM,SUBITEMNAME,4 RESULT,'E'COLHEAD FROM DESIGNSTKORDER1*/"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INSERT INTO TEMPTABLEDB..DESIGNSTKORDER1 (DESIGNER,ITEM,SUBITEMNAME,RESULT,COLHEAD,REORDPCS,REORDWT,STKPCS,STKWT,SDIFFPCS,SDIFFWT,EDIFFPCS,EDIFFWT,SUBITEM)'"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SELECT DISTINCT DESIGNER,ITEM,''SUB TOTAL'',3 RESULT,''S''COLHEAD,'"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' SUM(REORDPCS),SUM(REORDWT),SUM(STKPCS),SUM(STKWT),SUM(SDIFFPCS),SUM(SDIFFWT),SUM(EDIFFPCS),SUM(EDIFFWT),''ZZ.SUB TOTAL'''"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' FROM TEMPTABLEDB..DESIGNSTKORDER1 GROUP BY DESIGNER,ITEM'"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' INSERT INTO TEMPTABLEDB..DESIGNSTKORDER1(DESIGNER,ITEM,SUBITEMNAME,RESULT,COLHEAD,REORDPCS,REORDWT,STKPCS,STKWT,SDIFFPCS,SDIFFWT,EDIFFPCS,EDIFFWT,SUBITEM)  '"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' Select DISTINCT ''ZZZZZZZ'',''ZZZZZZZZZZZZ'',''GRAND TOTAL'',5 RESULT,''G'' COLHEAD'"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' ,SUM(REORDPCS),SUM(REORDWT),SUM(STKPCS),SUM(STKWT),SUM(SDIFFPCS),SUM(SDIFFWT),SUM(EDIFFPCS),SUM(EDIFFWT),''GRAND TOTAL'''"
            strSql += vbCrLf + " 					SELECT @STRQRY= @STRQRY+ CHAR(13) + ' FROM TEMPTABLEDB..DESIGNSTKORDER1 WHERE RESULT IN (2) '"
            strSql += vbCrLf + " 					PRINT @STRQRY"
            strSql += vbCrLf + " 					EXEC (@STRQRY)"
            strSql += vbCrLf + " 					/*UPDATE DESIGNSTKORDER1 SET SUBITEM = SUBITEMNAME WHERE COLHEAD = 'E'*/"
            strSql += vbCrLf + " 				END"
            strSql += vbCrLf + " 			END"
            strSql += vbCrLf + " 			/*IF @SIZE='Y'"
            strSql += vbCrLf + " 			BEGIN"
            strSql += vbCrLf + " 				/*SELECT CASE WHEN COLHEAD = 'E' THEN '' ELSE SUBITEMNAME END SUBITEMNAME,*/"
            strSql += vbCrLf + " 				SELECT @STRQRY =  ' SELECT SUBITEMNAME,[RANGE],REORDPCS,REORDWT,STKPCS,STKWT,SDIFFPCS,SDIFFWT,EDIFFPCS,EDIFFWT,DESIGNER,ITEM,SUBITEM,FROMWEIGHT,TOWEIGHT,RESULT,COLHEAD,SIZENAME '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' FROM TEMPTABLEDB..DESIGNSTKORDER T ORDER BY DESIGNER,ITEM,SUBITEM,RESULT,SIZENAME,FROMWEIGHT,TOWEIGHT,[RANGE],COLHEAD'"
            strSql += vbCrLf + " 				PRINT @STRQRY"
            strSql += vbCrLf + " 				EXEC (@STRQRY)"
            strSql += vbCrLf + " 			END"
            strSql += vbCrLf + " 			IF @SIZE='N'"
            strSql += vbCrLf + " 			BEGIN"
            strSql += vbCrLf + " 				/*SELECT CASE WHEN COLHEAD = 'E' THEN '' ELSE SUBITEMNAME END SUBITEMNAME,*/"
            strSql += vbCrLf + " 				SELECT @STRQRY =  ' SELECT SUBITEMNAME,[RANGE],REORDPCS,REORDWT,STKPCS,STKWT,SDIFFPCS,SDIFFWT,EDIFFPCS,EDIFFWT,DESIGNER,ITEM,SUBITEM,FROMWEIGHT,TOWEIGHT,RESULT,COLHEAD '"
            strSql += vbCrLf + " 				SELECT @STRQRY= @STRQRY+ CHAR(13) + ' FROM TEMPTABLEDB..DESIGNSTKORDER1 T ORDER BY DESIGNER,ITEM,SUBITEM,RESULT,[RANGE],FROMWEIGHT,TOWEIGHT'"
            strSql += vbCrLf + " 				PRINT @STRQRY"
            strSql += vbCrLf + " 				EXEC (@STRQRY)"
            strSql += vbCrLf + " 			END*/"
            strSql += vbCrLf + " 	END"
            strSql += vbCrLf + " END"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPITEMSIZE') IS NOT NULL DROP TABLE TEMPITEMSIZE"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPCOUNTER') IS NOT NULL DROP TABLE TEMPCOUNTER"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPSUBITEMMAST') IS NOT NULL DROP TABLE TEMPSUBITEMMAST"
            strSql += vbCrLf + " IF OBJECT_ID('TEMPCOSTCENTRE') IS NOT NULL DROP TABLE TEMPCOSTCENTRE"
            cmd = New OleDbCommand(strSql, cn, tran)
            cmd.ExecuteNonQuery()

        End If
        strSql = " EXEC " & dbName & "..REM_SP_RPT_STOCKREORDERNEW"
        strSql += vbCrLf + " @METALID = 'ALL'"
        strSql += vbCrLf + " ,@ITEMID = 'ALL'"
        strSql += vbCrLf + " ,@SUBITEMID = 'ALL'"
        strSql += vbCrLf + " ,@COSTID = ''"
        strSql += vbCrLf + " ,@COUNTER = 'ALL'"
        strSql += vbCrLf + " ,@SIZEID='ALL'"
        strSql += vbCrLf + " ,@ASONDATE = '" & Date.Now.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + " ,@COMPANYID = '" & dbName.Replace("ADMINDB", "") & "'"
        strSql += vbCrLf + " ,@RTYPE = 'B'"
        strSql += vbCrLf + " ,@RMODE = ''"
        strSql += vbCrLf + " ,@SIZE='N'"
        strSql += vbCrLf + " ,@WITHORDER='Y'"
        strSql += vbCrLf + " ,@DESIGNREORDER='Y'"
        cmd = New OleDbCommand(strSql, cn)
        cmd.ExecuteNonQuery()

        strSql = " SELECT SUBITEMNAME,[RANGE],SDIFFPCS,SDIFFWT "
        strSql += vbCrLf + " ,EDIFFPCS ,EDIFFWT  ,RESULT,COLHEAD "
        strSql += vbCrLf + " FROM TEMPTABLEDB..DESIGNSTKORDER1 T WHERE RESULT=2 "
        strSql += vbCrLf + " ORDER BY DESIGNER,ITEM,SUBITEM,RESULT,[RANGE],FROMWEIGHT,TOWEIGHT"
        cmd = New OleDbCommand(strSql, cn)
        cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        Return dt
    End Function

#End Region

#Region "User Define Function"
    Public Shared Function Decrypt(ByVal Pwd As String) As String
        Dim strDecryptPwd As String = Nothing
        Try
            For cnt As Integer = 1 To Len(Pwd)
                Dim IntAscii As Integer = 0
                IntAscii = (Asc(Strings.Mid(Pwd, cnt, 1)) - (cnt * 2) - 14)
                strDecryptPwd = strDecryptPwd & Chr(IntAscii)
            Next
        Catch ex As Exception
            MsgBox("ERROR :" & ex.Message & vbCrLf & vbCrLf & "POSITION :" & ex.StackTrace)
        End Try
        Return strDecryptPwd
    End Function
#End Region


    ''' <summary>
    ''' Not used At Present Future may be
    ''' </summary>
    ''' <remarks></remarks>


#Region "Taskbar Notification"

#End Region

    'Dim HourTimer As DateTime
    'Dim lbl1 As String

    'Private Sub funRemainder()
    '    HourTimer = DateTime.Now
    '    lbl1 = HourTimer.ToString
    '    Timer1.Interval = 10
    '    Timer1.Enabled = True
    '    Timer2.Interval = 10
    '    Timer2.Enabled = True
    '    Timer3.Interval = 10
    '    Timer3.Enabled = True
    '    Timer1.Start()
    'End Sub

    'Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
    '    Dim oneHours As DateTime = DateTime.Now
    '    lbl1 = oneHours.ToString
    '    Dim result As TimeSpan = oneHours - HourTimer
    '    If result.Minutes > 1 Then
    '        Timer1.Stop()
    '        HourTimer = DateTime.Now
    '        lbl1 = HourTimer.ToString
    '    End If
    '    Timer2.Start()
    'End Sub

    'Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
    '    Dim oneHours As DateTime = DateTime.Now
    '    lbl1 = oneHours.ToString
    '    Dim result As TimeSpan = oneHours - HourTimer
    '    If result.Minutes > 1 Then
    '        Timer2.Stop()
    '        HourTimer = DateTime.Now
    '        lbl1 = HourTimer.ToString
    '    End If
    '    Timer3.Start()
    'End Sub

    'Private Sub Timer3_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer3.Tick
    '    Dim oneHours As DateTime = DateTime.Now
    '    lbl1 = oneHours.ToString
    '    Dim result As TimeSpan = oneHours - HourTimer
    '    If result.Minutes > 1 Then
    '        Timer3.Stop()
    '        HourTimer = DateTime.Now
    '        lbl1 = HourTimer.ToString
    '    End If
    '    Timer1.Start()
    'End Sub

    Private Sub frmMain_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Dispose()
    End Sub

    
End Class