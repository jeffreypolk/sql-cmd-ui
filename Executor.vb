Public Class Executor
    Property Server As String
    Property Database As String
    Property InputFile As String
    Property OutputWriter As PS.ExeBase.Output.IOutputWriter

    Private Shared _SQLCMDPath As String

    Public Shared Property SQLCMDPath As String
        Get
            If String.IsNullOrEmpty(_SQLCMDPath) Then
                SetSQLCmdPath()
            End If
            Return _SQLCMDPath
        End Get
        Set(value As String)
            _SQLCMDPath = value
        End Set
    End Property

    Public Sub Execute()

        OutputWriter.WriteFormatLine("Using {0}", SQLCMDPath)
        Dim Proc As New PS.ExeBase.ProcessRunner
        With Proc
            .FileName = SQLCMDPath
            .Arguments = String.Format("-S ""{0}"" -d ""{1}"" -i ""{2}"" -b", Server, Database, InputFile)
            .OutputWriter = OutputWriter
            .Run()
        End With
    End Sub

    Private Shared Sub SetSQLCmdPath()
        Dim Paths As New List(Of String)
        Paths.Add("C:\Program Files\Microsoft SQL Server\110\Tools\Binn\SQLCMD.exe")
        Paths.Add("C:\Program Files\Microsoft SQL Server\100\Tools\Binn\SQLCMD.exe")

        For Each p In Paths
            If IO.File.Exists(p) Then
                SQLCMDPath = p
                Exit Sub
            End If
        Next

        'check the PATH variable
        For Each p In Environment.GetEnvironmentVariable("PATH").Split(";")
            If IO.File.Exists(String.Concat(p, "SQLCMD.exe")) Then
                SQLCMDPath = String.Concat(p, "SQLCMD.exe")
                Exit Sub
            End If
        Next

    End Sub
End Class
