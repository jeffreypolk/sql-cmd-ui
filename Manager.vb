Imports PS.ExeBase

Public Class Manager
    Implements PS.ExeBase.IManager

    Public Sub ExecuteCommandLine() Implements IManager.ExecuteCommandLine
        If Args.ArgExists("dostuff") Then
            'execute the "dostuff" operation
        Else
            'unsupported arguments
            Throw New NotImplementedException()
        End If
    End Sub

    Public Function GetStartupForm() As Form Implements IManager.GetStartupForm
        'return a reference to the startup form
        Return New MainForm
    End Function
End Class
