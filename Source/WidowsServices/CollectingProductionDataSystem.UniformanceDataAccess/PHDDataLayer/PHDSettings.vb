Namespace PHDDataLayer
    Public Class PHDSettings
        Private _startDate As String = "Now-1h"
        Private _endDate As String = "Now"
        Private _sampleFrequency As UInteger = 300
        Private _sampleType As Uniformance.PHD.SAMPLETYPE = Uniformance.PHD.SAMPLETYPE.Snapshot
        Public Property StartDate() As String
            Get
                Return _startDate
            End Get
            Set(ByVal value As String)
                _startDate = value
            End Set
        End Property

        Public Property EndDate() As String
            Get
                Return _endDate
            End Get
            Set(ByVal value As String)
                _endDate = value
            End Set
        End Property

        Public Property SampleType() As Uniformance.PHD.SAMPLETYPE
            Get
                Return _sampleType
            End Get
            Set(ByVal value As Uniformance.PHD.SAMPLETYPE)
                _sampleType = value
            End Set
        End Property

        Public Property SampleFrequency() As UInteger
            Get
                Return _sampleFrequency
            End Get
            Set(ByVal value As UInteger)
                _sampleFrequency = value
            End Set
        End Property
    End Class
End Namespace
