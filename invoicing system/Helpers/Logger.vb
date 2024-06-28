Imports System.IO
Public Class Logger
    Private Shared ReadOnly logFilePath As String = HttpContext.Current.Server.MapPath("~/App_Data/ErrorLog.txt")

    Public Shared Sub LogError(message As String)
        Try
            Using writer As New StreamWriter(logFilePath, True)
                writer.WriteLine($"[{DateTime.Now}] Error: {message}")
            End Using
        Catch ex As Exception
            ' Handle or log the exception that occurred while logging itself
            ' For simplicity, here we just write to the console
            Console.WriteLine($"Failed to write to log file: {ex.Message}")
        End Try
    End Sub
End Class
