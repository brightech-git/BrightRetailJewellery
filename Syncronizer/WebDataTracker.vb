Imports System.Data.OleDb
Public Class WebDataTracker
    Private _Admindb As String
    Private _CnWeb As OleDbConnection
    Private _Cn As OleDbConnection
    Private StrSql As String
    Private _WebDbTblPrefix As String
    Private R_CostId As String
    Dim UpdRow As DataRow = Nothing
    Public Sub New(ByVal _Cn As OleDbConnection, ByVal _CnWeb As OleDbConnection, ByVal _WebDbTblPrefix As String, ByVal R_CostId As String, ByVal _Admindb As String)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me._Cn = _Cn
        Me._CnWeb = _CnWeb
        Me._WebDbTblPrefix = _WebDbTblPrefix
        Me.R_CostId = R_CostId
        Me._Admindb = _Admindb
    End Sub
    Private Sub btnExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        txtUid.Enabled = True
        txtUid.Clear()
        DgvData.DataSource = Nothing
        Me.Refresh()
        txtUid.Select()
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.Click
        btnUpdate.Enabled = False
        UpdRow = Nothing
        Dim _Da As OleDbDataAdapter
        Dim R_DtReceiveInfo As New DataTable
        StrSql = " SELECT TOP 1 * FROM " & _WebDbTblPrefix & "_SYNCTABLE WHERE TOID = '" & R_CostId & "' "
        If Val(txtUid.Text) <> 0 Then
            StrSql += " AND UID = " & Val(txtUid.Text) & ""
        End If
        StrSql += " ORDER BY FROMID,UID"
        R_DtReceiveInfo = New DataTable
        _Da = New OleDbDataAdapter(StrSql, _CnWeb)
        _Da.Fill(R_DtReceiveInfo)
        If Not R_DtReceiveInfo.Rows.Count > 0 Then
            MsgBox("Record not Found", MsgBoxStyle.Information)
            Exit Sub
        End If
        UpdRow = R_DtReceiveInfo.Rows(0)

        Dim ZipFilePath As String = Nothing
        Dim XmlFilePath As String = Nothing
        Dim myByte() As Byte = UpdRow.Item("CONTENT")
        ZipFilePath = IO.Path.GetTempPath & UpdRow.Item("UPDFILE").ToString & ".ZIP"
        XmlFilePath = IO.Path.GetTempPath & UpdRow.Item("UPDFILE").ToString & ".XML"
        If IO.File.Exists(ZipFilePath) Then IO.File.Delete(ZipFilePath)
        If IO.File.Exists(XmlFilePath) Then IO.File.Delete(XmlFilePath)

        Dim fileStr As New IO.FileStream(ZipFilePath, IO.FileMode.Create, IO.FileAccess.ReadWrite)
        fileStr.Write(myByte, 0, myByte.Length)
        fileStr.Close()
        Dim objZip As New Zipper
        If Not objZip.UnZip(ZipFilePath, IO.Path.GetTempPath) Then
            If IO.File.Exists(ZipFilePath) Then IO.File.Delete(ZipFilePath)
        Else
            If IO.File.Exists(ZipFilePath) Then IO.File.Delete(ZipFilePath)
        End If
        Dim DsReceive As New DataSet
        DsReceive.ReadXml(XmlFilePath, XmlReadMode.ReadSchema)
        DsReceive.AcceptChanges()
        DgvData.DataSource = DsReceive.Tables(0)
        For CNT As Integer = 0 To DgvData.ColumnCount - 1
            DgvData.Columns(CNT).ReadOnly = True
            DgvData.Columns(CNT).SortMode = DataGridViewColumnSortMode.NotSortable
            DgvData.Columns(CNT).Visible = False
        Next
        With DgvData
            .Columns("FROMID").Width = 60
            .Columns("TOID").Width = 50
            .Columns("SQLTEXT").Width = 482
            .Columns("UID").Width = 60
            .Columns("IMAGE_CTRLID").Width = 50
            .Columns("TAGIMAGENAME").Width = 70
            .Columns("TAGIMAGE").Width = 70

            .Columns("FROMID").HeaderText = "FROM ID"
            .Columns("TOID").HeaderText = "TO ID"
            .Columns("SQLTEXT").HeaderText = "SQLTEXT"
            .Columns("UID").HeaderText = "UID @" & UpdRow.Item("TOID").ToString
            .Columns("IMAGE_CTRLID").HeaderText = "IMAGE CTRLID"
            .Columns("TAGIMAGENAME").HeaderText = "TAGIMG NAME"

            .Columns("SQLTEXT").ReadOnly = False
            .Columns("IMAGE_CTRLID").ReadOnly = False

            .Columns("FROMID").Visible = True
            .Columns("TOID").Visible = True
            .Columns("SQLTEXT").Visible = True
            .Columns("UID").Visible = True
            .Columns("IMAGE_CTRLID").Visible = True
            .Columns("TAGIMAGENAME").Visible = True
            .Columns("TAGIMAGE").Visible = True
        End With
        btnUpdate.Enabled = True
        DgvData.Select()
    End Sub

    Private Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Dim S_Dt As New DataTable
        S_Dt = CType(DgvData.DataSource, DataTable).Copy
        S_Dt.AcceptChanges()

        Dim LockCounter As Integer = 0
        Dim _Tran As OleDbTransaction = Nothing
        Dim _TranWeb As OleDbTransaction = Nothing
        Dim S_UpdFile As String
        Dim _Cmd As OleDbCommand

        _Tran = _Cn.BeginTransaction
GET_SENDLOCK:
        If LockCounter >= 20000 Then
            Throw New Exception("DeadLock!!!  Getting SYNC-LOCK-SEND value")
        End If
        StrSql = "UPDATE " & _Admindb & "..SOFTCONTROL SET CTLTEXT = 'Y'"
        StrSql += " WHERE CTLID = 'SYNC-LOCK-SEND' AND CTLTEXT = ''"
        _Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        If _Cmd.ExecuteNonQuery() = 0 Then
            LockCounter += 1
            GoTo GET_SENDLOCK
        End If
RecordFound:
        _TranWeb = Nothing
        _TranWeb = _CnWeb.BeginTransaction
        S_UpdFile = UpdRow.Item("UPDFILE").ToString

        Dim DsSend As New DataSet
        Dim XmlFilePath As String = Nothing
        Dim ZipFilePath As String = Nothing
        XmlFilePath = IO.Path.GetTempPath & S_UpdFile & ".xml"
        ZipFilePath = IO.Path.GetTempPath & S_UpdFile & ".zip"
        DsSend.Tables.Add(S_Dt)
        DsSend.WriteXml(XmlFilePath, XmlWriteMode.WriteSchema)
        Dim objZip As New Zipper
        If Not objZip.Zip(XmlFilePath, ZipFilePath) Then
            If IO.File.Exists(XmlFilePath) Then IO.File.Delete(XmlFilePath)
            If IO.File.Exists(ZipFilePath) Then IO.File.Delete(ZipFilePath)
        Else
            IO.File.Delete(XmlFilePath)
        End If
        Dim fileStr As New IO.FileStream(ZipFilePath, IO.FileMode.Open, IO.FileAccess.Read)
        Dim reader As New IO.BinaryReader(fileStr)
        Dim result As Byte() = reader.ReadBytes(CType(fileStr.Length, Integer))
        fileStr.Read(result, 0, result.Length)
        fileStr.Close()

        StrSql = " UPDATE " & _WebDbTblPrefix & "_SYNCTABLE SET CONTENT = ? WHERE UID = " & Val(UpdRow.Item("UID").ToString) & ""
        _Cmd = New OleDbCommand(StrSql, _CnWeb, _TranWeb)
        _Cmd.Parameters.AddWithValue("@CONTENT", result)
        _Cmd.ExecuteNonQuery()

        'StrSql = " DELETE FROM " & _WebDbTblPrefix & "_SYNCTABLE"
        'StrSql += " WHERE UID = " & Val(UpdRow.Item("UID").ToString) & ""
        '_Cmd = New OleDbCommand(StrSql, _CnWeb, _TranWeb)
        '_Cmd.ExecuteNonQuery()
        'Dim fiINfo As New IO.FileInfo(ZipFilePath)
        'StrSql = " INSERT INTO " & _WebDbTblPrefix & "_SYNCTABLE"
        'StrSql += vbCrLf + " (FROMID,TOID"
        'StrSql += vbCrLf + " ,CONTENT,UPDFILE"
        'StrSql += vbCrLf + "  )"
        'StrSql += vbCrLf + "  VALUES"
        'StrSql += vbCrLf + "  (?,?,?,?)"
        'Dim CmdIns As New OleDbCommand(StrSql, _CnWeb, _TranWeb)
        'Dim fileStr As New IO.FileStream(ZipFilePath, IO.FileMode.Open, IO.FileAccess.Read)
        'Dim reader As New IO.BinaryReader(fileStr)
        'Dim result As Byte() = reader.ReadBytes(CType(fileStr.Length, Integer))
        'fileStr.Read(result, 0, result.Length)
        'fileStr.Close()

        'CmdIns.Parameters.AddWithValue("@FROMID", R_CostId)
        'CmdIns.Parameters.AddWithValue("@TOID", UpdRow.Item("TOID").ToString)
        'CmdIns.Parameters.AddWithValue("@CONTENT", result)
        'CmdIns.Parameters.AddWithValue("@UPDFILE", S_UpdFile)
        'CmdIns.ExecuteNonQuery()

        StrSql = "UPDATE " & _Admindb & "..SOFTCONTROL SET CTLTEXT = ''"
        StrSql += " WHERE CTLID = 'SYNC-LOCK-SEND'"
        _Cmd = New OleDbCommand(StrSql, _Cn, _Tran)
        _Cmd.ExecuteNonQuery()
        _Tran.Commit()
        _TranWeb.Commit()
        _Tran = Nothing
        _TranWeb = Nothing
        MsgBox("Updated..")
        Environment.Exit(0)
    End Sub
End Class