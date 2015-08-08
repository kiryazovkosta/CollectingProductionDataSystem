Imports Uniformance.PHD

Namespace PHDDataLayer

    Public Class PhdCommon
        Public Enum TAG_PARAMETER
            Tagno
            TagName
            Description
            Units
            Tolerance
            ToleranceType
            CollectionEnable
            DemandCalculationEnable
            ManualInputEnable
            DownloadEnable
            StoreEnable
            EditEnable
            ArchiveResampleEnable
            SourceSystem
            SourceTagname
            SourceTagtype
            SourceAttribute
            SourceUnits
            SourceCollector
            ScanFrequency
            ScanUnit
            SourceIndexA
            SourceIndexB
            SourceIndexC
            SourceIndexD
            RemoteTag
            DataType
            DataSize
            HighExtreme
            LowExtreme
            Quantum
            HiHiLimit
            HiLimit
            LoLimit
            LoLoLimit
            HiHiEnable
            HiEnable
            LoEnable
            LoLoEnable
            FilterConstant
            CompressionToleranceFactor
            MinimumCompression
            SigmaLimit
            SigmaSamples
            GateLevel
            PercentFill
            QueueSize
            ExtrapolationDampIntervals
            ResampleMethod
            InterpolationMethod
            ParentTagname
            ParentTagno
            LinkName
            PointName
            ParameterName
            TagSyncEnable
            TagSyncRuleName
            AssetName
            ItemName
            EnumEnable
        End Enum

        Public Shared Function ReturnExactPHDValue(ByVal _dataclass As PhdDataAccess, ByVal PHD_TAG As String, ByVal TIME_STAMP As Date, Optional ByVal na As Boolean = False) As Object
            Dim res As Object = Nothing
            Dim ds As DataSet = _dataclass.BrowseData(PHD_TAG, TIME_STAMP, TIME_STAMP, 1)
            For Each dr In ds.Tables(0).Rows
                If IsDBNull(dr.Item("Timestamp")) = False AndAlso _
                        dr.Item("Timestamp") = TIME_STAMP AndAlso _
                        IsDBNull(dr.Item("Value")) = False Then
                    res = IIf(na = True And dr.item("Confidence") < _dataclass.GetMinConfidence, _dataclass.GetBadValString, dr.Item("Value"))
                    Exit For
                End If
            Next
            Return IIf(res Is Nothing And na = True, _dataclass.GetNAstring, res)
        End Function

        Public Shared Function ReturnExactPHDSnapshotValue(ByVal _dataclass As PhdDataAccess, ByVal PHD_TAG As String, ByVal TIME_STAMP As Date, Optional ByVal na As Boolean = False) As Object
            Dim res As Object = Nothing
            Dim ds As DataSet = _dataclass.TakeSnapshot(PHD_TAG, TIME_STAMP, 1)
            For Each dr In ds.Tables(0).Rows
                If IsDBNull(dr.Item("Timestamp")) = False AndAlso _
                        dr.Item("Timestamp") = TIME_STAMP AndAlso _
                        IsDBNull(dr.Item("Value")) = False Then
                    res = IIf(na = True And dr.item("Confidence") < _dataclass.GetMinConfidence, _dataclass.GetBadValString, dr.Item("Value"))
                    Exit For
                End If
            Next
            Return IIf(res Is Nothing And na = True, _dataclass.GetNAstring, res)
        End Function

        Public Shared Function ReturnPHDValue(ByVal _dataclass As PhdDataAccess, ByVal PHD_TAG As String, ByVal TIME_STAMP As Date, Optional ByVal na As Boolean = False) As Object
            Dim res As Object = Nothing
            Dim ds As DataSet = _dataclass.BrowseData(PHD_TAG, TIME_STAMP, TIME_STAMP, 1)
            For Each dr In ds.Tables(0).Rows
                If IsDBNull(dr.Item("Timestamp")) = False AndAlso _
                        CDate(dr.Item("Timestamp")) <= TIME_STAMP AndAlso _
                          IsDBNull(dr.Item("Value")) = False Then
                    res = dr.Item("Value")
                    Exit For
                End If
            Next
            Return IIf(res Is Nothing And na = True, _dataclass.GetNAstring, res)
        End Function

        Public Shared Function ReturnExactPHDAvgDayValue(ByVal _dataclass As PhdDataAccess, ByVal PHD_TAG As String, ByVal TIME_STAMP As Date, Optional ByVal na As Boolean = False) As Object
            Dim res As Object = Nothing
            Dim ds As DataSet = _dataclass.TakeAverageDayVal(PHD_TAG, TIME_STAMP, 1)
            For Each dr In ds.Tables(0).Rows
                If IsDBNull(dr.Item("Timestamp")) = False AndAlso _
                        dr.Item("Timestamp") = TIME_STAMP AndAlso _
                        IsDBNull(dr.Item("Value")) = False Then
                    res = IIf(na = True And dr.item("Confidence") < _dataclass.GetMinConfidence, _dataclass.GetBadValString, dr.Item("Value"))
                    Exit For
                End If
            Next
            Return IIf(res Is Nothing And na = True, _dataclass.GetNAstring, res)
        End Function

        Public Shared Function ReturnPeriodExactPHDSnapshotValue(ByVal _dataclass As PhdDataAccess, ByVal PHD_TAG As String, ByVal START_TIMESTAMP As Date, ByVal END_TIMESTAMP As Date, Optional ByVal na As Boolean = False) As Object
            Dim res As Object = Nothing
            Dim time_stamp As Date = Nothing
            Dim ds As DataSet = _dataclass.TakePeriodSnapshot(PHD_TAG, START_TIMESTAMP, END_TIMESTAMP)
            For Each dr In ds.Tables(0).Rows
                If IsDBNull(dr.Item("Timestamp")) = False AndAlso _
                        IsDBNull(dr.Item("Value")) = False Then
                    time_stamp = dr.item("Timestamp")
                    If time_stamp >= START_TIMESTAMP AndAlso time_stamp <= END_TIMESTAMP Then
                        If res Is Nothing Then res = 0
                        res = CDbl(res) + CDbl(dr.Item("Value"))
                    End If
                End If
            Next
            Return IIf(na = True And res Is Nothing, _dataclass.GetNAstring, res)
        End Function

        Public Shared Function ReturnPeriodExactPHDValue(ByVal _dataclass As PhdDataAccess, ByVal PHD_TAG As String, ByVal START_TIMESTAMP As Date, ByVal END_TIMESTAMP As Date, Optional ByVal na As Boolean = False) As Object
            Dim res As Object = Nothing
            Dim time_stamp As Date = Nothing
            Dim ds As DataSet = _dataclass.BrowseData(PHD_TAG, START_TIMESTAMP, END_TIMESTAMP)
            For Each dr In ds.Tables(0).Rows
                If IsDBNull(dr.Item("Timestamp")) = False AndAlso _
                        IsDBNull(dr.Item("Value")) = False Then
                    time_stamp = dr.item("Timestamp")
                    If time_stamp >= START_TIMESTAMP AndAlso time_stamp <= END_TIMESTAMP Then
                        If res Is Nothing Then res = 0
                        res = CDbl(res) + CDbl(dr.Item("Value"))
                    End If
                End If
            Next
            Return IIf(na = True And res Is Nothing, _dataclass.GetNAstring, res)
        End Function

        Public Shared Function GetPhdTagParameter(ByVal _dataclass As PhdDataAccess, ByVal tagName As String, ByVal parameter As TAG_PARAMETER) As Object
            Return Utilities.GetNullString(_dataclass.FetchTagDefinition(tagName, System.Enum.GetName(GetType(TAG_PARAMETER), parameter)))
        End Function

        Public Shared Function GetLastPHDValue(ByVal _dataClass As PhdDataAccess, _
                                        ByVal PHD_TAG As String, ByVal TIME_STAMP As Date, _
                                        ByRef confidence As Object, _
                                        ByRef timeStamp As Object, _
                                        Optional ByVal na As Boolean = False) As Object
            Dim res As Object = String.Empty
            If Not _dataClass.connectFail Then
                Try
                    res = ReturnLastPHDValue(_dataClass, PHD_TAG, TIME_STAMP, confidence, timeStamp, na)
                Catch pex As PHDErrorException
                    If _dataClass.IsConnectFail(pex) Then
                        _dataClass.connectFail = True
                        Return _dataClass.GetConErrString
                    End If
                    Throw pex
                End Try
            Else
                Return _dataClass.GetConErrString
            End If
            Return res
        End Function

        Public Shared Function ReturnLastPHDValue(ByVal _dataClass As PhdDataAccess, _
                                           ByVal PHD_TAG As String, _
                                           ByVal TIME_STAMP As Date, _
                                           ByRef confidence As Object, _
                                           ByRef timestamp As Object, _
                                           Optional ByVal na As Boolean = False) As Object
            Dim res As Object = Nothing
            Dim ds As DataSet = _dataClass.BrowseData(PHD_TAG, TIME_STAMP, TIME_STAMP, 1)
            For Each dr In ds.Tables(0).Rows
                If IsDBNull(dr.Item("Timestamp")) = False AndAlso _
                        IsDBNull(dr.Item("Value")) = False Then
                    confidence = dr.item("Confidence")
                    res = dr.Item("Value")
                    timestamp = dr.Item("Timestamp")
                    Exit For
                End If
            Next
            Return IIf(res Is Nothing And na = True, _dataClass.GetNAstring, res)
        End Function
    End Class
End Namespace

