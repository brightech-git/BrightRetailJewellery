Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO

Public Class frmSignatureDia

    Private mousedown As Boolean = False
    Dim objAddressDia As New frmAddressDia
    Private thispath As New System.Drawing.Drawing2D.GraphicsPath
    Private pathlist As New List(Of Drawing2D.GraphicsPath)
    Public Cn1 As New SqlConnection
    Public cmd1 As New SqlCommand
    Public dt As New DataTable
    Public Strsql As String
    Public SIGNPATH As String
    Public sno As String
    Public tempimagepath As String



    Private Sub PictureBox1_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles PictureBox1.MouseDown
        mousedown = True
        Dim gpath As New Drawing2D.GraphicsPath
        gpath.AddLine(New Point(e.X, e.Y), New Point(e.X, e.Y))
        pathlist.Add(gpath)
    End Sub

    Private Sub PictureBox1_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs) Handles PictureBox1.MouseMove
        If mousedown Then
            Dim apoint As PointF = New PointF(e.X, e.Y)
            Dim lastpoint As PointF = New PointF(e.X, e.Y)
            pathlist(pathlist.Count - 1).AddLine(lastpoint, apoint)
            PictureBox1.Refresh()
        End If
    End Sub

    Private Sub PictureBox1_MouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles PictureBox1.MouseUp
        mousedown = False
    End Sub

    Private Sub PictureBox1_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles PictureBox1.Paint
        e.Graphics.Clear(Color.White)
        For Each thispath In pathlist
            e.Graphics.DrawPath(New Pen(Color.Black, 4), thispath)
        Next
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        pathlist.Clear()
        PictureBox1.Refresh()
        PictureBox2.Visible = False
        PictureBox1.Visible = True
    End Sub

    Public Shared Function GetPersonalInfoSno(ByVal tran1 As OleDbTransaction) As String
GETNSNO:
        Dim tSno As Integer = 0
        Dim strSql As String
        Dim cmd As OleDbCommand
        strSql = " SELECT CTLTEXT AS TAGSNO FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'PERSONALINFOCODE'"
        tSno = Val(objGPack.GetSqlValue(strSql, , , tran1))
        ''UPDATING 
        ''TAGNO INTO SOFTCONTROL
  
        strSql = " SELECT SNO FROM " & cnAdminDb & "..PERSONALINFO WHERE SNO = '" & GetCostId(cnCostId) & (tSno + 1).ToString & "'"
        If objGPack.GetSqlValue(strSql, , "-1", tran1) <> "-1" Then
            GoTo GETNSNO
        End If
        Return GetCostId(cnCostId) & (tSno + 1).ToString
    End Function

    Private Sub Btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_save.Click
        

    End Sub
  
    Private Function piccap()

        Strsql = " SELECT CTLTEXT FROM " & cnAdminDb & "..SOFTCONTROL WHERE CTLID = 'SIGNPATH'"
        SIGNPATH = UCase(objGPack.GetSqlValue(Strsql, "CTLTEXT", , ))
        If Not SIGNPATH.EndsWith("\") And SIGNPATH <> "" Then SIGNPATH += "\"
        SIGNPATH += sno & ".Jpeg"

        Dim data As IDataObject
        Dim bmap As Image
        tempimagepath = Application.StartupPath & "\tst.jpg"

        If File.Exists(tempimagepath) Then File.Delete(tempimagepath)
        data = Clipboard.GetDataObject()
        If SIGNPATH <> "" Then
            bmap = CType(data.GetData(GetType(System.Drawing.Bitmap)), Image)
            PictureBox2.Image.Save(tempimagepath, System.Drawing.Imaging.ImageFormat.Jpeg)
            If System.IO.File.Exists(tempimagepath) = True Then
                Image.FromFile(Application.StartupPath & "\tst.jpg").Dispose()
            End If
        End If
    End Function
 
   
    Function Getdata(ByVal qry As String, ByVal cn As OleDbConnection) As DataTable
        dt = New DataTable
        Dim dtt As New DataTable
        da = New OleDbDataAdapter(qry, cn)
        da.Fill(dtt)
        Return dtt
    End Function
  
    Private Sub Form3_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        
        
        'Getconstrings()
        'Getconoledb()
        'GetSQLcon()
        'Getmemfile()
        'Strsql = "SELECT DBNAME FROM " & cnadmindb & "..DBMASTER ORDER BY STARTDATE DESC"
        'dt = Getdata(Strsql, Cn)
        'da.Fill(dt)
        'If dt.Rows.Count > 0 Then
        '    cnstockdb = dt.Rows(0).Item(0)
        'End If
        'If Dbtype = "S" Then
        '    Strsql = "PROVIDER= SQLOLEDB.1;Persist Security Info=False; Initial Catalog=" & cnstockdb & ";Data Source=" & servername & ";user id=" & Dbuser & ";password=" & Dbpwd & ";"
        'Else
        '    Strsql = "Provider= SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=" & cnstockdb & ";Data Source=" & servername & ""
        'End If
        'Cnn = New OleDbConnection(Strsql)
        'If Not Cnn.State = ConnectionState.Open Then Cnn.Open()
        PictureBox2.Visible = False
        Button1_Click(Me, New EventArgs)
    End Sub
    'Public Function Savesignature()

    '    Dim strsql As String
    '    Dim PID As String

    '    Strsql = "select SNO from " & cnadmindb & "..personalinfo where sno in (select psno from " & cnadmindb & "..customerinfo where batchno='" & bbatchno & "')"
    '    Dim dtt As New DataTable
    '    dtt = Getdata(strsql, cn)
    '    If dtt.Rows.Count > 0 Then
    '        PID = dtt.Rows(0).Item("SNO").ToString
    '    End If

    '    'Dim cmd As New SqlCommand("INSERT INTO " & cnadmindb & "..IMG VALUES(@photo,@name)", Cn1)
    '    Dim cmd As New SqlCommand("UPDATE " & cnadmindb & "..personalinfo SET PIMAGE=@photo where SNO='" & PID & "'", Cn1)
    '    'cmd.Parameters.AddWithValue("@name", TextBox1.Text)
    '    Dim ms As New MemoryStream()
    '    PictureBox2.BackgroundImage.Save(ms, PictureBox2.BackgroundImage.RawFormat)
    '    Dim data As Byte() = ms.GetBuffer()
    '    Dim p As New SqlParameter("@photo", SqlDbType.Image)
    '    p.Value = data
    '    cmd.Parameters.Add(p)
    '    cmd.ExecuteNonQuery()
    '    MessageBox.Show("Signature has been saved", "Save", MessageBoxButtons.OK)


    '    '*************OLD*************************    
    '    '*************OLD*************************
    '    'Dim ms = New MemoryStream()
    '    'PictureBox2.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp)
    '    'Dim data As Byte() = ms.GetBuffer()
    '    'Strsql = "update " & cnadmindb & "..personalinfo set pimage='" & data & "' where SNO='" & PID & "'"
    '    'Cmd = New OleDbCommand(Strsql, Cn)
    '    'Cmd.Parameters.Add(New OleDbParameter("@photo", SqlDbType.Image))
    '    'Cmd.Parameters("@photo").Value = data
    '    'Cmd.ExecuteNonQuery()
    'End Function
    'Public Function Getconstrings()
    '    Dim file As New FileStream(Application.StartupPath + "\coninfo.ini", FileMode.Open)
    '    Dim fstream As New StreamReader(file)
    '    fstream.BaseStream.Seek(0, SeekOrigin.Begin)
    '    Dim temp As String, comcode As String
    '    comcode = Mid(fstream.ReadLine, 21)
    '    temp = Mid(fstream.ReadLine, 21)
    '    cnadmindb = comcode + "ADMINDB"
    '    servername = Mid(fstream.ReadLine, 21)
    '    fstream.ReadLine()
    '    Dbpwd = Mid(fstream.ReadLine, 21)
    '    Dim strDecryptPwd As String = ""
    '    Dim Pwd As String
    '    Dim cnt As Integer
    '    Dim IntAscii As Integer
    '    Pwd = Dbpwd
    '    If Len(Pwd) = 0 Then
    '        Dbpwd = ""
    '    Else
    '        For cnt = 1 To Len(Pwd)
    '            IntAscii = 0
    '            IntAscii = (Asc(Strings.Mid(Pwd, cnt, 1)) - (cnt * 2) - 14)
    '            strDecryptPwd = strDecryptPwd & Chr(IntAscii)
    '        Next
    '        Dbpwd = strDecryptPwd
    '    End If
    '    Dbtype = Mid(fstream.ReadLine, 21)
    '    Dbuser = Mid(fstream.ReadLine, 21)
    '    If Dbuser = "" Then Dbuser = "sa"
    '    fstream.Close()
    'End Function
    'Public Function Getmemfile(Optional ByVal dbname As String = Nothing) As OleDb.OleDbConnection
    '    Dim file As New FileStream(Application.StartupPath + "\Billprint.mem", FileMode.Open)
    '    Dim fstream As New StreamReader(file)
    '    fstream.BaseStream.Seek(0, SeekOrigin.Begin)
    '    btype = Mid(fstream.ReadLine, 17)
    '    bbatchno = Mid(fstream.ReadLine, 17)
    '    btrandate = Mid(fstream.ReadLine, 17)
    '    bduplicate = Mid(fstream.ReadLine, 17)
    '    fstream.ReadLine()
    '    fstream.Close()
    'End Function
    'Public Function GetSQLcon(Optional ByVal dbname As String = Nothing) As SqlConnection
    '    Try
    '        If Dbtype = "S" Then
    '            Strsql = "PROVIDER= SQLOLEDB.1;Persist Security Info=False; Initial Catalog=" & dbname & ";Data Source=" & servername & ";user id=" & Dbuser & ";password=" & Dbpwd & ";"
    '        Else
    '            dbname = cnadmindb
    '            'Strsql = "PROVIDER= SQLEXPRESS;Persist Security Info=False; Initial Catalog=" & dbname & ";Data Source=" & servername & ";user id=" & Dbuser & ";password=" & Dbpwd & ";"
    '            Strsql = "Data Source=" & servername & ";Initial Catalog=" & dbname & ";User ID=SA"
    '            'Strsql = "PROVIDER= SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=" & dbname & ";Data Source=" & servername & ""
    '        End If
    '        Cn1 = New SqlConnection(Strsql)
    '        If Not Cn1.State = ConnectionState.Open Then Cn1.Open()
    '        'Strsql = "SELECT DBNAME, STARTDATE,ENDDATE FROM " & cnadmindb & "..DBMASTER WHERE DBTYPE='T' ORDER BY STARTDATE DESC"
    '        'Dim dtt As New DataTable
    '        'dtt = Getdata(Strsql, Cn)
    '        'If dtt.Rows.Count > 0 Then
    '        '    cnstockdb = dtt.Rows(0).Item("DBNAME").ToString
    '        'End If

    '        Return Cn1
    '    Catch ex As Exception
    '        MsgBox(ex.Message & vbCrLf & ex.StackTrace)
    '    End Try
    'End Function

    'Public Function Getconoledb(Optional ByVal dbname As String = Nothing) As OleDb.OleDbConnection
    '    Try
    '        If Dbtype = "S" Then
    '            Strsql = "PROVIDER= SQLOLEDB.1;Persist Security Info=False; Initial Catalog=" & dbname & ";Data Source=" & servername & ";user id=" & Dbuser & ";password=" & Dbpwd & ";"
    '        Else
    '            dbname = cnadmindb
    '            Strsql = "PROVIDER= SQLOLEDB.1;Persist Security Info=False; Initial Catalog=" & dbname & ";Data Source=" & servername & ";user id=" & Dbuser & ";password=" & Dbpwd & ";"
    '            'Strsql = "PROVIDER= SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=" & dbname & ";Data Source=" & servername & ""
    '        End If
    '        Cn = New OleDbConnection(Strsql)
    '        If Not Cn.State = ConnectionState.Open Then Cn.Open()
    '        Strsql = "SELECT DBNAME, STARTDATE,ENDDATE FROM " & cnadmindb & "..DBMASTER WHERE DBTYPE='T' ORDER BY STARTDATE DESC"
    '        Dim dtt As New DataTable
    '        dtt = Getdata(Strsql, Cn)
    '        If dtt.Rows.Count > 0 Then
    '            cnstockdb = dtt.Rows(0).Item("DBNAME").ToString
    '        End If

    '        Return Cn
    '    Catch ex As Exception
    '        MsgBox(ex.Message & vbCrLf & ex.StackTrace)
    '    End Try
    'End Function

    Private Sub Btn_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btn_Close.Click
        PictureBox1.Visible = False
        PictureBox2.Visible = True

        Dim FileFromCopy As String
        Dim FileToCopy As String


        tran = Nothing
        tran = cn.BeginTransaction
        sno = GetPersonalInfoSno(tran)
        tran.Commit()
        tran = Nothing


        FileFromCopy = Application.StartupPath & "\tst.jpg"
        FileToCopy = Application.StartupPath & "\Newtst.jpg"


        Dim bmp As New Drawing.Bitmap(PictureBox1.Width, PictureBox1.Height)
        PictureBox1.DrawToBitmap(bmp, PictureBox1.ClientRectangle)

        Dim bmp1 As New Drawing.Bitmap(PictureBox1.Width, PictureBox1.Height)
        PictureBox1.DrawToBitmap(bmp1, PictureBox1.ClientRectangle)

        If System.IO.File.Exists(FileFromCopy) = True Then
            Image.FromFile(Application.StartupPath & "\tst.jpg").Dispose()
        End If

        bmp.Save(FileFromCopy, System.Drawing.Imaging.ImageFormat.Bmp)
        bmp.Dispose()

        If System.IO.File.Exists(FileToCopy) = False Then
            System.IO.File.Copy(FileFromCopy, FileToCopy)
        End If

        PictureBox2.Image = bmp1
        PictureBox2.BackgroundImage = Image.FromFile(FileToCopy)
        piccap()
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Form3_LocationChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LocationChanged

    End Sub

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        Button1_Click(Me, New EventArgs)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Btn_Close_Click(Me, New EventArgs)
    End Sub

    Private Sub frmSignatureDia_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
    End Sub


    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        Btn_save_Click(Me, New EventArgs)
    End Sub

    Private Sub frmSignatureDia_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Not PictureBox2.Image Is Nothing Then Btn_Close_Click(Me, New EventArgs)
        End If
    End Sub


End Class