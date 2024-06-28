Imports System.IO
Imports System.Text
Imports System.Linq
Imports System.Globalization

Public Class CsvHelper
    Public Shared Function ReadCsv(Of T)(filePath As String, createFunc As Func(Of String(), T)) As List(Of T)
        Dim results As New List(Of T)
        Using reader As New StreamReader(filePath)
            While Not reader.EndOfStream
                Dim line = reader.ReadLine()
                Dim values = line.Split(","c)
                results.Add(createFunc(values))
            End While
        End Using
        Return results
    End Function

    Public Shared Sub WriteCsv(Of T)(filePath As String, data As List(Of T), selector As Func(Of T, String()))
        Using writer As New StreamWriter(filePath, False, Encoding.UTF8)
            For Each item In data
                Dim line = String.Join(",", selector(item))
                writer.WriteLine(line)
            Next
        End Using
    End Sub


End Class
