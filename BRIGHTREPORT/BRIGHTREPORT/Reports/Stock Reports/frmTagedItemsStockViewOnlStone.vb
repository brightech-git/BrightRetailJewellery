Imports SYSTEM.DATA.OLEDB
Imports SYSTEM.IO
Public Class FRMTAGEDITEMSSTOCKVIEWONLSTONE
    'LAST MODIFIED ON 20/08/2015-REMOVED GROUPER TOTAL AND ADD IT IN QUERY ITSELF COZ SLOW IN FOR LOOP DONE BY-JEGAN
    Dim CMD As OLEDBCOMMAND
    Dim DA As OLEDBDATAADAPTER
    'Dim OBJMOREOPTION As New FRMCHECKOTHERMASTER
    Dim STRSQL As String
    Dim TEMPDT As New DATATABLE("OTHERDETAILS")
    Dim DTSTONEDETAILS As New DATATABLE("STONEDETAILS")
    Dim DTGRANDTOTALDETAILS As New DATATABLE("GRANDTOTAL")
    Dim DEFAULTPIC As String = OBJGPACK.GETSQLVALUE("SELECT CTLTEXT FROM " & CNADMINDB & "..SOFTCONTROL WHERE CTLID = 'PICPATH'")
    Dim HEADERBGCOLOR As New SYSTEM.DRAWING.COLOR
    Dim DIARND As Integer = VAL(GETADMINDBSOFTVALUE("ROUNDOFF-DIA", 4))
    Dim DTCOMPANY As New DataTable
    Dim DTITEMNAME As New DataTable
    Dim DTSUBITEMNAME As New DataTable
    Dim DTMETAL As New DataTable
    Dim DT As New DATATABLE
    Dim DTCATEGORY As New DATATABLE
    Dim ROWFILLSTATE As Boolean = False
    Dim OBJSEARCH As Object = Nothing
    Dim STUDDED As Boolean = False
    Dim OTHMASTER As Boolean = False
    Dim _TAGDUPPRINT As Boolean = IIF(GETADMINDBSOFTVALUE("TAGCHKDUPPRINT", "N") = "Y", True, False)
    Dim INCLUDE As String = ""
    Dim DTCOSTCENTRE As New DATATABLE



    Function FUNCEXIT() As Integer
        Me.CLOSE()
    End Function


    Private Sub FRMTAGEDITEMSSTOCKVIEWONLSTONE_KEYPRESS(ByVal SENDER As Object, ByVal E As SYSTEM.WINDOWS.FORMS.KEYPRESSEVENTARGS) Handles ME.KEYPRESS
        If E.KEYCHAR = CHR(KEYS.ESCAPE) Then
            If TABMAIN.SELECTEDTAB.NAME = TABVIEW.NAME Then
                BTNBACK_CLICK(Me, New EVENTARGS)
            End If
        ElseIf E.KEYCHAR = CHR(KEYS.ENTER) Then
            SENDKEYS.SEND("{TAB}")
        End If
    End Sub

    Private Sub FRMTAGEDITEMSSTOCKVIEWONLSTONE_LOAD(ByVal SENDER As SYSTEM.OBJECT, ByVal E As SYSTEM.EVENTARGS) Handles MYBASE.LOAD
        If Not DEFAULTPIC.ENDSWITH("\") Then DEFAULTPIC += "\"

        'ME.WINDOWSTATE = FORMWINDOWSTATE.MAXIMIZED
        'TABMAIN.ITEMSIZE = NEW SYSTEM.DRAWING.SIZE(1, 1)
        Me.TABMAIN.REGION = New REGION(New RECTANGLEF(Me.TABGENERAL.LEFT, Me.TABGENERAL.TOP, Me.TABGENERAL.WIDTH, Me.TABGENERAL.HEIGHT))
        GRPFILTRATION.LOCATION = New POINT((SCREENWID - GRPFILTRATION.WIDTH) / 2, ((SCREENHIT - 128) - GRPFILTRATION.HEIGHT) / 2)

        PNLTOTALGRIDVIEW.DOCK = DOCKSTYLE.FILL
        HEADERBGCOLOR = SYSTEM.DRAWING.SYSTEMCOLORS.CONTROLLIGHT

        TEMPDT.COLUMNS.ADD("MAXDESCRIPTION", GetType(String))
        TEMPDT.COLUMNS.ADD("MAXVALUES", GetType(String))
        TEMPDT.COLUMNS.ADD("MINDESCRIPTION", GetType(String))
        TEMPDT.COLUMNS.ADD("MINVALUES", GetType(String))
        TEMPDT.COLUMNS.ADD("OTHERDESC1", GetType(String))
        TEMPDT.COLUMNS.ADD("OTHERVAL1", GetType(String))
        TEMPDT.COLUMNS.ADD("OTHERDESC2", GetType(String))
        TEMPDT.COLUMNS.ADD("OTHERVAL2", GetType(String))

        DTSTONEDETAILS.COLUMNS.ADD("DESCRIPTION", GetType(String))
        DTSTONEDETAILS.COLUMNS.ADD("DIAMOND", GetType(String))
        DTSTONEDETAILS.COLUMNS.ADD("STONE", GetType(String))
        DTSTONEDETAILS.COLUMNS.ADD("PRECIOUS", GetType(String))

        DTGRANDTOTALDETAILS.COLUMNS.ADD("DESCRIPTION", GetType(String))
        DTGRANDTOTALDETAILS.COLUMNS.ADD("PCS", GetType(String))
        DTGRANDTOTALDETAILS.COLUMNS.ADD("GRSWT", GetType(String))
        DTGRANDTOTALDETAILS.COLUMNS.ADD("LESSWT", GetType(String))
        DTGRANDTOTALDETAILS.COLUMNS.ADD("NETWT", GetType(String))
        DTGRANDTOTALDETAILS.COLUMNS.ADD("EXTRAWT", GetType(String))
        DTGRANDTOTALDETAILS.COLUMNS.ADD("SALVALUE", GetType(String))

        ''CHECKING COSTCENTRE STATUS
        STRSQL = " SELECT 1 FROM " & CNADMINDB & "..SOFTCONTROL WHERE CTLTEXT = 'Y' AND CTLID = 'COSTCENTRE'"
        Dim DT As New DATATABLE
        DT.CLEAR()
        DA = New OLEDBDATAADAPTER(STRSQL, CN)
        DA.FILL(DT)
        'IF DT.ROWS.COUNT > 0 THEN
        '    CMBCOSTCENTER.ENABLED = TRUE
        'ELSE
        '    CMBCOSTCENTER.ENABLED = FALSE
        'END IF
        If DT.ROWS.COUNT > 0 Then
            CHKCMBCOSTCENTRE.ENABLED = True
        Else
            CHKCMBCOSTCENTRE.ENABLED = False
        End If

        STRSQL = " SELECT 'ALL' METALNAME,'ALL' METALID,1 RESULT"
        STRSQL += " UNION ALL"
        STRSQL += " SELECT METALNAME,METALID,2 RESULT FROM " & CNADMINDB & "..METALMAST "
        STRSQL += " ORDER BY RESULT,METALNAME"
        DTMETAL = New DATATABLE
        DA = New OLEDBDATAADAPTER(STRSQL, CN)
        DA.FILL(DTMETAL)
        BrighttechPack.GLOBALMETHODS.FILLCOMBO(CHKCMBMETAL, DTMETAL, "METALNAME", , "ALL")


        STRSQL = " SELECT 'ALL' COMPANYNAME,'ALL' COMPANYID,1 RESULT"
        STRSQL += " UNION ALL"
        STRSQL += " SELECT COMPANYNAME,COMPANYID,2 RESULT FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N'"
        STRSQL += " ORDER BY RESULT,COMPANYNAME"
        DTCOMPANY = New DataTable
        DA = New OleDbDataAdapter(STRSQL, cn)
        DA.Fill(DTCOMPANY)

        BrighttechPack.GlobalMethods.FillCombo(chkCmbCompany, DTCOMPANY, "COMPANYNAME", , strCompanyName)

        STRSQL = " SELECT 'ALL' ITEMNAME,'0' ITEMID,1 RESULT"
        STRSQL += " UNION ALL"
        STRSQL += " SELECT ITEMNAME,ITEMID,2 RESULT FROM " & cnAdminDb & "..ITEMMAST WHERE ISNULL(ACTIVE,'')<>'N'"
        STRSQL += " ORDER BY RESULT,ITEMNAME"
        DTITEMNAME = New DataTable
        DA = New OleDbDataAdapter(STRSQL, cn)
        DA.Fill(DTITEMNAME)

        BrighttechPack.GlobalMethods.FillCombo(chkcmbItemName, DTITEMNAME, "ITEMNAME", True, "ALL")

        STRSQL = " SELECT 'ALL' SUBITEMNAME,'0' SUBITEMID,1 RESULT"
        STRSQL += " UNION ALL"
        STRSQL += " SELECT SUBITEMNAME,SUBITEMID,2 RESULT FROM " & cnAdminDb & "..SUBITEMMAST WHERE ISNULL(ACTIVE,'')<>'N'"
        STRSQL += " ORDER BY RESULT,SUBITEMNAME"
        DTSUBITEMNAME = New DataTable
        DA = New OleDbDataAdapter(STRSQL, cn)
        DA.Fill(DTSUBITEMNAME)

        BrighttechPack.GlobalMethods.FillCombo(chkcmbSubItemName, DTSUBITEMNAME, "SUBITEMNAME", True, "ALL")
        BTNNEW_CLICK(Me, New EventArgs)
    End Sub

    Private Sub BTNVIEW_SEARCH_CLICK(ByVal SENDER As System.Object, ByVal E As System.EventArgs) Handles btnView_Search.Click
        gridTotalView.DataSource = Nothing
        gridTotalView.Refresh()
        If Not CheckBckDays(userId, Me.Name, dtpAsOnDate_OWN.Value) Then dtpAsOnDate_OWN.Focus() : Exit Sub
        Try

            STRSQL = " IF (SELECT COUNT(*) FROM SYSOBJECTS WHERE NAME = 'TEMP" & systemId & "ONLSTONE')>0"
            STRSQL += vbCrLf + " DROP TABLE TEMP" & systemId & "ONLSTONE"
            STRSQL += vbCrLf + " SELECT CONVERT(VARCHAR (50),SNO)SNO,TAGNO"

            If ChkWithSubItem.Checked Then
                STRSQL += vbCrLf + " ,STNITEMID STNITEMID"
                STRSQL += vbCrLf + " ,(SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.STNITEMID) STNITEM"
                STRSQL += vbCrLf + " ,CASE WHEN ISNULL(STNSUBITEMID,0) = 0 THEN NULL ELSE STNSUBITEMID END STNSUBITEMID"
                STRSQL += vbCrLf + " ,(SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID=T.STNSUBITEMID) SUBITEM"
                STRSQL += vbCrLf + " ,(SELECT  ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID IN (SELECT ITEMID  FROM " & cnAdminDb & "..ITEMTAG WHERE SNO=T.TAGSNO))  ITEM_NAME"
                STRSQL += vbCrLf + " ,(SELECT  SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID IN (SELECT SUBITEMID  FROM " & cnAdminDb & " ..ITEMTAG WHERE SNO=T.TAGSNO))  SUBITEM_NAME"
            Else
                STRSQL += vbCrLf + " ,CASE WHEN ISNULL(STNSUBITEMID,0) = 0 THEN STNITEMID ELSE STNSUBITEMID END STNITEMID"
                STRSQL += vbCrLf + " ,CASE WHEN ISNULL(STNSUBITEMID,0) = 0 THEN "
                STRSQL += vbCrLf + " (SELECT ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID=T.STNITEMID) "
                STRSQL += vbCrLf + " ELSE "
                STRSQL += vbCrLf + " (SELECT SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID=T.STNSUBITEMID) END STNITEM"
                STRSQL += vbCrLf + " ,(SELECT  ITEMNAME FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMID IN (SELECT ITEMID  FROM " & cnAdminDb & "..ITEMTAG WHERE SNO=T.TAGSNO))  ITEM_NAME"
                STRSQL += vbCrLf + " ,(SELECT  SUBITEMNAME FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMID IN (SELECT SUBITEMID  FROM " & cnAdminDb & " ..ITEMTAG WHERE SNO=T.TAGSNO))  SUBITEM_NAME"
            End If

            STRSQL += vbCrLf + " ,CASE WHEN T.STONEUNIT='C' AND METALID <> 'D' THEN STNWT ELSE NULL  END STNCRTWT"
            STRSQL += vbCrLf + " ,CASE WHEN T.STONEUNIT='C' AND METALID <> 'D' THEN STNRATE ELSE NULL  END STNCRTRATE"
            STRSQL += vbCrLf + " ,CASE WHEN T.STONEUNIT='G' AND METALID <> 'D' THEN STNWT ELSE NULL  END STNGRMWT"
            STRSQL += vbCrLf + " ,CASE WHEN T.STONEUNIT='G' AND METALID <> 'D' THEN STNRATE ELSE NULL END STNGRMRATE"
            STRSQL += vbCrLf + " ,CASE WHEN METALID  = 'D' THEN STNPCS ELSE NULL  END DIAPCS"
            STRSQL += vbCrLf + " ,CASE WHEN METALID  = 'D' THEN STNWT ELSE NULL  END DIAWT"
            STRSQL += vbCrLf + " ,CASE WHEN METALID  = 'D' THEN STNRATE ELSE NULL END DIARATE"
            STRSQL += vbCrLf + " ,STNAMT,ISSDATE,DESCRIP,'' COLHEAD "
            STRSQL += vbCrLf + " INTO TEMP" & systemId & "ONLSTONE FROM " & cnAdminDb & "..ITEMTAGSTONE T INNER JOIN " & cnAdminDb & "..ITEMMAST I"
            STRSQL += vbCrLf + " ON I.ITEMID = T.STNITEMID "
            STRSQL += vbCrLf + " WHERE TAGSNO IN (SELECT SNO  FROM " & cnAdminDb & "..ITEMTAG WHERE ISSDATE IS NULL) "

            If chkdate.Checked = True Then
                STRSQL += vbCrLf + " AND T.RECDATE BETWEEN '" & dtpAsOnDate_OWN.Value.ToString("yyyy-MM-dd") & "' AND '" & dtpTo_OWN.Value.ToString("yyyy-MM-dd") & "' "
            Else
                STRSQL += vbCrLf + " AND T.RECDATE <= '" & dtpAsOnDate_OWN.Value.ToString("yyyy-MM-dd") & "' "
            End If

            If chkCmbCostCentre.Text = "ALL" Or chkCmbCostCentre.Text = "" Then
            Else
                STRSQL += vbCrLf + "AND T.COSTID IN (SELECT COSTID FROM " & cnAdminDb & "..COSTCENTRE WHERE COSTNAME IN (" & GetQryString(chkCmbCostCentre.Text) & "))"
            End If

            If chkCmbCompany.Text <> "ALL" And chkCmbCompany.Text <> "" Then
            Else
                STRSQL += vbCrLf + " AND T.COMPANYID IN (SELECT COMPANYID FROM " & cnAdminDb & "..COMPANY WHERE ISNULL(ACTIVE,'')<>'N')"
            End If


            If chkcmbItemName.Text.ToUpper <> "ALL" And chkcmbItemName.Text.ToUpper <> "" Then
                STRSQL += vbCrLf + " AND T.ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN (" & GetQryString(chkcmbItemName.Text) & "))"
            End If
            If chkcmbSubItemName.Text.ToUpper <> "ALL" And chkcmbSubItemName.Text.ToUpper <> "" Then
                STRSQL += vbCrLf + " AND T.TAGSNO IN (SELECT SNO FROM " & cnAdminDb & "..ITEMTAG "
                STRSQL += vbCrLf + " WHERE SUBITEMID IN (SELECT SUBITEMID FROM " & cnAdminDb & "..SUBITEMMAST WHERE SUBITEMNAME IN (" & GetQryString(chkcmbSubItemName.Text) & ")))"
            End If
            STRSQL += vbCrLf + " ORDER BY TAGNO"
            CMD = New OleDbCommand(STRSQL, cn) : CMD.CommandTimeout = 1000
            CMD.CommandTimeout = 1000
            CMD.ExecuteNonQuery()




            STRSQL = "INSERT INTO TEMP" & systemId & "ONLSTONE (TAGNO,STNITEMID,STNCRTWT,STNCRTRATE,STNGRMWT,STNGRMRATE,DIAPCS,DIAWT,DIARATE,STNAMT,COLHEAD,ISSDATE,DESCRIP) "
            STRSQL += vbCrLf + "SELECT 'TOTAL','',SUM(STNCRTWT),SUM(STNCRTRATE),SUM(STNGRMWT),SUM(STNGRMRATE),SUM(DIAPCS),SUM(DIAWT),SUM(DIARATE),SUM(STNAMT),'G',NULL,'' FROM TEMP" & systemId & "ONLSTONE "
            CMD = New OleDbCommand(STRSQL, cn)
            CMD.ExecuteNonQuery()

            STRSQL = "alter table TEMP" & systemId & "ONLSTONE alter column STNITEMID varchar(20)"
            CMD = New OleDbCommand(STRSQL, cn)
            CMD.ExecuteNonQuery()

            STRSQL = "UPDATE  TEMP" & systemId & "ONLSTONE SET STNITEMID='' WHERE STNITEMID=0 "
            CMD = New OleDbCommand(STRSQL, cn)
            CMD.ExecuteNonQuery()

            STRSQL = "SELECT * FROM TEMP" & systemId & "ONLSTONE ORDER BY TAGNO,COLHEAD "
            CMD = New OleDbCommand(STRSQL, cn)
            CMD.ExecuteNonQuery()
            DT = New DataTable
            DA = New OleDbDataAdapter(STRSQL, cn)
            DA.Fill(DT)
            If Not DT.Rows.Count > 1 Then
                btnView_Search.Enabled = True
                MsgBox("STOCK NOT AVAILABLE", MsgBoxStyle.Information)
                Exit Sub
            End If
            Dim TITLE As String
            If chkdate.Checked = True Then
                TITLE = "ONL STONE DETAILS FROM " & dtpAsOnDate_OWN.Text & " TO " & dtpTo_OWN.Text & ""
            Else
                TITLE = "ONL STONE DETAILS AS ON DATE " & dtpAsOnDate_OWN.Text & " "
            End If
            TITLE = TITLE + " ROWS : " + DT.Rows.Count.ToString
            'IF SELECTEDCOSTCENTRE <> "" THEN TITLE += " COSTCENTRE [" + SELECTEDCOSTCENTRE + "]"
            'IF SELECTEDCASHCOUNTER <> "" THEN TITLE += " CASHCOUNTER [" + SELECTEDCASHCOUNTER + "]"
            lblReportTitle.Text = TITLE
            lblReportTitle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            lblReportTitle.Visible = True
            gridTotalView.DataSource = DT
            FormatGridColumns(gridTotalView, False, False, True, False)
            tabMain.SelectedTab = tabView
            'PROP_SETS()
            With gridTotalView
                .Columns("COLHEAD").Visible = False
                .Columns("issdate").Visible = False
                .Columns("DESCRIP").Visible = False
                .Columns("STNITEMID").DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight
            End With
            RESIZETOOLSTRIPMENUITEM_CLICK(SENDER, E)
        Catch EX As Exception
            MsgBox(EX.Message + vbCrLf + EX.StackTrace)
        Finally
            btnView_Search.Enabled = True
        End Try
    End Sub

    Private Sub GRIDTOTALVIEW_CELLFORMATTING(ByVal SENDER As Object, ByVal E As SYSTEM.WINDOWS.FORMS.DATAGRIDVIEWCELLFORMATTINGEVENTARGS) Handles GRIDTOTALVIEW.CELLFORMATTING
        If gridTotalView.Rows(E.RowIndex).Cells("COLHEAD").Value.ToString = "G" Then
            gridTotalView.Rows(E.RowIndex).DefaultCellStyle = reportTotalStyle
            'ElseIf gridTotalView.Rows(E.RowIndex).Cells("APPROVAL").Value.ToString = "A" Then
            '    gridTotalView.Rows(E.RowIndex).DefaultCellStyle.BackColor = Color.Orchid
            'ElseIf gridTotalView.Rows(E.RowIndex).Cells("APPROVAL").Value.ToString = "R" Then
            '    gridTotalView.Rows(E.RowIndex).DefaultCellStyle.BackColor = Color.Orchid
        End If
    End Sub

    Private Sub GRIDFULLDETAILS_CELLFORMATTING(ByVal SENDER As Object, ByVal E As SYSTEM.WINDOWS.FORMS.DATAGRIDVIEWCELLFORMATTINGEVENTARGS)
        If E.COLUMNINDEX = 0 Or E.COLUMNINDEX = 2 Or E.COLUMNINDEX = 4 Or E.COLUMNINDEX = 6 Then
            E.CELLSTYLE.BACKCOLOR = HEADERBGCOLOR
            E.CELLSTYLE.SELECTIONBACKCOLOR = HEADERBGCOLOR
        End If
        If E.COLUMNINDEX = 1 Or E.COLUMNINDEX = 3 Or E.COLUMNINDEX = 5 Or E.COLUMNINDEX = 7 Then
            E.CELLSTYLE.FONT = New SYSTEM.DRAWING.FONT("VERDANA", 8.25!, SYSTEM.DRAWING.FONTSTYLE.BOLD)
        End If
    End Sub

    Private Sub GRIDGRANDTOTAL_CELLFORMATTING(ByVal SENDER As Object, ByVal E As SYSTEM.WINDOWS.FORMS.DATAGRIDVIEWCELLFORMATTINGEVENTARGS)
        If E.COLUMNINDEX = 0 Or E.ROWINDEX = 0 Then
            E.CELLSTYLE.BACKCOLOR = HEADERBGCOLOR
            E.CELLSTYLE.SELECTIONBACKCOLOR = HEADERBGCOLOR
        End If
        If E.ROWINDEX <> 0 And E.COLUMNINDEX <> 0 Then
            E.CELLSTYLE.FONT = New SYSTEM.DRAWING.FONT("VERDANA", 8.25!, SYSTEM.DRAWING.FONTSTYLE.BOLD)
        End If
    End Sub

    Private Sub GRIDSTONEDETAILS_CELLFORMATTING(ByVal SENDER As Object, ByVal E As SYSTEM.WINDOWS.FORMS.DATAGRIDVIEWCELLFORMATTINGEVENTARGS)
        If E.COLUMNINDEX = 0 Or E.ROWINDEX = 0 Then
            E.CELLSTYLE.BACKCOLOR = HEADERBGCOLOR
            E.CELLSTYLE.SELECTIONBACKCOLOR = HEADERBGCOLOR
        End If
        If E.ROWINDEX <> 0 And E.COLUMNINDEX <> 0 Then
            E.CELLSTYLE.FONT = New SYSTEM.DRAWING.FONT("VERDANA", 8.25!, SYSTEM.DRAWING.FONTSTYLE.BOLD)
        End If
    End Sub

    Private Sub TXTITEMCODE_KEYDOWN(ByVal SENDER As Object, ByVal E As SYSTEM.WINDOWS.FORMS.KEYEVENTARGS) Handles TXTITEMCODE_NUM.KEYDOWN
        Dim ITEMID As String
        If E.KEYCODE = KEYS.INSERT Then
            STRSQL = " SELECT DISTINCT"
            STRSQL += VBCRLF + " ITEMID, "
            STRSQL += VBCRLF + " (SELECT ITEMNAME FROM " & CNADMINDB & "..ITEMMAST WHERE ITEMID = T.ITEMID)AS ITEMNAME"
            STRSQL += VBCRLF + " FROM " & CNADMINDB & "..ITEMMAST AS T"
            If CHKCMBMETAL.TEXT <> "ALL" And CHKCMBMETAL.TEXT <> "" Then
                STRSQL += VBCRLF + " WHERE METALID IN (SELECT METALID FROM " & CNADMINDB & "..METALMAST WHERE METALNAME IN (" & GETQRYSTRING(CHKCMBMETAL.TEXT) & "))"
            End If
            If CHKCMBCATEGORY.TEXT <> "ALL" And CHKCMBCATEGORY.TEXT <> "" Then
                STRSQL += VBCRLF + " AND CATCODE IN (SELECT CATCODE FROM " & CNADMINDB & "..CATEGORY WHERE CATNAME IN(" & GETQRYSTRING(CHKCMBCATEGORY.TEXT) & "))"
            End If
            ITEMID = BrighttechPack.SEARCHDIALOG.SHOW("FIND ITEMID", STRSQL, CN, 1)
            STRSQL = " SELECT ITEMNAME FROM " & CNADMINDB & "..ITEMMAST WHERE ITEMID = '" & ITEMID & "'"

            Dim DT As New DATATABLE
            DT.CLEAR()
            DA = New OLEDBDATAADAPTER(STRSQL, CN)
            DA.FILL(DT)
            If DT.ROWS.COUNT > 0 Then
                TXTITEMCODE_NUM.TEXT = ITEMID
                TXTITEMNAME.TEXT = DT.ROWS(0).ITEM("ITEMNAME")
            End If
        End If
    End Sub

    Private Sub TXTITEMCODE_KEYPRESS(ByVal SENDER As Object, ByVal E As SYSTEM.WINDOWS.FORMS.KEYPRESSEVENTARGS) Handles TXTITEMCODE_NUM.KEYPRESS
        If E.KEYCHAR = CHR(KEYS.ENTER) Then
            STRSQL = " SELECT ITEMNAME,STUDDEDSTONE FROM " & CNADMINDB & "..ITEMMAST WHERE ITEMID = '" & TXTITEMCODE_NUM.TEXT & "'"
            If CHKCMBMETAL.TEXT <> "ALL" And CHKCMBMETAL.TEXT <> "" Then
                STRSQL += VBCRLF + " AND METALID IN (SELECT METALID FROM " & CNADMINDB & "..METALMAST WHERE METALNAME IN (" & GETQRYSTRING(CHKCMBMETAL.TEXT) & "))"
            End If
            Dim DT As New DATATABLE
            DT.CLEAR()
            DA = New OLEDBDATAADAPTER(STRSQL, CN)
            DA.FILL(DT)
            If DT.ROWS.COUNT > 0 Then
                TXTITEMNAME.TEXT = DT.ROWS(0).ITEM("ITEMNAME")
                If DT.ROWS(0).ITEM("STUDDEDSTONE").TOSTRING = "Y" Then
                    STUDDED = True
                Else
                    STUDDED = False
                End If
            Else
                TXTITEMNAME.CLEAR()
            End If
        End If
    End Sub

    Private Sub TXTITEMNAME_TEXTCHANGED(ByVal SENDER As Object, ByVal E As SYSTEM.EVENTARGS) Handles TXTITEMNAME.TEXTCHANGED
        If TXTITEMNAME.TEXT <> "" Then
            Dim DTSITEM As DATATABLE
            CMBSUBITEMNAME.ITEMS.CLEAR()
            CMBSUBITEMNAME.ITEMS.ADD("ALL")
            STRSQL = " SELECT SUBITEMNAME FROM " & CNADMINDB & "..SUBITEMMAST WHERE ITEMID = '" & TXTITEMCODE_NUM.TEXT & "' ORDER BY SUBITEMNAME"
            DTSITEM = New DATATABLE
            DA = New OLEDBDATAADAPTER(STRSQL, CN)
            DA.FILL(DTSITEM)
            BrighttechPack.GLOBALMETHODS.FILLCOMBO(CMBSUBITEMNAME, DTSITEM, "SUBITEMNAME", False)
            'OBJGPACK.FILLCOMBO(STRSQL, CMBSUBITEMNAME, FALSE)
            CMBSUBITEMNAME.TEXT = "ALL"
            If CMBSUBITEMNAME.ITEMS.COUNT > 0 Then
                CMBSUBITEMNAME.ENABLED = True
            Else
                CMBSUBITEMNAME.ENABLED = False
            End If

            CMBSIZE.ITEMS.CLEAR()
            CMBSIZE.ITEMS.ADD("ALL")
            STRSQL = " SELECT SIZENAME FROM " & CNADMINDB & "..ITEMSIZE WHERE ITEMID = " & VAL(TXTITEMCODE_NUM.TEXT) & " ORDER BY SIZENAME"
            OBJGPACK.FILLCOMBO(STRSQL, CMBSIZE, False)
            CMBSIZE.TEXT = "ALL"
            If CMBSIZE.ITEMS.COUNT > 0 Then
                CMBSIZE.ENABLED = True
            Else
                CMBSIZE.ENABLED = False
            End If
        Else
            CMBSUBITEMNAME.ITEMS.CLEAR()
            CMBSUBITEMNAME.ENABLED = False
        End If
    End Sub

    Private Sub EXITTOOLSTRIPMENUITEM_CLICK(ByVal SENDER As SYSTEM.OBJECT, ByVal E As SYSTEM.EVENTARGS) Handles EXITTOOLSTRIPMENUITEM.CLICK
        FUNCEXIT()
    End Sub

    Private Sub BTNEXIT_CLICK(ByVal SENDER As SYSTEM.OBJECT, ByVal E As SYSTEM.EVENTARGS) Handles BTNEXIT.CLICK
        FUNCEXIT()
    End Sub



    Private Sub NEWTOOLSTRIPMENUITEM_CLICK(ByVal SENDER As SYSTEM.OBJECT, ByVal E As SYSTEM.EVENTARGS) Handles NEWTOOLSTRIPMENUITEM.CLICK
        BTNNEW_CLICK(Me, New EVENTARGS)
    End Sub

    Private Sub BTNNEW_CLICK(ByVal SENDER As SYSTEM.OBJECT, ByVal E As SYSTEM.EVENTARGS) Handles BTNNEW.CLICK
        OBJGPACK.TEXTCLEAR(Me)
        CMBSUBITEMNAME.ITEMS.CLEAR()
        CMBSUBITEMNAME.ENABLED = False
        ' NEWLY ADD ON 18/08/2015

        CMBGROUPBY.ITEMS.CLEAR()


        CMBGROUPBY.ITEMS.ADD("SUBITEM")

        ''COSTCENTER

        If CHKCMBCOSTCENTRE.ENABLED = True Then
            STRSQL = " SELECT 'ALL' COSTNAME,'ALL' COSTID,1 RESULT"
            STRSQL += VBCRLF + " UNION ALL"
            STRSQL += VBCRLF + " SELECT COSTNAME,CONVERT(VARCHAR,COSTID),2 RESULT FROM " & CNADMINDB & "..COSTCENTRE"
            STRSQL += VBCRLF + " ORDER BY RESULT,COSTNAME"
            DTCOSTCENTRE = New DATATABLE
            DA = New OLEDBDATAADAPTER(STRSQL, CN)
            DA.FILL(DTCOSTCENTRE)
            BrighttechPack.GLOBALMETHODS.FILLCOMBO(CHKCMBCOSTCENTRE, DTCOSTCENTRE, "COSTNAME", , IIF(CNDEFAULTCOSTID, "ALL", CNCOSTNAME))
            If STRUSERCENTRAILSED <> "Y" And CNDEFAULTCOSTID = False Then CHKCMBCOSTCENTRE.ENABLED = False
        End If

        BrighttechPack.GLOBALMETHODS.FILLCOMBO(CHKCMBCOMPANY, DTCOMPANY, "COMPANYNAME", , STRCOMPANYNAME)
        dtpAsOnDate_OWN.VALUE = GETENTRYDATE(GETSERVERDATE)
        BrighttechPack.GLOBALMETHODS.FILLCOMBO(CHKCMBMETAL, DTMETAL, "METALNAME", , "ALL")
        STRSQL = VBCRLF + " SELECT DISTINCT ITEMNAME FROM " & CNADMINDB & "..ITEMMAST WHERE ISNULL(METALID,'') IN ('T','D')"
        STRSQL += VBCRLF + " ORDER BY ITEMNAME"
        Dim DTITEM As New DATATABLE
        DA = New OLEDBDATAADAPTER(STRSQL, CN)
        DA.FILL(DTITEM)
        'IF DTITEM.ROWS.COUNT > 0 THEN
        '    CHKCMBSTONENAME.ITEMS.ADD("ALL", TRUE)
        '    BrighttechPack.GLOBALMETHODS.FILLCOMBO(CHKCMBSTONENAME, DTITEM, "ITEMNAME", FALSE, "ALL")
        'END IF
        'STUDDED = FALSE
        'CHKCMBSTONENAME.ENABLED = STUDDED
        'CHKCMBSTSUBITEMNAME.ENABLED = STUDDED
        CHKCMBMETAL.SELECT()
    End Sub

    Private Sub BTNBACK_CLICK(ByVal SENDER As SYSTEM.OBJECT, ByVal E As SYSTEM.EVENTARGS) Handles BTNBACK.CLICK
        TABMAIN.SELECTEDTAB = TABGENERAL
        TXTITEMCODE_NUM.FOCUS()
    End Sub

    Private Sub BTNEXCEL_CLICK(ByVal SENDER As SYSTEM.OBJECT, ByVal E As SYSTEM.EVENTARGS) Handles BTNEXPORT.CLICK
        If Not BrighttechPack.METHODS.GETRIGHTS(_DTUSERRIGHTS, Me.NAME, BrighttechPack.METHODS.RIGHTMODE.EXCEL) Then Exit Sub
        If GRIDTOTALVIEW.ROWS.COUNT > 0 Then
            BrightPosting.GEXPORT.POST(Me.NAME, STRCOMPANYNAME, LBLREPORTTITLE.TEXT, GRIDTOTALVIEW, BrightPosting.GEXPORT.GEXPORTTYPE.EXPORT)
        End If
    End Sub

    Private Sub BTNPRINT_CLICK(ByVal SENDER As SYSTEM.OBJECT, ByVal E As SYSTEM.EVENTARGS) Handles BTNPRINT.CLICK
        If Not BrighttechPack.METHODS.GETRIGHTS(_DTUSERRIGHTS, Me.NAME, BrighttechPack.METHODS.RIGHTMODE.PRINT) Then Exit Sub
        If GRIDTOTALVIEW.ROWS.COUNT > 0 Then
            BrightPosting.GEXPORT.POST(Me.NAME, STRCOMPANYNAME, LBLREPORTTITLE.TEXT, GRIDTOTALVIEW, BrightPosting.GEXPORT.GEXPORTTYPE.PRINT)
        End If
    End Sub

    Public Sub New()

        ' THIS CALL IS REQUIRED BY THE WINDOWS FORM DESIGNER.
        INITIALIZECOMPONENT()
        ' ADD ANY INITIALIZATION AFTER THE INITIALIZECOMPONENT() CALL.

    End Sub
    Private Sub CHKDATE_CHECKEDCHANGED(ByVal SENDER As SYSTEM.OBJECT, ByVal E As SYSTEM.EVENTARGS) Handles CHKDATE.CHECKEDCHANGED
        If CHKDATE.CHECKED Then
            DATETO.VISIBLE = True
            dtpTo_OWN.VISIBLE = True
            CHKDATE.TEXT = "FROM"
        Else
            DATETO.VISIBLE = False
            dtpTo_OWN.VISIBLE = False
            CHKDATE.TEXT = "ASONDATE"
        End If
    End Sub


    Private Sub LABEL4_CLICK(ByVal SENDER As SYSTEM.OBJECT, ByVal E As SYSTEM.EVENTARGS)

    End Sub

    Private Sub TXTLOTNO_NUM_TEXTCHANGED(ByVal SENDER As SYSTEM.OBJECT, ByVal E As SYSTEM.EVENTARGS)

    End Sub

    Private Sub LABEL10_CLICK(ByVal SENDER As SYSTEM.OBJECT, ByVal E As SYSTEM.EVENTARGS)

    End Sub

    Private Sub CMBSTOCKTYPE_SELECTEDINDEXCHANGED(ByVal SENDER As SYSTEM.OBJECT, ByVal E As SYSTEM.EVENTARGS) Handles CMBSTOCKTYPE.SELECTEDINDEXCHANGED

    End Sub

    Private Sub LABEL6_CLICK(ByVal SENDER As SYSTEM.OBJECT, ByVal E As SYSTEM.EVENTARGS) Handles LABEL6.CLICK

    End Sub

    Private Sub TXTTORATE_WET_TEXTCHANGED(ByVal SENDER As SYSTEM.OBJECT, ByVal E As SYSTEM.EVENTARGS)

    End Sub

    Private Sub TXTTODIAWT_WET_TEXTCHANGED(ByVal SENDER As SYSTEM.OBJECT, ByVal E As SYSTEM.EVENTARGS)

    End Sub

    Private Sub TXTFROMDIAWT_WET_TEXTCHANGED(ByVal SENDER As SYSTEM.OBJECT, ByVal E As SYSTEM.EVENTARGS)

    End Sub

    Private Sub TXTFROMRATE_WET_TEXTCHANGED(ByVal SENDER As SYSTEM.OBJECT, ByVal E As SYSTEM.EVENTARGS)

    End Sub

    Private Sub TXTAGEFRM_TEXTCHANGED(ByVal SENDER As SYSTEM.OBJECT, ByVal E As SYSTEM.EVENTARGS)

    End Sub

    Private Sub CHKCMBGROUPBY_SELECTEDINDEXCHANGED(ByVal SENDER As SYSTEM.OBJECT, ByVal E As SYSTEM.EVENTARGS)

    End Sub

    Private Sub CHKLSTBOXORDERBY_SELECTEDINDEXCHANGED(ByVal SENDER As SYSTEM.OBJECT, ByVal E As SYSTEM.EVENTARGS)

    End Sub

    Private Sub LABEL25_CLICK(ByVal SENDER As SYSTEM.OBJECT, ByVal E As SYSTEM.EVENTARGS)

    End Sub

    Private Sub LABEL18_CLICK(ByVal SENDER As SYSTEM.OBJECT, ByVal E As SYSTEM.EVENTARGS)

    End Sub

    Private Sub LABEL31_CLICK(ByVal SENDER As SYSTEM.OBJECT, ByVal E As SYSTEM.EVENTARGS)

    End Sub

    Private Sub LABEL15_CLICK(ByVal SENDER As SYSTEM.OBJECT, ByVal E As SYSTEM.EVENTARGS)

    End Sub

    Private Sub LABEL13_CLICK(ByVal SENDER As SYSTEM.OBJECT, ByVal E As SYSTEM.EVENTARGS)

    End Sub

    Private Sub LABEL11_CLICK(ByVal SENDER As SYSTEM.OBJECT, ByVal E As SYSTEM.EVENTARGS)

    End Sub

    Private Sub TXTFROMWT_WET_TEXTCHANGED(ByVal SENDER As SYSTEM.OBJECT, ByVal E As SYSTEM.EVENTARGS)

    End Sub

    Private Sub TXTTOWT_WET_TEXTCHANGED(ByVal SENDER As SYSTEM.OBJECT, ByVal E As SYSTEM.EVENTARGS)

    End Sub

    Private Sub TXTLOTNOTO_NUM_TEXTCHANGED(ByVal SENDER As SYSTEM.OBJECT, ByVal E As SYSTEM.EVENTARGS)

    End Sub

    Private Sub LABEL29_CLICK(ByVal SENDER As SYSTEM.OBJECT, ByVal E As SYSTEM.EVENTARGS)

    End Sub

    Private Sub TXTPCS_NUM_TEXTCHANGED(ByVal SENDER As SYSTEM.OBJECT, ByVal E As SYSTEM.EVENTARGS)

    End Sub

    Private Sub RBTREGULAR_CHECKEDCHANGED(ByVal SENDER As SYSTEM.OBJECT, ByVal E As SYSTEM.EVENTARGS)

    End Sub

    Private Sub RBTALL_CHECKEDCHANGED(ByVal SENDER As SYSTEM.OBJECT, ByVal E As SYSTEM.EVENTARGS)

    End Sub

    Private Sub TOOLTIP1_POPUP(ByVal SENDER As SYSTEM.OBJECT, ByVal E As SYSTEM.WINDOWS.FORMS.POPUPEVENTARGS) Handles TOOLTIP1.POPUP

    End Sub
    Private Sub RESIZETOOLSTRIPMENUITEM_CLICK(ByVal SENDER As SYSTEM.OBJECT, ByVal E As SYSTEM.EVENTARGS) Handles RESIZETOOLSTRIPMENUITEM.CLICK
        RESIZETOOLSTRIPMENUITEM.CHECKED = True
        If GRIDTOTALVIEW.ROWCOUNT > 0 Then
            If RESIZETOOLSTRIPMENUITEM.CHECKED Then
                GRIDTOTALVIEW.AUTOSIZECOLUMNSMODE = DATAGRIDVIEWAUTOSIZECOLUMNMODE.ALLCELLS
                GRIDTOTALVIEW.INVALIDATE()
                For Each DGVCOL As DATAGRIDVIEWCOLUMN In GRIDTOTALVIEW.COLUMNS
                    DGVCOL.WIDTH = DGVCOL.WIDTH
                Next
                GRIDTOTALVIEW.AUTOSIZECOLUMNSMODE = DATAGRIDVIEWAUTOSIZECOLUMNMODE.NONE
            Else
                For Each DGVCOL As DATAGRIDVIEWCOLUMN In GRIDTOTALVIEW.COLUMNS
                    DGVCOL.WIDTH = DGVCOL.WIDTH
                Next
                GRIDTOTALVIEW.AUTOSIZECOLUMNSMODE = DATAGRIDVIEWAUTOSIZECOLUMNMODE.NONE
            End If
        End If
    End Sub


    Private Sub GRPFILTRATION_ENTER(ByVal SENDER As SYSTEM.OBJECT, ByVal E As SYSTEM.EVENTARGS) Handles GRPFILTRATION.ENTER

    End Sub

    Private Sub chkdate_CheckedChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkdate.CheckedChanged
        dtpTo_OWN.Value = Now.Date.ToString()
    End Sub
    Private Sub chkcmbItemName_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkcmbItemName.Leave
        STRSQL = " SELECT 'ALL' SUBITEMNAME,'0' SUBITEMID,1 RESULT"
        STRSQL += " UNION ALL"
        STRSQL += " SELECT SUBITEMNAME,SUBITEMID,2 RESULT FROM " & cnAdminDb & "..SUBITEMMAST WHERE ISNULL(ACTIVE,'')<>'N'"
        If chkcmbItemName.Text.ToUpper <> "ALL" Then
            STRSQL += " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN ('" & chkcmbItemName.Text.Replace(",", "','") & "') )"
        End If
        STRSQL += " ORDER BY RESULT,SUBITEMNAME"
        DTSUBITEMNAME = New DataTable
        DA = New OleDbDataAdapter(STRSQL, cn)
        DA.Fill(DTSUBITEMNAME)

        BrighttechPack.GlobalMethods.FillCombo(chkcmbSubItemName, DTSUBITEMNAME, "SUBITEMNAME", , "ALL")
    End Sub

    Private Sub chkcmbItemName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkcmbItemName.SelectedIndexChanged
        STRSQL = " SELECT 'ALL' SUBITEMNAME,'0' SUBITEMID,1 RESULT"
        STRSQL += " UNION ALL"
        STRSQL += " SELECT SUBITEMNAME,SUBITEMID,2 RESULT FROM " & cnAdminDb & "..SUBITEMMAST WHERE ISNULL(ACTIVE,'')<>'N'"
        If chkcmbItemName.Text.ToUpper <> "ALL" Then
            STRSQL += " AND ITEMID IN (SELECT ITEMID FROM " & cnAdminDb & "..ITEMMAST WHERE ITEMNAME IN ('" & chkcmbItemName.Text.Replace(",", "','") & "') )"
        End If
        STRSQL += " ORDER BY RESULT,SUBITEMNAME"
        DTSUBITEMNAME = New DataTable
        DA = New OleDbDataAdapter(STRSQL, cn)
        DA.Fill(DTSUBITEMNAME)

        BrighttechPack.GlobalMethods.FillCombo(chkcmbSubItemName, DTSUBITEMNAME, "SUBITEMNAME", , "ALL")
    End Sub
End Class
