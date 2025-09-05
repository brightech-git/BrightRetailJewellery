Imports java.util.zip
Imports java.io

Public Class Zipper
    Public Function UnZip(ByVal strZipFileName As String, ByVal strLocation As String) As Boolean
        Try
            Dim FileInputStream As New java.io.FileInputStream(strZipFileName)
            Dim ZipInputStream As New java.util.zip.ZipInputStream(FileInputStream)
            Dim bTrue As Boolean = True
            Dim sbBuf(1024) As SByte

            While 1 = 1
                Dim ZipEntry As ZipEntry = ZipInputStream.getNextEntry()

                If ZipEntry Is Nothing Then Exit While

                If ZipEntry.isDirectory Then
                    If Not My.Computer.FileSystem.DirectoryExists(strLocation & ZipEntry.getName) Then
                        My.Computer.FileSystem.CreateDirectory(strLocation & ZipEntry.getName)
                    End If
                Else
                    Dim desFile As String = strLocation.Replace("\", "/")
                    If Mid(desFile, desFile.Length - 1).ToString <> "/" Then
                        desFile = desFile & "/"
                    End If
                    Dim FileOutputStream As New java.io.FileOutputStream(desFile & ZipEntry.getName())

                    While 1 = 1
                        Dim iLen As Integer = ZipInputStream.read(sbBuf)
                        If iLen < 0 Then Exit While
                        FileOutputStream.write(sbBuf, 0, iLen)
                    End While
                    FileOutputStream.close()
                End If
            End While
            ZipInputStream.close()
            FileInputStream.close()
            Return True
        Catch ex As Exception
            Return False
        End Try
        Return False
    End Function

    Public Function Zip(ByVal strFileName As String, ByVal strZipFileName As String) As Boolean
        Try
            Dim oFileOutputStream As New java.io.FileOutputStream(strZipFileName)
            Dim oZipOutputStream As New java.util.zip.ZipOutputStream(oFileOutputStream)

            'For Each strFileName As String In strFileNames
            Dim oFileInputStream As New java.io.FileInputStream(strFileName)
            'Dim oZipEntry As New java.util.zip.ZipEntry(strFileNames.Substring(3).Replace("\", "/"))
            Dim oZipEntry As New java.util.zip.ZipEntry(strFileName.Substring(CInt(Math.Round(CDbl((Conversion.Val(strFileName.LastIndexOf("\")) + 1)))), CInt(Math.Round(CDbl(((strFileName.Length - 1) - Conversion.Val(strFileName.LastIndexOf("\"))))))))

            oZipOutputStream.putNextEntry(oZipEntry)
            Dim sbBuf(1024) As SByte
            While 1 = 1
                Dim iLen As Integer = oFileInputStream.read(sbBuf)
                If iLen < 0 Then Exit While
                oZipOutputStream.write(sbBuf, 0, iLen)
            End While
            oZipOutputStream.closeEntry()
            oFileInputStream.close()
            'Next

            oZipOutputStream.close()
            oFileOutputStream.close()
            Return True
        Catch ex As Exception
            Return False
        End Try
        Return False
    End Function

End Class
