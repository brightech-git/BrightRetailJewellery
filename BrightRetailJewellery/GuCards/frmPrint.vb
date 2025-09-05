Imports System.IO
Imports System.Drawing.Printing
Imports System.Drawing.Printing.PrinterSettings
Imports System.Data.OleDb
Public Class FRM_PRINTDIA
    Dim oWrite As System.IO.StreamWriter = Nothing
    Dim strSql As String
    Dim da As New OleDbDataAdapter
    Dim cmd As New OleDbCommand
    Dim dt As New DataTable
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Try
            Dim printerNames As System.Drawing.Printing.PrinterSettings.StringCollection = Nothing
            Dim printDoc As New System.Drawing.Printing.PrintDocument
            For Each name As String In System.Drawing.Printing.PrinterSettings.InstalledPrinters
                cmbPrinterNames.Items.Add(name)
            Next
            If cmbPrinterNames.Items.Count > 0 Then
                cmbPrinterNames.Text = printDoc.PrinterSettings.PrinterName
            End If
        Catch ex As Exception
            cmbPrinterNames.Items.Clear()
        End Try

    End Sub
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            If Not cmbPrinterNames.Items.Count > 0 Then
                MsgBox("Printer Name Should Not Empty", MsgBoxStyle.Information)
                Me.Close()
                Exit Sub
            End If
            If cmbPrinterNames.Text.Contains(" ") Then
                Dim msg As String = Nothing
                msg = "Invalid Printer Name" + vbCrLf
                msg += "1.Printer Name Should not contain white spaces" + vbCrLf
                msg += "2.Printer Name length should not exceed 8 charachters"
                MsgBox(msg, MsgBoxStyle.Critical, "Error!!!")
                Me.Close()
                Exit Sub
            End If
            'If File.Exists(Application.StartupPath + "\BarCodePrint.txt") = True Then
            '    File.Delete(Application.StartupPath + "\BarCodePrint.txt")
            'End If
            oWrite = System.IO.File.CreateText("c:\BarCodePrint.txt")
            FuncBarcodeTagPrint()
            oWrite.Close()
            Dim batWriter As StreamWriter = Nothing
            batWriter = System.IO.File.CreateText("c:\Print.bat")
            'batWriter.WriteLine("NET USE LPT1 \\SERVER\" + cmbPrinterNames.Text)
            batWriter.WriteLine("TYPE " & "c:\BarcodePrint.txt > " & cmbPrinterNames.Text)
            batWriter.Close()

            ''Dim printDoc As New PrintDocument
            ''printDoc.PrinterSettings.PrinterName = cmbPrinterNames.Text
            ''printDoc.DocumentName = "D:\\BillPrint.txt"
            'printDoc.DocumentName = Application.StartupPath   + "\billprint.txt"


            'printDoc = Application.StartupPath   + "\billprint.txt"

            'Dim printSet As New PrinterSettings
            'printSet.PrinterName = cmbPrinterNames.Text
            'printSet.PrintFileName = Application.StartupPath   + "\billprint.txt"
            'printSet.PrintToFile = True


            'MsgBox(Application.StartupPath   + "\BillPrint.bat")
            System.Diagnostics.Process.Start("c:\Print.bat")
            Me.Close()
        Catch ex As Exception
            If oWrite Is Nothing = False Then
                oWrite.Close()
            End If
            MsgBox("Message :" + ex.Message + "StackTrace   :" + ex.StackTrace)
        End Try

    End Sub

    Public Sub FuncBarcodeTagPrint()

        If BarcodeDescrip.ToUpper().Contains("NAKSHATRA") = True Then
            strSql = "             SELECT "
            strSql += "             T.TAGNO,T.SALVALUE,T.GRSWT"
            strSql += "             ,(SELECT SNAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = T.ITEMTYPEID) ITEMTYPENAME"
            strSql += "             ,S.STNPCS "
            'strSql += "             ,(SELECT STNPCS FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE ISNULL(TAGNO,'') = ISNULL(T.TAGNO,'') AND TAGSNO = SNO) STNPCS"
            strSql += "             ,CONVERT(NUMERIC(15,3),S.STNWT) STNWT"
            'strSql += "             ISNULL((SELECT STNWT FROM " & cnAdminDb & "..ITEMTAGSTONE WHERE ISNULL(TAGNO,'') = ISNULL(T.TAGNO,'') AND TAGSNO = SNO),0)/5) STNWT"
            strSql += "             ,T.NARRATION "
            'strSql += "             FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += "             FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += "             left outer JOIN  " & cnAdminDb & "..ITEMTAGSTONE AS S"
            strSql += "             ON ISNULL(S.TAGNO,'') IN (ISNULL(T.TAGNO,'')) AND ISNULL(S.TAGSNO,0) IN (ISNULL(T.SNO,0))"
            strSql += "             WHERE T.TAGNO ='" & BarcodeTagNo & "' AND T.SNO =" & BarcodeSno & ""
            'strSql += "             AND DESCRIP LIKE 'NAKSHATHRA%'"
            dt = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            Dim diaQlt As String = ""
            Dim diaStyle As String = ""
            Dim cnt As Integer = 0
            If dt.Rows.Count > 0 Then
                Dim IndexOfSlash As Integer = 0
                IndexOfSlash = dt.Rows(0).Item("NARRATION").ToString.IndexOf("/")
                If IndexOfSlash > 0 Then
                    If IndexOfSlash + 1 <= dt.Rows(0).Item("NARRATION").ToString.Length Then
                        diaQlt = dt.Rows(0).Item("NARRATION").ToString.Substring(IndexOfSlash + 1)
                    End If
                    diaStyle = dt.Rows(0).Item("NARRATION").ToString.Substring(0, IndexOfSlash - 1)
                End If

                oWrite.WriteLine("FMT(1, 699D, 120D, 0, 0, 1)")
                oWrite.WriteLine("DMD(1)")
                oWrite.WriteLine("DPD(1)")
                oWrite.WriteLine("ACL()")
                oWrite.WriteLine("COL(0)")
                oWrite.WriteLine("FAG(2)")
                oWrite.WriteLine("CFL(1, 690D, 109D, 111, 1, 1)")
                oWrite.WriteLine("CFL(2, 690D, 85D, 111, 1, 1)")
                oWrite.WriteLine("BDN(1, 2)")
                oWrite.WriteLine("BFL(3, 675D, 40D, 0, 28, 29D)")
                oWrite.WriteLine("CFL(4, 498D, 109D, 111, 1, 1)")
                oWrite.WriteLine("CFL(5, 522D, 80D, 110, 1, 1)")
                oWrite.WriteLine("CFL(6, 522D, 53D, 110, 1, 1)")
                oWrite.WriteLine("CFL(7, 522D, 29D, 111, 1, 1)")
                oWrite.WriteLine("CFL(8, 666D, 61D, 111, 1, 1)")
                oWrite.WriteLine("DAT(1,DIA QLT:" & diaQlt & ")")
                oWrite.WriteLine("DAT(2,MRP:" & dt.Rows(0).Item("SALVALUE").ToString & ")")
                oWrite.WriteLine("DAT(3,/6" & BarcodeTagNo & ")")
                oWrite.WriteLine("DAT(4," & BarcodeTagNo & ")")
                oWrite.WriteLine("DAT(5,GD KT///W:" & dt.Rows(0).Item("ITEMTYPENAME").ToString & "Y///" & dt.Rows(0).Item("GRSWT").ToString & ")")
                cnt = 5
                For cntWt As Integer = 0 To dt.Rows.Count - 1
                    cnt += 1
                    oWrite.WriteLine("DAT(" & cnt & ",DIA P///W:" & dt.Rows(cntWt).Item("STNPCS").ToString & "   ///" & dt.Rows(cntWt).Item("STNWT").ToString & " )")
                Next cntWt
                oWrite.WriteLine("DAT(" & cnt + 1 & ",CHN WT:)")
                oWrite.WriteLine("DAT(" & cnt + 2 & ", " & diaStyle & ")")
                oWrite.WriteLine("PRT(1, 0, 1)")
            End If
        Else
            ''MODIFIED SAFI JULY 01
            strSql = " SELECT"
            strSql += " NARRATION,TAGNO,GRSWT,ITEMNAME,ITEMTYPENAME,STNPCS,STNWT"
            strSql += " ,CASE WHEN STNSUBITEMID = 0 THEN (SELECT TOP 1 SHORTNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = X.STNITEMID) "
            strSql += " ELSE (SELECT TOP 1 SHORTNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID = X.STNSUBITEMID) END AS STNITEMNAME"
            strSql += " FROM"
            strSql += " ("
            strSql += " SELECT T.NARRATION,T.TAGNO,T.GRSWT"
            strSql += " ,(SELECT SHORTNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID) ITEMNAME"
            strSql += " ,(SELECT SNAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = T.ITEMTYPEID) ITEMTYPENAME"
            strSql += " ,S.STNPCS ,CONVERT(NUMERIC(15,3),S.STNWT) STNWT"
            strSql += " ,S.STNITEMID,S.STNSUBITEMID"
            strSql += " FROM " & cnAdminDb & "..ITEMTAG AS T"
            strSql += " left outer JOIN  " & cnAdminDb & "..ITEMTAGSTONE AS S"
            strSql += " ON ISNULL(S.TAGNO,'') IN (ISNULL(T.TAGNO,'')) AND ISNULL(S.TAGSNO,0) IN (ISNULL(T.SNO,0))"
            strSql += " WHERE T.TAGNO ='" & BarcodeTagNo & "'"
            strSql += " AND T.SNO =" & BarcodeSno & ""
            strSql += " )X ORDER BY STNITEMID"
            'strSql = "             SELECT "
            'strSql += "             NARRATION,TAGNO,GRSWT,ITEMNAME,ITEMTYPENAME,STNPCS,STNWT"
            'strSql += "             ,(SELECT TOP 1 SHORTNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE ITEMID IN (X.STNITEMID)) STNITEMNAME"
            'strSql += "             FROM"
            'strSql += "             ("
            'strSql += "             SELECT T.NARRATION,T.TAGNO,T.GRSWT"
            'strSql += "             ,(SELECT SHORTNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = T.ITEMID) ITEMNAME"
            'strSql += "             ,(SELECT SNAME FROM " & cnAdminDb & "..ITEMTYPE WHERE ITEMTYPEID = T.ITEMTYPEID) ITEMTYPENAME"
            'strSql += "             ,S.STNPCS ,CONVERT(NUMERIC(15,3),S.STNWT) STNWT"
            'strSql += "             ,S.STNITEMID"
            'strSql += "             FROM " & cnAdminDb & "..ITEMTAG AS T"
            'strSql += "             INNER JOIN  " & cnAdminDb & "..ITEMTAGSTONE AS S"
            'strSql += "             ON ISNULL(S.TAGNO,'') IN (ISNULL(T.TAGNO,'')) AND ISNULL(S.TAGSNO,0) IN (ISNULL(T.SNO,0))"
            'strSql += "             WHERE T.TAGNO ='" & BarcodeTagNo & "' AND T.SNO =" & BarcodeSno & ""
            'strSql += "             )X ORDER BY STNITEMID"
            dt = New DataTable
            da = New OleDbDataAdapter(strSql, cn)
            da.Fill(dt)
            Dim sETTING As String = ""
            Dim RHODIUM As String = ""
            Dim cnt As Integer = 0
            If dt.Rows.Count > 0 Then
                Dim IndexOfSlash As Integer = 0
                If dt.Rows(0).Item("NARRATION").ToString.Length > 1 Then
                    sETTING = dt.Rows(0).Item("NARRATION").ToString.Substring(0, 2)
                    IndexOfSlash = dt.Rows(0).Item("NARRATION").ToString.IndexOf("/")
                    If IndexOfSlash > 0 Then
                        If dt.Rows(0).Item("NARRATION").ToString.Contains("/") = True Then
                            If IndexOfSlash + 3 <= dt.Rows(0).Item("NARRATION").ToString.Length Then
                                RHODIUM = dt.Rows(0).Item("NARRATION").ToString.Substring(IndexOfSlash + 1, 2)
                            End If
                        End If
                    End If
                End If

                oWrite.WriteLine("FMT(1, 699D, 120D, 0, 0, 1)")
                oWrite.WriteLine("DMD(1)")
                oWrite.WriteLine("DPD(1)")
                oWrite.WriteLine("ACL()")
                oWrite.WriteLine("COL(0)")
                oWrite.WriteLine("FAG(2)")
                oWrite.WriteLine("CFL(1, 642D, 109D, 111, 1, 1)")
                oWrite.WriteLine("CFL(2, 682D, 91D, 111, 1, 1)")
                oWrite.WriteLine("CFL(3, 637D, 68D, 111, 1, 1)")
                oWrite.WriteLine("CFL(4, 682D, 45D, 111, 1, 1)")
                oWrite.WriteLine("CFL(5, 490D, 101D, 111, 1, 1)")
                oWrite.WriteLine("CFL(6, 682D, 26D, 111, 1, 1)")
                oWrite.WriteLine("CFL(7, 490D, 77D, 111, 1, 1)")
                oWrite.WriteLine("BDN(1, 2)")
                oWrite.WriteLine("BFL(8, 473D, 32D, 0, 28, 22D)")
                oWrite.WriteLine("CFL(9, 452D, 52D, 111, 1, 1)")
                oWrite.WriteLine("DAT(1, " & dt.Rows(0).Item("ITEMNAME").ToString & ")")
                cnt = 2
                Dim stNitemName As String = ""
                For cntRow As Integer = 0 To 4
                    'If 4 <> cntRow Then
                    If dt.Rows.Count - 1 >= cntRow Then
                        If stNitemName <> dt.Rows(cntRow).Item("STNITEMNAME").ToString Then
                            oWrite.WriteLine("DAT(" & cnt & ", " & dt.Rows(cntRow).Item("STNITEMNAME").ToString & ":" & dt.Rows(cntRow).Item("STNPCS").ToString & "///" & dt.Rows(cntRow).Item("STNWT").ToString & "CT)")
                        Else
                            oWrite.WriteLine("DAT(" & cnt & ", " & Space(dt.Rows(cntRow).Item("STNITEMNAME").ToString.Length) & dt.Rows(cntRow).Item("STNPCS").ToString & "///" & dt.Rows(cntRow).Item("STNWT").ToString & "CT)")
                        End If
                        stNitemName = dt.Rows(cntRow).Item("STNITEMNAME").ToString
                    Else
                        If cnt <> 5 Then
                            oWrite.WriteLine("DAT(" & cnt & ", )")
                        End If
                    End If
                    'End If
                    cnt += 1
                Next
                'oWrite.WriteLine("DAT(2,DIA:14///1.07CT)")
                'oWrite.WriteLine("DAT(3,2///0.25CT)")
                'oWrite.WriteLine("DAT(4,BUT:1///0.075CT)")
                oWrite.WriteLine("DAT(" & 5 & ",GD:" & dt.Rows(0).Item("GRSWT").ToString & " GM)")
                'oWrite.WriteLine("DAT(6,C///S B1/ 1.07CT")
                oWrite.WriteLine("DAT(" & cnt & "," & dt.Rows(0).Item("ITEMTYPENAME").ToString & " " & sETTING & " " & RHODIUM & ")")
                oWrite.WriteLine("DAT(" & cnt + 1 & ",/6" & dt.Rows(0).Item("TAGNO").ToString & ")")
                oWrite.WriteLine("DAT(" & cnt + 2 & ", " & dt.Rows(0).Item("TAGNO").ToString & ")")
                oWrite.WriteLine("PRT(1, 0, 1)")
            End If
        End If
        'oWrite.WriteLine("FMT(1, 699D, 120D, 0, 0, 1)")
        'oWrite.WriteLine("DMD(1)")
        'oWrite.WriteLine("DPD(1)")
        'oWrite.WriteLine("ACL()")
        'oWrite.WriteLine("COL(0)")
        'oWrite.WriteLine("FAG(2)")
        'oWrite.WriteLine("CFL(1, 642D, 109D, 111, 1, 1)")
        'oWrite.WriteLine("CFL(2, 682D, 91D, 111, 1, 1)")
        'oWrite.WriteLine("CFL(3, 637D, 68D, 111, 1, 1)")
        'oWrite.WriteLine("CFL(4, 682D, 45D, 111, 1, 1)")
        'oWrite.WriteLine("CFL(5, 490D, 101D, 111, 1, 1)")
        'oWrite.WriteLine("CFL(6, 682D, 26D, 111, 1, 1)")
        'oWrite.WriteLine("CFL(7, 490D, 77D, 111, 1, 1)")
        'oWrite.WriteLine("BDN(1, 2)")
        'oWrite.WriteLine("BFL(8, 473D, 32D, 0, 28, 22D)")
        'oWrite.WriteLine("CFL(9, 452D, 52D, 111, 1, 1)")
        'oWrite.WriteLine("DAT(1, STUD)")
        'oWrite.WriteLine("DAT(2,DIA:14///1.07CT)")
        'oWrite.WriteLine("DAT(3,2///0.25CT)")
        'oWrite.WriteLine("DAT(4,BUT:1///0.075CT)")
        'oWrite.WriteLine("DAT(5,GD:25.500GM)")
        'oWrite.WriteLine("DAT(6,C///S B1/ 1.07CT")
        'oWrite.WriteLine("DAT(7,18KT OP RH)")
        'oWrite.WriteLine("DAT(8,/6S1)")
        'oWrite.WriteLine("DAT(9, S1)")
        'oWrite.WriteLine("PRT(1, 0, 1)")
        'oWrite.WriteLine("FMT(1, 699D, 120D, 0, 0, 1)")
        'oWrite.WriteLine("DMD(1)")
        'oWrite.WriteLine("DPD(1)")
        'oWrite.WriteLine("ACL()")
        'oWrite.WriteLine("COL(0)")
        'oWrite.WriteLine("FAG(2)")
        'oWrite.WriteLine("CFL(1, 658D, 93D, 113, 1, 1)")
        'oWrite.WriteLine("CFL(2, 682D, 61D, 114, 1, 1)")
        'oWrite.WriteLine("CFL(3, 679D, 29D, 114, 1, 1)")
        'oWrite.WriteLine("CFL(4, 490D, 101D, 114, 1, 1)")
        'oWrite.WriteLine("BDN(1, 2)")
        'oWrite.WriteLine("BFL(5, 483D, 32D, 0, 28, 22D)")
        'oWrite.WriteLine("CFL(6, 466D, 52D, 114, 1, 1)")
        'oWrite.WriteLine("CFL(7, 490D, 77D, 114, 1, 1)")
        'oWrite.WriteLine("DAT(1,NV RING)")
        'oWrite.WriteLine("DAT(2,DIA:1///0.07CT)")
        'oWrite.WriteLine("DAT(3,GD:25.500GM)")
        'oWrite.WriteLine("DAT(4,22KT CL 13J)")
        'oWrite.WriteLine("DAT(5,/6NV1)")
        'oWrite.WriteLine("DAT(6, NV1)")
        'oWrite.WriteLine("DAT(7, NILA)")
        'oWrite.WriteLine("PRT(1, 0, 1)")
        BarcodeTagNo = ""
        BarcodeSno = 0
        BarcodeDescrip = ""

    End Sub

    Private Sub btnFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFile.Click
        Try
            Dim objSaveFileDiag As New SaveFileDialog
            'funcWtireToNotepad()
            If objSaveFileDiag.ShowDialog = Windows.Forms.DialogResult.OK Then
                File.Copy(Application.StartupPath + "\BillPrint.txt", objSaveFileDiag.FileName, True)
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            Me.Close()
        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnSettings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSettings.Click
        '//Dim obj As New frmPrintSettings
        'obj.ShowDialog()
        btnCancel.Focus()
    End Sub

    Private Sub frmPrint_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub frmPrint_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub
End Class