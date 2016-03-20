Public Class MainForm

    Private _ExecuteTimer As Stopwatch

    Private Sub MainForm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        PS.ExeBase.Util.InitializeFonts(Me)
    End Sub

    Private Sub btnBrowse_Click(sender As System.Object, e As System.EventArgs) Handles btnBrowse.Click
        With OpenScriptDialog
            .FileName = txtInput.Text
            If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                txtInput.Text = .FileName
            End If
        End With
    End Sub

    Private Sub btnExecute_Click(sender As System.Object, e As System.EventArgs) Handles btnExecute.Click

        'test for sqlcmd
        If String.IsNullOrEmpty(Executor.SQLCMDPath) Then
            PS.ExeBase.Util.ShowMessage("SQLCMD.exe could not be found in the usual locations")
            With OpenSQLCMDDialog
                If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                    Executor.SQLCMDPath = .FileName
                Else
                    Exit Sub
                End If
            End With
        End If

        txtOutput.Clear()
        txtOutput.AppendText(String.Format("Start execution {0}.{1}: {2}{3}", txtServer.Text, txtDatabase.Text, txtInput.Text, vbCrLf))
        btnExecute.Enabled = False
        Cursor = Cursors.WaitCursor
        BackgroundWorker1.RunWorkerAsync()

    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As System.Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Dim Output As New PS.ExeBase.Output.RichTextOutputWriter(txtOutput)
        Dim Exec As New Executor
        With Exec
            .Server = txtServer.Text
            .Database = txtDatabase.Text
            .InputFile = txtInput.Text
            .OutputWriter = Output
            _ExecuteTimer = Stopwatch.StartNew()
            .Execute()
        End With
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        _ExecuteTimer.Stop()
        txtOutput.AppendText(String.Format("Stop execution: {0} ms{1}", _ExecuteTimer.ElapsedMilliseconds, vbCrLf))
        Cursor = Cursors.Default
        btnExecute.Enabled = True
    End Sub

    Private Sub txtCopyOutput_Click(sender As System.Object, e As System.EventArgs) Handles txtCopyOutput.Click
        Clipboard.SetText(txtOutput.Text)
    End Sub

End Class
