Public Class BrowseCollections

    Public Class BrowseCollectionsSet
        Private _raw As New BrowseCollections
        Private _snap As New BrowseCollections
        Private _rawPeriod As New BrowseCollections
        Private _snapPeriod As New BrowseCollections
        Private _snapAvgDay As New BrowseCollections

        Public ReadOnly Property Raw As BrowseCollections
            Get
                Return _raw
            End Get
        End Property

        Public ReadOnly Property Snap As BrowseCollections
            Get
                Return _snap
            End Get
        End Property

        Public ReadOnly Property RawPeriod As BrowseCollections
            Get
                Return _rawPeriod
            End Get
        End Property

        Public ReadOnly Property SnapPeriod As BrowseCollections
            Get
                Return _snapPeriod
            End Get
        End Property

        Public ReadOnly Property SnapAvgDay As BrowseCollections
            Get
                Return _snapAvgDay
            End Get
        End Property
    End Class

    Public Class TagGroup
        Private _startTime As Date
        Private _endTime As Date
        Private _data As DataSet
        Private _phdServer As Integer
        Private _dataClass As PHDDataLayer.PhdDataAccess

        Public tags As New Dictionary(Of String, String)

        Public Sub New(ByVal phdServer As Integer, dataClass As PHDDataLayer.PhdDataAccess, ByVal startTime As Date, ByVal endTime As Date)
            Me._phdServer = phdServer
            Me._startTime = startTime
            Me._endTime = endTime
            Me._dataClass = dataClass
            Me._data = Nothing
        End Sub

        Public ReadOnly Property StartTime As Date
            Get
                Return _startTime
            End Get
        End Property

        Public ReadOnly Property EndTime As Date
            Get
                Return _endTime
            End Get
        End Property

        Public ReadOnly Property PhdServer As Integer
            Get
                Return _phdServer
            End Get
        End Property

        Public ReadOnly Property DataClass As PHDDataLayer.PhdDataAccess
            Get
                Return _dataClass
            End Get
        End Property

        Public Property data As DataSet
            Set(value As DataSet)
                _data = value
                Dim dt As DataTable = data.Tables(0)
                dt.PrimaryKey = New DataColumn() {dt.Columns("Tagname"), dt.Columns("Timestamp")}
            End Set
            Get
                Return _data
            End Get
        End Property

        Public Sub AddTag(tagName As String)
            If Not tags.ContainsKey(tagName) Then
                tags.Add(tagName, tagName)
            End If
        End Sub
    End Class

    Public tagGroups As New Dictionary(Of String, TagGroup)

    Private Function MakeKey(ByVal phdServer As Integer, ByVal startTime As Date, ByVal endTime As Date) As String
        Return String.Format("{0}_{1:yyyyMMddHHmmss}{2:yyyyMMddHHmmss}", phdServer, startTime, endTime)
    End Function

    Public Sub InsertIntoTagGroup(ByVal phdServer As Integer, dataClass As PHDDataLayer.PhdDataAccess, ByVal tagName As String, ByVal startTime As Date, ByVal endTime As Date)
        Dim key As String = MakeKey(phdServer, startTime, endTime)
        Dim group As TagGroup = Nothing
        If tagGroups.ContainsKey(key) Then
            group = tagGroups.Item(key)
            group.AddTag(tagName)
        Else
            group = New TagGroup(phdServer, dataClass, startTime, endTime)
            group.AddTag(tagName)
            tagGroups.Add(key, group)
        End If
    End Sub

    Public Function GetTagGroupData(ByVal phdServer As Integer, startTime As Date, endTime As Date) As DataSet
        Dim key As String = MakeKey(phdServer, startTime, endTime)
        If tagGroups.ContainsKey(key) Then
            Return tagGroups.Item(key).data
        Else
            Return Nothing
        End If
    End Function

    Public Function GetTagGroup(ByVal phdServer As Integer, startTime As Date, endTime As Date) As TagGroup
        Dim key As String = MakeKey(phdServer, startTime, endTime)
        If tagGroups.ContainsKey(key) Then
            Return tagGroups.Item(key)
        Else
            Return Nothing
        End If
    End Function
End Class
