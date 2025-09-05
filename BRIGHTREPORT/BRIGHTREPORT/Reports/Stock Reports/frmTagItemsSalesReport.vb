Imports System.Data.OleDb
Public Class frmTagItemsSalesReport
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim objGridShower As frmGridDispDia

    Private Sub frmTagItemsSalesReport_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        'strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMPTAGSALETITLE')> 0"
        'strSql += "     DROP TABLE TEMPTAGSALETITLE"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()

        'strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMPTAGSALE')> 0"
        'strSql += "     DROP TABLE TEMPTAGSALE"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()

        'strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMPTAGSTONESALE')> 0"
        'strSql += "    DROP TABLE TEMPTAGSTONESALE"
        'cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        'cmd.ExecuteNonQuery()
    End Sub


    Private Sub frmTagItemsSalesReport_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub frmTagItemsSalesReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        grpContainer.Location = New Point((ScreenWid - grpContainer.Width) / 2, ((ScreenHit - 128) - grpContainer.Height) / 2)
        strSql = " SELECT METALNAME FROM " & cnAdminDb & "..METALMAST WHERE ACTIVE = 'Y' ORDER BY METALNAME"
        FillCheckedListBox(strSql, chkLstMetal)
        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            chkLstCostCentre.Enabled = True
            chkLstCostCentre.Items.Clear()
            strSql = " SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            'FillCheckedListBox(strSql, chkLstCostCentre)
            Dim dt As New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            For i As Integer = 0 To dt.Rows.Count - 1
                If cnCostName = dt.Rows(i).Item("COSTNAME").ToString And cnDefaultCostId = False Then
                    chkLstCostCentre.Items.Add(dt.Rows(i).Item("COSTNAME").ToString, True)
                Else
                    chkLstCostCentre.Items.Add(dt.Rows(i).Item("COSTNAME").ToString, False)
                End If
            Next
            If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkLstCostCentre.Enabled = False
        Else
            chkLstCostCentre.Enabled = False
            chkLstCostCentre.Items.Clear()
        End If
        strSql = " SELECT ITEMCTRNAME FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ACTIVE = 'Y' ORDER BY ITEMCTRNAME"
        FillCheckedListBox(strSql, chkLstItemCounter)
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        objGPack.TextClear(Me)
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        dtpFrom.Select()
        Prop_Gets()
    End Sub
    Private Sub chkMetalSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkMetalSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstMetal, chkMetalSelectAll.Checked)
    End Sub

    Private Sub chkCostCentreSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCostCentreSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstCostCentre, chkCostCentreSelectAll.Checked)
    End Sub

    Private Sub chkItemCounterSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkItemCounterSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstItemCounter, chkItemCounterSelectAll.Checked)
    End Sub

    Private Sub chkItemAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkItemSelectAll.CheckedChanged
        SetChecked_CheckedList(chkLstItem, chkItemSelectAll.Checked)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        btnSearch.Enabled = False
        If Not CheckBckDays(userId, Me.Name, dtpFrom.Value) Then dtpFrom.Focus() : Exit Sub
        Dim chkCostName As String = GetChecked_CheckedList(chkLstCostCentre)
        Dim chkItemCounter As String = GetChecked_CheckedList(chkLstItemCounter)
        Dim chkMetalName As String = GetChecked_CheckedList(chkLstMetal)
        Dim chkItemName As String = GetChecked_CheckedList(chkLstItem)
        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMPTAGSALETITLE')> 0" + vbCrLf
        strSql += vbCrLf + "     DROP TABLE TEMPTAGSALETITLE" + vbCrLf
        strSql += vbCrLf + " SELECT 'TAG ITEM SALES REPORT FROM " & dtpFrom.Text & " TO " & dtpTo.Text & "'  AS TITLE INTO TEMPTAGSALETITLE " + vbCrLf
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMPTAGSALE')> 0" + vbCrLf
        strSql += vbCrLf + "     DROP TABLE TEMPTAGSALE" + vbCrLf
        strSql += vbCrLf + " DECLARE @DEFPATH VARCHAR(200)" + vbCrLf
        strSql += vbCrLf + " SELECT @DEFPATH = '" & _DefaultPic & "'" + vbCrLf
        strSql += vbCrLf + " Select  SNO,PARTICULAR,TRANDATE,TAGNO,PCS,PURITY,GRSWT,NETWT,RATE,WASTAGE, MC,AMOUNT,DESIGNER,PCTFILE,REMARK1,RESULT  INTO TEMPTAGSALE  FROM ("
        strSql += vbCrLf + " SELECT SNO" + vbCrLf
        strSql += vbCrLf + " ,CASE WHEN SUBITEMID <> 0 THEN (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.SUBITEMID AND ITEMID = T.ITEMID)" + vbCrLf
        strSql += vbCrLf + " ELSE (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID) END AS PARTICULAR" + vbCrLf
        strSql += vbCrLf + " ,TRANDATE,TAGNO,PCS,PURITY,GRSWT,NETWT,RATE,WASTAGE,MCHARGE MC,AMOUNT" + vbCrLf
        strSql += vbCrLf + " ,(SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERID = T.TAGDESIGNER)AS DESIGNER" + vbCrLf
        strSql += vbCrLf + " ,(SELECT TOP 1 PCTFILE FROM " & cnAdminDb & "..ITEMTAG WHERE TAGNO = T.TAGNO AND ITEMID = T.ITEMID AND BATCHNO = T.BATCHNO) AS PCTFILE,REMARK1,0 AS RESULT" + vbCrLf
        strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE AS T" + vbCrLf
        strSql += vbCrLf + "  WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + "  AND ISNULL(TAGNO,'') <> '' AND ISNULL(T.CANCEL,'') = '' AND TRANTYPE='SA'"
        If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If chkItemCounter <> "" Then strSql += vbCrLf + " AND ITEMCTRID IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & chkItemCounter & "))"
        If chkItemName <> "" Then strSql += vbCrLf + " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & chkItemName & "))"
        If chkMetalName <> "" Then strSql += vbCrLf + " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        strSql += vbCrLf + " UNION ALL" + vbCrLf
        strSql += vbCrLf + " SELECT '' AS  SNO,'TOTAL' AS PARTICULAR" + vbCrLf
        strSql += vbCrLf + ",''AS  TRANDATE,'' AS  TAGNO,SUM(PCS) PCS,0 AS PURITY,SUM(GRSWT)GRSWT ,SUM(NETWT)NETWT,0 RATE,SUM(WASTAGE) WASTAGE,SUM(MCHARGE) MC,SUM(AMOUNT) AMOUNT" + vbCrLf
        strSql += vbCrLf + ",'' DESIGNER,'' AS PCTFILE,'' REMARK1,1 AS RESULT" + vbCrLf
        strSql += vbCrLf + "  FROM " & cnStockDb & "..ISSUE AS T" + vbCrLf
        strSql += vbCrLf + "  WHERE TRANDATE BETWEEN '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + "  AND ISNULL(TAGNO,'') <> '' AND ISNULL(T.CANCEL,'') = '' AND TRANTYPE='SA'"
        If chkCostName <> "" Then strSql += vbCrLf + " AND COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & chkCostName & "))"
        If chkItemCounter <> "" Then strSql += vbCrLf + " AND ITEMCTRID IN (SELECT ITEMCTRID FROM " & cnAdminDb & "..ITEMCOUNTER WHERE ITEMCTRNAME IN (" & chkItemCounter & "))"
        If chkItemName <> "" Then strSql += vbCrLf + " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & chkItemName & "))"
        If chkMetalName <> "" Then strSql += vbCrLf + " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & ")))"
        strSql += vbCrLf + ")X ORDER BY RESULT" + vbCrLf
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " ALTER TABLE TEMPTAGSALE ADD TAGIMAGE IMAGE"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        strSql = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMPTAGSTONESALE')> 0" + vbCrLf
        strSql += vbCrLf + "    DROP TABLE TEMPTAGSTONESALE" + vbCrLf
        strSql += vbCrLf + " SELECT ISSSNO," + vbCrLf
        strSql += vbCrLf + " CASE WHEN STNSUBITEMID <> 0 THEN (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = T.STNSUBITEMID AND ITEMID = T.STNITEMID)" + vbCrLf
        strSql += vbCrLf + " ELSE (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.STNITEMID) END AS PARTICULAR" + vbCrLf
        strSql += vbCrLf + " ,STNPCS,STNWT,STNRATE,STNAMT,CASE WHEN CALCMODE = 'W' THEN 'WIGHT' ELSE 'PIECE' END CALCMODE" + vbCrLf
        strSql += vbCrLf + " ,CASE WHEN STONEUNIT = 'C' THEN 'CARAT' ELSE 'GRAM' END STONEUNIT,(SELECT DIASTONE FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.STNITEMID)STNTYPE" + vbCrLf
        strSql += vbCrLf + " INTO TEMPTAGSTONESALE" + vbCrLf
        strSql += vbCrLf + " FROM " & cnStockDb & "..ISSSTONE AS T" + vbCrLf
        strSql += vbCrLf + " WHERE ISSSNO IN (SELECT SNO FROM TEMPTAGSALE)" + vbCrLf
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        cmd.ExecuteNonQuery()

        Dim dtImage As New DataTable
        strSql = " SELECT SNO,PCTFILE FROM TEMPTAGSALE"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtImage)
        If Not dtImage.Rows.Count > 0 Then
            MsgBox("Record Not Found", MsgBoxStyle.Information)
            btnSearch.Enabled = True
            Exit Sub
        End If
        Try
            For Each ro As DataRow In dtImage.Rows
                Dim serverPath As String = Nothing
                Dim fileDestPath As String = _DefaultPic & ro!PCTFILE.ToString
                If IO.File.Exists(fileDestPath) Then
                    Dim Finfo As IO.FileInfo
                    Finfo = New IO.FileInfo(fileDestPath)
                    'Finfo.IsReadOnly = False
                    If IO.Directory.Exists(Finfo.Directory.FullName) Then
                        Dim fileStr As New IO.FileStream(fileDestPath, IO.FileMode.Open, IO.FileAccess.Read)
                        Dim reader As New IO.BinaryReader(fileStr)
                        Dim result As Byte() = reader.ReadBytes(CType(fileStr.Length, Integer))
                        fileStr.Read(result, 0, result.Length)
                        fileStr.Close()

                        strSql = " UPDATE TEMPTAGSALE SET TAGIMAGE = ? WHERE SNO = '" & ro!SNO.ToString & "'"
                        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
                        cmd.Parameters.AddWithValue("@image", result)
                        cmd.ExecuteNonQuery()
                    End If
                End If
            Next
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            Exit Sub
        Finally
            btnSearch.Enabled = True
            btnSearch.Focus()
        End Try
        'ProgressBarShow(ProgressBarStyle.Marquee)
        'ProgressBarStep("Loading Report")
        Dim objReport As New BrighttechReport
        Dim objRptViewer As New frmReportViewer
        objRptViewer.CrystalReportViewer1.ReportSource = objReport.ShowReport(New rptTagItemSalesReport, cnDataSource)
        ' objRptViewer.MdiParent = Main
        objRptViewer.Dock = DockStyle.Fill
        objRptViewer.Show()
        objRptViewer.CrystalReportViewer1.Select()
        'ProgressBarHide()
        Prop_Sets()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub chkLstItem_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLstItem.GotFocus
        Dim chkMetalName As String = GetChecked_CheckedList(chkLstMetal)
        strSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST "
        strSql += " WHERE ACTIVE = 'Y'"
        If chkMetalName <> "" Then strSql += " AND METALID IN (SELECT METALID FROM " & cnAdminDb & "..METALMAST WHERE METALNAME IN (" & chkMetalName & "))"
        strSql += " ORDER BY ITEMNAME"
        FillCheckedListBox(strSql, chkLstItem)
        SetChecked_CheckedList(chkLstItem, chkItemSelectAll.Checked)
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New frmTagItemsSalesReport_Reports
        'obj.p_chkCostCentreSelectAll = chkCostCentreSelectAll.Checked
        'GetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre)
        obj.p_chkMetalSelectAll = chkMetalSelectAll.Checked
        GetChecked_CheckedList(chkLstMetal, obj.p_chkLstMetal)
        obj.p_chkItemSelectAll = chkItemSelectAll.Checked
        GetChecked_CheckedList(chkLstItem, obj.p_chkLstItem)
        obj.p_chkItemCounterSelectAll = chkItemCounterSelectAll.Checked
        GetChecked_CheckedList(chkLstItemCounter, obj.p_chkLstItemCounter)
        SetSettingsObj(obj, Me.Name, GetType(frmTagItemsSalesReport_Reports))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmTagItemsSalesReport_Reports
        GetSettingsObj(obj, Me.Name, GetType(frmTagItemsSalesReport_Reports))
        'chkCostCentreSelectAll.Checked = obj.p_chkCostCentreSelectAll
        'SetChecked_CheckedList(chkLstCostCentre, obj.p_chkLstCostCentre, cnCostName)
        chkMetalSelectAll.Checked = obj.p_chkMetalSelectAll
        SetChecked_CheckedList(chkLstMetal, obj.p_chkLstMetal, Nothing)
        chkItemSelectAll.Checked = obj.p_chkItemSelectAll
        SetChecked_CheckedList(chkLstItem, obj.p_chkLstItem, Nothing)
        chkItemCounterSelectAll.Checked = obj.p_chkItemCounterSelectAll
        SetChecked_CheckedList(chkLstItemCounter, obj.p_chkLstItemCounter, Nothing)

    End Sub
End Class


Public Class frmTagItemsSalesReport_Reports

    Private chkCostCentreSelectAll As Boolean = False
    Public Property p_chkCostCentreSelectAll() As Boolean
        Get
            Return chkCostCentreSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkCostCentreSelectAll = value
        End Set
    End Property

    Private chkLstCostCentre As New List(Of String)
    Public Property p_chkLstCostCentre() As List(Of String)
        Get
            Return chkLstCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkLstCostCentre = value
        End Set
    End Property
    Private chkMetalSelectAll As Boolean = False
    Public Property p_chkMetalSelectAll() As Boolean
        Get
            Return chkMetalSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkMetalSelectAll = value
        End Set
    End Property

    Private chkLstMetal As New List(Of String)
    Public Property p_chkLstMetal() As List(Of String)
        Get
            Return chkLstMetal
        End Get
        Set(ByVal value As List(Of String))
            chkLstMetal = value
        End Set
    End Property

    Private chkItemSelectAll As Boolean = False
    Public Property p_chkItemSelectAll() As Boolean
        Get
            Return chkItemSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkItemSelectAll = value
        End Set
    End Property

    Private chkLstItem As New List(Of String)
    Public Property p_chkLstItem() As List(Of String)
        Get
            Return chkLstItem
        End Get
        Set(ByVal value As List(Of String))
            chkLstItem = value
        End Set
    End Property

    Private chkItemCounterSelectAll As Boolean = False
    Public Property p_chkItemCounterSelectAll() As Boolean
        Get
            Return chkItemCounterSelectAll
        End Get
        Set(ByVal value As Boolean)
            chkItemCounterSelectAll = value
        End Set
    End Property

    Private chkLstItemCounter As New List(Of String)
    Public Property p_chkLstItemCounter() As List(Of String)
        Get
            Return chkLstItemCounter
        End Get
        Set(ByVal value As List(Of String))
            chkLstItemCounter = value
        End Set
    End Property
End Class