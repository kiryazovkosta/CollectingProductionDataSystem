Imports System.Threading
Imports log4net

Public Class Phd2SqlInventoryService
    Public Shared Logger As log4net.ILog

    Public MyTimerTanks As Timer

    Public Shared CloseFired As Boolean = False

    Private Shared ReadOnly lockObjectTanks As New Object

    Public Sub New()
        InitializeComponent()
        Logger = LogManager.GetLogger("CollectingProductionDataSystem.Phd2SqlInventory")
    End Sub

    Public Sub CallStart()
        OnStart(New String() {})
    End Sub

    Public Sub CallStop()
        OnStop()
    End Sub

    Protected Overrides Sub OnStart(ByVal args() As String)
        Logger.Info("Service started!")
        SetRegionalSettings()
        If My.Settings.SYNC_TANKS Then
            MyTimerTanks = New Timer(AddressOf TimerHandlerTanks, Nothing, 0, System.Threading.Timeout.Infinite)
        End If
    End Sub

    Protected Overrides Sub OnStop()
        CloseFired = True
        Dim notifyObjectTanks As WaitHandle = New AutoResetEvent(False)
        If My.Settings.SYNC_TANKS Then
            MyTimerTanks.Change(0, System.Threading.Timeout.Infinite)
            MyTimerTanks.Dispose(notifyObjectTanks)
        Else
            DirectCast(notifyObjectTanks, AutoResetEvent).Set()
        End If

        notifyObjectTanks.WaitOne(Timeout.Infinite)
        Logger.Info("Service stopped!")
    End Sub

    Public Shared Sub SetRegionalSettings()
        If My.Settings.FORCE_REGIONAL_SETTINGS Then
            Logger.Debug(String.Format("Forcing regional settings to {0}!", My.Settings.CULTURE_INFO))
            Dim newCi As System.Globalization.CultureInfo = New System.Globalization.CultureInfo(My.Settings.CULTURE_INFO, False)
            newCi.NumberFormat.CurrencyDecimalSeparator = "."
            newCi.NumberFormat.CurrencyGroupSeparator = String.Empty
            newCi.NumberFormat.NumberDecimalSeparator = "."
            newCi.NumberFormat.NumberGroupSeparator = String.Empty
            newCi.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy"
            newCi.DateTimeFormat.ShortTimePattern = "HH:mm:ss"
            newCi.DateTimeFormat.LongTimePattern = "HH:mm:ss"
            newCi.DateTimeFormat.LongDatePattern = "dd.MM.yyyy"
            System.Threading.Thread.CurrentThread.CurrentCulture = newCi
        End If
    End Sub

    Sub TimerHandlerTanks(ByVal state As Object)
        SyncLock lockObjectTanks
            Try
                If CloseFired Then Exit Sub
                SetRegionalSettings()
                Logger.Info("Begin")
                For index = 0 To 220 Step My.Settings.GET_TANKS_SIZE
                    Inventory.Instance.Iterate(My.Settings.IGNORE_DST, index, index + My.Settings.GET_TANKS_SIZE)
                Next
                Logger.Info("End")
            Catch ex As Exception

            Finally
                If Not CloseFired Then
                    MyTimerTanks.Change(CLng(My.Settings.IDLE_TIMER_TANKS.TotalMilliseconds), System.Threading.Timeout.Infinite)
                End If
            End Try
        End SyncLock
    End Sub


End Class
