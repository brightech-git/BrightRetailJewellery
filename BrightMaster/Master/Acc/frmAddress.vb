Imports System.Data.OleDb
Public Class frmAddress

#Region "Variable"

    Dim da As OleDbDataAdapter
    Dim cmd As OleDbCommand
    Dim strSql As String
    Dim TabName As String
    'Area
    
    Dim costid As String
    Dim cityid As String

    Dim UpdateFlag As Boolean
    Dim UpdateAreaId As String
    Dim UpdateArea As String
    Dim UpdatePincode As String
    Dim UpdateCityId As String

    'State
    Dim UpdateCountryId As String
    Dim UpdateCountry As String

    'City
    Dim stateid As String
    Dim UpdateCity As String

    'State
    Dim UpdateStateId As String
    Dim UpdateState As String

    'Id Proof

    Dim dtable As DataTable
    Dim flag As Boolean = False

#End Region

#Region "Form Event"

    Private Sub frmAddress_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnANew_Click(Me, New EventArgs())
        btnCNew_Click(Me, New EventArgs())
        btnSNew_Click(Me, New EventArgs())
        btnINew_Click(Me, New EventArgs())
        btnCoNew_Click(Me, New EventArgs())
    End Sub

    Private Sub frmAddress_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        TabName = tabMain.SelectedTab.Name

        If TabName.Contains("tabArea") Then
            If e.KeyCode = Keys.Enter Then
                If gridViewArea.Focused Then Return
                SendKeys.Send("{TAB}")
            End If

        ElseIf TabName.Contains("tabCountry") Then
            If e.KeyCode = Keys.Enter Then
                If gridviewCountry.Focused Then Return
                SendKeys.Send("{TAB}")
            End If

        ElseIf TabName.Contains("tabCity") Then
            If e.KeyCode = Keys.Enter Then
                If gridviewCity.Focused Then Return
                SendKeys.Send("{TAB}")
            End If

        ElseIf TabName.Contains("tabState") Then
            If e.KeyCode = Keys.Enter Then
                If gridviewState.Focused Then Return
                SendKeys.Send("{TAB}")
            End If

        ElseIf TabName.Contains("tabId") Then
            If e.KeyCode = Keys.Enter Then
                If gridviewIdProof.Focused Then Return
                SendKeys.Send("{TAB}")
            End If
        End If
        
    End Sub
#End Region

#Region "Area Button Event"

    Private Sub btnASave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnASave.Click
        If txtArea__Man.Text = "" Then
            MessageBox.Show("Enter Area Name")
            txtArea__Man.Focus()
            Return
        End If
        If txtPincode__Man.Text = "" Then
            MessageBox.Show("Enter Pincode")
            txtPincode__Man.Focus()
            Return
        End If
        If UpdateFlag = False Then
            strSql = "SELECT 1 VALUE FROM " & cnAdminDb & "..AREAMAST WHERE AREAID = '" & txtAreaId_MAN.Text & "'"
            If objGPack.GetSqlValue(strSql, "VALUE", "0") = "1" Then
                MsgBox("Area Id already Exists...", MsgBoxStyle.Information)
                txtArea__Man.Focus()
                Return
            End If

            strSql = "SELECT 1 VALUE FROM " & cnAdminDb & "..AREAMAST WHERE AREANAME = '" & txtArea__Man.Text & "'"
            If objGPack.GetSqlValue(strSql, "VALUE", "0") = "1" Then
                MsgBox("Area already Exists...", MsgBoxStyle.Information)
                txtArea__Man.Focus()
                Return
            End If

            strSql = "SELECT CITYID FROM " & cnAdminDb & "..CITYMAST WHERE CITYNAME = '" & CmbCityName.Text & "'"
            cmd = New OleDbCommand(strSql, cn)
            cityid = cmd.ExecuteScalar().ToString()

            strSql = " INSERT INTO " & cnAdminDb & "..AREAMAST"
            strSql += " ("
            strSql += " AREAID,AREANAME"
            strSql += " ,USERID,UPDATED,UPTIME"
            strSql += " ,PINCODE,CITYID"
            strSql += " ) VALUES("
            strSql += " '" & txtAreaId_MAN.Text & "'" 'Narration
            strSql += " ,'" & txtArea__Man.Text & "'" 'ModuleId
            strSql += " ,'" & userId & "'" 'UserId
            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'Updated
            strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'Uptime
            strSql += " ,'" & txtPincode__Man.Text & "'"
            strSql += " ,'" & cityid & "'"
            strSql += " )"
            Try
                ExecQuery(SyncMode.Master, strSql, cn)
                btnANew_Click(Me, New EventArgs())
            Catch ex As OleDbException
                If ex.ErrorCode = 2627 Then
                    MsgBox("Area Name Already Exist", MsgBoxStyle.Information)
                    txtArea__Man.Focus()
                Else
                    MsgBox(ex.Message)
                    MsgBox(ex.StackTrace)
                End If
            Catch ex As Exception
                MsgBox(ex.Message)
                MsgBox(ex.StackTrace)
            Finally
            End Try
        Else
            strSql = "SELECT 1 VALUE FROM " & cnAdminDb & "..AREAMAST WHERE AREAID = '" & txtAreaId_MAN.Text & "' AND AREAID != '" & UpdateAreaId & "'"
            If objGPack.GetSqlValue(strSql, "VALUE", "0") = "1" Then
                MsgBox("Area Id already Exists...", MsgBoxStyle.Information)
                txtArea__Man.Focus()
                Return
            End If

            strSql = "SELECT 1 VALUE FROM " & cnAdminDb & "..AREAMAST WHERE AREANAME = '" & txtArea__Man.Text & "' AND AREANAME != '" & UpdateArea & "'"
            If objGPack.GetSqlValue(strSql, "VALUE", "0") = "1" Then
                MsgBox("Area already Exists...", MsgBoxStyle.Information)
                txtArea__Man.Focus()
                Return
            End If

            strSql = "SELECT CITYID FROM " & cnAdminDb & "..CITYMAST WHERE CITYNAME = '" & CmbCityName.Text & "'"
            cmd = New OleDbCommand(strSql, cn)
            cityid = cmd.ExecuteScalar().ToString()

            strSql = " UPDATE " & cnAdminDb & "..AREAMAST"
            strSql += " SET AREANAME = '" & txtArea__Man.Text & "'"
            strSql += " ,USERID = '" & userId & "'"
            strSql += " ,UPDATED = '" & Today.Date.ToString("yyyy-MM-dd") & "'"
            strSql += " ,UPTIME = '" & Date.Now.ToLongTimeString & "'"
            strSql += " ,PINCODE = '" & txtPincode__Man.Text & "'"
            strSql += " ,CITYID = '" & cityid & "'"
            strSql += " WHERE AREAID = '" & txtAreaId_MAN.Text & "'"

            Try
                ExecQuery(SyncMode.Master, strSql, cn)
                btnANew_Click(Me, New EventArgs())
            Catch ex As OleDbException
                If ex.ErrorCode = 2627 Then
                    MsgBox("Area Name Already Exist", MsgBoxStyle.Information)
                    txtArea__Man.Focus()
                Else
                    MsgBox(ex.Message)
                    MsgBox(ex.StackTrace)
                End If
            Catch ex As Exception
                MsgBox(ex.Message)
                MsgBox(ex.StackTrace)
            Finally
            End Try
        End If
    End Sub
    Private Sub btnADelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnADelete.Click
        strSql = " SELECT 1 VALUE FROM " & cnAdminDb & "..ACHEAD WHERE AREAID = '" & txtAreaId_MAN.Text & "'"
        If objGPack.GetSqlValue(strSql, "VALUES", "0") = "1" Then
            MsgBox("Area Id already Exists...", MsgBoxStyle.Information)
            txtArea__Man.Focus()
            Return
        End If

        strSql = " DELETE FROM " & cnAdminDb & "..AREAMAST"
        strSql += " WHERE AREAID = '" & txtAreaId_MAN.Text & "'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        If MessageBox.Show("Do you want Delete this Item", "Delete Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.OK Then
            If ExecQuery(SyncMode.Master, strSql, cn) Then
                MsgBox("Successfully Deleted..")
                btnANew_Click(Me, New EventArgs())
            End If
        End If
    End Sub

    Private Sub btnAExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAExit.Click
        Me.Close()
    End Sub
    Private Sub btnANew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnANew.Click
        txtArea__Man.Focus()
        txtAreaId_MAN.Text = ""
        'strSql = " SELECT ISNULL(REPLICATE('0',3 - LEN(CONVERT(VARCHAR,MAX(CONVERT(NUMERIC,AREAID))))) +  CONVERT(VARCHAR,MAX(CONVERT(NUMERIC,AREAID) + 1)),'001') AREAID FROM " & cnAdminDb & "..AREAMAST"
        'txtAreaId_MAN.Text = objGPack.GetSqlValue(strSql, "AREAID", "001")

        strSql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='COSTID'"
        cmd = New OleDbCommand(strSql, cn)
        costid = cmd.ExecuteScalar().ToString()
        If costid <> "" Then
            strSql = "SELECT MAX(CAST(SUBSTRING(AREAID,3,5) AS INT))+1 AREAID FROM " & cnAdminDb & "..AREAMAST"
        Else
            strSql = "SELECT MAX(CONVERT(INT,AREAID))+1 AREAID FROM " & cnAdminDb & "..AREAMAST"
        End If
        'strSql = "SELECT ISNULL(REPLICATE('0',5-LEN(CONVERT(VARCHAR,MAX(SUBSTRING(AREAID,3,5)))))+CONVERT(VARCHAR,MAX(SUBSTRING(AREAID,3,5))+1),'001')AREAID FROM " & cnAdminDb & "..AREAMAST"
        txtAreaId_MAN.Text = costid + objGPack.GetSqlValue(strSql, "AREAID", "1")

        txtArea__Man.Text = ""
        txtPincode__Man.Text = ""
        CmbCityName.Text = ""
        UpdateArea = ""
        UpdateAreaId = ""
        UpdatePincode = ""
        UpdateCityId = ""
        UpdateFlag = False

        CmbCityName.Items.Clear()
        strSql = " SELECT CITYNAME FROM " & cnAdminDb & "..CITYMAST ORDER BY CITYNAME"
        objGPack.FillCombo(strSql, CmbCityName, False, False)

        strSql = " SELECT AREAID,AREANAME,(SELECT CITYNAME FROM " & cnAdminDb & "..CITYMAST WHERE CITYID=AR.CITYID)CITYNAME,PINCODE FROM " & cnAdminDb & "..AREAMAST AR ORDER BY AREANAME"
        Dim dtArea As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtArea)
        gridViewArea.DataSource = dtArea
        If gridViewArea.ColumnCount > 0 Then
            gridViewArea.Columns("AREANAME").Width = 200
            gridViewArea.Columns("CITYNAME").Width = 200
            gridViewArea.Columns("PINCODE").Width = 100
            gridViewArea.Columns("AREAID").Visible = False
            gridViewArea.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            gridViewArea.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 9, FontStyle.Bold)
        End If

        txtArea__Man.Focus()
    End Sub
    Private Sub btnAGrid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAGrid.Click
        strSql = " SELECT AREAID,AREANAME,(SELECT CITYNAME FROM " & cnAdminDb & "..CITYMAST WHERE CITYID=AR.CITYID)CITYNAME,PINCODE FROM " & cnAdminDb & "..AREAMAST AR ORDER BY AREANAME"
        Dim dtArea As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtArea)
        gridViewArea.DataSource = dtArea
        If gridViewArea.ColumnCount > 0 Then
            gridViewArea.Columns("AREANAME").Width = 200
            gridViewArea.Columns("CITYNAME").Width = 200
            gridViewArea.Columns("PINCODE").Width = 100
            gridViewArea.Columns("AREAID").Visible = False
            gridViewArea.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            gridViewArea.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            gridViewArea.Focus()
        End If
    End Sub
#End Region

#Region "Gridview Area"
    'Area
    Private Sub gridViewArea_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridViewArea.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            txtAreaId_MAN.Text = gridViewArea.CurrentRow.Cells("AREAID").Value.ToString
            txtArea__Man.Text = gridViewArea.CurrentRow.Cells("AREANAME").Value.ToString
            txtPincode__Man.Text = gridViewArea.CurrentRow.Cells("PINCODE").Value.ToString
            CmbCityName.Text = gridViewArea.CurrentRow.Cells("CITYNAME").Value.ToString
            UpdateAreaId = txtAreaId_MAN.Text
            UpdateArea = txtArea__Man.Text
            UpdatePincode = txtPincode__Man.Text
            UpdateCityId = CmbCityName.Text
            UpdateFlag = True
            txtArea__Man.Select()
        End If
    End Sub
#End Region

#Region "City Button Event"
    Private Sub btnCSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCSave.Click
        If txtCity__Man.Text = "" Then
            MessageBox.Show("Enter City Name")
            txtCity__Man.Focus()
            Return
        End If
        If CmbStateName.Text = "" Then
            MessageBox.Show("Select Country Name")
            CmbStateName.Focus()
            Return
        End If
        If UpdateFlag = False Then
            strSql = "SELECT 1 VALUE FROM " & cnAdminDb & "..CITYMAST WHERE CITYID = '" & txtCityId_MAN.Text & "'"
            If objGPack.GetSqlValue(strSql, "VALUE", "0") = "1" Then
                MsgBox("City Id already Exists...", MsgBoxStyle.Information)
                txtCity__Man.Focus()
                Return
            End If

            strSql = "SELECT 1 VALUE FROM " & cnAdminDb & "..CITYMAST WHERE CITYNAME = '" & txtCity__Man.Text & "'"
            If objGPack.GetSqlValue(strSql, "VALUE", "0") = "1" Then
                MsgBox("City already Exists...", MsgBoxStyle.Information)
                txtCity__Man.Focus()
                Return
            End If

            strSql = "SELECT STATEID FROM " & cnAdminDb & "..STATEMAST WHERE STATENAME = '" & CmbStateName.Text & "'"
            cmd = New OleDbCommand(strSql, cn)
            stateid = cmd.ExecuteScalar().ToString()

            strSql = " INSERT INTO " & cnAdminDb & "..CITYMAST"
            strSql += " ("
            strSql += " CITYID,CITYNAME,DISTRICT,ZONAL,STATEID"
            strSql += " ) VALUES ("
            strSql += " '" & txtCityId_MAN.Text & "'" 'CityId
            strSql += " ,'" & txtCity__Man.Text & "'" 'Cityname
            strSql += " ,'" & txtDistrict.Text & "'" 'District
            strSql += " ,'" & txtZonal.Text & "'" 'Zonal
            strSql += " ,'" & stateid & "'"
            strSql += " )"
            Try
                ExecQuery(SyncMode.Master, strSql, cn)

                btnCNew_Click(Me, New EventArgs())
            Catch ex As OleDbException
                'If cn.State = cnnectionState.Open Then

                'End If
                If ex.ErrorCode = 2627 Then
                    MsgBox("City Name Already Exist..", MsgBoxStyle.Information)
                    txtCity__Man.Focus()
                Else
                    'If cn.State = cnnectionState.Open Then

                    'End If
                    MsgBox(ex.Message)
                    MsgBox(ex.StackTrace)
                End If
            Catch ex As Exception
                'If cn.State = cnnectionState.Open Then

                'End If
                MsgBox(ex.Message)
                MsgBox(ex.StackTrace)
            Finally
            End Try
        Else
            strSql = "SELECT 1 VALUE FROM " & cnAdminDb & "..CITYMAST WHERE CITYID = '" & txtCityId_MAN.Text & "' AND CITYID != '" & UpdateCityId & "'"
            If objGPack.GetSqlValue(strSql, "VALUE", "0") = "1" Then
                MsgBox("City Id already Exists...", )
                txtCity__Man.Focus()
                Return
            End If

            strSql = "SELECT 1 VALUE FROM " & cnAdminDb & "..CITYMAST WHERE CITYNAME = '" & txtCity__Man.Text & "' AND CITYNAME != '" & UpdateCity & "'"
            If objGPack.GetSqlValue(strSql, "VALUE", "0") = "1" Then
                MsgBox("City already Exists...", MsgBoxStyle.Information)
                txtCity__Man.Focus()
                Return
            End If

            strSql = "SELECT STATEID FROM " & cnAdminDb & "..STATEMAST WHERE STATENAME = '" & CmbStateName.Text & "'"
            cmd = New OleDbCommand(strSql, cn)
            stateid = cmd.ExecuteScalar().ToString()

            strSql = " UPDATE " & cnAdminDb & "..CITYMAST"
            strSql += " SET CITYNAME = '" & txtCity__Man.Text & "'"
            strSql += " ,DISTRICT = '" & txtDistrict.Text & "'"
            strSql += " ,ZONAL = '" & txtZonal.Text & "'"
            strSql += " ,STATEID = '" & stateid & "'"
            strSql += " WHERE CITYID = '" & txtCityId_MAN.Text & "'"

            Try
                ExecQuery(SyncMode.Master, strSql, cn)
                btnCNew_Click(Me, New EventArgs())
            Catch ex As OleDbException
                
                If ex.ErrorCode = 2627 Then
                    MsgBox("City Name Already Exist", MsgBoxStyle.Information)
                    txtCity__Man.Focus()
                Else
                    MsgBox(ex.Message)
                    MsgBox(ex.StackTrace)
                End If
            Catch ex As Exception
                MsgBox(ex.Message)
                MsgBox(ex.StackTrace)
            Finally
                
            End Try
        End If

    End Sub


    Private Sub btnCGrid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCGrid.Click
        strSql = " SELECT CITYID,CITYNAME,DISTRICT,ZONAL,(SELECT STATENAME FROM " & cnAdminDb & "..STATEMAST WHERE STATEID=CI.STATEID)STATENAME FROM " & cnAdminDb & "..CITYMAST CI ORDER BY CITYNAME"
        Dim dtCity As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCity)
        gridviewCity.DataSource = dtCity
        If gridviewCity.ColumnCount > 0 Then
            gridviewCity.Columns("CITYNAME").Width = 200
            gridviewCity.Columns("DISTRICT").Width = 150
            gridviewCity.Columns("ZONAL").Width = 150
            gridviewCity.Columns("STATENAME").Width = 150
            gridviewCity.Columns("CITYID").Visible = False
            gridviewCity.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            gridviewCity.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            gridviewCity.Focus()
        End If
    End Sub

    Private Sub btnCNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCNew.Click
        txtCity__Man.Focus()
        txtCityId_MAN.Text = ""
        'strSql = " SELECT ISNULL(REPLICATE('0',3 - LEN(CONVERT(VARCHAR,MAX(CONVERT(NUMERIC,CITYID))))) +  CONVERT(VARCHAR,MAX(CONVERT(NUMERIC,CITYID) + 1)),'001') CITYID FROM " & cnAdminDb & "..CITYMAST"
        strSql = "SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID='COSTID'"
        cmd = New OleDbCommand(strSql, cn)
        costid = cmd.ExecuteScalar().ToString()
        If costid <> "" Then
            strSql = " SELECT ISNULL(CONVERT(VARCHAR,MAX(SUBSTRING(CITYID,3,5)) + 1),'1')CITYID FROM " & cnAdminDb & "..CITYMAST"

            txtCityId_MAN.Text = costid + objGPack.GetSqlValue(strSql, "CITYID", "1")
        Else
            strSql = " SELECT MAX(CONVERT(INT,CITYID))+1 CITYID FROM " & cnAdminDb & "..CITYMAST"
            txtCityId_MAN.Text = objGPack.GetSqlValue(strSql, "CITYID", "1")
        End If
        txtCity__Man.Text = ""
        CmbStateName.Text = ""
        txtDistrict.Text = ""
        txtZonal.Text = ""
        UpdateCity = ""
        UpdateCityId = ""
        UpdateFlag = False

        CmbStateName.Items.Clear()
        strSql = " SELECT STATENAME FROM " & cnAdminDb & "..STATEMAST ORDER BY STATENAME"
        objGPack.FillCombo(strSql, CmbStateName, False, False)

        strSql = " SELECT CITYID,CITYNAME,DISTRICT,ZONAL,(SELECT STATENAME FROM " & cnAdminDb & "..STATEMAST WHERE STATEID=CI.STATEID)STATENAME FROM " & cnAdminDb & "..CITYMAST CI ORDER BY CITYNAME"
        Dim dtCity As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCity)
        gridviewCity.DataSource = dtCity
        If gridviewCity.ColumnCount > 0 Then
            gridviewCity.Columns("CITYNAME").Width = 200
            gridviewCity.Columns("DISTRICT").Width = 150
            gridviewCity.Columns("ZONAL").Width = 150
            gridviewCity.Columns("STATENAME").Width = 150
            gridviewCity.Columns("CITYID").Visible = False
            gridviewCity.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            gridviewCity.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 9, FontStyle.Bold)
        End If
        txtCity__Man.Focus()
    End Sub

    Private Sub btnCExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCExit.Click
        Me.Close()
    End Sub

    Private Sub btnCDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCDelete.Click
        strSql = " DELETE FROM " & cnAdminDb & "..CITYMAST"
        strSql += " WHERE CITYID = '" & txtCityId_MAN.Text & "'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        If MessageBox.Show("Do you want Delete this Item", "Delete Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.OK Then
            If ExecQuery(SyncMode.Master, strSql, cn) Then
                MsgBox("Successfully Deleted..")
                btnCNew_Click(Me, New EventArgs())
            End If
        End If
    End Sub

#End Region

#Region "City GridView Event"

    Private Sub gridviewCity_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridviewCity.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            txtCityId_MAN.Text = gridviewCity.CurrentRow.Cells("CITYID").Value.ToString
            txtCity__Man.Text = gridviewCity.CurrentRow.Cells("CITYNAME").Value.ToString
            txtDistrict.Text = gridviewCity.CurrentRow.Cells("DISTRICT").Value.ToString
            txtZonal.Text = gridviewCity.CurrentRow.Cells("ZONAL").Value.ToString
            CmbStateName.Text = gridviewCity.CurrentRow.Cells("STATENAME").Value.ToString
            UpdateCityId = txtCityId_MAN.Text
            UpdateCity = txtCity__Man.Text
            UpdateFlag = True
            txtCity__Man.Select()
        End If
    End Sub
#End Region

#Region "Button Event State"

    Private Sub btnSSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSSave.Click
        If txtState__Man.Text = "" Then
            MessageBox.Show("Enter State Name")
            txtState__Man.Focus()
            Return
        End If
        If txtCountry__Man.Text = "" Then
            MessageBox.Show("Enter Country Name")
            txtCountry__Man.Focus()
            Return
        End If
        If UpdateFlag = False Then
            strSql = "SELECT 1 VALUE FROM " & cnAdminDb & "..STATEMAST WHERE STATEID = '" & txtStateId_MAN.Text & "'"
            If objGPack.GetSqlValue(strSql, "VALUE", "0") = "1" Then
                MessageBox.Show("State Id already Exists...", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtState__Man.Focus()
                Return
            End If

            strSql = "SELECT 1 VALUE FROM " & cnAdminDb & "..STATEMAST WHERE STATENAME = '" & txtState__Man.Text & "'"
            If objGPack.GetSqlValue(strSql, "VALUE", "0") = "1" Then
                MessageBox.Show("State already Exists...", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtState__Man.Focus()
                Return
            End If

            strSql = " INSERT INTO " & cnAdminDb & "..STATEMAST"
            strSql += " ("
            strSql += " STATEID,STATENAME"
            strSql += " ,USERID,UPDATED,UPTIME,COUNTRYNAME"
            strSql += " ) VALUES("
            strSql += " '" & txtStateId_MAN.Text & "'" 'Narration
            strSql += " ,'" & txtState__Man.Text & "'" 'ModuleId
            strSql += " ,'" & userId & "'" 'UserId
            strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'" 'Updated
            strSql += " ,'" & Date.Now.ToLongTimeString & "'" 'Uptime
            strSql += " ,'" & txtCountry__Man.Text & "'"
            strSql += " )"
            Try
                ExecQuery(SyncMode.Master, strSql, cn)
                btnSNew_Click(Me, New EventArgs())
            Catch ex As OleDbException
                If ex.ErrorCode = 2627 Then
                    MsgBox("State Name Already Exist", MsgBoxStyle.Information)
                    txtState__Man.Focus()
                Else
                    MsgBox(ex.Message & vbCrLf & ex.StackTrace, MsgBoxStyle.Information)
                End If
            Catch ex As Exception
                MsgBox(ex.Message & vbCrLf & ex.StackTrace, MsgBoxStyle.Information)
            Finally
            End Try
        Else
            strSql = "SELECT 1 VALUE FROM " & cnAdminDb & "..STATEMAST WHERE STATEID = '" & txtStateId_MAN.Text & "' AND STATEID != '" & UpdateStateId & "'"
            If objGPack.GetSqlValue(strSql, "VALUE", "0") = "1" Then
                MessageBox.Show("State Id already Exists...", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtState__Man.Focus()
                Return
            End If

            strSql = "SELECT 1 VALUE FROM " & cnAdminDb & "..STATEMAST WHERE STATENAME = '" & txtState__Man.Text & "' AND STATENAME != '" & UpdateState & "'"
            If objGPack.GetSqlValue(strSql, "VALUE", "0") = "1" Then
                MessageBox.Show("State already Exists...", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtState__Man.Focus()
                Return
            End If

            strSql = " UPDATE " & cnAdminDb & "..STATEMAST"
            strSql += " SET STATENAME = '" & txtState__Man.Text & "'"
            strSql += " ,USERID = '" & userId & "'"
            strSql += " ,UPDATED = '" & Today.Date.ToString("yyyy-MM-dd") & "'"
            strSql += " ,UPTIME = '" & Date.Now.ToLongTimeString & "'"
            strSql += " ,COUNTRYNAME = '" & txtCountry__Man.Text & "'"
            strSql += " WHERE STATEID = '" & txtStateId_MAN.Text & "'"

            Try
                ExecQuery(SyncMode.Master, strSql, cn)

                btnSNew_Click(Me, New EventArgs())
            Catch ex As OleDbException
                If ex.ErrorCode = 2627 Then
                    MsgBox("State Name Already Exist", MsgBoxStyle.Information)
                    txtState__Man.Focus()
                Else
                    MsgBox(ex.Message & vbCrLf & ex.StackTrace, MsgBoxStyle.Information)
                End If
            Catch ex As Exception
                MsgBox(ex.Message & vbCrLf & ex.StackTrace, MsgBoxStyle.Information)
            Finally
            End Try
        End If
    End Sub

    Private Sub btnSNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSNew.Click
        txtState__Man.Focus()
        txtStateId_MAN.Text = ""
        'strSql = " SELECT ISNULL(MAX(CONVERT(NUMERIC,STATEID))),0)+1,'1') STATEID FROM " & cnAdminDb & "..STATEMAST"
        strSql = "SELECT ISNULL(MAX(CONVERT(INT,STATEID)),0)+1 STATEID FROM " & cnAdminDb & "..STATEMAST"
        txtStateId_MAN.Text = objGPack.GetSqlValue(strSql, "STATEID", "1")
        txtState__Man.Text = ""
        txtCountry__Man.Text = ""
        UpdateState = ""
        UpdateStateId = ""
        UpdateFlag = False

        strSql = " SELECT STATEID,STATENAME,COUNTRYNAME FROM " & cnAdminDb & "..STATEMAST ORDER BY STATENAME"
        Dim dtstate As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtstate)
        gridviewState.DataSource = dtstate
        If gridviewState.ColumnCount > 0 Then
            gridviewState.Columns("STATENAME").Width = 350
            gridviewState.Columns("COUNTRYNAME").Width = 150
            gridviewState.Columns("STATEID").Visible = False
            gridviewState.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            gridviewState.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 9, FontStyle.Bold)
        End If

        txtState__Man.Focus()
    End Sub

    Private Sub btnSGrid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSGrid.Click
        strSql = " SELECT STATEID,STATENAME,COUNTRYNAME FROM " & cnAdminDb & "..STATEMAST ORDER BY STATENAME"
        Dim dtState As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtState)
        gridviewState.DataSource = dtState
        If gridviewState.ColumnCount > 0 Then
            gridviewState.Columns("STATENAME").Width = 350
            gridviewState.Columns("COUNTRYNAME").Width = 150
            gridviewState.Columns("STATEID").Visible = False
            gridviewState.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            gridviewState.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            gridviewState.Focus()
        End If
    End Sub

    Private Sub btnSExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSExit.Click
        Me.Close()
    End Sub

    Private Sub btnSDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSDelete.Click
        strSql = " DELETE FROM " & cnAdminDb & "..STATEMAST"
        strSql += " WHERE STATEID = '" & txtStateId_MAN.Text & "'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        If MessageBox.Show("Do you want Delete this Item", "Delete Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.OK Then
            If ExecQuery(SyncMode.Master, strSql, cn) Then
                MsgBox("Successfully Deleted..")
                btnSNew_Click(Me, New EventArgs())
            End If
        End If
    End Sub
#End Region

#Region "Gridview Event State"
    Private Sub gridviewState_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridviewState.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            txtStateId_MAN.Text = gridviewState.CurrentRow.Cells("STATEID").Value.ToString
            txtState__Man.Text = gridviewState.CurrentRow.Cells("STATENAME").Value.ToString
            txtCountry__Man.Text = gridviewState.CurrentRow.Cells("COUNTRYNAME").Value.ToString
            UpdateStateId = txtStateId_MAN.Text
            UpdateState = txtState__Man.Text
            UpdateFlag = True
            txtState__Man.Select()
        End If
    End Sub
#End Region


#Region "Button Id Proof"

    Private Sub btnISave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnISave.Click
        If txtName.Text = "" Then
            MessageBox.Show("Enter Proof Name")
            txtName.Focus()
            Return
        End If

        If txtMaxlen_NUM.Text = "" Then
            MessageBox.Show("Enter MaxLength")
            txtMaxlen_NUM.Focus()
            Return
        End If

        If txtDispOrd_NUM.Text = "" Then
            MessageBox.Show("Enter Display Order")
            txtDispOrd_NUM.Focus()
            Return
        End If

        If Val(txtMaxlen_NUM.Text) > 20 Then
            MsgBox("Should Not Exceeds MaxLength", MsgBoxStyle.Information)
            txtMaxlen_NUM.Focus()
            Return
        End If

        If flag = False Then
            strSql = "SELECT 1 VALUE FROM " & cnAdminDb & "..IDPROOF WHERE NAME = '" & txtName.Text & "'"
            If objGPack.GetSqlValue(strSql, "VALUE", "0") = "1" Then
                MsgBox("Name already Exists ...", MsgBoxStyle.Information)
                txtName.Focus()
                Return
            End If
        End If


        'strSql = "SELECT MAX(CONVERT(INT,ID))+1 ID FROM " & cnAdminDb & " IDPROOF"
        'txtProofId.Text = objGPack.GetSqlValue(strSql, "ID", "1")

        If flag = False Then
            funcInsert()
            MsgBox("Saved")
            funcCallGrid()
            btnINew_Click(Me, New EventArgs())
        Else
            funcUpdate()
            MsgBox("Updated")
            funcCallGrid()
            btnINew_Click(Me, New EventArgs())
        End If
    End Sub

    Private Sub btnINew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnINew.Click

        txtName.Focus()
        CmbActive.Items.Clear()
        CmbActive.Items.Add("YES")
        CmbActive.Items.Add("NO")
        CmbActive.Text = "YES"
        'dtable = New DataTable

        'strSql = "SELECT MAX(CONVERT(INT,ID))+1 ID FROM " & cnAdminDb & "..IDPROOF"
        'da = New OleDbDataAdapter(strSql, cn)
        'da.Fill(dtable)
        'txtProofId.Text = Val(dtable.Rows(0).Item(0).ToString)

        strSql = "SELECT ISNULL(MAX(ID),0)+1 ID FROM " & cnAdminDb & "..IDPROOF"
        da = New OleDbDataAdapter(strSql, cn)
        dtable = New DataTable
        da.Fill(dtable)
        txtProofId.Text = Val(dtable.Rows(0).Item(0).ToString)
        txtName.Text = ""
        txtMaxlen_NUM.Text = ""
        txtDispOrd_NUM.Text = ""
        txtIFormat.Text = ""
        funcCallGrid()
        txtName.Focus()
        txtName.CharacterCasing = CharacterCasing.Normal
        flag = False
    End Sub

    Private Sub btnIGrid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIGrid.Click
        funcCallGrid()
    End Sub

    Private Sub btnIExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIExit.Click
        Me.Close()
    End Sub

    Private Sub btnIDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIDelete.Click
        strSql = " DELETE FROM " & cnAdminDb & "..IDPROOF"
        strSql += " WHERE ID = '" & txtProofId.Text & "'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        If MessageBox.Show("Do you want Delete this Item", "Delete Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.OK Then
            If ExecQuery(SyncMode.Master, strSql, cn) Then
                MsgBox("Successfully Deleted..")
                btnINew_Click(Me, New EventArgs())
            End If
        End If
    End Sub
#End Region

#Region "IDPROOF GRIDVIEW"
    Private Sub gridviewIdProof_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridviewIdProof.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            If gridviewIdProof.RowCount > 0 Then
                gridviewIdProof.CurrentCell = gridviewIdProof.CurrentCell
                funcGetDetails(gridviewIdProof.Item("ID", gridviewIdProof.CurrentRow.Index).Value.ToString)
                'Me.SelectNextControl(Me, True, True, True, True)
            End If
        ElseIf e.KeyCode = Keys.Escape Then
            'Me.SelectNextControl(Me, True, True, True, True)
        End If

    End Sub
#End Region


#Region "USER DEFINE FUNCTION IDPROOF"
    Function funcCallGrid()

        gridviewIdProof.DataSource = Nothing
        strSql = " SELECT ID,NAME,LENGTH"
        strSql += " ,CASE WHEN ACTIVE='N' THEN 'NO' ELSE 'YES' END ACTIVE"
        strSql += " ,DISPORDER,FORMAT "
        strSql += " FROM " & cnAdminDb & "..IDPROOF "
        strSql += " ORDER BY ID"
        Dim dtidProof As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtidProof)
        gridviewIdProof.DataSource = dtidProof
        If gridviewIdProof.ColumnCount > 0 Then
            gridviewIdProof.Columns("ID").Visible = False
            gridviewIdProof.Columns("NAME").Width = 150
            gridviewIdProof.Columns("LENGTH").Width = 150
            gridviewIdProof.Columns("ACTIVE").Width = 100
            gridviewIdProof.Columns("DISPORDER").Width = 150
            gridviewIdProof.Columns("FORMAT").Width = 150
            gridviewIdProof.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            gridviewIdProof.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            gridviewIdProof.Focus()
        End If
    End Function
    Function funcGetDetails(ByVal Id As Integer)
        Dim dt As New DataTable
        dt.Clear()
        strSql = " SELECT *"
        strSql += " FROM " & cnAdminDb & "..IDPROOF WHERE ID = " & Id
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dt)
        If Not dt.Rows.Count > 0 Then
            Return 0
        End If
        With dt.Rows(0)
            txtProofId.Text = .Item("ID").ToString
            txtName.Text = .Item("NAME").ToString
            txtMaxlen_NUM.Text = .Item("LENGTH").ToString
            txtDispOrd_NUM.Text = .Item("DISPORDER").ToString
            CmbActive.Text = IIf(.Item("ACTIVE").ToString = "N", "NO", "YES")
            txtIFormat.Text = .Item("FORMAT").ToString
        End With
        flag = True
        txtProofId.Enabled = False
        txtName.Focus()
        txtName.Select()
        Return 0
    End Function

    Function funcUpdate()
        strSql = " UPDATE " & cnAdminDb & "..IDPROOF SET"
        strSql += " ID= '" & txtProofId.Text & "'"
        strSql += " ,NAME= '" & txtName.Text & "'"
        strSql += " ,LENGTH= '" & txtMaxlen_NUM.Text & "'"
        strSql += " ,ACTIVE=  '" & CmbActive.Text.Substring(0, 1) & "'"
        strSql += " ,DISPORDER= '" & txtDispOrd_NUM.Text & "'"
        strSql += " ,UPDATED = '" & Today.Date.ToString("yyyy-MM-dd") & "'"
        strSql += " ,UPTIME = '" & Date.Now.ToLongTimeString() & "'"
        strSql += " ,FORMAT = '" & txtIFormat.Text & "'"
        strSql += " WHERE ID = '" & txtProofId.Text & "'"
        Try
            ExecQuery(SyncMode.Master, strSql, cn)
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox(ex.StackTrace)
        End Try
        Return 0
    End Function

    Function funcInsert()

        strSql = " INSERT INTO " & cnAdminDb & "..IDPROOF"
        strSql += " ("
        strSql += " ID,NAME"
        strSql += " ,LENGTH,ACTIVE,DISPORDER"
        strSql += " ,UPDATED,UPTIME,FORMAT"
        strSql += " ) VALUES("
        strSql += " '" & txtProofId.Text & "'"
        strSql += " ,'" & txtName.Text & "'"
        strSql += " ,'" & txtMaxlen_NUM.Text & "'"
        strSql += " ,'" & CmbActive.Text.Substring(0, 1) & "'"
        strSql += " ," & txtDispOrd_NUM.Text & ""
        strSql += " ,'" & Today.Date.ToString("yyyy-MM-dd") & "'"
        strSql += " ,'" & Date.Now.ToLongTimeString() & "'"
        strSql += " ,'" & txtIFormat.Text & "'"
        strSql += " )"
        Try
            ExecQuery(SyncMode.Master, strSql, cn)
        Catch ex As Exception
            MsgBox(ex.ToString, MsgBoxStyle.Information)
        Finally
        End Try
    End Function
#End Region

#Region "ToolStrip"
    Private Sub GridToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GridToolStripMenuItem.Click
        TabName = tabMain.SelectedTab.Name
        If TabName.Contains("tabArea") Then
            btnAGrid_Click(Me, New EventArgs())
        ElseIf TabName.Contains("tabCity") Then
            btnCGrid_Click(Me, New EventArgs())
        ElseIf TabName.Contains("tabState") Then
            btnSGrid_Click(Me, New EventArgs())
        ElseIf TabName.Contains("tabId") Then
            btnIGrid_Click(Me, New EventArgs())
        End If
    End Sub
    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        TabName = tabMain.SelectedTab.Name
        If TabName.Contains("tabArea") Then
            btnASave_Click(Me, New EventArgs())
        ElseIf TabName.Contains("tabCity") Then
            btnCSave_Click(Me, New EventArgs())
        ElseIf TabName.Contains("tabState") Then
            btnSSave_Click(Me, New EventArgs())
        ElseIf TabName.Contains("tabId") Then
            btnISave_Click(Me, New EventArgs())
        End If
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        TabName = tabMain.SelectedTab.Name
        If TabName.Contains("tabArea") Then
            btnANew_Click(Me, New EventArgs())
        ElseIf TabName.Contains("tabCity") Then
            btnCNew_Click(Me, New EventArgs())
        ElseIf TabName.Contains("tabState") Then
            btnSNew_Click(Me, New EventArgs())
        ElseIf TabName.Contains("tabId") Then
            btnINew_Click(Me, New EventArgs())
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        TabName = tabMain.SelectedTab.Name
        If TabName.Contains("tabArea") Then
            Me.Close()
        ElseIf TabName.Contains("tabCity") Then
            Me.Close()
        ElseIf TabName.Contains("tabState") Then
            Me.Close()
        ElseIf TabName.Contains("tabId") Then
            Me.Close()
        End If
    End Sub

    Private Sub DeleteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteToolStripMenuItem.Click
        TabName = tabMain.SelectedTab.Name
        If TabName.Contains("tabArea") Then
            btnADelete_Click(Me, New EventArgs())
        ElseIf TabName.Contains("tabCity") Then
            btnCDelete_Click(Me, New EventArgs())
        ElseIf TabName.Contains("tabState") Then
            btnSDelete_Click(Me, New EventArgs())
        ElseIf TabName.Contains("tabId") Then
            btnIDelete_Click(Me, New EventArgs())
        End If
    End Sub
#End Region

#Region "TextBox Event IDProof"
    Private Sub txtMaxlen_NUM_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMaxlen_NUM.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Val(txtMaxlen_NUM.Text) > 20 Then
                MsgBox("Should Not Exceeds MaxLength", MsgBoxStyle.Information)
            End If
        End If
    End Sub
#End Region

    Private Sub btnCoNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCoNew.Click
        strSql = "SELECT ISNULL(MAX(CONVERT(INT,COUNTRYID)),0)+1 COUNTRYID FROM " & cnAdminDb & "..COUNTRYMAST"
        txtCountryId_MAN.Text = objGPack.GetSqlValue(strSql, "COUNTRYID", "1")
        txtCountry_MAN.Text = ""
        UpdateCountry = ""
        UpdateCountryId = ""
        UpdateFlag = False

        strSql = " SELECT COUNTRYID,COUNTRYNAME FROM " & cnAdminDb & "..COUNTRYMAST ORDER BY COUNTRYNAME"
        Dim dtCountry As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCountry)
        gridviewCountry.DataSource = dtCountry
        If gridviewCountry.ColumnCount > 0 Then
            gridviewCountry.Columns("COUNTRYNAME").Width = 350
            gridviewCountry.Columns("COUNTRYID").Visible = False
            gridviewCountry.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            gridviewCountry.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 9, FontStyle.Bold)
        End If
        txtCountry_MAN.Focus()
    End Sub

    Private Sub btnCoSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCoSave.Click
        If txtCountry_MAN.Text = "" Then
            MessageBox.Show("Enter Country Name")
            txtCountry_MAN.Focus()
            Return
        End If
        If UpdateFlag = False Then
            strSql = "SELECT 1 VALUE FROM " & cnAdminDb & "..COUNTRYMAST WHERE COUNTRYID = '" & txtCountryId_MAN.Text & "'"
            If objGPack.GetSqlValue(strSql, "VALUE", "0") = "1" Then
                MessageBox.Show("Country Id already Exists...", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtCountryId_MAN.Focus()
                Return
            End If


            strSql = " INSERT INTO " & cnAdminDb & "..COUNTRYMAST"
            strSql += " ("
            strSql += " COUNTRYID,COUNTRYNAME"
            strSql += " ) VALUES("
            strSql += " '" & txtCountryId_MAN.Text & "'"
            strSql += " ,'" & txtCountry_MAN.Text & "'"
            strSql += " )"
            Try
                ExecQuery(SyncMode.Master, strSql, cn)
                btnCoNew_Click(Me, New EventArgs())
            Catch ex As OleDbException
                If ex.ErrorCode = 2627 Then
                    MsgBox("Country Name Already Exist", MsgBoxStyle.Information)
                    txtCountryId_MAN.Focus()
                Else
                    MsgBox(ex.Message & vbCrLf & ex.StackTrace, MsgBoxStyle.Information)
                End If
            Catch ex As Exception
                MsgBox(ex.Message & vbCrLf & ex.StackTrace, MsgBoxStyle.Information)
            Finally
            End Try
        Else
            strSql = "SELECT 1 VALUE FROM " & cnAdminDb & "..COUNTRYMAST WHERE COUNTRYID = '" & txtCountryId_MAN.Text & "' AND COUNTRYID != '" & UpdateCountryId & "'"
            If objGPack.GetSqlValue(strSql, "VALUE", "0") = "1" Then
                MessageBox.Show("Country Id already Exists...", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                txtState__Man.Focus()
                Return
            End If

            strSql = " UPDATE " & cnAdminDb & "..COUNTRYMAST"
            strSql += " SET COUNTRYNAME = '" & txtCountry_MAN.Text & "'"
            strSql += " WHERE COUNTRYID = '" & txtCountryId_MAN.Text & "'"
            Try
                ExecQuery(SyncMode.Master, strSql, cn)
                btnCoNew_Click(Me, New EventArgs())
            Catch ex As OleDbException
                If ex.ErrorCode = 2627 Then
                    MsgBox("Country Name Already Exist", MsgBoxStyle.Information)
                    txtCountry_MAN.Focus()
                Else
                    MsgBox(ex.Message & vbCrLf & ex.StackTrace, MsgBoxStyle.Information)
                End If
            Catch ex As Exception
                MsgBox(ex.Message & vbCrLf & ex.StackTrace, MsgBoxStyle.Information)
            Finally
            End Try
        End If
    End Sub

    Private Sub btnCoGrid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCoGrid.Click
        strSql = " SELECT COUNTRYID,COUNTRYNAME FROM " & cnAdminDb & "..COUNTRYMAST ORDER BY COUNTRYNAME"
        Dim dtCountry As New DataTable
        da = New OleDbDataAdapter(strSql, cn)
        da.Fill(dtCountry)
        gridviewCountry.DataSource = dtCountry
        If gridviewCountry.ColumnCount > 0 Then
            gridviewCountry.Columns("COUNTRYNAME").Width = 350
            gridviewCountry.Columns("COUNTRYID").Visible = False
            gridviewCountry.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            gridviewCountry.ColumnHeadersDefaultCellStyle.Font = New Font("VERDANA", 9, FontStyle.Bold)
            gridviewCountry.Focus()
        End If
    End Sub

    Private Sub btnCoExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCoExit.Click
        Me.Close()
    End Sub

    Private Sub btnCoDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCoDelete.Click
        strSql = " DELETE FROM " & cnAdminDb & "..COUNTRYMAST"
        strSql += " WHERE COUNTRYID = '" & txtCountryId_MAN.Text & "'"
        cmd = New OleDbCommand(strSql, cn) : cmd.CommandTimeout = 1000
        If MessageBox.Show("Do you want Delete this Item", "Delete Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.OK Then
            If ExecQuery(SyncMode.Master, strSql, cn) Then
                MsgBox("Successfully Deleted..")
                btnCoNew_Click(Me, New EventArgs())
            End If
        End If
    End Sub

    Private Sub gridviewCountry_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles gridviewCountry.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            txtCountryId_MAN.Text = gridviewCountry.CurrentRow.Cells("COUNTRYID").Value.ToString
            txtCountry_MAN.Text = gridviewCountry.CurrentRow.Cells("COUNTRYNAME").Value.ToString
            UpdateCountryId = txtCountryId_MAN.Text
            UpdateCountry = txtCountry_MAN.Text
            UpdateFlag = True
            txtCountry_MAN.Select()
        End If
    End Sub
End Class