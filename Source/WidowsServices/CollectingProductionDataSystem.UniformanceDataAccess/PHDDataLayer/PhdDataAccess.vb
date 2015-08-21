Imports System.Security.Principal
Imports Uniformance.PHD
Imports System.Collections
Imports System.Data

Namespace PHDDataLayer
    ''' <summary>
    ''' Class to manage the Phd connections and populate the Tag Filter Tree 
    ''' </summary>
    Public Class PhdDataAccess

        Private PHD_HOST As String = String.Empty
        Private PHD_PORT As Integer = 3150
        Private PHD_USERNAME As String = String.Empty
        Private PHD_PASSWORD As String = String.Empty
        Private PHD_APIVERSION As String = "RAPI200"
        Private NT_USERNAME As String = String.Empty
        Private NT_PASSWORD As String = String.Empty
        Private MAX_ROWS As Integer = 1000
        Private STRING_NA As String = String.Empty
        Private STRING_BAD_VAL As String = String.Empty
        Private MIN_CONFIDENCE As Integer = 100
        Private ORA_CONN_STR As String = String.Empty
        Private STRING_ERR As String = String.Empty
        Private STRING_CONN_ERR As String = String.Empty
        Private STRING_ORA_ERR_PREFIX As String = String.Empty
        Private _STRING_E_CONNECT_FAIL As String = New PHDErrorException(PHD_ERRORS.E_CONNECT_FAIL).Message
        Private _connectFail As Boolean = False

        Private _htConn As Hashtable = New Hashtable

        Public Enum PHD_ERRORS
            E_CONNECT_FAIL = 10044
        End Enum

        Public ReadOnly Property GetPhdHost() As String
            Get
                Return PHD_HOST
            End Get
        End Property

        Public Property connectFail As Boolean
            Get
                Return _connectFail
            End Get
            Set(value As Boolean)
                _connectFail = value
            End Set
        End Property

        Public ReadOnly Property STRING_E_CONNECT_FAIL As String
            Get
                Return _STRING_E_CONNECT_FAIL
            End Get
        End Property

        Public ReadOnly Property GetNAstring As String
            Get
                Return STRING_NA
            End Get
        End Property

        Public ReadOnly Property GetBadValString As String
            Get
                Return STRING_BAD_VAL
            End Get
        End Property

        Public ReadOnly Property GetMinConfidence() As String
            Get
                Return MIN_CONFIDENCE
            End Get
        End Property

        Public ReadOnly Property GetORA_CONN_STR As String
            Get
                Return ORA_CONN_STR
            End Get
        End Property

        Public ReadOnly Property GetErrString As String
            Get
                Return STRING_ERR
            End Get
        End Property

        Public ReadOnly Property GetConErrString As String
            Get
                Return STRING_CONN_ERR
            End Get
        End Property

        Public ReadOnly Property GetOraErrPrefixString As String
            Get
                Return STRING_ORA_ERR_PREFIX
            End Get
        End Property

        Public Function IsConnectFail(pex As PHDErrorException) As Boolean
            Return IIf(pex.Message = STRING_E_CONNECT_FAIL, True, False)
        End Function

        ''' <summary>
        ''' Open a connection to a Phd Server
        ''' </summary>
        ''' <param name="HostName">Phd Server name</param>
        ''' <param name="UserName">Phd Username</param>
        ''' <param name="Password">Phd Password</param>
        ''' <param name="API">Version of the API to use for connecting to Phd</param>
        ''' <param name="_phdSettings"></param>
        ''' <returns></returns>
        Public Function OpenConection(ByVal HostName As String, ByVal NTUserName As String, ByVal NTPassword As String, ByVal UserName As String, ByVal Password As String, ByVal API As Uniformance.PHD.SERVERVERSION, ByVal _phdSettings As PHDSettings) As PhdConnection
            Dim _conn As PhdConnection = Nothing

            'check the HashTable to see if that connection is available
            If _htConn.ContainsKey(WindowsIdentity.GetCurrent.Name + "_" + HostName + "_" + API.ToString()) = False Then
                _conn = New PhdConnection
                _conn.AccessMethod = API
                _conn.HostName = HostName
                _conn.UserName = UserName
                _conn.Password = Password

                _conn.NTUserName = NTUserName
                _conn.NTPassword = NTPassword

                _conn.Open()

                _htConn.Add(WindowsIdentity.GetCurrent.Name + "_" + HostName + "_" + API.ToString(), _conn)
            Else
                _conn = _htConn(WindowsIdentity.GetCurrent.Name + "_" + HostName + "_" + API.ToString())
            End If

            If Not _phdSettings Is Nothing Then _
                _conn.Settings = _phdSettings

            Return _conn
        End Function

        Public Function OpenConnection(Optional ByVal _phdSettings As PHDSettings = Nothing) As PhdConnection
            Dim _conn As PhdConnection = Nothing

            'check the HashTable to see if that connection is available
            If _htConn.ContainsKey(WindowsIdentity.GetCurrent.Name + "_" + PHD_HOST + "_" + PHD_APIVERSION) = False Then
                _conn = New PhdConnection
                _conn.AccessMethod = System.Enum.Parse(GetType(Uniformance.PHD.SERVERVERSION), PHD_APIVERSION)
                _conn.HostName = PHD_HOST
                _conn.Port = PHD_PORT

                _conn.UserName = PHD_USERNAME
                _conn.Password = PHD_PASSWORD

                _conn.NTUserName = NT_USERNAME
                _conn.NTPassword = NT_PASSWORD

                _conn.Open()

                _htConn.Add(WindowsIdentity.GetCurrent.Name + "_" + PHD_HOST + "_" + PHD_APIVERSION, _conn)
            Else
                _conn = _htConn(WindowsIdentity.GetCurrent.Name + "_" + PHD_HOST + "_" + PHD_APIVERSION)
            End If

            If Not _phdSettings Is Nothing Then _
                _conn.Settings = _phdSettings

            Return _conn
        End Function

        Private Sub CheckForConnection(ByVal _conn As PhdConnection)
            If _htConn.ContainsKey(WindowsIdentity.GetCurrent.Name + "_" + _conn.HostName + "_" + _conn.AccessMethod.ToString()) = False Then _
              _htConn.Add(WindowsIdentity.GetCurrent.Name + "_" + _conn.HostName + "_" + _conn.AccessMethod.ToString(), _conn)
        End Sub

        ' ''' <summary>
        ' ''' 
        ' ''' </summary>
        ' ''' <param name="_conn">Phd connection</param>
        ' ''' <param name="_tagname">Tag Name that was selected</param>
        ' ''' <param name="maxRows">Maximum number if rows to return</param>
        ' ''' <param name="_scanRate">Scan rate or frequency of data collection</param>
        ' ''' <returns></returns>
        'Public Function BrowseData(ByVal _conn As PhdConnection, ByVal _tagname As String, ByVal maxRows As UInteger, ByVal _scanRate As Long) As DataSet
        '    'check the HashTable to see if that connection is available
        '    CheckForConnection(_conn)

        '    Dim _filter As TagFilter = New TagFilter()
        '    _filter.ScanRate = _scanRate
        '    _filter.Tagname = _tagname
        '    Return _conn.oPhd.BrowsingTags(maxRows, _filter)
        'End Function

        'Public Function BrowseData(ByVal _tagname As String, ByVal maxRows As UInteger, ByVal _scanRate As Long) As DataSet
        '    Return BrowseData(OpenConnection, _tagname, maxRows, _scanRate)
        'End Function

        ' ''' <summary>
        ' ''' 
        ' ''' </summary>
        ' ''' <param name="_conn">Phd connection</param>
        ' ''' <param name="_tagname">Tag Name that was selected</param>
        ' ''' <param name="_collector">Collector that was selected</param>
        ' ''' <param name="maxRows">Maximum number if rows to return</param>
        ' ''' <param name="_scanRate">Scan rate or frequency of data collection</param>
        ' ''' <returns></returns>
        'Public Function BrowseData(ByVal _conn As PhdConnection, ByVal _tagname As String, ByVal _collector As String, ByVal maxRows As UInteger, ByVal _scanRate As Long) As DataSet
        '    'check the HashTable to see if that connection is available
        '    CheckForConnection(_conn)

        '    Dim _filter As TagFilter = New TagFilter()
        '    _filter.ScanRate = _scanRate
        '    _filter.Collector = _collector
        '    _filter.Tagname = _tagname

        '    Return _conn.oPhd.BrowsingTags(maxRows, _filter)
        'End Function

        'Public Function BrowseData(ByVal _tagname As String, ByVal _collector As String, ByVal maxRows As UInteger, ByVal _scanRate As Long) As DataSet
        '    Return BrowseData(OpenConnection, _tagname, _collector, maxRows, _scanRate)
        'End Function


        ' ''' <summary>
        ' ''' 
        ' ''' </summary>
        ' ''' <param name="_conn">Phd connection</param>
        ' ''' <param name="_tagname">Tag Name that was selected</param>
        ' ''' <param name="_collector">Collector that was selected</param>
        ' ''' <param name="_parent">Parent Tag Name</param>
        ' ''' <param name="maxRows">Maximum number if rows to return</param>
        ' ''' <param name="_scanRate">Scan rate or frequency of data collection</param>
        ' ''' <returns></returns>
        'Public Function BrowseData(ByVal _conn As PhdConnection, ByVal _tagname As String, ByVal _collector As String, ByVal _parent As String, ByVal maxRows As UInteger, ByVal _scanRate As Long) As DataSet
        '    'check the HashTable to see if that connection is available
        '    CheckForConnection(_conn)

        '    Dim _filter As TagFilter = New TagFilter()
        '    _filter.ScanRate = _scanRate
        '    _filter.Collector = _collector
        '    _filter.Tagname = _tagname

        '    If _parent Is Nothing Then
        '        _filter.Collector = _collector
        '    Else
        '        _filter.ParentTagname = _parent
        '    End If

        '    Return _conn.oPhd.BrowsingTags(maxRows, _filter)
        'End Function

        ' ''' <summary>
        ' ''' 
        ' ''' </summary>
        ' ''' <param name="_tagname">Tag Name that was selected</param>
        ' ''' <param name="_collector">Collector that was selected</param>
        ' ''' <param name="_parent">Parent Tag Name</param>
        ' ''' <param name="maxRows">Maximum number if rows to return</param>
        ' ''' <param name="_scanRate">Scan rate or frequency of data collection</param>
        ' ''' <returns></returns>
        'Public Function BrowseData(ByVal _tagname As String, ByVal _collector As String, ByVal _parent As String, ByVal maxRows As UInteger, ByVal _scanRate As Long) As DataSet
        '    'check the HashTable to see if that connection is available
        '    Dim _conn As PhdConnection = OpenConnection()
        '    Dim _filter As TagFilter = New TagFilter()
        '    _filter.ScanRate = _scanRate
        '    _filter.Collector = _collector
        '    _filter.Tagname = _tagname
        '    _filter.Collector = _collector
        '    _filter.ParentTagname = _parent

        '    Return _conn.oPhd.BrowsingTags(maxRows, _filter)
        'End Function

        ' ''' <summary>
        ' ''' Return all Collectors for the specified Phd connection
        ' ''' </summary>
        ' ''' <param name="_conn">Phd connection</param>
        ' ''' <returns>Dataset with Collector data</returns>
        'Public Function BrowseCollectorData(ByVal _conn As PhdConnection) As DataSet
        '    'check the HashTable to see if that connection is available
        '    CheckForConnection(_conn)

        '    Return _conn.oPhd.GetRDIs()
        'End Function

        'Public Function BrowseCollectorData() As DataSet
        '    Return BrowseCollectorData(OpenConnection)
        'End Function

        ' ''' <summary>
        ' ''' Return all Parent Tags
        ' ''' </summary>
        ' ''' <param name="_conn">Phd connection</param>
        ' ''' <returns>Dataset with Parent Tags</returns>
        'Public Function BrowseParentData(ByVal _conn As PhdConnection) As DataSet
        '    'check the HashTable to see if that connection is available
        '    CheckForConnection(_conn)

        '    Return _conn.oPhd.GetParentTags()
        'End Function

        'Public Function BrowseParentData() As DataSet
        '    Return BrowseParentData(OpenConnection)
        'End Function
        Private Shared Function CompensateDST(ignoreDST As Boolean) As TimeSpan
            Dim tz As TimeZone = System.TimeZone.CurrentTimeZone
            Dim dst As Boolean = tz.IsDaylightSavingTime(Now)
            Return IIf(dst AndAlso ignoreDST, TimeSpan.FromHours(1), TimeSpan.FromHours(0))
        End Function

        Public Shared Sub BrowseGroupData(ByVal bc As BrowseCollections, ByVal maxRows As UInteger, ByVal ignoreDST As Boolean)
            For Each group As BrowseCollections.TagGroup In bc.tagGroups.Values
                group.DataClass.BrowseGroupData(group, maxRows, ignoreDST)
            Next
        End Sub

        Public Sub BrowseGroupData(ByVal group As BrowseCollections.TagGroup, ByVal maxRows As UInteger, ByVal ignoreDST As Boolean)
            'check the HashTable to see if that connection is available
            Try
                BrowseGroupData(OpenConnection, group, maxRows, ignoreDST)
            Catch ex As Exception
                CloseAllConnections()
                BrowseGroupData(OpenConnection, group, maxRows, ignoreDST)
            End Try
        End Sub

        Public Sub BrowseGroupData(ByVal _conn As PhdConnection, ByVal group As BrowseCollections.TagGroup, ByVal maxRows As UInteger, ByVal ignoreDST As Boolean)
            'check the HashTable to see if that connection is available
            CheckForConnection(_conn)
            Dim _tags As New Tags
            For Each tag In group.tags
                Try
                    _tags.Add(New Tag(tag.Value))
                Catch ex As Exception
                End Try
            Next
            If _tags.Count = 0 Then Exit Sub
            If ignoreDST Then
                _conn.oPhd.StartTime = _conn.oPhd.ConvertToPHDTime(group.StartTime + CompensateDST(ignoreDST))
                _conn.oPhd.EndTime = _conn.oPhd.ConvertToPHDTime(group.EndTime + CompensateDST(ignoreDST))
            Else
                _conn.oPhd.StartTime = _conn.oPhd.ConvertToPHDTime(group.StartTime)
                _conn.oPhd.EndTime = _conn.oPhd.ConvertToPHDTime(group.EndTime)
            End If
            _conn.oPhd.Sampletype = SAMPLETYPE.Raw
            _conn.oPhd.MaximumRows = maxRows
            group.data = _conn.oPhd.FetchRowData(_tags)
            If ignoreDST AndAlso Not group.data Is Nothing AndAlso group.data.Tables.Count > 0 Then
                For Each dr As DataRow In group.data.Tables(0).Rows
                    If Not dr.IsNull("Timestamp") Then
                        Dim tstamp As Date = dr("Timestamp")
                        tstamp -= CompensateDST(ignoreDST)
                        dr("Timestamp") = tstamp
                    End If
                Next
                group.data.Tables(0).AcceptChanges()
            End If
        End Sub

        'Public Function BrowseData(ByVal _conn As PhdConnection, ByVal _tagname As String, ByVal _startTime As Date, ByVal _endTime As Date, ByVal maxRows As UInteger) As DataSet
        '    'check the HashTable to see if that connection is available
        '    CheckForConnection(_conn)

        '    _conn.oPhd.StartTime = _conn.oPhd.ConvertToPHDTime(_startTime)
        '    _conn.oPhd.EndTime = _conn.oPhd.ConvertToPHDTime(_endTime)
        '    _conn.oPhd.Sampletype = SAMPLETYPE.Raw
        '    _conn.oPhd.MaximumRows = maxRows
        '    Return _conn.oPhd.FetchRowData(_tagname)
        'End Function

        'Public Function BrowseData(ByVal _tagname As String, ByVal _startTime As Date, ByVal _endTime As Date, ByVal maxRows As UInteger) As DataSet
        '    Try
        '        Return BrowseData(OpenConnection, _tagname, _startTime, _endTime, maxRows)
        '    Catch ex As Exception
        '        CloseAllConnections()
        '        Return BrowseData(OpenConnection, _tagname, _startTime, _endTime, maxRows)
        '    End Try
        'End Function


        'Public Function BrowseData(ByVal _tagname As String, ByVal _startTime As Date, ByVal _endTime As Date) As DataSet
        '    Return BrowseData(OpenConnection, _tagname, _startTime, _endTime, MAX_ROWS)
        'End Function

        'Public Function TakeSnapshot(ByVal _conn As PhdConnection, ByVal _tagname As String, ByVal _startTime As Date, ByVal maxRows As UInteger) As DataSet
        '    'check the HashTable to see if that connection is available
        '    CheckForConnection(_conn)

        '    _conn.oPhd.StartTime = _conn.oPhd.ConvertToPHDTime(_startTime)
        '    _conn.oPhd.EndTime = _conn.oPhd.ConvertToPHDTime(_startTime)
        '    _conn.oPhd.Sampletype = SAMPLETYPE.Snapshot
        '    _conn.oPhd.MaximumRows = maxRows
        '    Return _conn.oPhd.FetchRowData(_tagname)
        'End Function

        'Public Function TakeSnapshot(ByVal _tagname As String, ByVal _startTime As Date, ByVal maxRows As UInteger) As DataSet
        '    Try
        '        Return TakeSnapshot(OpenConnection, _tagname, _startTime, maxRows)
        '    Catch ex As Exception
        '        CloseAllConnections()
        '        Return TakeSnapshot(OpenConnection, _tagname, _startTime, maxRows)
        '    End Try
        'End Function

        Public Sub TakeGroupSnapshot(ByVal group As BrowseCollections.TagGroup, ByVal maxRows As UInteger, ByVal ignoreDST As Boolean)
            'check the HashTable to see if that connection is available
            Try
                TakeGroupSnapshot(OpenConnection, group, maxRows, ignoreDST)
            Catch ex As Exception
                CloseAllConnections()
                TakeGroupSnapshot(OpenConnection, group, maxRows, ignoreDST)
            End Try
        End Sub

        Public Sub TakeGroupSnapshot(ByVal _conn As PhdConnection, ByVal group As BrowseCollections.TagGroup, ByVal maxRows As UInteger, ByVal ignoreDST As Boolean)
            'check the HashTable to see if that connection is available
            CheckForConnection(_conn)
            Dim _tags As New Tags
            For Each tag In group.tags
                Try
                    _tags.Add(New Tag(tag.Value))
                Catch ex As Exception
                End Try
            Next
            If _tags.Count = 0 Then Exit Sub
            If ignoreDST Then
                _conn.oPhd.StartTime = _conn.oPhd.ConvertToPHDTime(group.StartTime + CompensateDST(ignoreDST))
                _conn.oPhd.EndTime = _conn.oPhd.ConvertToPHDTime(group.EndTime + CompensateDST(ignoreDST))
            Else
                _conn.oPhd.StartTime = _conn.oPhd.ConvertToPHDTime(group.StartTime)
                _conn.oPhd.EndTime = _conn.oPhd.ConvertToPHDTime(group.EndTime)
            End If
           
            _conn.oPhd.Sampletype = SAMPLETYPE.Snapshot
            _conn.oPhd.MaximumRows = maxRows
            group.data = _conn.oPhd.FetchRowData(_tags)
            If ignoreDST AndAlso Not group.data Is Nothing AndAlso group.data.Tables.Count > 0 Then
                For Each dr As DataRow In group.data.Tables(0).Rows
                    If Not dr.IsNull("Timestamp") Then
                        Dim tstamp As Date = dr("Timestamp")
                        tstamp -= CompensateDST(ignoreDST)
                        dr("Timestamp") = tstamp
                    End If
                Next
                group.data.Tables(0).AcceptChanges()
            End If
        End Sub

        Public Shared Sub TakeGroupSnapshot(ByVal bc As BrowseCollections, ByVal maxRows As UInteger, ByVal ignoreDST As Boolean)
            For Each group As BrowseCollections.TagGroup In bc.tagGroups.Values
                group.DataClass.TakeGroupSnapshot(group, maxRows, ignoreDST)
            Next
        End Sub

        'Public Function TakePeriodSnapshot(ByVal _conn As PhdConnection, ByVal _tagname As String, ByVal _startTime As Date, ByVal _endTime As Date, ByVal maxRows As UInteger) As DataSet
        '    'check the HashTable to see if that connection is available
        '    CheckForConnection(_conn)
        '    _conn.oPhd.StartTime = _conn.oPhd.ConvertToPHDTime(_startTime)
        '    _conn.oPhd.EndTime = _conn.oPhd.ConvertToPHDTime(_endTime)
        '    _conn.oPhd.Sampletype = SAMPLETYPE.Snapshot
        '    _conn.oPhd.SampleFrequency = 24 * 3600
        '    _conn.oPhd.MaximumRows = maxRows
        '    Return _conn.oPhd.FetchRowData(_tagname)
        'End Function

        'Public Function TakePeriodSnapshot(ByVal _tagname As String, ByVal _startTime As Date, ByVal _endTime As Date, ByVal maxRows As UInteger) As DataSet
        '    Try
        '        Return TakePeriodSnapshot(OpenConnection, _tagname, _startTime, _endTime, maxRows)
        '    Catch ex As Exception
        '        CloseAllConnections()
        '        Return TakePeriodSnapshot(OpenConnection, _tagname, _startTime, _endTime, maxRows)
        '    End Try
        'End Function

        'Public Function TakePeriodSnapshot(ByVal _tagname As String, ByVal _startTime As Date, ByVal _endTime As Date) As DataSet
        '    Return TakePeriodSnapshot(OpenConnection, _tagname, _startTime, _endTime, MAX_ROWS)
        'End Function

        Public Shared Sub TakeGroupPeriodSnapshot(ByVal bc As BrowseCollections, ByVal maxRows As UInteger)
            For Each group As BrowseCollections.TagGroup In bc.tagGroups.Values
                group.DataClass.TakeGroupPeriodSnapshot(group, maxRows)
            Next
        End Sub

        Public Sub TakeGroupPeriodSnapshot(ByVal group As BrowseCollections.TagGroup, ByVal maxRows As UInteger)
            'check the HashTable to see if that connection is available
            Try
                TakeGroupPeriodSnapshot(OpenConnection, group, maxRows)
            Catch ex As Exception
                CloseAllConnections()
                TakeGroupPeriodSnapshot(OpenConnection, group, maxRows)
            End Try
        End Sub

        Public Sub TakeGroupPeriodSnapshot(ByVal _conn As PhdConnection, ByVal group As BrowseCollections.TagGroup, ByVal maxRows As UInteger)
            'check the HashTable to see if that connection is available
            CheckForConnection(_conn)
            Dim _tags As New Tags
            For Each tag In group.tags
                Try
                    _tags.Add(New Tag(tag.Value))
                Catch ex As Exception
                End Try
            Next
            If _tags.Count = 0 Then Exit Sub
            _conn.oPhd.StartTime = _conn.oPhd.ConvertToPHDTime(group.StartTime)
            _conn.oPhd.EndTime = _conn.oPhd.ConvertToPHDTime(group.EndTime)
            _conn.oPhd.Sampletype = SAMPLETYPE.Snapshot
            _conn.oPhd.SampleFrequency = 24 * 3600
            _conn.oPhd.MaximumRows = maxRows
            group.data = _conn.oPhd.FetchRowData(_tags)
        End Sub

        Public Class BackupStructure
            Private sampleType As SAMPLETYPE
            Private sampleFrequency As UInteger
            Private reductionFrequency As UInteger
            Private reductionType As REDUCTIONTYPE
            Private reductionOffset As REDUCTIONOFFSET
            Private useSampleFrequency As Boolean
            Private maxRows As UInteger

            Public Sub New(_conn As PhdConnection)
                sampleType = _conn.oPhd.Sampletype
                sampleFrequency = _conn.oPhd.SampleFrequency
                reductionFrequency = _conn.oPhd.ReductionFrequency
                reductionType = _conn.oPhd.ReductionType
                reductionOffset = _conn.oPhd.ReductionOffset
                useSampleFrequency = _conn.oPhd.UseSampleFrequency
                maxRows = _conn.oPhd.MaximumRows
            End Sub

            Public Sub Restore(ByRef _conn As PhdConnection)
                _conn.oPhd.Sampletype = sampleType
                _conn.oPhd.SampleFrequency = sampleFrequency
                _conn.oPhd.ReductionFrequency = reductionFrequency
                _conn.oPhd.ReductionType = reductionType
                _conn.oPhd.ReductionOffset = reductionOffset
                _conn.oPhd.UseSampleFrequency = useSampleFrequency
                _conn.oPhd.MaximumRows = maxRows
            End Sub
        End Class

        Public Shared Sub TakeGroupAverageDayVal(ByVal bc As BrowseCollections, ByVal maxRows As UInteger)
            For Each group As BrowseCollections.TagGroup In bc.tagGroups.Values
                group.DataClass.TakeGroupAverageDayVal(group, maxRows)
            Next
        End Sub

        Public Sub TakeGroupAverageDayVal(ByVal _conn As PhdConnection, ByVal group As BrowseCollections.TagGroup, ByVal maxRows As UInteger)
            'check the HashTable to see if that connection is available
            CheckForConnection(_conn)
            Dim _tags As New Tags
            For Each tag In group.tags
                Try
                    _tags.Add(New Tag(tag.Value))
                Catch ex As Exception
                End Try
            Next
            If _tags.Count = 0 Then Exit Sub
            Dim backup As New BackupStructure(_conn)
            Try
                _conn.oPhd.StartTime = _conn.oPhd.ConvertToPHDTime(group.StartTime)
                _conn.oPhd.EndTime = _conn.oPhd.ConvertToPHDTime(group.EndTime)
                _conn.oPhd.Sampletype = SAMPLETYPE.Snapshot
                _conn.oPhd.SampleFrequency = 1
                _conn.oPhd.ReductionFrequency = 86400
                _conn.oPhd.ReductionType = REDUCTIONTYPE.Average
                _conn.oPhd.ReductionOffset = REDUCTIONOFFSET.After
                _conn.oPhd.UseSampleFrequency = False
                _conn.oPhd.MaximumRows = maxRows
                group.data = _conn.oPhd.FetchRowData(_tags)
            Finally
                backup.Restore(_conn)
            End Try
        End Sub

        Public Sub TakeGroupAverageDayVal(ByVal group As BrowseCollections.TagGroup, ByVal maxRows As UInteger)
            Try
                TakeGroupAverageDayVal(OpenConnection, group, maxRows)
            Catch ex As Exception
                CloseAllConnections()
                TakeGroupAverageDayVal(OpenConnection, group, maxRows)
            End Try
        End Sub

        'Public Function TakeAverageDayVal(ByVal _conn As PhdConnection, ByVal _tagname As String, _startTime As Date, ByVal maxRows As UInteger) As DataSet
        '    'check the HashTable to see if that connection is available
        '    CheckForConnection(_conn)
        '    Dim backup As New BackupStructure(_conn)
        '    Try
        '        _conn.oPhd.StartTime = _conn.oPhd.ConvertToPHDTime(_startTime)
        '        _conn.oPhd.EndTime = _conn.oPhd.ConvertToPHDTime(_startTime)
        '        _conn.oPhd.Sampletype = SAMPLETYPE.Snapshot
        '        _conn.oPhd.SampleFrequency = 1
        '        _conn.oPhd.ReductionFrequency = 86400
        '        _conn.oPhd.ReductionType = REDUCTIONTYPE.Average
        '        _conn.oPhd.ReductionOffset = REDUCTIONOFFSET.After
        '        _conn.oPhd.UseSampleFrequency = False
        '        _conn.oPhd.MaximumRows = maxRows
        '        Return _conn.oPhd.FetchRowData(_tagname)
        '    Finally
        '        backup.Restore(_conn)
        '    End Try
        'End Function

        'Public Function TakeAverageDayVal(ByVal _tagname As String, ByVal _startTime As Date, ByVal maxRows As UInteger) As DataSet
        '    Try
        '        Return TakeAverageDayVal(OpenConnection, _tagname, _startTime, maxRows)
        '    Catch ex As Exception
        '        CloseAllConnections()
        '        Return TakeAverageDayVal(OpenConnection, _tagname, _startTime, maxRows)
        '    End Try
        'End Function

        Public Function FetchTagDefinition(ByVal _conn As PhdConnection, ByVal _tagname As String, ByVal _parameter As String) As Object
            'check the HashTable to see if that connection is available
            CheckForConnection(_conn)

            Dim ds As DataSet = _conn.oPhd.TagDfn(_tagname)
            If (Not (ds) Is Nothing) AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                Dim dt As System.Data.DataTable = ds.Tables(0)
                Dim dc As System.Data.DataColumn
                For Each dc In dt.Columns
                    If dc.ColumnName = _parameter Then Return dt.Rows(0).Item(_parameter)
                Next
            End If
            Return STRING_NA
        End Function

        Public Function FetchTagDefinition(ByVal _tagname As String, ByVal _parameter As String) As Object
            Return FetchTagDefinition(OpenConnection, _tagname, _parameter)
        End Function

        Public Function GetLoExtreme(ByVal _conn As PhdConnection, ByVal _tagname As String) As Object
            Dim ds As DataSet = _conn.oPhd.TagDfn(_tagname)
            If (Not (ds) Is Nothing) AndAlso ds.Tables.Count > 0 AndAlso _
                ds.Tables(0).Rows.Count > 0 AndAlso _
                IsDBNull(ds.Tables(0).Rows(0).Item("LowExtreme")) = False Then

                Return ds.Tables(0).Rows(0).Item("LowExtreme")
            Else
                Return 0
            End If
        End Function

        Public Function GetLoExtreme(ByVal _tagname As String) As Object
            Return GetLoExtreme(OpenConnection, _tagname)
        End Function

        'Public Sub PutData(ByVal _conn As PhdConnection, ByVal _tagname As String, ByVal _timeStamp As Date, ByVal _value As Object, ByVal _confidence As SByte)
        '    'check the HashTable to see if that connection is available
        '    CheckForConnection(_conn)
        '    _conn.oPhd.ModifyTag(New Tag(_tagname), _value, _conn.oPhd.ConvertToPHDTime(_timeStamp), _confidence)
        'End Sub

        'Public Sub PutData(ByVal _tagname As String, ByVal _timeStamp As Date, ByVal _value As Object, ByVal _confidence As SByte)
        '    PutData(OpenConnection, _tagname, _timeStamp, _value, _confidence)
        'End Sub

        'Public Sub DeleteData(ByVal _conn As PhdConnection, ByVal _tagname As String, ByVal _timeStamp As Date)
        '    'check the HashTable to see if that connection is available
        '    CheckForConnection(_conn)
        '    _conn.oPhd.DeleteTagData(New Tag(_tagname), _conn.oPhd.ConvertToPHDTime(_timeStamp))
        'End Sub

        'Public Sub DeleteData(ByVal _tagname As String, ByVal _timeStamp As Date)
        '    DeleteData(OpenConnection, _tagname, _timeStamp)
        'End Sub

        Public Sub CloseAllConnections()
            'cycle through the hash table and close connections
            Dim _conn As PHDDataLayer.PhdConnection = Nothing
            For Each key As Object In _htConn.Keys
                _conn = _htConn.Item(key)
                Try
                    _conn.Close()
                Catch ex As Exception

                End Try
            Next
            _htConn.Clear()
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
            CloseAllConnections()
        End Sub

        Public Sub New(ByVal _phd_host As String, ByVal _phd_port As Integer, ByVal _phd_username As String, _
                       ByVal _phd_password As String, ByVal _phd_apiversion As String, ByVal _nt_username As String, _
                       ByVal _nt_password As String, ByVal _max_rows As Integer, ByVal _string_na As String, ByVal _string_bad_val As String, _
                       ByVal _min_confidence As Integer)
            PHD_HOST = _phd_host
            PHD_PORT = _phd_port
            PHD_USERNAME = _phd_username
            PHD_PASSWORD = _phd_password
            PHD_APIVERSION = _phd_apiversion
            NT_USERNAME = _nt_username
            NT_PASSWORD = _nt_password
            MAX_ROWS = _max_rows
            STRING_NA = _string_na
            STRING_BAD_VAL = _string_bad_val
            MIN_CONFIDENCE = _min_confidence
        End Sub

        Public Function BrowseData(ByVal _conn As PhdConnection, ByVal _tagname As String, ByVal _startTime As Date, ByVal _endTime As Date, ByVal maxRows As UInteger) As DataSet
            'check the HashTable to see if that connection is available
            CheckForConnection(_conn)

            _conn.oPhd.StartTime = _conn.oPhd.ConvertToPHDTime(_startTime)
            _conn.oPhd.EndTime = _conn.oPhd.ConvertToPHDTime(_endTime)
            _conn.oPhd.Sampletype = SAMPLETYPE.Raw
            _conn.oPhd.MaximumRows = maxRows
            Return _conn.oPhd.FetchRowData(_tagname)
        End Function

        Public Function BrowseData(ByVal _tagname As String, ByVal _startTime As Date, ByVal _endTime As Date, ByVal maxRows As UInteger) As DataSet
            Try
                Return BrowseData(OpenConnection, _tagname, _startTime, _endTime, maxRows)
            Catch ex As Exception
                CloseAllConnections()
                Return BrowseData(OpenConnection, _tagname, _startTime, _endTime, maxRows)
            End Try
        End Function

        Public Function BrowseData(ByVal _tagname As String, ByVal _startTime As Date, ByVal _endTime As Date) As DataSet
            Return BrowseData(OpenConnection, _tagname, _startTime, _endTime, MAX_ROWS)
        End Function

        Public Function TakeSnapshot(ByVal _conn As PhdConnection, ByVal _tagname As String, ByVal _startTime As Date, ByVal maxRows As UInteger) As DataSet
            'check the HashTable to see if that connection is available
            CheckForConnection(_conn)

            _conn.oPhd.StartTime = _conn.oPhd.ConvertToPHDTime(_startTime)
            _conn.oPhd.EndTime = _conn.oPhd.ConvertToPHDTime(_startTime)
            _conn.oPhd.Sampletype = SAMPLETYPE.Snapshot
            _conn.oPhd.MaximumRows = maxRows
            Return _conn.oPhd.FetchRowData(_tagname)
        End Function

        Public Function TakePeriodSnapshot(ByVal _conn As PhdConnection, ByVal _tagname As String, ByVal _startTime As Date, ByVal _endTime As Date, ByVal maxRows As UInteger) As DataSet
            'check the HashTable to see if that connection is available
            CheckForConnection(_conn)
            _conn.oPhd.StartTime = _conn.oPhd.ConvertToPHDTime(_startTime)
            _conn.oPhd.EndTime = _conn.oPhd.ConvertToPHDTime(_endTime)
            _conn.oPhd.Sampletype = SAMPLETYPE.Snapshot
            _conn.oPhd.SampleFrequency = 24 * 3600
            _conn.oPhd.MaximumRows = maxRows
            Return _conn.oPhd.FetchRowData(_tagname)
        End Function

        Public Function TakePeriodSnapshot(ByVal _tagname As String, ByVal _startTime As Date, ByVal _endTime As Date, ByVal maxRows As UInteger) As DataSet
            Try
                Return TakePeriodSnapshot(OpenConnection, _tagname, _startTime, _endTime, maxRows)
            Catch ex As Exception
                CloseAllConnections()
                Return TakePeriodSnapshot(OpenConnection, _tagname, _startTime, _endTime, maxRows)
            End Try
        End Function

        Public Function TakePeriodSnapshot(ByVal _tagname As String, ByVal _startTime As Date, ByVal _endTime As Date) As DataSet
            Return TakePeriodSnapshot(OpenConnection, _tagname, _startTime, _endTime, MAX_ROWS)
        End Function

        Public Function TakeSnapshot(ByVal _tagname As String, ByVal _startTime As Date, ByVal maxRows As UInteger) As DataSet
            Try
                Return TakeSnapshot(OpenConnection, _tagname, _startTime, maxRows)
            Catch ex As Exception
                CloseAllConnections()
                Return TakeSnapshot(OpenConnection, _tagname, _startTime, maxRows)
            End Try
        End Function

        Public Function TakeAverageDayVal(ByVal _conn As PhdConnection, ByVal _tagname As String, _startTime As Date, ByVal maxRows As UInteger) As DataSet
            'check the HashTable to see if that connection is available
            CheckForConnection(_conn)
            Dim backup As New BackupStructure(_conn)
            Try
                _conn.oPhd.StartTime = _conn.oPhd.ConvertToPHDTime(_startTime)
                _conn.oPhd.EndTime = _conn.oPhd.ConvertToPHDTime(_startTime)
                _conn.oPhd.Sampletype = SAMPLETYPE.Snapshot
                _conn.oPhd.SampleFrequency = 1
                _conn.oPhd.ReductionFrequency = 86400
                _conn.oPhd.ReductionType = REDUCTIONTYPE.Average
                _conn.oPhd.ReductionOffset = REDUCTIONOFFSET.After
                _conn.oPhd.UseSampleFrequency = False
                _conn.oPhd.MaximumRows = maxRows
                Return _conn.oPhd.FetchRowData(_tagname)
            Finally
                backup.Restore(_conn)
            End Try
        End Function

        Public Function TakeAverageDayVal(ByVal _tagname As String, ByVal _startTime As Date, ByVal maxRows As UInteger) As DataSet
            Try
                Return TakeAverageDayVal(OpenConnection, _tagname, _startTime, maxRows)
            Catch ex As Exception
                CloseAllConnections()
                Return TakeAverageDayVal(OpenConnection, _tagname, _startTime, maxRows)
            End Try
        End Function
    End Class
End Namespace