Imports System.Data.OleDb
Public Class frmSubsidLed
    Dim objGridShower As frmGridDispDia
    Dim strSql As String
    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim ds As New DataSet
    Dim dt As New DataTable
    Dim dtAcName As New DataTable
    Dim dtcostcentre As New DataTable

    Private Sub Prop_Sets()
        Dim obj As New frmSubsidled_Properties
        'GetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre)
        obj.p_cmbMetal = cmbMetal.Text
        SetSettingsObj(obj, Me.Name, GetType(frmSubsidled_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New frmSubsidled_Properties
        GetSettingsObj(obj, Me.Name, GetType(frmSubsidled_Properties))
        'SetChecked_CheckedList(chkCmbCostCentre, obj.p_chkCmbCostCentre, "ALL")
        cmbMetal.Text = obj.p_cmbMetal
    End Sub


    Private Sub frmSubsidLed_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        funcnew()
    End Sub
    Private Sub funcnew()
        dtpFrom.Value = GetServerDate()
        dtpTo.Value = GetServerDate()
        cmbMetal.Items.Clear()
        strSql = " SELECT 'ALL' AS CAPTION"
        strSql += " UNION ALL"
        strSql += " SELECT CAPTION FROM " & cnAdminDb & "..ACCENTRYMASTER "
        'strSql += " WHERE TYPE NOT IN('R','P') "
        'strSql += " ORDER BY DISPLAYORDER"
        objGPack.FillCombo(strSql, cmbMetal, False)
        strSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT COSTNAME,CONVERT(vARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
        strSql += " ORDER BY RESULT,COSTNAME"
        dtcostcentre = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtcostcentre)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtcostcentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
        If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
    End Sub


    Public Function GetSelectedcostId(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = ""
        End If
        Return retStr
    End Function
    Public Function GetSelectedAccCode(ByVal chkLst As BrighttechPack.CheckedComboBox, ByVal WithQuotes As Boolean) As String
        Dim retStr As String = ""
        If chkLst.Items.Count > 0 Then
            For cnt As Integer = 0 To chkLst.CheckedItems.Count - 1
                If WithQuotes Then retStr += "'"
                retStr += objGPack.GetSqlValue("SELECT ACCODE FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME = '" & chkLst.CheckedItems.Item(cnt).ToString & "'")
                If WithQuotes Then retStr += "'"
                If cnt <> chkLst.CheckedItems.Count - 1 Then
                    retStr += ","
                End If
            Next
        Else
            retStr = ""
        End If
        Return retStr
    End Function

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim paymode As String = objGPack.GetSqlValue("SELECT PAYMODE FROM " & cnAdminDb & "..ACCENTRYMASTER WHERE CAPTION = '" & cmbMetal.Text & "'")
        Dim SelectedCostId As String = GetSelectedcostId(chkCmbCostCentre, False)
        Dim selectedAccCode As String = GetSelectedAccCode(chkCmbAcName, False)
        Dim IsNewFormat As Boolean = False
        strSql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='ACBOOKRPTFORMAT_NEW'"
        Dim dtTemp As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtTemp)
        If dtTemp.Rows.Count > 0 Then
            If dtTemp.Rows(0).Item("CTLTEXT").ToString.ToUpper = "Y" Then
                IsNewFormat = True
            Else
                IsNewFormat = False
            End If
        End If
        If paymode = "" Then paymode = "ALL"
        'If paymode = "" Then MsgBox("Please select One valid Subsidy Book") : Exit Sub
        If IsNewFormat = True Then
            strSql = "EXEC " & cnAdminDb & "..SP_RPT_ACCSUBDYLEDGNEW"
        Else
            strSql = "EXEC " & cnAdminDb & "..SP_RPT_ACCSUBDYLEDG"
        End If
        strSql += vbCrLf + " @DBNAME='" & cnStockDb & "'"
        strSql += vbCrLf + ",@TEMPTAB= 'TEMP" & systemId & "SUBDY'"
        strSql += vbCrLf + ",@FROMDATE='" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + ",@TODATE='" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        strSql += vbCrLf + ",@COMPANYID='" & strCompanyId & "'"
        strSql += vbCrLf + ",@COSTID='" & SelectedCostId & "'"
        strSql += vbCrLf + ",@SUBDYLEDG='" & paymode & "'"
        strSql += vbCrLf + ",@ACCODE='" & selectedAccCode & "'"
        strSql += vbCrLf + ",@WITHCANCEL='" & IIf(chkwithcancel.Checked, "Y", "N") & "'"
        da = New OleDbDataAdapter(strSql, cn)
        ds = New DataSet()
        dt = New DataTable()
        da.Fill(ds)
        dt = ds.Tables(0)
        If dt.Rows.Count > 0 Then
            objGridShower = New frmGridDispDia
            objGridShower.Name = Me.Name
            objGridShower.gridView.RowTemplate.Height = 21
            objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
            objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            objGridShower.Size = Me.Size
            objGridShower.Text = cmbMetal.Text & " BOOK"
            Dim tit As String = cmbMetal.Text & " BOOK"
            tit += vbCrLf & " For the period " + dtpFrom.Text + " TO " + dtpTo.Text
            If chkCmbCostCentre.Enabled = True Then tit += vbCrLf & "COST CENTRE :" & Replace(GetChecked_CheckedList(chkCmbCostCentre), "'", "")

            objGridShower.lblTitle.Text = tit
            AddHandler objGridShower.gridView.ColumnWidthChanged, AddressOf objGridShower.gridView_ColumnWidthChanged
            objGridShower.StartPosition = FormStartPosition.CenterScreen
            objGridShower.dsGrid.DataSetName = objGridShower.Name
            objGridShower.dsGrid = ds.Copy()
            objGridShower.formuser = userId
            objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
            objGridShower.FormReSize = True
            objGridShower.FormReLocation = False
            objGridShower.pnlFooter.Visible = False
            objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            FormatGridColumns(objGridShower.gridView, False, False, , False)
            objGridShower.gridView.Columns("Particular").Width = 400
            objGridShower.gridView.Columns("CANCEL").Visible = False
            objGridShower.Show()
            objGridShower.FormReSize = True
            objGridShower.gridViewHeader.Visible = True
            For i As Integer = 0 To objGridShower.gridView.Rows.Count - 1
                With objGridShower.gridView.Rows(i)
                    If .Cells("CANCEL").Value = "Y" Then
                        .DefaultCellStyle.BackColor = Color.LightPink
                    End If
                End With
                
            Next
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        funcnew()
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub frmSubsidLed_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub chkCmbAcName_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCmbAcName.Enter
        strSql = " SELECT 'ALL' ACNAME,'ALL' ACCODE,1 RESULT"
        strSql += " UNION ALL"
        strSql += " SELECT ACNAME,CONVERT(vARCHAR,ACCODE),2 RESULT FROM " & cnAdminDb & "..ACHEAD"
      
        strSql += " ORDER BY RESULT,ACNAME"
        dtAcName = New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtAcName)
        BrighttechPack.GlobalMethods.FillCombo(chkCmbAcName, dtAcName, "ACNAME", , "ALL")
    End Sub
End Class
Public Class frmSubsidled_Properties
    Private chkCmbCostCentre As New List(Of String)
    Public Property p_chkCmbCostCentre() As List(Of String)
        Get
            Return chkCmbCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCostCentre = value
        End Set
    End Property
    Private cmbMetal As String = "ALL"
    Public Property p_cmbMetal() As String
        Get
            Return cmbMetal
        End Get
        Set(ByVal value As String)
            cmbMetal = value
        End Set
    End Property

End Class