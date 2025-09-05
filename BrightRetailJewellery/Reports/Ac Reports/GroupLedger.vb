Imports System.Data.OleDb
Imports System.IO
Imports System.Text
Imports System.Net.Mail
Imports System.Net.Mail.SmtpClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Public Class GroupLedger

    Enum GRType
        GroupLedger = 0
        ConfirmationLetter = 1
    End Enum
    Dim objGridShower As frmGridDispDia
    Dim StrSql As String
    Dim Cmd As OleDbCommand
    Dim Da As OleDbDataAdapter
    Dim frmType As GRType = GRType.GroupLedger
    Dim PdfFilePath As String = ""

    Dim PdfMail As Boolean = IIf(GetAdmindbSoftValue("MAILPDF_CFORMLETTER", "N") = "Y", True, False)
    Dim dtCostCentre As New DataTable

    Public Sub New(ByVal frmType As GRType)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.frmType = frmType
    End Sub

    Private Sub GroupLedger_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
    Private Sub GroupLedger_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GrpContainer.Location = New Point((ScreenWid - GrpContainer.Width) / 2, ((ScreenHit - 128) - GrpContainer.Height) / 2)
        If GetAdmindbSoftValue("COSTCENTRE") = "Y" Then
            chkCmbCostCentre.Enabled = True
            StrSql = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
            StrSql += " UNION ALL"
            StrSql += " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & cnAdminDb & "..COSTCENTRE"
            StrSql += " ORDER BY RESULT,COSTNAME"
            dtCostCentre = New DataTable
            Da = New OleDbDataAdapter(StrSql, cn)
            Da.Fill(dtCostCentre)
            BrighttechPack.GlobalMethods.FillCombo(chkCmbCostCentre, dtCostCentre, "COSTNAME", , IIf(cnDefaultCostId, "ALL", cnCostName))
            If strUserCentrailsed <> "Y" And cnDefaultCostId = False Then chkCmbCostCentre.Enabled = False
        Else
            chkCmbCostCentre.Enabled = False
        End If
        StrSql = " SELECT ACGRPNAME FROM " & cnAdminDb & "..ACGROUP ORDER BY ACGRPNAME"
        chklstgroup.Items.Clear()
        FillCheckedListBox(StrSql, chklstgroup)
        If frmType = GRType.ConfirmationLetter Then
            GrpConf.Visible = True
            ChkSepCol.Visible = False
        Else
            Dim Ls As Integer = GrpConf.Height
            btnExit.Location = New Drawing.Point(btnExit.Location.X, (btnExit.Location.Y - Ls))
            btnSearch.Location = New Drawing.Point(btnSearch.Location.X, (btnSearch.Location.Y - Ls))
            btnNew.Location = New Drawing.Point(btnNew.Location.X, (btnNew.Location.Y - Ls))
            GrpContainer.Height = GrpContainer.Height - Ls
            GrpConf.Visible = False
            ChkSepCol.Visible = True
        End If
        PdfFilePath = GetAdmindbSoftValue("PDF_FILEPATH", "")
        If PdfMail = True Then
            If PdfFilePath.Trim = "" Then
                MsgBox("PDF file path not found.", MsgBoxStyle.Information)
            End If
        End If
        chkgpletterwithled_CheckedChanged(Me, New EventArgs())
        btnNew_Click(Me, New EventArgs)
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click, NewToolStripMenuItem.Click
        dtpFrom.Value = cnTranFromDate
        dtpTo.Value = cnTranToDate
        chkWithAddress.Checked = True
        'rbtDetailed.Checked = True
        Prop_Gets()
        dtpFrom.Select()
    End Sub

    Private Sub chkCostCentreSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'SetChecked_CheckedList(chkLstCostCentre, chkCostCentreSelectAll.Checked)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim chkCostName As String = Nothing
        Dim chkCostID As String = Nothing
        Dim crdr As String = Nothing
        Dim chkgroupName As String = Nothing
        Dim chkAcName As String = Nothing
        Dim RptType As String = Nothing
        Prop_Sets()
        If chkCmbCostCentre.Text <> "ALL" And chkCmbCostCentre.Text <> "" Then
            chkCostName = GetQryString(chkCmbCostCentre.Text)
            chkCostName = chkCostName.Replace("'", "")
            chkCostID = GetSelectedCostId(chkCmbCostCentre, True)
        Else
            chkCostName = "ALL"
            chkCostID = "ALL"
        End If

        If chklstgroup.CheckedItems.Count = chklstgroup.Items.Count Then
            chkgroupName = "ALL"
        Else
            chkgroupName = GetChecked_CheckedList(chklstgroup, False)
        End If

        If chklstAcname.CheckedItems.Count = chklstAcname.Items.Count Then
            chkAcName = "ALL"
        Else
            chkAcName = GetChecked_CheckedList(chklstAcname, False)
        End If

        If rbtDetailed.Checked = True Then
            RptType = "D"
        ElseIf rbtSummary.Checked = True Then
            RptType = "S"
        End If

        If ChkSepCol.Checked = True Then
            crdr = "Y"
        Else
            crdr = "N"
        End If

        If GrpConf.Visible Then
            If chkgpletterwithled.Checked = True And chkconfletter.Checked = True And chksmithbal.Checked = False Then
                RptType = "A"
            ElseIf (chkgpletterwithled.Checked = True And chkconfletter.Checked = True And chksmithbal.Checked = True) _
            Or (chkconfletter.Checked = True And chksmithbal.Checked = True) Then
                RptType = "W"
            ElseIf chkconfletter.Checked = True Then
                RptType = "L"
            End If
        End If
        Dim Remark2 As String = IIf(GetAdmindbSoftValue("CFORMLETTER-REMARK2", "N") = "Y", "Y", "N")
        StrSql = vbCrLf + " IF OBJECT_ID('TEMPTABLEDB..GROUPLEDGER_RES''" & systemId & "''') IS NOT NULL DROP TABLE TEMPTABLEDB..GROUPLEDGER_RES" & systemId & ""
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()

        StrSql = vbCrLf + " EXEC " & cnStockDb & "..SP_RPT_GROUPLEDGER"
        StrSql += vbCrLf + " @FROMDATE = '" & dtpFrom.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " ,@TODATE = '" & dtpTo.Value.ToString("yyyy-MM-dd") & "'"
        StrSql += vbCrLf + " ,@RPTYPE = '" & RptType & "'"
        StrSql += vbCrLf + " ,@COSTNAME = '" & chkCostName & "'"
        StrSql += vbCrLf + " ,@ACGRPNAME = '" & IIf(chkgroupName = "", "ALL", chkgroupName) & "'"
        StrSql += vbCrLf + " ,@ACCNAME = '" & IIf(chkAcName = "", "ALL", chkAcName) & "'"
        StrSql += vbCrLf + " ,@COMPANYID = '" & strCompanyId & "'"
        StrSql += vbCrLf + " ,@NILBAL='" & IIf(ChkNilBalance.Checked, "Y", "N") & "'"
        StrSql += vbCrLf + " ,@SYSID='" & systemId & "'"
        StrSql += vbCrLf + " ,@CRDR='" & crdr & "' "
        StrSql += vbCrLf + " ,@FORMAT='" & Remark2 & "' "
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.CommandTimeout = 1000

        If RptType = "A" Or RptType = "L" Or RptType = "W" Then
            Cmd.ExecuteNonQuery()
            StrSql = vbCrLf + " IF OBJECT_ID('" & cnStockDb & "..GROUPLEDGER_RES') IS NOT NULL DROP TABLE " & cnStockDb & "..GROUPLEDGER_RES"
            StrSql += vbCrLf + " SELECT * INTO   " & cnStockDb & "..GROUPLEDGER_RES FROM TEMPTABLEDB..GROUPLEDGER_RES" & systemId & " ORDER BY ACNAME1,CONVERT(DATETIME,ISNULL(TRANDATE,''),103),TRANNO"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            Dim objReport As New GiritechReport
            Dim objRptViewer As New frmReportViewer
            StrSql = "SELECT COMPANYNAME,ADDRESS1,ADDRESS2,ADDRESS3,AREACODE,CASE WHEN ISNULL(PHONE,'') <> '' THEN 'PH : ' + PHONE ELSE '' END PHONE,CASE WHEN ISNULL(EMAIL,'') <> '' THEN 'EMAIL : ' + EMAIL ELSE '' END EMAIL,PANNO,LOCALTAXNO AS TINNO,CSTNO,GSTNO "
            StrSql = StrSql & " from " & cnAdminDb & "..company where companyid='" & strCompanyId & "'"
            Dim tdt As New DataTable
            Dim tda As New OleDbDataAdapter(StrSql, cn)
            tda.Fill(tdt)
            Dim objrepcompany As New CrystalDecisions.Shared.ParameterValues
            Dim objrepcompany1 As New CrystalDecisions.Shared.ParameterDiscreteValue
            Dim objrepcompany2 As New CrystalDecisions.Shared.ParameterValues
            Dim objrepcompany3 As New CrystalDecisions.Shared.ParameterDiscreteValue
            Dim objrepaddress As New CrystalDecisions.Shared.ParameterValues
            Dim objrepaddress1 As New CrystalDecisions.Shared.ParameterDiscreteValue
            Dim objrepmail As New CrystalDecisions.Shared.ParameterValues
            Dim objrepmail1 As New CrystalDecisions.Shared.ParameterDiscreteValue
            Dim objrptpandet As New CrystalDecisions.Shared.ParameterValues
            Dim objrptpandet1 As New CrystalDecisions.Shared.ParameterDiscreteValue
            Dim objrepsmithbal As New CrystalDecisions.Shared.ParameterValues
            Dim objrepsmithbal1 As New CrystalDecisions.Shared.ParameterDiscreteValue
            Dim objFromDate As New CrystalDecisions.Shared.ParameterDiscreteValue
            Dim objToDate As New CrystalDecisions.Shared.ParameterDiscreteValue
            Dim objTinDeatils As New CrystalDecisions.Shared.ParameterDiscreteValue
            Dim objGstno As New CrystalDecisions.Shared.ParameterValues
            Dim objGstno1 As New CrystalDecisions.Shared.ParameterDiscreteValue
            objTinDeatils.Value = "TIN: " & tdt.Rows(0).Item("TINNO").ToString & ", CST: " & tdt.Rows(0).Item("CSTNO").ToString & ", PAN: " & tdt.Rows(0).Item("PANNO").ToString
            objFromDate.Value = dtpFrom.Value.Date.ToString("dd-MM-yyyy")
            objToDate.Value = dtpTo.Value.Date.ToString("dd-MM-yyyy")

            If chkWithAddress.Checked Then
                objrepcompany1.Value = tdt.Rows(0).Item("companyname").ToString
                objrepcompany3.Value = tdt.Rows(0).Item("companyname").ToString
                objrepaddress1.Value = tdt.Rows(0).Item("address1").ToString & "," & tdt.Rows(0).Item("address2").ToString & "," & vbCrLf + tdt.Rows(0).Item("address3").ToString
                objrepmail1.Value = tdt.Rows(0).Item("areacode").ToString & tdt.Rows(0).Item("phone").ToString & ", " & tdt.Rows(0).Item("email").ToString
                objrptpandet1.Value = tdt.Rows(0).Item("panno").ToString
                objGstno1.Value = "GSTNO : " + tdt.Rows(0).Item("GSTNO").ToString
            Else
                objrepcompany1.Value = ""
                objrepaddress1.Value = ""
                objrepmail1.Value = ""
                objrptpandet1.Value = tdt.Rows(0).Item("panno").ToString
                objGstno1.Value = tdt.Rows(0).Item("GSTNO").ToString
                objrepcompany3.Value = tdt.Rows(0).Item("companyname").ToString
            End If


            objrepsmithbal1.Value = ""
            objrepsmithbal.Add(objrepsmithbal1)
            objrepcompany.Add(objrepcompany1)
            objrepcompany2.Add(objrepcompany3)
            objrepaddress.Add(objrepaddress1)
            objrepmail.Add(objrepmail1)
            objrptpandet.Add(objrptpandet1)
            objGstno.Add(objGstno1)


            Dim Nathellaform As Boolean = IIf(GetAdmindbSoftValue("CFORMLETTER-NATHELLA", "N") = "Y", True, False)
            Dim s As String = ""
            Dim objCformletter As New Cformletter
            Dim objCformletters As New Cformletters
            Dim objCformletter2 As New Cformletter2

            If Nathellaform = True And Remark2 <> "Y" Then
                objCformletter.SetParameterValue("Rptprmcompany", objrepcompany1)
                objCformletter.SetParameterValue("rptprmaddress1", objrepaddress1)
                objCformletter.SetParameterValue("repprmaddress2", objrepmail1)
                objCformletter.SetParameterValue("rptpandet", objrptpandet)
                objCformletter.SetParameterValue("rptGSTNO", objGstno)
                objCformletter.SetParameterValue("rptsmithbal", objrepsmithbal)
                If chkgpletterwithled.Checked = False Then
                    objCformletter.GroupHeaderSection6.SectionFormat.EnableSuppress = True
                    objCformletter.GroupHeaderSection5.SectionFormat.EnableNewPageAfter = False
                Else
                    objCformletter.GroupHeaderSection6.SectionFormat.EnableSuppress = False
                    objCformletter.GroupHeaderSection5.SectionFormat.EnableNewPageAfter = True
                End If
            ElseIf Nathellaform = True And Remark2 = "Y" Then
                objCformletter2.SetParameterValue("Rptprmcompany", objrepcompany1)
                objCformletter2.SetParameterValue("rptprmaddress1", objrepaddress1)
                objCformletter2.SetParameterValue("repprmaddress2", objrepmail1)
                objCformletter2.SetParameterValue("rptpandet", objrptpandet)
                objCformletter2.SetParameterValue("rptGSTNO", objGstno)
                objCformletter2.SetParameterValue("rptsmithbal", objrepsmithbal)
                objCformletter2.SetParameterValue("FromDate", objFromDate)
                objCformletter2.SetParameterValue("ToDate", objToDate)
                objCformletter2.SetParameterValue("TINDETAILS", objTinDeatils)
                If chkgpletterwithled.Checked = False Then
                    objCformletter2.GroupHeaderSection6.SectionFormat.EnableSuppress = True
                    objCformletter2.GroupHeaderSection5.SectionFormat.EnableNewPageAfter = False
                Else
                    objCformletter2.GroupHeaderSection6.SectionFormat.EnableSuppress = False
                    objCformletter2.GroupHeaderSection5.SectionFormat.EnableNewPageAfter = True
                End If
            Else
                objCformletters.SetParameterValue("Rptprmcompany", objrepcompany1)
                objCformletters.SetParameterValue("rptprmaddress1", objrepaddress1)
                objCformletters.SetParameterValue("repprmaddress2", objrepmail1)
                objCformletters.SetParameterValue("rptpandet", objrptpandet)
                objCformletters.SetParameterValue("rptGSTNO", objGstno)
                objCformletters.SetParameterValue("rptsmithbal", objrepsmithbal)
                objCformletters.SetParameterValue("Rptprmcompany1", objrepcompany3)
                If chkgpletterwithled.Checked = False Then
                    objCformletters.GroupHeaderSection6.SectionFormat.EnableSuppress = True
                    objCformletters.GroupHeaderSection5.SectionFormat.EnableNewPageAfter = False
                Else
                    objCformletters.GroupHeaderSection6.SectionFormat.EnableSuppress = False
                    objCformletters.GroupHeaderSection5.SectionFormat.EnableNewPageAfter = True
                End If
            End If

            StrSql = "IF OBJECT_ID('TEMPTABLEDB..TEMPSIGNATURE')IS NOT NULL DROP TABLE TEMPTABLEDB..TEMPSIGNATURE "
            StrSql += vbCrLf + " CREATE TABLE TEMPTABLEDB..TEMPSIGNATURE(SIGN IMAGE)"
            Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
            Cmd.ExecuteNonQuery()

            Dim fileDestPath As String = Application.StartupPath & "\SIGN.bmp"
            If IO.File.Exists(fileDestPath) Then
                Dim Finfo As IO.FileInfo
                Finfo = New IO.FileInfo(fileDestPath)
                Dim bmp As New Bitmap(Finfo.FullName)
                Dim width As Integer = bmp.Width
                Dim height As Integer = bmp.Height
                Dim resizeimg As Boolean = False
                If width > 3000 Then
                    width = 3000
                    resizeimg = True
                End If
                If height > 2400 Then
                    height = 2400
                    resizeimg = True
                End If
                bmp.Dispose()
                If resizeimg = True Then
                    Dim fileName = Finfo.FullName
                    Dim CropRect As New Rectangle(0, 0, width, height)
                    Dim OrignalImage = Image.FromFile(fileName)
                    Dim CropImage = New Bitmap(CropRect.Width, CropRect.Height)
                    Using grp = Graphics.FromImage(CropImage)
                        grp.DrawImage(OrignalImage, New Rectangle(0, 0, CropRect.Width, CropRect.Height), CropRect, GraphicsUnit.Pixel)
                        OrignalImage.Dispose()
                        CropImage.Save(fileName)
                    End Using
                End If

                Finfo.IsReadOnly = False
                If IO.Directory.Exists(Finfo.Directory.FullName) Then
                    Dim fileStr As New IO.FileStream(fileDestPath, IO.FileMode.Open, IO.FileAccess.Read)
                    Dim reader As New IO.BinaryReader(fileStr)
                    Dim result As Byte() = reader.ReadBytes(CType(fileStr.Length, Integer))
                    fileStr.Read(result, 0, result.Length)
                    fileStr.Close()
                    StrSql = " INSERT INTO MASTER..TEMPSIGNATURE(SIGN)VALUES (?)"
                    Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
                    Cmd.Parameters.AddWithValue("@image", result)
                    Cmd.ExecuteNonQuery()
                End If
            End If

            If Nathellaform = True And Remark2 <> "Y" Then
                objRptViewer.CrystalReportViewer1.ReportSource = objReport.ShowReport(objCformletter, cnDataSource, cnStockDb)
            ElseIf Nathellaform = True And Remark2 = "Y" Then
                objRptViewer.CrystalReportViewer1.ReportSource = objReport.ShowReport(objCformletter2, cnDataSource, cnStockDb)
            Else
                objRptViewer.CrystalReportViewer1.ReportSource = objReport.ShowReport(objCformletters, cnDataSource, cnStockDb)
            End If
            objRptViewer.MdiParent = Main
            objRptViewer.ShowInTaskbar = False
            objRptViewer.WindowState = FormWindowState.Maximized
            objRptViewer.Dock = DockStyle.Fill
            objRptViewer.Show()
            objRptViewer.CrystalReportViewer1.Select()
            If PdfMail And chklstAcname.CheckedItems.Count = 1 Then
                If MsgBox("Do you want to send mail? ", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    If PdfFilePath.Trim = "" Then
                        MsgBox("PDF file path not found. Cannot send mail", MsgBoxStyle.Information)
                        Exit Sub
                    End If
                    Try
                        Dim notify_Email As NotifyIcon
                        Dim AcName As String = GetChecked_CheckedList(chklstAcname, False)
                        Dim EmailId As String = objGPack.GetSqlValue("SELECT EMAILID FROM " & cnAdminDb & "..ACHEAD WHERE ACNAME='" & AcName & "'")
                        notify_Email = New NotifyIcon()
                        notify_Email.Text = "Creating Pdf for " + AcName
                        notify_Email.Visible = True
                        notify_Email.Icon = My.Resources.email
                        notify_Email.ShowBalloonTip(20000, "Information", "Creating Pdf for " + AcName, ToolTipIcon.Info)
                        Dim CrExportOptions As ExportOptions
                        Dim CrDiskFileDestinationOptions As New DiskFileDestinationOptions()
                        Dim CrFormatTypeOptions As New PdfRtfWordFormatOptions()
                        Dim filePath As String
                        filePath = PdfFilePath
                        If IO.File.Exists(filePath + "\CformLetter_" & AcName & "_" & GetServerDate().Replace("-", "") & ".pdf") Then IO.File.Delete(filePath + "\CformLetter_" & AcName & "_" & GetServerDate().Replace("-", "") & ".pdf")
                        CrDiskFileDestinationOptions.DiskFileName = filePath + "\CformLetter_" & AcName & "_" & GetServerDate().Replace("-", "") & ".pdf"
                        CrFormatTypeOptions.UsePageRange = False
                        If Nathellaform = True Then
                            CrExportOptions = objCformletter.ExportOptions
                        Else
                            CrExportOptions = objCformletters.ExportOptions
                        End If
                        With CrExportOptions
                            .ExportDestinationType = ExportDestinationType.DiskFile
                            .ExportFormatType = ExportFormatType.PortableDocFormat
                            .DestinationOptions = CrDiskFileDestinationOptions
                            .FormatOptions = CrFormatTypeOptions
                        End With
                        If Nathellaform = True Then
                            objCformletter.Export()
                        Else
                            objCformletters.Export()
                        End If
                        Dim SEND As Boolean = False
                        If EmailId <> "" Then
                            notify_Email.Text = "Email is sending to " + AcName
                            notify_Email.ShowBalloonTip(20000, "Email notification", "Email is sending to " + AcName, ToolTipIcon.Info)
                            If NEWMAILSEND(EmailId, "Dear Sir/Madam,<br/><br/>&nbsp;&nbsp;<br/><br/><br/><br/>Accounts Confirmation Report.&nbsp;<br/><br/>Please find the Attachment . <br/><br/>", filePath + "\CformLetter.pdf") = 1 Then
                                SEND = True
                            Else
                                SEND = False
                            End If
                            notify_Email.ShowBalloonTip(5000, "Email notification", "Email successfully send to " + AcName, ToolTipIcon.Info)
                        End If
                        If SEND = True Then
                            MsgBox("Email successfully send to " + AcName, MsgBoxStyle.Information)
                        Else
                            MsgBox("Email sending failed to  " + AcName, MsgBoxStyle.Information)
                        End If
                        notify_Email.Visible = False
                        notify_Email.Dispose()

                    Catch ex As Exception
                        Dim msg As String = "Email sending failed!"
                        msg += vbCrLf + ex.Message
                        MsgBox(msg, MsgBoxStyle.Information)
                    End Try

                End If
            End If
            Exit Sub
        End If
        Dim dtGrid As New DataTable
        dtGrid.Columns.Add("KEYNO", GetType(Integer))
        dtGrid.Columns("KEYNO").AutoIncrement = True
        dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
        dtGrid.Columns("KEYNO").AutoIncrementStep = 1
        Da = New OleDbDataAdapter(Cmd)
        Da.Fill(dtGrid)
        dtGrid.Columns("KEYNO").SetOrdinal(dtGrid.Columns.Count - 1)
        If Not dtGrid.Rows.Count > 0 Then
            MsgBox("Record not found", MsgBoxStyle.Information)
            Exit Sub
        End If
        objGridShower = New frmGridDispDia
        objGridShower.Name = Me.Name
        objGridShower.gridView.RowTemplate.Height = 21
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Font = New Font("verdana", 8, FontStyle.Bold)
        objGridShower.gridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        objGridShower.Text = "GROUP LEDGER"
        Dim tit As String = strCompanyName + "GROUP LEDGER" + vbCrLf
        tit += " DATE FROM " & dtpFrom.Text + " TO " + dtpTo.Text
        tit += IIf(chkCostName <> "", " :" & chkCostName, "")
        objGridShower.lblTitle.Text = tit
        objGridShower.StartPosition = FormStartPosition.CenterScreen
        objGridShower.dsGrid.DataSetName = objGridShower.Name
        objGridShower.dsGrid.Tables.Add(dtGrid)
        objGridShower.gridView.DataSource = objGridShower.dsGrid.Tables(0)
        objGridShower.FormReSize = False
        objGridShower.FormReLocation = False
        objGridShower.pnlFooter.Visible = False
        objGridShower.gridViewHeader.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
        DataGridView_Formatting(objGridShower.gridView)
        FormatGridColumns(objGridShower.gridView, False, False, , False)
        objGridShower.Show()
        objGridShower.FormReSize = True
        objGridShower.FormReLocation = True
        If ChkSepCol.Checked Then
            objGridShower.gridViewHeader.Visible = True
            GridViewHeaderCreator(objGridShower.gridViewHeader)
        Else
            objGridShower.gridViewHeader.Visible = False
        End If
        FillGridGroupStyle_KeyNoWise(objGridShower.gridView)
        objGridShower.gridView_ColumnWidthChanged(Me, New DataGridViewColumnEventArgs(objGridShower.gridView.Columns("PARTICULAR")))
        StrSql = vbCrLf + " IF OBJECT_ID('" & cnStockDb & "..GROUPLEDGER_RES') IS NOT NULL DROP TABLE " & cnStockDb & "..GROUPLEDGER_RES"
        Cmd = New OleDbCommand(StrSql, cn) : Cmd.CommandTimeout = 1000
        Cmd.CommandTimeout = 1000
        Cmd.ExecuteNonQuery()
    End Sub
    Private Sub DataGridView_Formatting(ByVal Dgv As DataGridView)
        With Dgv
            For Each dgvCol As DataGridViewColumn In Dgv.Columns
                dgvCol.SortMode = DataGridViewColumnSortMode.NotSortable
                dgvCol.Visible = False
            Next
            .Columns("PARTICULAR").Width = 300
            .Columns("OPENING").Width = 118
            .Columns("DEBIT").Width = 118
            .Columns("CREDIT").Width = 118
            .Columns("CLOSING").Width = 118
            .Columns("ACCODE").Width = 60
            .Columns("PHONENO").Width = 80
            .Columns("MOBILE").Width = 80

            .Columns("PARTICULAR").Visible = True
            .Columns("OPENING").Visible = True
            .Columns("DEBIT").Visible = True
            .Columns("CREDIT").Visible = True
            .Columns("CLOSING").Visible = True
            If ChkSepCol.Checked Then
                .Columns("OPENINGCR").Visible = True
                .Columns("OPENINGDR").Visible = True
                .Columns("CLOSINGCR").Visible = True
                .Columns("CLOSINGDR").Visible = True
                .Columns("OPENING").Visible = False
                .Columns("CLOSING").Visible = False
                .Columns("OPENINGCR").HeaderText = "CREDIT"
                .Columns("OPENINGDR").HeaderText = "DEBIT"
                .Columns("CLOSINGCR").HeaderText = "CREDIT"
                .Columns("CLOSINGDR").HeaderText = "DEBIT"
            End If
            .Columns("PHONENO").Visible = rbtDetailed.Checked
            .Columns("MOBILE").Visible = rbtDetailed.Checked
            .Columns("OPENING").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("OPENINGDR").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("OPENINGCR").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("DEBIT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CREDIT").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CLOSING").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CLOSINGDR").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("CLOSINGCR").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            FillGridGroupStyle_KeyNoWise(Dgv)
        End With
    End Sub
    Private Sub GridViewHeaderCreator(ByVal gridviewHead As DataGridView)
        Dim dtHead As New DataTable
        StrSql = "SELECT ''[PARTICULAR]"
        StrSql += " ,''[OPENINGDR~OPENINGCR]"
        StrSql += " ,''[DEBIT~CREDIT]"
        StrSql += " ,''[CLOSINGDR~CLOSINGCR]"
        If rbtDetailed.Checked Then
            StrSql += " ,''[PHONENO]"
            StrSql += " ,''[MOBILE]"
        End If
        StrSql += " ,''SCROLL"
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtHead)
        gridviewHead.DataSource = dtHead
        gridviewHead.Columns("PARTICULAR").HeaderText = ""
        gridviewHead.Columns("OPENINGDR~OPENINGCR").HeaderText = "OPENING"
        gridviewHead.Columns("DEBIT~CREDIT").HeaderText = "TRANSACTION"
        gridviewHead.Columns("CLOSINGDR~CLOSINGCR").HeaderText = "CLOSING"
        If rbtDetailed.Checked Then
            gridviewHead.Columns("PHONENO").HeaderText = "PHONENO"
            gridviewHead.Columns("MOBILE").HeaderText = "MOBILE"
        End If
        gridviewHead.Columns("SCROLL").HeaderText = ""
        gridviewHead.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        SetGridHeadColWidth(gridviewHead)
    End Sub

    Private Shared m_Bitmap As Byte() = Nothing

    Public ReadOnly Property Bitmap() As Byte()
        Get
            Dim fs As New FileStream(Application.StartupPath & "\signature.bmp", FileMode.Open)
            Dim br As New BinaryReader(fs)
            Dim length As Integer = CInt(br.BaseStream.Length)
            m_Bitmap = New Byte(length - 1) {}
            m_Bitmap = br.ReadBytes(length)
            br.Close()
            fs.Close()
            Return m_Bitmap
        End Get
    End Property
    Public Function NEWMAILSEND(ByVal ToMail As String, ByVal MESSAGE As String, Optional ByVal Attachpath As String = "")
        If ToMail.Trim = "" Then Return 0 : Exit Function
        If MESSAGE.Trim = "" Then Return 0 : Exit Function
        Dim smtpServer As New SmtpClient()
        Dim mail As New MailMessage
        Dim Counter As Integer = 0

        Dim FromId As String = Nothing
        Dim MailServer As String = Nothing
        Dim Password As String = Nothing
        Try
            FromId = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'MAILSERVER'")
            If FromId.Contains("@") = False Or FromId.Contains(".") = False Then
                FromId = objGPack.GetSqlValue("SELECT CTLNAME FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'MAILSERVER'")
            End If
            Password = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'MAILPASSWORD'")
            Password = BrighttechPack.Methods.Decrypt(Password)
            Dim MailServer1 As String = Nothing
            Dim MailServer2 As String = Nothing
            If FromId.Contains("@") = True Then
                Dim SplitMailServer() As String = Split(FromId, "@")
                If Not SplitMailServer Is Nothing Then
                    MailServer1 = SplitMailServer(0)
                    MailServer2 = Trim(SplitMailServer(1))
                    MailServer2 = "@" & MailServer2
                End If
            End If
            If Trim(MailServer2) = "@gmail.com" Then
                smtpServer.Host = "smtp.gmail.com"
                smtpServer.Port = 587
                smtpServer.EnableSsl = True
            ElseIf Trim(MailServer2) = "@yahoo.com" Then
                smtpServer.Port = 465
                smtpServer.Host = "smtp.mail.yahoo.com"
                smtpServer.EnableSsl = True
            ElseIf Trim(MailServer2) = "@yahoo.co.in" Then
                smtpServer.Port = 587
                smtpServer.Host = "smtp.mail.yahoo.com"
            ElseIf Trim(MailServer2) = "@hotmail.com" Then
                smtpServer.Port = 587
                smtpServer.Host = "smtp.live.com"
                smtpServer.EnableSsl = True
            Else
                smtpServer.Port = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SMTP-PORT'")
                smtpServer.Host = objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SMTP-HOSTNAME'")
                smtpServer.EnableSsl = IIf(objGPack.GetSqlValue("SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SMTP-SSL'").ToString.ToUpper() = "Y", True, False)
            End If
            If FromId = "" Then MsgBox("Sender Id is Empty", MsgBoxStyle.Information) : Exit Function
            mail.From = New MailAddress(FromId)
            mail.To.Add(New MailAddress(ToMail))
            mail.Subject = "Accounts Confirmation Letter"
            mail.Body = MESSAGE
            mail.IsBodyHtml = True
            If File.Exists(Attachpath) = True Then mail.Attachments.Add(New Attachment(Attachpath))
            smtpServer.Credentials = New Net.NetworkCredential(FromId.Trim.ToString, Password.Trim.ToString)
            smtpServer.Timeout = 400000
            smtpServer.Send(mail)
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            Return 0
        End Try
        Return 1
    End Function
    Private Sub chklstgroup_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chklstgroup.SelectedIndexChanged
        accupload()
    End Sub
    Private Sub accupload()
        Dim Chkresgrpname As String
        Chkresgrpname = ""
        If chklstAcname.SelectedItems.Count > 0 Then Exit Sub
        chklstAcname.Items.Clear()
        If chklstgroup.CheckedItems.Count > 0 Then
            Chkresgrpname = GetChecked_CheckedList(chklstgroup, False)
            If Trim(Chkresgrpname) <> "" Then
                StrSql = " "
                StrSql = " SELECT ACNAME FROM " & cnAdminDb & "..ACHEAD WHERE ACGRPCODE IN ("
                StrSql = StrSql & " SELECT ACGRPCODE FROM " & cnAdminDb & "..ACGROUP "
                StrSql = StrSql & " WHERE ACGRPNAME IN ('" & Replace(Chkresgrpname, ",", "','") & "'))"
                StrSql = StrSql & " ORDER BY 1"
                FillCheckedListBox(StrSql, chklstAcname)
                'btnNew_Click(Me, New EventArgs)
            End If
        End If
    End Sub

    Private Sub Prop_Sets()
        Dim obj As New GroupLedger_Properties
        GetChecked_CheckedList(chklstgroup, obj.p_chklstgroup)
        GetChecked_CheckedList(chklstAcname, obj.p_chklstAcname)
        obj.p_rbtDetailed = rbtDetailed.Checked
        obj.p_rbtSummary = rbtSummary.Checked
        obj.p_chkconfletter = chkconfletter.Checked
        obj.p_chkgpletterwithled = chkgpletterwithled.Checked
        obj.p_ChkSepCol = ChkSepCol.Checked
        obj.p_ChkWithNil = ChkNilBalance.Checked
        obj.p_ChkSmithBal = chksmithbal.Checked
        SetSettingsObj(obj, Me.Name, GetType(GroupLedger_Properties))
    End Sub

    Private Sub Prop_Gets()
        Dim obj As New GroupLedger_Properties
        Dim defAcGrpName As String = ""
        Dim chkAcGrpName As String = ""
        Dim chkAcName As String = ""
        Dim defAcName As String = ""
        Dim sIndex As Integer = 0
        Dim tIndex As Integer = 0

        GetSettingsObj(obj, Me.Name, GetType(GroupLedger_Properties))
        SetChecked_CheckedList(chklstgroup, obj.p_chklstgroup, Nothing)
        chkAcGrpName = GetChecked_CheckedList(chklstgroup, False)
        Dim Acgrpname() As String = Split(chkAcGrpName, ",")
        If Not Acgrpname Is Nothing Then
            defAcGrpName = Acgrpname(0)
        End If
        sIndex = chklstgroup.FindStringExact(defAcGrpName, 0)
        chklstgroup.SelectedIndex = sIndex
        accupload()
        SetChecked_CheckedList(chklstAcname, obj.p_chklstAcname, Nothing)
        chkAcName = GetChecked_CheckedList(chklstAcname, False)
        Dim Acname() As String = Split(chkAcName, ",")
        If Not Acname Is Nothing Then
            defAcName = Acname(0)
        End If
        tIndex = chklstAcname.FindStringExact(defAcName, 0)
        chklstAcname.SelectedIndex = tindex
        rbtDetailed.Checked = obj.p_rbtDetailed
        rbtSummary.Checked = obj.p_rbtSummary
        chkconfletter.Checked = obj.p_chkconfletter
        chkgpletterwithled.Checked = obj.p_chkgpletterwithled
        ChkSepCol.Checked = obj.p_ChkSepCol
        ChkNilBalance.Checked = obj.p_ChkWithNil
        chksmithbal.Checked = obj.p_ChkSmithBal
    End Sub

    Private Sub chkacgrpSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkacgrpSelectAll.CheckedChanged
        SetChecked_CheckedList(chklstgroup, chkacgrpSelectAll.Checked)
        accupload()
    End Sub

    Private Sub chkaccSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkaccSelectAll.CheckedChanged
        SetChecked_CheckedList(chklstAcname, chkaccSelectAll.Checked)
    End Sub

    Private Sub chkgpletterwithled_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkgpletterwithled.CheckedChanged
        If chkgpletterwithled.Checked Then
            chksmithbal.Enabled = True
        Else
            chksmithbal.Enabled = False
            chksmithbal.Checked = False
        End If
    End Sub
End Class

Public Class GroupLedger_Properties
    Private chkLstCostCentre As New List(Of String)
    Public Property p_chkLstCostCentre() As List(Of String)
        Get
            Return chkLstCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkLstCostCentre = value
        End Set
    End Property

    Private chkCmbCostCentre As New List(Of String)
    Public Property P_chkCmbCostCentre() As List(Of String)
        Get
            Return chkCmbCostCentre
        End Get
        Set(ByVal value As List(Of String))
            chkCmbCostCentre = value
        End Set
    End Property

    Private chklstgroup As New List(Of String)
    Public Property p_chklstgroup() As List(Of String)
        Get
            Return chklstgroup
        End Get
        Set(ByVal value As List(Of String))
            chklstgroup = value
        End Set
    End Property
    Private chklstAcname As New List(Of String)
    Public Property p_chklstAcname() As List(Of String)
        Get
            Return chklstAcname
        End Get
        Set(ByVal value As List(Of String))
            chklstAcname = value
        End Set
    End Property
    Private rbtDetailed As Boolean = True
    Public Property p_rbtDetailed() As Boolean
        Get
            Return rbtDetailed
        End Get
        Set(ByVal value As Boolean)
            rbtDetailed = value
        End Set
    End Property
    Private ChkSepCol As Boolean = False
    Public Property p_ChkSepCol() As Boolean
        Get
            Return ChkSepCol
        End Get
        Set(ByVal value As Boolean)
            ChkSepCol = value
        End Set
    End Property
    Private rbtSummary As Boolean = False
    Public Property p_rbtSummary() As Boolean
        Get
            Return rbtSummary
        End Get
        Set(ByVal value As Boolean)
            rbtSummary = value
        End Set
    End Property
    Private chkconfletter As Boolean = False
    Public Property p_chkconfletter() As Boolean
        Get
            Return chkconfletter
        End Get
        Set(ByVal value As Boolean)
            chkconfletter = value
        End Set
    End Property
    Private chkgpletterwithled As Boolean = False
    Public Property p_chkgpletterwithled() As Boolean
        Get
            Return chkgpletterwithled
        End Get
        Set(ByVal value As Boolean)
            chkgpletterwithled = value
        End Set
    End Property
    Private ChkWithNil As Boolean = False
    Public Property p_ChkWithNil() As Boolean
        Get
            Return ChkWithNil
        End Get
        Set(ByVal value As Boolean)
            ChkWithNil = value
        End Set
    End Property
    Private ChkSmithBal As Boolean = False
    Public Property p_ChkSmithBal() As Boolean
        Get
            Return ChkSmithBal
        End Get
        Set(ByVal value As Boolean)
            ChkSmithBal = value
        End Set
    End Property
End Class
