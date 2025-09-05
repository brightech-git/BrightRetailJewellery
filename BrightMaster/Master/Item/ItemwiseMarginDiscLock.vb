Imports System.Data.OleDb
Public Class ItemwiseMarginDiscLock
    Dim StrSql As String
    Dim DA As OleDbDataAdapter
    Dim Cmd As OleDbCommand
    Dim Finalamtlockbase As String
    Dim RoleBasedDisc As String
    Dim SCREENNAME As String = "MARGIN"

    Private Sub ItemwiseDiscountLock_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        If e.KeyChar = Chr(Keys.Enter) Then
            SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub ItemwiseDiscountLock_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Finalamtlockbase = GetAdmindbSoftValue("FINALAMT_LOCKBASE", "M")
        RoleBasedDisc = GetAdmindbSoftValue("ROLEBASED_DISCOUNT", "N")

        StrSql = " SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ACTIVE = 'Y' ORDER BY ITEMNAME"
        cmbItem.Items.Clear()
        cmbItem.Items.Add("ALL")
        objGPack.FillCombo(StrSql, cmbItem, False, False)
        cmbItem.Text = "ALL"

        StrSql = " SELECT DESIGNERNAME FROM " & cnAdminDb & "..DESIGNER WHERE ACTIVE = 'Y' ORDER BY DESIGNERNAME"
        cmbDesigner.Items.Clear()
        objGPack.FillCombo(StrSql, cmbDesigner, False, False)

        cmbSubItem.Items.Clear()
        cmbSubItem.Items.Add("ALL")
        cmbSubItem.Text = "ALL"
        If GetAdmindbSoftValue("COSTCENTRE", "N") = "Y" Then
            cmbCostCentre_MAN.Enabled = True
            StrSql = "SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE ORDER BY COSTNAME"
            objGPack.FillCombo(StrSql, cmbCostCentre_MAN, , False)
        Else
            cmbCostCentre_MAN.Text = ""
            cmbCostCentre_MAN.Enabled = False
        End If

        StrSql = " SELECT ROLENAME FROM " & cnAdminDb & "..ROLEMASTER WHERE ACTIVE = 'Y' ORDER BY ROLENAME"
        cmbRole.Items.Clear()
        objGPack.FillCombo(StrSql, cmbRole, False, False)

        btnNew_Click(Me, New EventArgs)

        If RoleBasedDisc = "Y" And Finalamtlockbase = "D" Then
            rbtRoleBased.Enabled = True
            rbtRoleBased.Checked = False
            cmbRole.Enabled = False
            cmbRole.Text = ""
        Else
            rbtRoleBased.Enabled = False
            rbtRoleBased.Checked = False
            cmbRole.Enabled = False
            cmbRole.Text = ""
        End If
        lblUpdCostid.Visible = False
        cmbUpdateCostid_MAN.Visible = False
        btnUpdate.Visible = False
        SCREENNAME = IIf(Finalamtlockbase = "D", "DISC.", "MARGIN")
        Me.Text = "Itemwise/Tablewise/Designerwise " & SCREENNAME & " LOCK"
    End Sub

    Private Sub cmbItem_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbItem.SelectedIndexChanged
        cmbSubItem.Items.Clear()
        cmbSubItem.Items.Add("ALL")
        cmbSubItem.Text = "ALL"
        StrSql = " SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST"
        If cmbItem.Text <> "ALL" And cmbItem.Text <> "" Then
            StrSql += " WHERE ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "')"
        End If
        objGPack.FillCombo(StrSql, cmbSubItem, False, False)
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If cmbCostCentre_MAN.Enabled Then
            If cmbCostCentre_MAN.Text = "" Then MsgBox("Please select Cost centre", MsgBoxStyle.Information) : cmbCostCentre_MAN.Focus() : Exit Sub
        End If
        Dim updatetype As String
        If rbtTable.Checked Then
            updatetype = "T"
        ElseIf rbtDesigner.Checked Then
            updatetype = "D"
        ElseIf rbtRoleBased.Checked Then
            updatetype = "R"
        Else
            updatetype = "I"
        End If
        If updatetype = "T" Then
            StrSql = vbCrLf + "  SELECT DISTINCT W.TABLECODE AS PARTICULAR,W.TABLECODE TABLECODE,VA.SNO SNO,CASE WHEN VA.VAPER <> 0 THEN VA.VAPER ELSE NULL END VAPER,CASE WHEN VA.VAFIXPER <> 0 THEN VA.VAFIXPER ELSE NULL END VAFIXPER ,"
            StrSql += vbCrLf + "  CASE WHEN VA.VAPERGM <> 0 THEN VA.VAPERGM ELSE NULL END VAPERGM "
            StrSql += vbCrLf + "  ,CASE WHEN VA.WASTPER <> 0 THEN VA.WASTPER ELSE NULL END WASTPER"
            StrSql += vbCrLf + "  ,CASE WHEN VA.MCPER <> 0 THEN VA.MCPER ELSE NULL END MCPER"
            StrSql += vbCrLf + "  ,CASE WHEN VA.ADDVAPER <> 0 THEN VA.ADDVAPER ELSE NULL END ADDVAPER"
            StrSql += vbCrLf + "  ,CASE WHEN VA.GRSAMTPER <> 0 THEN VA.GRSAMTPER ELSE NULL END GRSAMT "
            StrSql += vbCrLf + "  ,CASE WHEN VA.DIAAMTPER <> 0 THEN VA.DIAAMTPER ELSE NULL END DIAAMT"
            StrSql += vbCrLf + "  FROM " & cnAdminDb & "..WMCTABLE W LEFT JOIN " & cnAdminDb & "..VACONTROL VA ON  W.TABLECODE= VA.TABLECODE AND ISNULL(VA.ROLEID,0)=0 "
            If cmbCostCentre_MAN.Enabled Then
                StrSql += " AND VA.COSTID= (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "')"
            End If
            StrSql += " WHERE ISNULL(W.TABLECODE,'') <> '' "
            StrSql += vbCrLf + "  ORDER BY W.TABLECODE DESC"
            Dim dtGrid As New DataTable
            dtGrid.Columns.Add("KEYNO", GetType(Integer))
            dtGrid.Columns("KEYNO").AutoIncrement = True
            dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
            dtGrid.Columns("KEYNO").AutoIncrementStep = 1
            DA = New OleDbDataAdapter(StrSql, cn)
            DA.Fill(dtGrid)
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
                .Columns("SNO").Visible = False
                .Columns("TABLECODE").Visible = False
                .Columns("PARTICULAR").Width = 300
            End With
        ElseIf updatetype = "D" Then
            If cmbDesigner.Text = "" Then MsgBox("Desiger is empty", MsgBoxStyle.Critical) : cmbDesigner.Focus() : Exit Sub
            StrSql = "SELECT DESIGNERID FROM " & cnAdminDb & "..DESIGNER WHERE DESIGNERNAME = '" & cmbDesigner.Text & "'"
            Dim Xdesignerid As Integer = objGPack.GetSqlValue(StrSql)
            StrSql = vbCrLf + "  SELECT * FROM "
            StrSql += vbCrLf + "  ("

            StrSql += vbCrLf + "  SELECT DISTINCT I.DESIGNERNAME AS PARTICULAR,'' SNO,DESIGNERNAME ,NULL VAPER,NULL VAFIXPER ,"
            StrSql += vbCrLf + "  NULL VAPERGM ,NULL WASTPER,NULL MCPER,NULL ADDVAPER"
            StrSql += vbCrLf + "  ,CASE WHEN VA.GRSAMTPER <> 0 THEN VA.GRSAMTPER ELSE NULL END GRSAMT "
            StrSql += vbCrLf + "  ,CASE WHEN VA.DIAAMTPER <> 0 THEN VA.DIAAMTPER ELSE NULL END DIAAMT"
            StrSql += vbCrLf + " ,'T'COLHEAD,''ITEM," & Xdesignerid & " DESIGNERID"
            StrSql += vbCrLf + " ,CONVERT(INT,NULL)ITEMID FROM " & cnAdminDb & "..DESIGNER I LEFT JOIN " & cnAdminDb & "..VACONTROL VA ON  I.DESIGNERID = VA.DESIGNERID AND ISNULL(VA.ROLEID,0)=0"
            If cmbCostCentre_MAN.Enabled Then
                StrSql += " and VA.COSTID= (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "')"
            End If
            StrSql += vbCrLf + " WHERE  I.DESIGNERID = " & Xdesignerid
            If cmbItem.Text <> "ALL" And cmbItem.Text <> "" Then
                StrSql += vbCrLf + " AND  VA.ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "')"
            End If
            StrSql += vbCrLf + "  UNION ALL"
            StrSql += vbCrLf + "  SELECT '   ' + ITEMNAME,VA.SNO,'" & cmbDesigner.Text & "' AS DESIGNERNAME,CASE WHEN VA.VAPER <> 0 AND ISNULL(VA.DESIGNERID,0)=" & Xdesignerid & " THEN VA.VAPER ELSE NULL END VAPER,CASE WHEN VA.VAFIXPER <> 0 AND ISNULL(VA.DESIGNERID,0)=" & Xdesignerid & " THEN VA.VAFIXPER ELSE NULL END VAFIXPER,"
            StrSql += vbCrLf + "  CASE WHEN VA.VAPERGM <> 0 AND ISNULL(VA.DESIGNERID,0)=" & Xdesignerid & " THEN VA.VAPERGM ELSE NULL END VAPERGM "
            StrSql += vbCrLf + "  ,CASE WHEN VA.WASTPER <> 0 AND ISNULL(VA.DESIGNERID,0)=" & Xdesignerid & " THEN VA.WASTPER ELSE NULL END WASTPER"
            StrSql += vbCrLf + "  ,CASE WHEN VA.MCPER <> 0 AND ISNULL(VA.DESIGNERID,0)=" & Xdesignerid & " THEN VA.MCPER ELSE NULL END MCPER"
            StrSql += vbCrLf + "  ,CASE WHEN VA.ADDVAPER <> 0 AND ISNULL(VA.DESIGNERID,0)=" & Xdesignerid & " THEN VA.ADDVAPER ELSE NULL END ADDVAPER   "
            StrSql += vbCrLf + "  ,CASE WHEN VA.GRSAMTPER <> 0 THEN VA.GRSAMTPER ELSE NULL END GRSAMT "
            StrSql += vbCrLf + "  ,CASE WHEN VA.DIAAMTPER <> 0 THEN VA.DIAAMTPER ELSE NULL END DIAAMT"
            StrSql += vbCrLf + "  ,'' COLHEAD,'N' AS ITEM," & Xdesignerid & " DESIGNERID,S.ITEMID"
            StrSql += vbCrLf + "  FROM " & cnAdminDb & "..ITEMMAST AS S LEFT JOIN " & cnAdminDb & "..VACONTROL VA ON  S.ITEMID = VA.ITEMID AND ISNULL(VA.ROLEID,0)=0 AND VA.DESIGNERID =" & Xdesignerid
            StrSql += vbCrLf + "  LEFT JOIN " & cnAdminDb & "..DESIGNER D ON D.DESIGNERID =VA.DESIGNERID  "
            StrSql += vbCrLf + " WHERE 1=1 "
            If cmbItem.Text <> "ALL" And cmbItem.Text <> "" Then
                StrSql += vbCrLf + " AND  VA.ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "')"
            End If
            If cmbCostCentre_MAN.Enabled Then
                StrSql += " AND VA.COSTID= (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "')"
            End If
            StrSql += vbCrLf + "  )X"
            StrSql += vbCrLf + "  ORDER BY DESIGNERNAME,COLHEAD desc,particular"
            Dim dtGrid As New DataTable
            dtGrid.Columns.Add("KEYNO", GetType(Integer))
            dtGrid.Columns("KEYNO").AutoIncrement = True
            dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
            dtGrid.Columns("KEYNO").AutoIncrementStep = 1
            DA = New OleDbDataAdapter(StrSql, cn)
            DA.Fill(dtGrid)

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
                .Columns("SNO").Visible = False
                .Columns("COLHEAD").Visible = False
                .Columns("DESIGNERNAME").Visible = False
                .Columns("DESIGNERID").Visible = False
                .Columns("PARTICULAR").Width = 300
            End With
        ElseIf updatetype = "R" Then
            If cmbRole.Text = "" Then MsgBox("Role is empty", MsgBoxStyle.Critical) : cmbRole.Focus() : Exit Sub
            StrSql = "SELECT ROLEID FROM " & cnAdminDb & "..ROLEMASTER WHERE ROLENAME = '" & cmbRole.Text & "'"
            Dim XRoleid As Integer = objGPack.GetSqlValue(StrSql)
            StrSql = vbCrLf + "  SELECT * FROM "
            StrSql += vbCrLf + "  ("
            'StrSql += vbCrLf + "  SELECT ITEMNAME AS PARTICULAR,ITEMNAME,CASE WHEN MARGINPER <> 0 THEN MARGINPER ELSE NULL END MARGINPER,CASE WHEN MARGINVALUE <> 0 THEN MARGINVALUE ELSE NULL END MARGINVALUE,'T'COLHEAD,SUBITEM,ITEMID,CONVERT(INT,NULL)SUBITEMID FROM " & cnAdminDb & "..ITEMMAST"
            StrSql += vbCrLf + "  SELECT DISTINCT I.ITEMNAME AS PARTICULAR"
            StrSql += vbCrLf + "  ,CASE WHEN ISNUll((SELECT TOP 1 SNO FROM " & cnAdminDb & "..VACONTROL WHERE ITEMID=I.ITEMID AND ROLEID=VA.ROLEID AND ISNULL(SUBITEMID,0)=0),'') <>'' "
            StrSql += vbCrLf + "  THEN (SELECT TOP 1 SNO FROM " & cnAdminDb & "..VACONTROL WHERE ITEMID=I.ITEMID AND ROLEID=VA.ROLEID AND ISNULL(SUBITEMID,0)=0) ELSE '' END SNO"
            StrSql += vbCrLf + "  ,'" & cmbRole.Text & "' AS ROLENAME,ITEMNAME"
            StrSql += vbCrLf + "  ,CASE WHEN VA.VAPER <> 0 THEN VA.VAPER ELSE NULL END VAPER,CASE WHEN VA.VAFIXPER <> 0 THEN VA.VAFIXPER ELSE NULL END VAFIXPER ,"
            StrSql += vbCrLf + "  CASE WHEN VA.VAPERGM <> 0 And ISNULL(VA.ROLEID,0)=" & XRoleid & " THEN VA.VAPERGM ELSE NULL END VAPERGM "
            StrSql += vbCrLf + " ,CASE WHEN VA.WASTPER <> 0 And ISNULL(VA.ROLEID,0)=" & XRoleid & " THEN VA.WASTPER ELSE NULL END WASTPER"
            StrSql += vbCrLf + " ,CASE WHEN VA.WASTAGE <> 0 And ISNULL(VA.ROLEID,0)=" & XRoleid & " THEN VA.WASTAGE ELSE NULL END WASTAGE"
            StrSql += vbCrLf + " ,CASE WHEN VA.MCPER <> 0 And ISNULL(VA.ROLEID,0)=" & XRoleid & " THEN VA.MCPER ELSE NULL END MCPER"
            StrSql += vbCrLf + " ,CASE WHEN VA.MC <> 0 And ISNULL(VA.ROLEID,0)=" & XRoleid & " THEN VA.MC ELSE NULL END MC"
            StrSql += vbCrLf + " ,CASE WHEN VA.ADDVAPER <> 0 And ISNULL(VA.ROLEID,0)=" & XRoleid & " THEN VA.ADDVAPER ELSE NULL END ADDVAPER"
            StrSql += vbCrLf + " ,CASE WHEN VA.GRSAMTPER <> 0 And ISNULL(VA.ROLEID,0)=" & XRoleid & " THEN VA.GRSAMTPER ELSE NULL END GRSAMT "
            StrSql += vbCrLf + " ,CASE WHEN VA.DIAAMTPER <> 0 And ISNULL(VA.ROLEID,0)=" & XRoleid & " THEN VA.DIAAMTPER ELSE NULL END DIAAMT"
            StrSql += vbCrLf + " ,CASE WHEN VA.STNAMTPER <> 0 And ISNULL(VA.ROLEID,0)=" & XRoleid & " THEN VA.STNAMTPER ELSE NULL END STNAMT"
            StrSql += vbCrLf + " ,'T'COLHEAD,SUBITEM,I.ITEMID,CONVERT(INT,NULL)SUBITEMID," & XRoleid & " ROLEID "
            StrSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMMAST I "
            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..VACONTROL VA ON  I.ITEMID = VA.ITEMID AND VA.ROLEID='" & XRoleid & "'"
            If cmbCostCentre_MAN.Enabled Then
                StrSql += " and VA.COSTID= (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "')"
            End If
            If cmbItem.Text <> "ALL" And cmbItem.Text <> "" Then
                StrSql += vbCrLf + " WHERE  I.ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "')"
            End If

            StrSql += vbCrLf + " UNION ALL"
            StrSql += vbCrLf + " SELECT '   ' + SUBITEMNAME,VA.SNO,'" & cmbRole.Text & "' AS ROLENAME"
            StrSql += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.ITEMID)AS ITEMNAME"
            StrSql += vbCrLf + " ,CASE WHEN VA.VAPER <> 0 THEN VA.VAPER ELSE NULL END VAPER,CASE WHEN VA.VAFIXPER <> 0 THEN VA.VAFIXPER ELSE NULL END VAFIXPER"
            StrSql += vbCrLf + " ,CASE WHEN VA.VAPERGM <> 0 And ISNULL(VA.ROLEID,0)=" & XRoleid & " THEN VA.VAPERGM ELSE NULL END VAPERGM "
            StrSql += vbCrLf + " ,CASE WHEN VA.WASTPER <> 0 And ISNULL(VA.ROLEID,0)=" & XRoleid & " THEN VA.WASTPER ELSE NULL END WASTPER"
            StrSql += vbCrLf + " ,CASE WHEN VA.WASTAGE <> 0 And ISNULL(VA.ROLEID,0)=" & XRoleid & " THEN VA.WASTAGE ELSE NULL END WASTAGE"
            StrSql += vbCrLf + " ,CASE WHEN VA.MCPER <> 0 And ISNULL(VA.ROLEID,0)=" & XRoleid & " THEN VA.MCPER ELSE NULL END MCPER"
            StrSql += vbCrLf + " ,CASE WHEN VA.MC <> 0 And ISNULL(VA.ROLEID,0)=" & XRoleid & " THEN VA.MC ELSE NULL END MC"
            StrSql += vbCrLf + " ,CASE WHEN VA.ADDVAPER <> 0 And ISNULL(VA.ROLEID,0)=" & XRoleid & " THEN VA.ADDVAPER ELSE NULL END ADDVAPER   "
            StrSql += vbCrLf + " ,CASE WHEN VA.GRSAMTPER <> 0 And ISNULL(VA.ROLEID,0)=" & XRoleid & " THEN VA.GRSAMTPER ELSE NULL END GRSAMT "
            StrSql += vbCrLf + " ,CASE WHEN VA.DIAAMTPER <> 0 And ISNULL(VA.ROLEID,0)=" & XRoleid & " THEN VA.DIAAMTPER ELSE NULL END DIAAMT"
            StrSql += vbCrLf + " ,CASE WHEN VA.STNAMTPER <> 0 And ISNULL(VA.ROLEID,0)=" & XRoleid & " THEN VA.STNAMTPER ELSE NULL END STNAMT"
            StrSql += vbCrLf + " ,'' COLHEAD,'N' AS SUBITEM,S.ITEMID,S.SUBITEMID," & XRoleid & " ROLEID"
            StrSql += vbCrLf + " FROM " & cnAdminDb & "..SUBITEMMAST AS S LEFT JOIN " & cnAdminDb & "..VACONTROL VA ON  S.SUBITEMID = VA.SUBITEMID AND VA.ROLEID='" & XRoleid & "' "
            If cmbCostCentre_MAN.Enabled Then
                StrSql += " AND VA.COSTID= (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "')"
            End If
            StrSql += vbCrLf + " WHERE 1=1"
            If cmbItem.Text <> "ALL" And cmbItem.Text <> "" Then
                StrSql += vbCrLf + " AND  S.ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "')"
            End If
            If cmbSubItem.Text <> "ALL" And cmbSubItem.Text <> "" Then
                StrSql += vbCrLf + " AND  SUBITEMNAME = '" & cmbSubItem.Text & "'"
            End If
            StrSql += vbCrLf + "  )X"
            StrSql += vbCrLf + "  ORDER BY ITEMNAME,COLHEAD DESC,PARTICULAR"
            Dim dtGrid As New DataTable
            dtGrid.Columns.Add("KEYNO", GetType(Integer))
            dtGrid.Columns("KEYNO").AutoIncrement = True
            dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
            dtGrid.Columns("KEYNO").AutoIncrementStep = 1
            DA = New OleDbDataAdapter(StrSql, cn)
            DA.Fill(dtGrid)

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
                .Columns("SNO").Visible = False
                .Columns("COLHEAD").Visible = False
                If .Columns.Contains("ROLENAME") Then .Columns("ROLENAME").Visible = False
                If .Columns.Contains("ROLEID") Then .Columns("ROLEID").Visible = False
                If .Columns.Contains("ITEMID") Then .Columns("ITEMID").Visible = False
                If .Columns.Contains("SUBITEMID") Then .Columns("SUBITEMID").Visible = False
                If .Columns.Contains("SUBITEM") Then .Columns("SUBITEM").Visible = False
                If .Columns.Contains("ITEMNAME") Then .Columns("ITEMNAME").Visible = False
                .Columns("PARTICULAR").Width = 300
            End With
        Else
            StrSql = vbCrLf + "  SELECT * FROM "
            StrSql += vbCrLf + "  ("
            'StrSql += vbCrLf + "  SELECT ITEMNAME AS PARTICULAR,ITEMNAME,CASE WHEN MARGINPER <> 0 THEN MARGINPER ELSE NULL END MARGINPER,CASE WHEN MARGINVALUE <> 0 THEN MARGINVALUE ELSE NULL END MARGINVALUE,'T'COLHEAD,SUBITEM,ITEMID,CONVERT(INT,NULL)SUBITEMID FROM " & cnAdminDb & "..ITEMMAST"
            StrSql += vbCrLf + "  SELECT DISTINCT I.ITEMNAME AS PARTICULAR"
            StrSql += vbCrLf + "  ,CASE WHEN ISNUll((SELECT TOP 1 SNO FROM " & cnAdminDb & "..VACONTROL WHERE ITEMID=I.ITEMID AND ISNULL(ROLEID,0)=0 AND ISNULL(SUBITEMID,0)=0),'') <>'' "
            StrSql += vbCrLf + "  THEN (SELECT TOP 1 SNO FROM " & cnAdminDb & "..VACONTROL WHERE ITEMID=I.ITEMID AND ISNULL(ROLEID,0)=0 AND ISNULL(SUBITEMID,0)=0) ELSE '' END SNO"
            ''StrSql += vbCrLf + "  ,'' SNO"
            StrSql += vbCrLf + "  ,ITEMNAME,CASE WHEN VA.VAPER <> 0 THEN VA.VAPER ELSE NULL END VAPER,CASE WHEN VA.VAFIXPER <> 0 THEN VA.VAFIXPER ELSE NULL END VAFIXPER ,"
            StrSql += vbCrLf + "  CASE WHEN VA.VAPERGM <> 0 THEN VA.VAPERGM ELSE NULL END VAPERGM "
            StrSql += vbCrLf + " ,CASE WHEN VA.WASTPER <> 0 THEN VA.WASTPER ELSE NULL END WASTPER"
            StrSql += vbCrLf + " ,CASE WHEN VA.MCPER <> 0 THEN VA.MCPER ELSE NULL END MCPER"
            StrSql += vbCrLf + " ,CASE WHEN VA.ADDVAPER <> 0 THEN VA.ADDVAPER ELSE NULL END ADDVAPER"
            StrSql += vbCrLf + " ,CASE WHEN VA.GRSAMTPER <> 0 THEN VA.GRSAMTPER ELSE NULL END GRSAMT "
            StrSql += vbCrLf + " ,CASE WHEN VA.DIAAMTPER <> 0 THEN VA.DIAAMTPER ELSE NULL END DIAAMT"
            StrSql += vbCrLf + " ,'T'COLHEAD,SUBITEM,I.ITEMID,CONVERT(INT,NULL)SUBITEMID "
            StrSql += vbCrLf + " FROM " & cnAdminDb & "..ITEMMAST I "
            StrSql += vbCrLf + " LEFT JOIN " & cnAdminDb & "..VACONTROL VA ON  I.ITEMID = VA.ITEMID AND ISNULL(VA.ROLEID,0)=0"
            If cmbCostCentre_MAN.Enabled Then
                StrSql += " and VA.COSTID= (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "')"
            End If
            If cmbItem.Text <> "ALL" And cmbItem.Text <> "" Then
                StrSql += vbCrLf + " WHERE  I.ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "')"
            End If

            StrSql += vbCrLf + "  UNION ALL"
            StrSql += vbCrLf + "  SELECT '   ' + SUBITEMNAME,VA.SNO,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID = S.ITEMID)AS ITEMNAME,CASE WHEN VA.VAPER <> 0 THEN VA.VAPER ELSE NULL END VAPER,CASE WHEN VA.VAFIXPER <> 0 THEN VA.VAFIXPER ELSE NULL END VAFIXPER,"
            StrSql += vbCrLf + "  CASE WHEN VA.VAPERGM <> 0 THEN VA.VAPERGM ELSE NULL END VAPERGM "
            StrSql += vbCrLf + "  ,CASE WHEN VA.WASTPER <> 0 THEN VA.WASTPER ELSE NULL END WASTPER"
            StrSql += vbCrLf + "  ,CASE WHEN VA.MCPER <> 0 THEN VA.MCPER ELSE NULL END MCPER"
            StrSql += vbCrLf + "  ,CASE WHEN VA.ADDVAPER <> 0 THEN VA.ADDVAPER ELSE NULL END ADDVAPER   "
            StrSql += vbCrLf + "  ,CASE WHEN VA.GRSAMTPER <> 0 THEN VA.GRSAMTPER ELSE NULL END GRSAMT "
            StrSql += vbCrLf + "  ,CASE WHEN VA.DIAAMTPER <> 0 THEN VA.DIAAMTPER ELSE NULL END DIAAMT"
            StrSql += ",'' COLHEAD,'N' AS SUBITEM,S.ITEMID,S.SUBITEMID"
            StrSql += vbCrLf + "  FROM " & cnAdminDb & "..SUBITEMMAST AS S LEFT JOIN " & cnAdminDb & "..VACONTROL VA ON  S.SUBITEMID = VA.SUBITEMID AND ISNULL(VA.ROLEID,0)=0 "
            If cmbCostCentre_MAN.Enabled Then
                StrSql += " AND VA.COSTID= (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "')"
            End If
            StrSql += vbCrLf + " WHERE 1=1"
            If cmbItem.Text <> "ALL" And cmbItem.Text <> "" Then
                StrSql += vbCrLf + " AND  S.ITEMID = (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME = '" & cmbItem.Text & "')"
            End If
            If cmbSubItem.Text <> "ALL" And cmbSubItem.Text <> "" Then
                StrSql += vbCrLf + " AND  SUBITEMNAME = '" & cmbSubItem.Text & "'"
            End If
            StrSql += vbCrLf + "  )X"
            StrSql += vbCrLf + "  ORDER BY ITEMNAME,COLHEAD DESC,PARTICULAR"
            Dim dtGrid As New DataTable
            dtGrid.Columns.Add("KEYNO", GetType(Integer))
            dtGrid.Columns("KEYNO").AutoIncrement = True
            dtGrid.Columns("KEYNO").AutoIncrementSeed = 0
            dtGrid.Columns("KEYNO").AutoIncrementStep = 1
            DA = New OleDbDataAdapter(StrSql, cn)
            DA.Fill(dtGrid)
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
                .Columns("SNO").Visible = False
                .Columns("COLHEAD").Visible = False
                .Columns("SUBITEM").Visible = False
                .Columns("ITEMNAME").Visible = False
                .Columns("ITEMID").Visible = False
                .Columns("SUBITEMID").Visible = False
                .Columns("PARTICULAR").Width = 300
            End With
        End If
        With gridView
            .Columns("VAPER").HeaderText = SCREENNAME & " VA %"
            .Columns("VAPERGM").HeaderText = SCREENNAME & " PER GM"
            .Columns("VAFIXPER").HeaderText = SCREENNAME & " VA FIXED %"
            .Columns("WASTPER").HeaderText = SCREENNAME & " WAST %"
            If .Columns.Contains("WASTAGE") Then .Columns("WASTAGE").HeaderText = SCREENNAME & " WASTAGE "
            If rbtRoleBased.Checked Then
                .Columns("MCPER").HeaderText = SCREENNAME & " MC PERGM"
            Else
                .Columns("MCPER").HeaderText = SCREENNAME & " MC %"
            End If
            ''.Columns("MCPER").HeaderText = SCREENNAME & " MC %"
            If .Columns.Contains("MC") Then .Columns("MC").HeaderText = SCREENNAME & " MC"
            .Columns("ADDVAPER").HeaderText = SCREENNAME & " EXTRA %"
            .Columns("GRSAMT").HeaderText = SCREENNAME & " GRSAMT %"
            .Columns("DIAAMT").HeaderText = SCREENNAME & " DIAAMT %"
            If .Columns.Contains("STNAMT") Then .Columns("STNAMT").HeaderText = SCREENNAME & " STNAMT %"
            .Columns("VAPER").Width = 70
            .Columns("VAPERGM").Width = 70
            .Columns("VAFIXPER").Width = 70
            .Columns("WASTPER").Width = 70
            .Columns("MCPER").Width = 70
            If .Columns.Contains("WASTAGE") Then .Columns("WASTAGE").Width = 70
            If .Columns.Contains("MC") Then .Columns("MC").Width = 70
            .Columns("VAPER").ReadOnly = False
            .Columns("VAPERGM").ReadOnly = False
            .Columns("VAFIXPER").ReadOnly = False
            .Columns("WASTPER").ReadOnly = False
            .Columns("MCPER").ReadOnly = False
            If .Columns.Contains("WASTAGE") Then .Columns("WASTAGE").ReadOnly = False
            If .Columns.Contains("STNAMT") Then .Columns("STNAMT").ReadOnly = False
            If .Columns.Contains("MC") Then .Columns("MC").ReadOnly = False
            .Columns("ADDVAPER").ReadOnly = False
            .Columns("GRSAMT").ReadOnly = False
            .Columns("DIAAMT").ReadOnly = False
            .Columns("VAPER").DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .Columns("VAPERGM").DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .Columns("VAFIXPER").DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .Columns("WASTPER").DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .Columns("MCPER").DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            If .Columns.Contains("MC") Then .Columns("MC").DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .Columns("ADDVAPER").DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .Columns("GRSAMT").DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .Columns("DIAAMT").DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            If .Columns.Contains("WASTAGE") Then .Columns("WASTAGE").DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            If .Columns.Contains("STNAMT") Then .Columns("STNAMT").DefaultCellStyle.Font = New Font("VERDANA", 8, FontStyle.Bold)
            .Columns("VAPER").DefaultCellStyle.Format = "0.00"
            .Columns("VAPERGM").DefaultCellStyle.Format = "0.00"
            .Columns("VAFIXPER").DefaultCellStyle.Format = "0.00"
            .Columns("WASTPER").DefaultCellStyle.Format = "0.00"
            .Columns("MCPER").DefaultCellStyle.Format = "0.00"
            If .Columns.Contains("MC") Then .Columns("MC").DefaultCellStyle.Format = "0.00"
            .Columns("ADDVAPER").DefaultCellStyle.Format = "0.00"
            .Columns("GRSAMT").DefaultCellStyle.Format = "0.00"
            .Columns("DIAAMT").DefaultCellStyle.Format = "0.00"
            If .Columns.Contains("WASTAGE") Then .Columns("WASTAGE").DefaultCellStyle.Format = "0.000"
            If .Columns.Contains("STNAMT") Then .Columns("STNAMT").DefaultCellStyle.Format = "0.00"
        End With
    End Sub

    Private Sub gridView_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gridView.CellEndEdit
        If Not gridView.RowCount > 0 Then Exit Sub
        If gridView.CurrentRow Is Nothing Then Exit Sub
        Try
            Dim mCostid As String = ""
            If cmbCostCentre_MAN.Enabled Then
                StrSql = "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME = '" & cmbCostCentre_MAN.Text & "'"
                mCostid = objGPack.GetSqlValue(StrSql)
            End If
            tran = Nothing
            tran = cn.BeginTransaction
            Dim Sno As String
            If rbtItem.Checked = True Then
                If gridView.CurrentRow.Cells("SUBITEM").Value.ToString = "Y" Then
                    If gridView.CurrentRow.Cells("SNO").Value.ToString = "" Then
                        'Dim Sno As String = GetNewSno(TranSnoType.ACCTRANCODE, tran)
                        Sno = GetNewSno(TranSnoType.VACONTROLCODE, tran, "GET_SNO_ADMIN")
                        StrSql = " INSERT INTO " & cnAdminDb & "..VACONTROL(SNO,COSTID,VAPER,VAPERGM,VAFIXPER"
                        StrSql += " ,WASTPER,MCPER,ADDVAPER,VATYPE,GRSAMTPER,DIAAMTPER,ITEMID,SUBITEMID,ACTIVE)"
                        StrSql += " VALUES('" & Sno & "','" & mCostid & "'"
                        StrSql += " ," & Val(gridView.CurrentRow.Cells("VAPER").Value.ToString) & ""
                        StrSql += " ," & Val(gridView.CurrentRow.Cells("VAPERGM").Value.ToString) & ""
                        StrSql += " ," & Val(gridView.CurrentRow.Cells("VAFIXPER").Value.ToString) & ""
                        StrSql += " ," & Val(gridView.CurrentRow.Cells("WASTPER").Value.ToString) & ""
                        StrSql += " ," & Val(gridView.CurrentRow.Cells("MCPER").Value.ToString) & ""
                        StrSql += " ," & Val(gridView.CurrentRow.Cells("ADDVAPER").Value.ToString) & ""
                        StrSql += " ,'" & Finalamtlockbase & "'"
                        StrSql += " ," & Val(gridView.CurrentRow.Cells("GRSAMT").Value.ToString) & ""
                        StrSql += " ," & Val(gridView.CurrentRow.Cells("DIAAMT").Value.ToString) & ","
                        StrSql += Val(gridView.CurrentRow.Cells("ITEMID").Value.ToString) & "," & Val(gridView.CurrentRow.Cells("SUBITEMID").Value.ToString) & ",'Y')"
                        gridView.CurrentRow.Cells("SNO").Value = Sno

                    Else
                        StrSql = " UPDATE " & cnAdminDb & "..VACONTROL "
                        StrSql += " SET VAPER=" & Val(gridView.CurrentRow.Cells("VAPER").Value.ToString) & ""
                        StrSql += " ,VAPERGM=" & Val(gridView.CurrentRow.Cells("VAPERGM").Value.ToString) & ""
                        StrSql += " ,VAFIXPER=" & Val(gridView.CurrentRow.Cells("VAFIXPER").Value.ToString) & ""
                        StrSql += " ,WASTPER=" & Val(gridView.CurrentRow.Cells("WASTPER").Value.ToString) & ""
                        StrSql += " ,MCPER=" & Val(gridView.CurrentRow.Cells("MCPER").Value.ToString) & ""
                        StrSql += " ,ADDVAPER=" & Val(gridView.CurrentRow.Cells("ADDVAPER").Value.ToString)
                        StrSql += " ,GRSAMTPER=" & Val(gridView.CurrentRow.Cells("GRSAMT").Value.ToString)
                        StrSql += " ,DIAAMTPER=" & Val(gridView.CurrentRow.Cells("DIAAMT").Value.ToString)
                        StrSql += " WHERE SNO ='" & gridView.CurrentRow.Cells("SNO").Value.ToString & "'"

                    End If
                    ExecQuery(SyncMode.Master, StrSql, cn, tran)
                    For cnt As Integer = e.RowIndex + 1 To gridView.RowCount - 1
                        If gridView.Rows(cnt).Cells("ITEMNAME").Value.ToString <> gridView.CurrentRow.Cells("ITEMNAME").Value.ToString Then Exit For
                        If gridView.Rows(cnt).Cells("SNO").Value.ToString = "" Then
                            'Dim Sno As String = GetNewSno(TranSnoType.ACCTRANCODE, tran)
                            Sno = GetNewSno(TranSnoType.VACONTROLCODE, tran, "GET_SNO_ADMIN")
                            StrSql = " INSERT INTO " & cnAdminDb & "..VACONTROL(SNO,COSTID,VAPER,VAPERGM,VAFIXPER"
                            StrSql += " ,WASTPER,MCPER,ADDVAPER,VATYPE,GRSAMTPER,DIAAMTPER,ITEMID,SUBITEMID,ACTIVE)"
                            StrSql += " VALUES('" & Sno & "','" & mCostid & "'," & Val(gridView.CurrentRow.Cells("VAPER").Value.ToString) & ""
                            StrSql += " ," & Val(gridView.CurrentRow.Cells("VAPERGM").Value.ToString) & ""
                            StrSql += " ," & Val(gridView.CurrentRow.Cells("VAFIXPER").Value.ToString) & ""
                            StrSql += " ," & Val(gridView.CurrentRow.Cells("WASTPER").Value.ToString) & ""
                            StrSql += " ," & Val(gridView.CurrentRow.Cells("MCPER").Value.ToString) & ""
                            StrSql += " ," & Val(gridView.CurrentRow.Cells("ADDVAPER").Value.ToString) & ""
                            StrSql += " ,'" & Finalamtlockbase & "'"
                            StrSql += " ," & Val(gridView.CurrentRow.Cells("GRSAMT").Value.ToString) & ""
                            StrSql += " ," & Val(gridView.CurrentRow.Cells("DIAAMT").Value.ToString) & ","
                            StrSql += Val(gridView.CurrentRow.Cells("ITEMID").Value.ToString) & "," & Val(gridView.Rows(cnt).Cells("SUBITEMID").Value.ToString) & ",'Y')"
                            gridView.Rows(cnt).Cells("SNO").Value = Sno
                        Else
                            StrSql = " UPDATE " & cnAdminDb & "..VACONTROL "
                            StrSql += " SET VAPER=" & Val(gridView.CurrentRow.Cells("VAPER").Value.ToString) & ""
                            StrSql += " ,VAPERGM=" & Val(gridView.CurrentRow.Cells("VAPERGM").Value.ToString) & ""
                            StrSql += " ,VAFIXPER=" & Val(gridView.CurrentRow.Cells("VAFIXPER").Value.ToString) & ""
                            StrSql += " ,WASTPER=" & Val(gridView.CurrentRow.Cells("WASTPER").Value.ToString) & ""
                            StrSql += " ,MCPER=" & Val(gridView.CurrentRow.Cells("MCPER").Value.ToString) & ""
                            StrSql += " ,ADDVAPER=" & Val(gridView.CurrentRow.Cells("ADDVAPER").Value.ToString)
                            StrSql += " ,GRSAMTPER=" & Val(gridView.CurrentRow.Cells("GRSAMT").Value.ToString)
                            StrSql += " ,DIAAMTPER=" & Val(gridView.CurrentRow.Cells("DIAAMT").Value.ToString)
                            StrSql += " WHERE SNO ='" & gridView.Rows(cnt).Cells("SNO").Value.ToString & "'"
                        End If
                        ExecQuery(SyncMode.Master, StrSql, cn, tran)
                        gridView.Rows(cnt).Cells("VAPER").Value = gridView.CurrentRow.Cells("VAPER").Value
                        gridView.Rows(cnt).Cells("VAFIXPER").Value = gridView.CurrentRow.Cells("VAFIXPER").Value
                        gridView.Rows(cnt).Cells("VAPERGM").Value = gridView.CurrentRow.Cells("VAPERGM").Value
                        gridView.Rows(cnt).Cells("WASTPER").Value = gridView.CurrentRow.Cells("WASTPER").Value
                        gridView.Rows(cnt).Cells("MCPER").Value = gridView.CurrentRow.Cells("MCPER").Value
                        gridView.Rows(cnt).Cells("ADDVAPER").Value = gridView.CurrentRow.Cells("ADDVAPER").Value
                        gridView.Rows(cnt).Cells("DIAAMT").Value = gridView.CurrentRow.Cells("DIAAMT").Value
                    Next
                    'gridView.CurrentCell.Value = DBNull.Value
                Else
                    If gridView.CurrentRow.Cells("SNO").Value.ToString = "" Then
                        'Dim Sno As String = GetNewSno(TranSnoType.ACCTRANCODE, tran)
                        Sno = GetNewSno(TranSnoType.VACONTROLCODE, tran, "GET_SNO_ADMIN")
                        StrSql = " INSERT INTO " & cnAdminDb & "..VACONTROL(SNO,COSTID,VAPER,VAPERGM,VAFIXPER"
                        StrSql += " ,WASTPER,MCPER,ADDVAPER,VATYPE,GRSAMTPER,DIAAMTPER,ITEMID,SUBITEMID,ACTIVE)"
                        StrSql += " VALUES('" & Sno & "','" & mCostid & "'"
                        StrSql += " ," & Val(gridView.CurrentRow.Cells("VAPER").Value.ToString) & ""
                        StrSql += " ," & Val(gridView.CurrentRow.Cells("VAPERGM").Value.ToString) & ""
                        StrSql += " ," & Val(gridView.CurrentRow.Cells("VAFIXPER").Value.ToString) & ""
                        StrSql += " ," & Val(gridView.CurrentRow.Cells("WASTPER").Value.ToString) & ""
                        StrSql += " ," & Val(gridView.CurrentRow.Cells("MCPER").Value.ToString) & ""
                        StrSql += " ," & Val(gridView.CurrentRow.Cells("ADDVAPER").Value.ToString) & ""
                        StrSql += " ,'" & Finalamtlockbase & "'"
                        StrSql += " ," & Val(gridView.CurrentRow.Cells("GRSAMT").Value.ToString) & ""
                        StrSql += " ," & Val(gridView.CurrentRow.Cells("DIAAMT").Value.ToString) & ","
                        StrSql += Val(gridView.CurrentRow.Cells("ITEMID").Value.ToString) & "," & Val(gridView.CurrentRow.Cells("SUBITEMID").Value.ToString) & ",'Y')"
                        gridView.CurrentRow.Cells("SNO").Value = Sno
                    Else
                        StrSql = " UPDATE " & cnAdminDb & "..VACONTROL "
                        StrSql += " SET VAPER=" & Val(gridView.CurrentRow.Cells("VAPER").Value.ToString) & ""
                        StrSql += " ,VAPERGM=" & Val(gridView.CurrentRow.Cells("VAPERGM").Value.ToString) & ""
                        StrSql += " ,VAFIXPER=" & Val(gridView.CurrentRow.Cells("VAFIXPER").Value.ToString) & ""
                        StrSql += " ,WASTPER=" & Val(gridView.CurrentRow.Cells("WASTPER").Value.ToString) & ""
                        StrSql += " ,MCPER=" & Val(gridView.CurrentRow.Cells("MCPER").Value.ToString) & ""
                        StrSql += " ,ADDVAPER=" & Val(gridView.CurrentRow.Cells("ADDVAPER").Value.ToString)
                        StrSql += " ,GRSAMTPER=" & Val(gridView.CurrentRow.Cells("GRSAMT").Value.ToString)
                        StrSql += " ,DIAAMTPER=" & Val(gridView.CurrentRow.Cells("DIAAMT").Value.ToString)
                        StrSql += " WHERE SNO ='" & gridView.CurrentRow.Cells("SNO").Value.ToString & "'"
                    End If
                    ExecQuery(SyncMode.Master, StrSql, cn, tran)
                End If
            ElseIf rbtDesigner.Checked Then
                If gridView.CurrentRow.Cells("SNO").Value.ToString = "" Then
                    'Dim Sno As String = GetNewSno(TranSnoType.ACCTRANCODE, tran)
                    Sno = GetNewSno(TranSnoType.VACONTROLCODE, tran, "GET_SNO_ADMIN")
                    StrSql = " INSERT INTO " & cnAdminDb & "..VACONTROL(SNO,COSTID,VAPER,VAPERGM,VAFIXPER"
                    StrSql += " ,WASTPER,MCPER,ADDVAPER,VATYPE,GRSAMTPER,DIAAMTPER,DESIGNERID,TABLECODE,ITEMID,SUBITEMID,ACTIVE)"
                    StrSql += " VALUES('" & Sno & "','" & mCostid & "'"
                    StrSql += " ," & Val(gridView.CurrentRow.Cells("VAPER").Value.ToString) & ""
                    StrSql += " ," & Val(gridView.CurrentRow.Cells("VAPERGM").Value.ToString) & ""
                    StrSql += " ," & Val(gridView.CurrentRow.Cells("VAFIXPER").Value.ToString) & ""
                    StrSql += " ," & Val(gridView.CurrentRow.Cells("WASTPER").Value.ToString) & ""
                    StrSql += " ," & Val(gridView.CurrentRow.Cells("MCPER").Value.ToString) & ""
                    StrSql += " ," & Val(gridView.CurrentRow.Cells("ADDVAPER").Value.ToString) & ""
                    StrSql += " ,'" & Finalamtlockbase & "'"
                    StrSql += " ," & Val(gridView.CurrentRow.Cells("GRSAMT").Value.ToString) & ""
                    StrSql += " ," & Val(gridView.CurrentRow.Cells("DIAAMT").Value.ToString) & ","
                    StrSql += " ," & Val(gridView.CurrentRow.Cells("DESIGNERID").Value.ToString) & ",''," & Val(gridView.CurrentRow.Cells("ITEMID").Value.ToString) & ",0,'Y')"
                    gridView.CurrentRow.Cells("SNO").Value = Sno
                Else
                    StrSql = " UPDATE " & cnAdminDb & "..VACONTROL "
                    StrSql += " SET VAPER=" & Val(gridView.CurrentRow.Cells("VAPER").Value.ToString) & ""
                    StrSql += " ,VAPERGM=" & Val(gridView.CurrentRow.Cells("VAPERGM").Value.ToString) & ""
                    StrSql += " ,VAFIXPER=" & Val(gridView.CurrentRow.Cells("VAFIXPER").Value.ToString) & ""
                    StrSql += " ,WASTPER=" & Val(gridView.CurrentRow.Cells("WASTPER").Value.ToString) & ""
                    StrSql += " ,MCPER=" & Val(gridView.CurrentRow.Cells("MCPER").Value.ToString) & ""
                    StrSql += " ,ADDVAPER=" & Val(gridView.CurrentRow.Cells("ADDVAPER").Value.ToString)
                    StrSql += " ,GRSAMTPER=" & Val(gridView.CurrentRow.Cells("GRSAMT").Value.ToString)
                    StrSql += " ,DIAAMTPER=" & Val(gridView.CurrentRow.Cells("DIAAMT").Value.ToString)
                    StrSql += " WHERE SNO ='" & gridView.CurrentRow.Cells("SNO").Value.ToString & "'"
                End If
                ExecQuery(SyncMode.Master, StrSql, cn, tran)
            ElseIf rbtRoleBased.Checked Then
                If gridView.CurrentRow.Cells("SUBITEM").Value.ToString = "Y" Then
                    If gridView.CurrentRow.Cells("SNO").Value.ToString = "" Then
                        'Dim Sno As String = GetNewSno(TranSnoType.ACCTRANCODE, tran)
                        Sno = GetNewSno(TranSnoType.VACONTROLCODE, tran, "GET_SNO_ADMIN")
                        StrSql = " INSERT INTO " & cnAdminDb & "..VACONTROL(SNO,COSTID,VAPER,VAPERGM,VAFIXPER"
                        StrSql += " ,WASTPER,MCPER,MC,ADDVAPER,VATYPE,GRSAMTPER,DIAAMTPER,ITEMID,SUBITEMID,ROLEID,ACTIVE,WASTAGE,STNAMTPER)"
                        StrSql += " VALUES('" & Sno & "','" & mCostid & "'"
                        StrSql += " ," & Val(gridView.CurrentRow.Cells("VAPER").Value.ToString) & ""
                        StrSql += " ," & Val(gridView.CurrentRow.Cells("VAPERGM").Value.ToString) & ""
                        StrSql += " ," & Val(gridView.CurrentRow.Cells("VAFIXPER").Value.ToString) & ""
                        StrSql += " ," & Val(gridView.CurrentRow.Cells("WASTPER").Value.ToString) & ""
                        StrSql += " ," & Val(gridView.CurrentRow.Cells("MCPER").Value.ToString) & ""
                        StrSql += " ," & Val(gridView.CurrentRow.Cells("MC").Value.ToString) & ""
                        StrSql += " ," & Val(gridView.CurrentRow.Cells("ADDVAPER").Value.ToString) & ""
                        StrSql += " ,'" & Finalamtlockbase & "'"
                        StrSql += " ," & Val(gridView.CurrentRow.Cells("GRSAMT").Value.ToString) & ""
                        StrSql += " ," & Val(gridView.CurrentRow.Cells("DIAAMT").Value.ToString) & ","
                        StrSql += Val(gridView.CurrentRow.Cells("ITEMID").Value.ToString) & ","
                        StrSql += Val(gridView.CurrentRow.Cells("SUBITEMID").Value.ToString) & ","
                        StrSql += Val(gridView.CurrentRow.Cells("ROLEID").Value.ToString) & ",'Y',"
                        StrSql += Val(gridView.CurrentRow.Cells("WASTAGE").Value.ToString) & ","
                        StrSql += Val(gridView.CurrentRow.Cells("STNAMT").Value.ToString) & ")"
                        gridView.CurrentRow.Cells("SNO").Value = Sno

                    Else
                        StrSql = " UPDATE " & cnAdminDb & "..VACONTROL "
                        StrSql += " SET VAPER=" & Val(gridView.CurrentRow.Cells("VAPER").Value.ToString) & ""
                        StrSql += " ,VAPERGM=" & Val(gridView.CurrentRow.Cells("VAPERGM").Value.ToString) & ""
                        StrSql += " ,VAFIXPER=" & Val(gridView.CurrentRow.Cells("VAFIXPER").Value.ToString) & ""
                        StrSql += " ,WASTPER=" & Val(gridView.CurrentRow.Cells("WASTPER").Value.ToString) & ""
                        StrSql += " ,MCPER=" & Val(gridView.CurrentRow.Cells("MCPER").Value.ToString) & ""
                        StrSql += " ,MC=" & Val(gridView.CurrentRow.Cells("MC").Value.ToString) & ""
                        StrSql += " ,ADDVAPER=" & Val(gridView.CurrentRow.Cells("ADDVAPER").Value.ToString)
                        StrSql += " ,GRSAMTPER=" & Val(gridView.CurrentRow.Cells("GRSAMT").Value.ToString)
                        StrSql += " ,DIAAMTPER=" & Val(gridView.CurrentRow.Cells("DIAAMT").Value.ToString)
                        StrSql += " ,WASTAGE=" & Val(gridView.CurrentRow.Cells("WASTAGE").Value.ToString)
                        StrSql += " ,STNAMTPER=" & Val(gridView.CurrentRow.Cells("STNAMT").Value.ToString)
                        StrSql += " WHERE SNO ='" & gridView.CurrentRow.Cells("SNO").Value.ToString & "'"

                    End If
                    ExecQuery(SyncMode.Master, StrSql, cn, tran)
                    For cnt As Integer = e.RowIndex + 1 To gridView.RowCount - 1
                        If gridView.Rows(cnt).Cells("ITEMNAME").Value.ToString <> gridView.CurrentRow.Cells("ITEMNAME").Value.ToString Then Exit For
                        If gridView.Rows(cnt).Cells("SNO").Value.ToString = "" Then
                            'Dim Sno As String = GetNewSno(TranSnoType.ACCTRANCODE, tran)
                            Sno = GetNewSno(TranSnoType.VACONTROLCODE, tran, "GET_SNO_ADMIN")
                            StrSql = " INSERT INTO " & cnAdminDb & "..VACONTROL(SNO,COSTID,VAPER,VAPERGM,VAFIXPER"
                            StrSql += " ,WASTPER,MCPER,MC,ADDVAPER,VATYPE,GRSAMTPER,DIAAMTPER,ITEMID,SUBITEMID,ROLEID,ACTIVE,WASTAGE,STNAMTPER)"
                            StrSql += " VALUES('" & Sno & "','" & mCostid & "'," & Val(gridView.CurrentRow.Cells("VAPER").Value.ToString) & ""
                            StrSql += " ," & Val(gridView.CurrentRow.Cells("VAPERGM").Value.ToString) & ""
                            StrSql += " ," & Val(gridView.CurrentRow.Cells("VAFIXPER").Value.ToString) & ""
                            StrSql += " ," & Val(gridView.CurrentRow.Cells("WASTPER").Value.ToString) & ""
                            StrSql += " ," & Val(gridView.CurrentRow.Cells("MCPER").Value.ToString) & ""
                            StrSql += " ," & Val(gridView.CurrentRow.Cells("MC").Value.ToString) & ""
                            StrSql += " ," & Val(gridView.CurrentRow.Cells("ADDVAPER").Value.ToString) & ""
                            StrSql += " ,'" & Finalamtlockbase & "'"
                            StrSql += " ," & Val(gridView.CurrentRow.Cells("GRSAMT").Value.ToString) & ""
                            StrSql += " ," & Val(gridView.CurrentRow.Cells("DIAAMT").Value.ToString) & ","
                            StrSql += Val(gridView.CurrentRow.Cells("ITEMID").Value.ToString) & ","
                            StrSql += Val(gridView.Rows(cnt).Cells("SUBITEMID").Value.ToString) & ","
                            StrSql += Val(gridView.Rows(cnt).Cells("ROLEID").Value.ToString) & ",'Y',"
                            StrSql += Val(gridView.Rows(cnt).Cells("WASTAGE").Value.ToString) & ","
                            StrSql += Val(gridView.Rows(cnt).Cells("STNAMT").Value.ToString) & ")"
                            gridView.Rows(cnt).Cells("SNO").Value = Sno
                        Else
                            StrSql = " UPDATE " & cnAdminDb & "..VACONTROL "
                            StrSql += " SET VAPER=" & Val(gridView.CurrentRow.Cells("VAPER").Value.ToString) & ""
                            StrSql += " ,VAPERGM=" & Val(gridView.CurrentRow.Cells("VAPERGM").Value.ToString) & ""
                            StrSql += " ,VAFIXPER=" & Val(gridView.CurrentRow.Cells("VAFIXPER").Value.ToString) & ""
                            StrSql += " ,WASTPER=" & Val(gridView.CurrentRow.Cells("WASTPER").Value.ToString) & ""
                            StrSql += " ,MCPER=" & Val(gridView.CurrentRow.Cells("MCPER").Value.ToString) & ""
                            StrSql += " ,MC=" & Val(gridView.CurrentRow.Cells("MC").Value.ToString) & ""
                            StrSql += " ,ADDVAPER=" & Val(gridView.CurrentRow.Cells("ADDVAPER").Value.ToString)
                            StrSql += " ,GRSAMTPER=" & Val(gridView.CurrentRow.Cells("GRSAMT").Value.ToString)
                            StrSql += " ,DIAAMTPER=" & Val(gridView.CurrentRow.Cells("DIAAMT").Value.ToString)
                            StrSql += " ,WASTAGE=" & Val(gridView.CurrentRow.Cells("WASTAGE").Value.ToString)
                            StrSql += " ,STNAMTPER=" & Val(gridView.CurrentRow.Cells("STNAMT").Value.ToString)
                            StrSql += " WHERE SNO ='" & gridView.Rows(cnt).Cells("SNO").Value.ToString & "'"
                        End If
                        ExecQuery(SyncMode.Master, StrSql, cn, tran)
                        gridView.Rows(cnt).Cells("VAPER").Value = gridView.CurrentRow.Cells("VAPER").Value
                        gridView.Rows(cnt).Cells("VAFIXPER").Value = gridView.CurrentRow.Cells("VAFIXPER").Value
                        gridView.Rows(cnt).Cells("VAPERGM").Value = gridView.CurrentRow.Cells("VAPERGM").Value
                        gridView.Rows(cnt).Cells("WASTPER").Value = gridView.CurrentRow.Cells("WASTPER").Value
                        gridView.Rows(cnt).Cells("MCPER").Value = gridView.CurrentRow.Cells("MCPER").Value
                        gridView.Rows(cnt).Cells("MC").Value = gridView.CurrentRow.Cells("MC").Value
                        gridView.Rows(cnt).Cells("ADDVAPER").Value = gridView.CurrentRow.Cells("ADDVAPER").Value
                        gridView.Rows(cnt).Cells("DIAAMT").Value = gridView.CurrentRow.Cells("DIAAMT").Value
                        gridView.Rows(cnt).Cells("WASTAGE").Value = gridView.CurrentRow.Cells("WASTAGE").Value
                        gridView.Rows(cnt).Cells("STNAMT").Value = gridView.CurrentRow.Cells("STNAMT").Value
                    Next
                    'gridView.CurrentCell.Value = DBNull.Value
                Else
                    If gridView.CurrentRow.Cells("SNO").Value.ToString = "" Then
                        'Dim Sno As String = GetNewSno(TranSnoType.ACCTRANCODE, tran)
                        Sno = GetNewSno(TranSnoType.VACONTROLCODE, tran, "GET_SNO_ADMIN")
                        StrSql = " INSERT INTO " & cnAdminDb & "..VACONTROL(SNO,COSTID,VAPER,VAPERGM,VAFIXPER"
                        StrSql += " ,WASTPER,MCPER,MC,ADDVAPER,VATYPE,GRSAMTPER,DIAAMTPER,ITEMID,SUBITEMID,ROLEID,ACTIVE,WASTAGE,STNAMTPER)"
                        StrSql += " VALUES('" & Sno & "','" & mCostid & "'"
                        StrSql += " ," & Val(gridView.CurrentRow.Cells("VAPER").Value.ToString) & ""
                        StrSql += " ," & Val(gridView.CurrentRow.Cells("VAPERGM").Value.ToString) & ""
                        StrSql += " ," & Val(gridView.CurrentRow.Cells("VAFIXPER").Value.ToString) & ""
                        StrSql += " ," & Val(gridView.CurrentRow.Cells("WASTPER").Value.ToString) & ""
                        StrSql += " ," & Val(gridView.CurrentRow.Cells("MCPER").Value.ToString) & ""
                        StrSql += " ," & Val(gridView.CurrentRow.Cells("MC").Value.ToString) & ""
                        StrSql += " ," & Val(gridView.CurrentRow.Cells("ADDVAPER").Value.ToString) & ""
                        StrSql += " ,'" & Finalamtlockbase & "'"
                        StrSql += " ," & Val(gridView.CurrentRow.Cells("GRSAMT").Value.ToString) & ""
                        StrSql += " ," & Val(gridView.CurrentRow.Cells("DIAAMT").Value.ToString) & ","
                        StrSql += Val(gridView.CurrentRow.Cells("ITEMID").Value.ToString) & ","
                        StrSql += Val(gridView.CurrentRow.Cells("SUBITEMID").Value.ToString) & ","
                        StrSql += Val(gridView.CurrentRow.Cells("ROLEID").Value.ToString) & ",'Y',"
                        StrSql += Val(gridView.CurrentRow.Cells("WASTAGE").Value.ToString) & ","
                        StrSql += Val(gridView.CurrentRow.Cells("STNAMT").Value.ToString) & ")"
                        gridView.CurrentRow.Cells("SNO").Value = Sno
                    Else
                        StrSql = " UPDATE " & cnAdminDb & "..VACONTROL "
                        StrSql += " SET VAPER=" & Val(gridView.CurrentRow.Cells("VAPER").Value.ToString) & ""
                        StrSql += " ,VAPERGM=" & Val(gridView.CurrentRow.Cells("VAPERGM").Value.ToString) & ""
                        StrSql += " ,VAFIXPER=" & Val(gridView.CurrentRow.Cells("VAFIXPER").Value.ToString) & ""
                        StrSql += " ,WASTPER=" & Val(gridView.CurrentRow.Cells("WASTPER").Value.ToString) & ""
                        StrSql += " ,MCPER=" & Val(gridView.CurrentRow.Cells("MCPER").Value.ToString) & ""
                        StrSql += " ,MC=" & Val(gridView.CurrentRow.Cells("MC").Value.ToString) & ""
                        StrSql += " ,ADDVAPER=" & Val(gridView.CurrentRow.Cells("ADDVAPER").Value.ToString)
                        StrSql += " ,GRSAMTPER=" & Val(gridView.CurrentRow.Cells("GRSAMT").Value.ToString)
                        StrSql += " ,DIAAMTPER=" & Val(gridView.CurrentRow.Cells("DIAAMT").Value.ToString)
                        StrSql += " ,WASTAGE=" & Val(gridView.CurrentRow.Cells("WASTAGE").Value.ToString)
                        StrSql += " ,STNAMTPER=" & Val(gridView.CurrentRow.Cells("STNAMT").Value.ToString)
                        StrSql += " WHERE SNO ='" & gridView.CurrentRow.Cells("SNO").Value.ToString & "'"
                    End If
                    ExecQuery(SyncMode.Master, StrSql, cn, tran)
                End If
            Else
                If gridView.CurrentRow.Cells("SNO").Value.ToString = "" Then
                    'Dim Sno As String = GetNewSno(TranSnoType.ACCTRANCODE, tran)
                    Sno = GetNewSno(TranSnoType.VACONTROLCODE, tran, "GET_SNO_ADMIN")
                    StrSql = " INSERT INTO " & cnAdminDb & "..VACONTROL(SNO,COSTID,VAPER,VAPERGM,VAFIXPER"
                    StrSql += " ,WASTPER,MCPER,ADDVAPER,VATYPE,GRSAMTPER,DIAAMTPER,TABLECODE,ITEMID,SUBITEMID,ACTIVE)"
                    StrSql += " VALUES('" & Sno & "','" & mCostid & "'"
                    StrSql += " ," & Val(gridView.CurrentRow.Cells("VAPER").Value.ToString) & ""
                    StrSql += " ," & Val(gridView.CurrentRow.Cells("VAPERGM").Value.ToString) & ""
                    StrSql += " ," & Val(gridView.CurrentRow.Cells("VAFIXPER").Value.ToString) & ""
                    StrSql += " ," & Val(gridView.CurrentRow.Cells("WASTPER").Value.ToString) & ""
                    StrSql += " ," & Val(gridView.CurrentRow.Cells("MCPER").Value.ToString) & ""
                    StrSql += " ," & Val(gridView.CurrentRow.Cells("ADDVAPER").Value.ToString) & ""
                    StrSql += " ,'" & Finalamtlockbase & "'"
                    StrSql += " ," & Val(gridView.CurrentRow.Cells("GRSAMT").Value.ToString) & ""
                    StrSql += " ," & Val(gridView.CurrentRow.Cells("DIAAMT").Value.ToString) & ""
                    StrSql += " ,'" & gridView.CurrentRow.Cells("TABLECODE").Value.ToString & "',0,0,'Y')"
                    gridView.CurrentRow.Cells("SNO").Value = Sno
                Else
                    StrSql = " UPDATE " & cnAdminDb & "..VACONTROL "
                    StrSql += " SET VAPER=" & Val(gridView.CurrentRow.Cells("VAPER").Value.ToString) & ""
                    StrSql += " ,VAPERGM=" & Val(gridView.CurrentRow.Cells("VAPERGM").Value.ToString) & ""
                    StrSql += " ,VAFIXPER=" & Val(gridView.CurrentRow.Cells("VAFIXPER").Value.ToString) & ""
                    StrSql += " ,WASTPER=" & Val(gridView.CurrentRow.Cells("WASTPER").Value.ToString) & ""
                    StrSql += " ,MCPER=" & Val(gridView.CurrentRow.Cells("MCPER").Value.ToString) & ""
                    StrSql += " ,ADDVAPER=" & Val(gridView.CurrentRow.Cells("ADDVAPER").Value.ToString)
                    StrSql += " ,GRSAMTPER=" & Val(gridView.CurrentRow.Cells("GRSAMT").Value.ToString)
                    StrSql += " ,DIAAMTPER=" & Val(gridView.CurrentRow.Cells("DIAAMT").Value.ToString)
                    StrSql += " WHERE SNO ='" & gridView.CurrentRow.Cells("SNO").Value.ToString & "'"
                End If
                ExecQuery(SyncMode.Master, StrSql, cn, tran)
            End If
            tran.Commit()
            tran = Nothing
        Catch ex As Exception
            If Not tran Is Nothing Then tran.Rollback()
            MsgBox(ex.Message + vbCrLf + ex.StackTrace)
        End Try
    End Sub
    Private Sub gridView_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles gridView.EditingControlShowing
        If Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("VAPERGM").Index And Not e.Control Is Nothing Then
            Dim tb As TextBox = CType(e.Control, TextBox)
            tb.Tag = "AMOUNT"
            AddHandler tb.KeyPress, AddressOf TextKeyPressEvent
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("VAPER").Index And Not e.Control Is Nothing Then
            Dim tb As TextBox = CType(e.Control, TextBox)
            tb.Tag = "PER"
            AddHandler tb.KeyPress, AddressOf TextKeyPressEvent
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("VAFIXPER").Index And Not e.Control Is Nothing Then
            Dim tb As TextBox = CType(e.Control, TextBox)
            tb.Tag = "PER"
            AddHandler tb.KeyPress, AddressOf TextKeyPressEvent
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("WASTPER").Index And Not e.Control Is Nothing Then
            Dim tb As TextBox = CType(e.Control, TextBox)
            tb.Tag = "PER"
            AddHandler tb.KeyPress, AddressOf TextKeyPressEvent
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("MCPER").Index And Not e.Control Is Nothing And rbtRoleBased.Checked = False Then
            Dim tb As TextBox = CType(e.Control, TextBox)
            tb.Tag = "PER"
            AddHandler tb.KeyPress, AddressOf TextKeyPressEvent
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("MCPER").Index And Not e.Control Is Nothing And rbtRoleBased.Checked = True Then
            Dim tb As TextBox = CType(e.Control, TextBox)
            tb.Tag = "AMOUNT"
            AddHandler tb.KeyPress, AddressOf TextKeyPressEvent
        ElseIf gridView.Columns.Contains("MC") And rbtRoleBased.Checked = True Then
            If Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("MC").Index And Not e.Control Is Nothing And rbtRoleBased.Checked = True Then
                Dim tb As TextBox = CType(e.Control, TextBox)
                tb.Tag = "AMOUNT"
                AddHandler tb.KeyPress, AddressOf TextKeyPressEvent
            End If
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("ADDVAPER").Index And Not e.Control Is Nothing Then
            Dim tb As TextBox = CType(e.Control, TextBox)
            tb.Tag = "PER"
            AddHandler tb.KeyPress, AddressOf TextKeyPressEvent
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("GRSAMT").Index And Not e.Control Is Nothing Then
            Dim tb As TextBox = CType(e.Control, TextBox)
            tb.Tag = "PER"
            AddHandler tb.KeyPress, AddressOf TextKeyPressEvent
        ElseIf Me.gridView.CurrentCell.ColumnIndex = gridView.Columns("DIAAMT").Index And Not e.Control Is Nothing Then
            Dim tb As TextBox = CType(e.Control, TextBox)
            tb.Tag = "PER"
            AddHandler tb.KeyPress, AddressOf TextKeyPressEvent
        End If
    End Sub
    Private Sub TextKeyPressEvent(ByVal sender As Object, ByVal e As KeyPressEventArgs)
        If CType(sender, TextBox).Tag = "AMOUNT" Then
            textBoxKeyPressValidator(CType(sender, TextBox), e, Datatype.Amount)
        Else
            textBoxKeyPressValidator(CType(sender, TextBox), e, Datatype.Percentage)
        End If
    End Sub

    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click, ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        cmbItem.Text = "ALL"
        cmbSubItem.Text = "ALL"
        gridView.DataSource = Nothing
        cmbCostCentre_MAN.Select()
        cmbCostCentre_MAN.Focus()
    End Sub

    Private Sub rbtTable_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtTable.CheckedChanged
        If rbtTable.Checked = True Then
            cmbItem.Enabled = False
            cmbSubItem.Enabled = False
            cmbDesigner.Enabled = False
            cmbRole.Enabled = False
        End If
    End Sub

    Private Sub rbtItem_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtItem.CheckedChanged
        If rbtItem.Checked = True Then
            cmbItem.Enabled = True
            cmbSubItem.Enabled = True
            cmbDesigner.Enabled = False
            cmbRole.Enabled = False
        End If
    End Sub

    Private Sub rbTDesigner_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtDesigner.CheckedChanged
        If rbtDesigner.Checked = True Then
            cmbItem.Enabled = False
            cmbSubItem.Enabled = False
            cmbDesigner.Enabled = True
            cmbRole.Enabled = False
        End If
    End Sub

    Private Sub rbtRoleBased_CheckedChanged(sender As Object, e As EventArgs) Handles rbtRoleBased.CheckedChanged
        If rbtRoleBased.Checked = True Then
            cmbItem.Enabled = True
            cmbSubItem.Enabled = True
            cmbRole.Enabled = True
            cmbDesigner.Enabled = False
            If GetAdmindbSoftValue("COSTCENTRE", "N") = "Y" Then
                lblUpdCostid.Visible = True
                cmbUpdateCostid_MAN.Visible = True
                btnUpdate.Visible = True
                StrSql = "SELECT COSTNAME FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME <>'" & cmbCostCentre_MAN.Text & "' ORDER BY COSTNAME"
                objGPack.FillCombo(StrSql, cmbUpdateCostid_MAN, , False)
                cmbUpdateCostid_MAN.Text = ""
            Else
                cmbUpdateCostid_MAN.Text = ""
                lblUpdCostid.Visible = False
                cmbUpdateCostid_MAN.Visible = False
                btnUpdate.Visible = False
            End If
        End If
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If rbtRoleBased.Checked = False Then Exit Sub
        If gridView.DataSource Is Nothing Then
            btnSearch_Click(Me, e)
        End If
        If cmbCostCentre_MAN.Text = "" Then MsgBox("Costcentre Should not Empty", MsgBoxStyle.Information) : cmbCostCentre_MAN.Focus() : Exit Sub
        If cmbUpdateCostid_MAN.Text = "" Then MsgBox("Update Costcentre Should not Empty", MsgBoxStyle.Information) : cmbUpdateCostid_MAN.Focus() : Exit Sub
        If cmbRole.Text = "" Then MsgBox("ROLE Should not Empty", MsgBoxStyle.Information) : cmbRole.Focus() : Exit Sub
        If cmbCostCentre_MAN.Text = cmbUpdateCostid_MAN.Text Then MsgBox("Costcentre Should not be Same", MsgBoxStyle.Information) : cmbUpdateCostid_MAN.Focus() : Exit Sub
        Dim fromCostId As String = "" : Dim ToCostId As String = ""
        fromCostId = GetSqlValue(cn, "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME ='" & cmbCostCentre_MAN.Text & "'").ToString
        ToCostId = GetSqlValue(cn, "SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME ='" & cmbUpdateCostid_MAN.Text & "'").ToString
        StrSql = "SELECT ROLEID FROM " & cnAdminDb & "..ROLEMASTER WHERE ROLENAME = '" & cmbRole.Text & "'"
        Dim CRoleid As Integer = objGPack.GetSqlValue(StrSql)


        StrSql = "DELETE FROM " & cnAdminDb & "..VACONTROL "
        StrSql += " WHERE ROLEID=" & CRoleid
        If ToCostId <> "" Then
            StrSql += " AND COSTID='" & ToCostId & "'"
        End If

        Dim sql As String = ""
        sql = "SELECT * FROM " & cnAdminDb & "..VACONTROL "
        sql += " WHERE ROLEID=" & CRoleid
        If fromCostId <> "" Then
            sql += " AND COSTID='" & fromCostId & "'"
        End If
        Dim dtvacontrol As New DataTable
        DA = New OleDbDataAdapter(sql, cn)
        DA.Fill(dtvacontrol)
        If dtvacontrol.Rows.Count > 0 Then
            Try
                tran = Nothing
                If ToCostId = "" Then
                    ExecQuery(SyncMode.Master, StrSql, cn, tran)
                Else
                    ExecQuery(SyncMode.Stock, StrSql, cn, tran, ToCostId)
                End If
                Dim sno As String
                For Each drr As DataRow In dtvacontrol.Rows
                    sno = GetNewSno(TranSnoType.VACONTROLCODE, tran, "GET_SNO_ADMIN")
                    sql = " INSERT INTO " & cnAdminDb & "..VACONTROL("
                    sql += vbCrLf + " SNO,COSTID,ITEMID,SUBITEMID,TABLECODE,DESIGNERID,VATYPE,VAPER,VAPERGM,VAFIXPER"
                    sql += vbCrLf + " ,WASTPER,MCPER,ADDVAPER,GRSAMTPER,DIAAMTPER,ACTIVE,USERID,ROLEID,MC,WASTAGE,STNAMTPER )"
                    sql += vbCrLf + " VALUES ("
                    sql += vbCrLf + " '" & sno & "','" & ToCostId & "','" & drr.Item("ITEMID").ToString & "','" & drr.Item("SUBITEMID").ToString & "','" & drr.Item("TABLECODE").ToString & "'"
                    sql += vbCrLf + " ,'" & drr.Item("DESIGNERID").ToString & "','" & drr.Item("VATYPE").ToString & "','" & Val(drr.Item("VAPER").ToString) & "','" & Val(drr.Item("VAPERGM").ToString) & "'"
                    sql += vbCrLf + " ,'" & Val(drr.Item("VAFIXPER").ToString) & "','" & Val(drr.Item("WASTPER").ToString) & "','" & Val(drr.Item("MCPER").ToString) & "','" & Val(drr.Item("ADDVAPER").ToString) & "'"
                    sql += vbCrLf + " ,'" & Val(drr.Item("GRSAMTPER").ToString) & "','" & Val(drr.Item("DIAAMTPER").ToString) & "','" & drr.Item("ACTIVE").ToString & "','" & Val(drr.Item("USERID").ToString) & "'"
                    sql += vbCrLf + " ,'" & Val(drr.Item("ROLEID").ToString) & "','" & Val(drr.Item("MC").ToString) & "','" & Val(drr.Item("WASTAGE").ToString) & "','" & Val(drr.Item("STNAMTPER").ToString) & "'"
                    sql += vbCrLf + " )"
                    ''Cmd = New OleDbCommand(sql, cn, tran)
                    ''Cmd.ExecuteNonQuery()
                    If ToCostId = "" Then
                        ExecQuery(SyncMode.Master, sql, cn, tran)
                    Else
                        ExecQuery(SyncMode.Stock, sql, cn, tran, ToCostId)
                    End If
                Next
                MsgBox("Transfered Sucessfully..", MsgBoxStyle.Information)
            Catch ex As Exception
                MsgBox(ex.Message + vbCrLf + ex.StackTrace)
            End Try
        End If

    End Sub
    Private Sub cmbCostCentre_MAN_TextChanged(sender As Object, e As EventArgs) Handles cmbCostCentre_MAN.TextChanged
        gridView.DataSource = Nothing
    End Sub
End Class