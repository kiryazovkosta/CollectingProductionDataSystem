Imports Uniformance.PHD

Namespace PHDDataLayer

    'Public Enum ConnectionType As Integer
    '    SDK_150
    '    SDK_200
    '    SDK_200_SRV
    'End Enum

    Public Class PhdConnection

        Private _hostName As String = "localhost"
        Private _userName As String = ""
        Private _password As String = ""

        Private _ntuserName As String = ""
        Private _ntpassword As String = ""

        Private _settings As PHDSettings = Nothing

        Private _port As Integer = 3100
        ' Private _startDate As String = "Now-1h";
        ' Private _endDate As String = "Now";
        ' private _sampleFrequency As UInteger = 300;
        Private _sampleType As Uniformance.PHD.SAMPLETYPE = Uniformance.PHD.SAMPLETYPE.Snapshot
        Private _accessMethod As Uniformance.PHD.SERVERVERSION = Uniformance.PHD.SERVERVERSION.API200

        Public oPhd As PHDHistorian = New PHDHistorian()

        Public Property HostName() As String
            Get
                Return _hostName
            End Get
            Set(ByVal value As String)
                _hostName = value
            End Set
        End Property

        Public Property NTUserName() As String
            Get
                Return _ntuserName
            End Get
            Set(ByVal value As String)
                _ntuserName = value
            End Set
        End Property

        Public Property NTPassword() As String
            Get
                Return _ntpassword
            End Get
            Set(ByVal value As String)
                _ntpassword = value
            End Set
        End Property

        Public Property UserName() As String
            Get
                Return _userName
            End Get
            Set(ByVal value As String)
                _userName = value
            End Set
        End Property

        Public Property Password() As String
            Get
                Return _password
            End Get
            Set(ByVal value As String)
                _password = value
            End Set
        End Property

        Public Property AccessMethod() As Uniformance.PHD.SERVERVERSION
            Get
                Return _accessMethod
            End Get
            Set(ByVal value As Uniformance.PHD.SERVERVERSION)
                _accessMethod = value
            End Set
        End Property

        Public Property Settings() As PHDSettings
            Get
                Return _settings
            End Get
            Set(ByVal value As PHDSettings)
                _settings = value
            End Set
        End Property

        Public Property Port() As Integer
            Get
                Return _port
            End Get
            Set(ByVal value As Integer)
                _port = value
            End Set
        End Property

        Public Function Open() As PHDHistorian
            Dim DefaultServer As PHDServer = New PHDServer(_hostName)
            DefaultServer.APIVersion = AccessMethod
            DefaultServer.Port = Port

            If _ntuserName.Length > 0 Then
                DefaultServer.WindowsUsername = _ntuserName
                DefaultServer.WindowsPassword = _ntpassword
            End If


            If _userName.Length > 0 Then
                DefaultServer.UserName = _userName
                DefaultServer.Password = _password
            End If

            oPhd.DefaultServer = DefaultServer
            Return oPhd
        End Function

        Public Sub Close()
            oPhd.Dispose()
        End Sub
    End Class
End Namespace
