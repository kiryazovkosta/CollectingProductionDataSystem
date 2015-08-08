Public Class TestForm
    Delegate Sub SetTextCallback([text] As String)
    Private svc As New Phd2SqlInventoryService

    Private Sub SetText(ByVal [text] As String)
        ' InvokeRequired required compares the thread ID of the
        ' calling thread to the thread ID of the creating thread.
        ' If these threads are different, it returns true.
        If Me.txtTrace.InvokeRequired Then
            Dim d As New SetTextCallback(AddressOf SetText)
            Me.Invoke(d, New Object() {[text]})
        Else
            Me.txtTrace.AppendText([text])
        End If
    End Sub

    Public Sub Notify(ByVal msg As String)
        Static Dim counter As Integer = 0
        counter += 1
        SetText(String.Format("{0,-12:D6} {1}", counter, msg))
    End Sub


    Private Sub btnTest_Click(sender As System.Object, e As System.EventArgs) Handles btnTest.Click
        If My.Settings.SYNC_TANKS Then
            For index = 0 To 220 Step 20
                'DEBUG_TRACE("Begin")
                Inventory.Instance.Iterate(My.Settings.IGNORE_DST, index, index + 20)
                'DEBUG_TRACE("End")
            Next

        End If
    End Sub

    Private Sub frmTest_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        'Trace.Listeners.Add(New AutomatedReportingSystem.Tracing.NotifyTrace(AddressOf Notify))
        Phd2SqlInventoryService.SetRegionalSettings()
    End Sub

    Private Sub btnClear_Click(sender As System.Object, e As System.EventArgs) Handles btnClear.Click
        txtTrace.Clear()
    End Sub

    Private Sub btnError_Click(sender As System.Object, e As System.EventArgs) Handles btnError.Click
        Try
            'DEBUG_TRACE("Sending test error...")
            'DEBUG_TRACE("Done!")
            'ERROR_TRACE("test")
        Catch ex As Exception
            'ERROR_TRACE(ex)
        End Try
    End Sub

    Private Sub btnRun_Click(sender As System.Object, e As System.EventArgs) Handles btnRun.Click
        svc.CallStart()
    End Sub

    Private Sub btnStop_Click(sender As System.Object, e As System.EventArgs) Handles btnStop.Click
        svc.CallStop()
    End Sub

    Private Sub btnTestCacheInsert_Click(sender As System.Object, e As System.EventArgs)

    End Sub
End Class