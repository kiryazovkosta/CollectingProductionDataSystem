'Imports AutomatedReportingSystem.UniformanceDataAccess
Imports CollectingProductionDataSystem.UniformanceDataAccess

Module Common
    Public Function ReturnExactPHDValue(ByVal phdServer As Integer, ByVal bc As BrowseCollections, ByVal _dataclass As PHDDataLayer.PhdDataAccess, ByVal PHD_TAG As String, ByVal TIME_STAMP As Date, Optional ByVal na As Boolean = False) As Object
        Dim res As Object = Nothing
        Dim ds As DataSet = bc.GetTagGroupData(phdServer, TIME_STAMP, TIME_STAMP)
        Dim dr As DataRow = ds.Tables(0).Rows.Find({PHD_TAG, TIME_STAMP})
        If Not dr Is Nothing Then
            If IsDBNull(dr.Item("Timestamp")) = False AndAlso _
                    dr.Item("Timestamp") = TIME_STAMP AndAlso _
                    IsDBNull(dr.Item("Value")) = False Then
                res = IIf(na = True And dr.Item("Confidence") < _dataclass.GetMinConfidence, _dataclass.GetBadValString, dr.Item("Value"))
            End If
        End If
        Return IIf(res Is Nothing And na = True, _dataclass.GetNAstring, res)
    End Function

    Public Function ReturnExactPHDSnapshotValue(ByVal phdServer As Integer, ByVal bc As BrowseCollections, ByVal _dataclass As PHDDataLayer.PhdDataAccess, ByVal PHD_TAG As String, ByVal TIME_STAMP As Date, Optional ByVal na As Boolean = False) As Object
        Dim res As Object = Nothing
        Dim ds As DataSet = bc.GetTagGroupData(phdServer, TIME_STAMP, TIME_STAMP)
        Dim dr As DataRow = ds.Tables(0).Rows.Find({PHD_TAG, TIME_STAMP})
        If Not dr Is Nothing Then
            If IsDBNull(dr.Item("Timestamp")) = False AndAlso _
                    dr.Item("Timestamp") = TIME_STAMP AndAlso _
                    IsDBNull(dr.Item("Value")) = False Then
                res = IIf(na = True And dr.Item("Confidence") < _dataclass.GetMinConfidence, _dataclass.GetBadValString, dr.Item("Value"))
            End If
        End If
        Return IIf(res Is Nothing And na = True, _dataclass.GetNAstring, res)
    End Function

    Public Function ReturnPHDSnapshotValue(ByVal phdServer As Integer, ByVal bc As BrowseCollections, ByVal _dataclass As PHDDataLayer.PhdDataAccess, ByVal PHD_TAG As String, ByVal TIME_STAMP As Date, Optional ByVal na As Boolean = False) As Object
        Dim res As Object = Nothing
        Dim ds As DataSet = bc.GetTagGroupData(phdServer, TIME_STAMP, TIME_STAMP)
        Dim rows() As DataRow = ds.Tables(0).Select(String.Format("Tagname='{0}'", PHD_TAG))
        For Each dr In rows
            If IsDBNull(dr.Item("Timestamp")) = False AndAlso _
                    dr.Item("Timestamp") <= TIME_STAMP AndAlso _
                    IsDBNull(dr.Item("Value")) = False Then
                res = IIf(na = True And dr.Item("Confidence") < _dataclass.GetMinConfidence, _dataclass.GetBadValString, dr.Item("Value"))
                Exit For
            End If
        Next
        Return IIf(res Is Nothing And na = True, _dataclass.GetNAstring, res)
    End Function

    Public Function ReturnPHDValue(ByVal phdServer As Integer, ByVal bc As BrowseCollections, ByVal _dataclass As PHDDataLayer.PhdDataAccess, ByVal PHD_TAG As String, ByVal TIME_STAMP As Date, Optional ByVal na As Boolean = False) As Object
        Dim res As Object = Nothing
        Dim ds As DataSet = bc.GetTagGroupData(phdServer, TIME_STAMP, TIME_STAMP)
        'Dim rows() As DataRow = ds.Tables(0).Select(String.Format("Tagname='{0}'", PHD_TAG))
        For Each dr In ds.Tables(0).Rows
            If IsDBNull(dr.Item("Timestamp")) = False AndAlso _
                           dr.Item("Timestamp") <= TIME_STAMP AndAlso _
                           IsDBNull(dr.Item("Value")) = False Then
                res = IIf(na = True And dr.Item("Confidence") < _dataclass.GetMinConfidence, _dataclass.GetBadValString, dr.Item("Value"))
                Exit For
            End If
        Next
        Return IIf(res Is Nothing And na = True, _dataclass.GetNAstring, res)
    End Function

    Public Function ReturnExactPHDAvgDayValue(ByVal phdServer As Integer, ByVal bc As BrowseCollections, ByVal _dataclass As PHDDataLayer.PhdDataAccess, ByVal PHD_TAG As String, ByVal TIME_STAMP As Date, Optional ByVal na As Boolean = False) As Object
        Dim res As Object = Nothing
        Dim ds As DataSet = bc.GetTagGroupData(phdServer, TIME_STAMP, TIME_STAMP)
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

    Public Function ReturnPeriodExactPHDSnapshotValue(ByVal phdServer As Integer, ByVal bc As BrowseCollections, ByVal _dataclass As PHDDataLayer.PhdDataAccess, ByVal PHD_TAG As String, ByVal START_TIMESTAMP As Date, ByVal END_TIMESTAMP As Date, Optional ByVal na As Boolean = False) As Object
        Dim res As Object = Nothing
        Dim time_stamp As Date = Nothing
        Dim ds As DataSet = bc.GetTagGroupData(phdServer, START_TIMESTAMP, END_TIMESTAMP)
        Dim rows() As DataRow = ds.Tables(0).Select(String.Format("Tagname='{0}'", PHD_TAG))
        For Each dr In rows
            If IsDBNull(dr.Item("Timestamp")) = False AndAlso _
                    IsDBNull(dr.Item("Value")) = False Then
                time_stamp = dr.Item("Timestamp")
                If time_stamp >= START_TIMESTAMP AndAlso time_stamp <= END_TIMESTAMP Then
                    If res Is Nothing Then res = 0
                    res = CDbl(res) + CDbl(dr.Item("Value"))
                End If
            End If
        Next
        Return IIf(na = True And res Is Nothing, _dataclass.GetNAstring, res)
    End Function

    Public Function ReturnPeriodExactPHDValue(ByVal phdServer As Integer, ByVal bc As BrowseCollections, ByVal _dataclass As PHDDataLayer.PhdDataAccess, ByVal PHD_TAG As String, ByVal START_TIMESTAMP As Date, ByVal END_TIMESTAMP As Date, Optional ByVal na As Boolean = False) As Object
        Dim res As Object = Nothing
        Dim time_stamp As Date = Nothing
        Dim ds As DataSet = bc.GetTagGroupData(phdServer, START_TIMESTAMP, END_TIMESTAMP)
        Dim rows() As DataRow = ds.Tables(0).Select(String.Format("Tagname='{0}'", PHD_TAG))
        For Each dr In rows
            If IsDBNull(dr.Item("Timestamp")) = False AndAlso _
                    IsDBNull(dr.Item("Value")) = False Then
                time_stamp = dr.Item("Timestamp")
                If time_stamp >= START_TIMESTAMP AndAlso time_stamp <= END_TIMESTAMP Then
                    If res Is Nothing Then res = 0
                    res = CDbl(res) + CDbl(dr.Item("Value"))
                End If
            End If
        Next
        Return IIf(na = True And res Is Nothing, _dataclass.GetNAstring, res)
    End Function
End Module
