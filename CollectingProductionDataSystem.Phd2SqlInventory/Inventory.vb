'Imports AutomatedReportingSystem.UniformanceDataAccess
Imports CollectingProductionDataSystem.UniformanceDataAccess
Imports log4net

Public Class Inventory
    Private Shared _instance As Inventory = Nothing

    Private Const PHD_TIMESTAMP As String = "Timestamp"
    Private Const PHD_TAGNAME As String = "Tagname"
    Private Const PHD_VALUE As String = "Value"
    Private Const PHD_CONFIDENCE As String = "Confidence"
    Private Const PHD_SERVER As Integer = 0

    Private Shared Logger As ILog

    Public Enum E_INV_PARAM
        Unknown = 0
        AverageTemperature
        FreeWaterLevel
        FreeWaterVolume
        GrossObservableVolume
        GrossStandardVolume
        LiquidLevel
        NetStandardVolume
        ObservableDensity
        ProductId
        ProductLevel
        ProductName
        ReferenceDensity
        TotalObservableVolume
        WeightInAir
        WeightInVacuum
        MaxVolume
        AvailableRoom
    End Enum

    Public Function GetInventoryParameterName(ByVal value As E_INV_PARAM) As String
        Return [Enum].GetName(GetType(E_INV_PARAM), value)
    End Function

    Private Sub New()
        _instance = Me
        Logger = LogManager.GetLogger("CollectingProductionDataSystem.Phd2SqlInventory")
    End Sub

    Public Shared ReadOnly Property Instance As Inventory
        Get
            If Inventory._instance Is Nothing Then
                _instance = New Inventory
            End If
            Return _instance
        End Get
    End Property

    Private Function EvaluateLastReportTime(ByVal lastRecord As ServiceDataSet.inventory_TanksDataMaxRecordTimestampRow) As Object
        If lastRecord.IsMaxRecordTimestampNull Then
            Return "Not Available"
        End If
        Return lastRecord.MaxRecordTimestamp
    End Function

    Private Sub PrepareTankRow(ByVal bcs As BrowseCollections.BrowseCollectionsSet,
                               ByVal tank As ServiceDataSet.inventory_TanksRow,
                               ByVal lastRecord As ServiceDataSet.inventory_TanksDataMaxRecordTimestampRow,
                               ByVal acqStartTime As DateTime,
                               ByVal currentTime As DateTime,
                               ByVal dataAccess As PHDDataLayer.PhdDataAccess,
                               ByVal enableSnapshot As Boolean,
                               ByVal sampleRate As TimeSpan,
                               ByVal offset As TimeSpan,
                               ByVal updateRecoverPeriod As TimeSpan)
        Dim reportTime As DateTime = acqStartTime
        If Not lastRecord.IsMaxRecordTimestampNull Then
            reportTime = lastRecord.MaxRecordTimestamp - updateRecoverPeriod
            If reportTime < acqStartTime Then
                reportTime = acqStartTime
            Else
                Dim xTime As DateTime = New DateTime(reportTime.Year, reportTime.Month, reportTime.Day, 0, 0, 0)
                While xTime <= reportTime
                    xTime += sampleRate
                End While
                reportTime = xTime
            End If
        End If

        Dim bc As UniformanceDataAccess.BrowseCollections = IIf(enableSnapshot, bcs.Snap, bcs.Raw)
        While reportTime < currentTime
            bc.InsertIntoTagGroup(PHD_SERVER, dataAccess, tank.PhdTagAverageTemperature, reportTime - offset, reportTime + offset)
            bc.InsertIntoTagGroup(PHD_SERVER, dataAccess, tank.PhdTagFreeWaterLevel, reportTime - offset, reportTime + offset)
            bc.InsertIntoTagGroup(PHD_SERVER, dataAccess, tank.PhdTagFreeWaterVolume, reportTime - offset, reportTime + offset)
            bc.InsertIntoTagGroup(PHD_SERVER, dataAccess, tank.PhdTagGrossObservableVolume, reportTime - offset, reportTime + offset)
            bc.InsertIntoTagGroup(PHD_SERVER, dataAccess, tank.PhdTagGrossStandardVolume, reportTime - offset, reportTime + offset)
            bc.InsertIntoTagGroup(PHD_SERVER, dataAccess, tank.PhdTagLiquidLevel, reportTime - offset, reportTime + offset)
            bc.InsertIntoTagGroup(PHD_SERVER, dataAccess, tank.PhdTagNetStandardVolume, reportTime - offset, reportTime + offset)
            bc.InsertIntoTagGroup(PHD_SERVER, dataAccess, tank.PhdTagObservableDensity, reportTime - offset, reportTime + offset)
            bc.InsertIntoTagGroup(PHD_SERVER, dataAccess, tank.PhdTagProductId, reportTime - offset, reportTime + offset)
            bc.InsertIntoTagGroup(PHD_SERVER, dataAccess, tank.PhdTagProductLevel, reportTime - offset, reportTime + offset)
            bc.InsertIntoTagGroup(PHD_SERVER, dataAccess, tank.PhdTagProductName, reportTime - offset, reportTime + offset)
            bc.InsertIntoTagGroup(PHD_SERVER, dataAccess, tank.PhdTagReferenceDensity, reportTime - offset, reportTime + offset)
            bc.InsertIntoTagGroup(PHD_SERVER, dataAccess, tank.PhdTagTotalObservableVolume, reportTime - offset, reportTime + offset)
            bc.InsertIntoTagGroup(PHD_SERVER, dataAccess, tank.PhdTagWeightInAir, reportTime - offset, reportTime + offset)
            bc.InsertIntoTagGroup(PHD_SERVER, dataAccess, tank.PhdTagWeightInVacuum, reportTime - offset, reportTime + offset)
            bc.InsertIntoTagGroup(PHD_SERVER, dataAccess, tank.PhdTagMaxVolume, reportTime - offset, reportTime + offset)
            bc.InsertIntoTagGroup(PHD_SERVER, dataAccess, tank.PhdTagAvailableRoom, reportTime - offset, reportTime + offset)

            Logger.DebugFormat("Tagname={0}, reportTime-ofset={1:dd.MM.yyyy HH:mm:ss}, reportTime+ofset={2:dd.MM.yyyy HH:mm:ss}",
                               tank.PhdTagObservableDensity,
                               reportTime - offset,
                               reportTime + offset)
            reportTime += sampleRate
        End While
        Logger.DebugFormat("controlPoint={0}, " &
                    "currentTime={1:dd.MM.yyyy HH:mm:ss}, " &
                    "acqStartTime={2:dd.MM.yyyy HH:mm:ss}, " &
                    "lastReportTime={3:dd.MM.yyyy HH:mm:ss}, " &
                    "enableSnapshot={4}, " &
                    "sampleRate={5}," &
                    "tagGroups={6}",
                    tank.ControlPoint,
                    currentTime,
                    acqStartTime,
                    EvaluateLastReportTime(lastRecord),
                    enableSnapshot,
                    sampleRate.ToString,
                    bc.tagGroups.Count.ToString)
    End Sub

    Private Sub AddToDictionary(ByVal dict As Dictionary(Of String, E_INV_PARAM), ByVal Key As String, Value As E_INV_PARAM)
        If Not dict.ContainsKey(Key) Then dict.Add(Key, Value)
    End Sub

    Private Sub LoadConfiguration(ByVal tags As Dictionary(Of String, E_INV_PARAM), ByVal tanks As ServiceDataSet.inventory_TanksDataTable, minimumTankId As Integer, maximumTankId As Integer)
        Dim taTanks As ServiceDataSetTableAdapters.inventory_TanksTableAdapter = New ServiceDataSetTableAdapters.inventory_TanksTableAdapter
        tanks.Clear()
        tanks.AcceptChanges()
        taTanks.Fill(tanks)
        Logger.DebugFormat("Tanks count={0}", tanks.Rows.Count)
        For Each tank As ServiceDataSet.inventory_TanksRow In tanks
            AddToDictionary(tags, tank.PhdTagAverageTemperature, E_INV_PARAM.AverageTemperature)
            AddToDictionary(tags, tank.PhdTagFreeWaterLevel, E_INV_PARAM.FreeWaterLevel)
            AddToDictionary(tags, tank.PhdTagFreeWaterVolume, E_INV_PARAM.FreeWaterVolume)
            AddToDictionary(tags, tank.PhdTagGrossObservableVolume, E_INV_PARAM.GrossObservableVolume)
            AddToDictionary(tags, tank.PhdTagGrossStandardVolume, E_INV_PARAM.GrossStandardVolume)
            AddToDictionary(tags, tank.PhdTagLiquidLevel, E_INV_PARAM.LiquidLevel)
            AddToDictionary(tags, tank.PhdTagNetStandardVolume, E_INV_PARAM.NetStandardVolume)
            AddToDictionary(tags, tank.PhdTagObservableDensity, E_INV_PARAM.ObservableDensity)
            AddToDictionary(tags, tank.PhdTagProductId, E_INV_PARAM.ProductId)
            AddToDictionary(tags, tank.PhdTagProductLevel, E_INV_PARAM.ProductLevel)
            AddToDictionary(tags, tank.PhdTagProductName, E_INV_PARAM.ProductName)
            AddToDictionary(tags, tank.PhdTagReferenceDensity, E_INV_PARAM.ReferenceDensity)
            AddToDictionary(tags, tank.PhdTagTotalObservableVolume, E_INV_PARAM.TotalObservableVolume)
            AddToDictionary(tags, tank.PhdTagWeightInAir, E_INV_PARAM.WeightInAir)
            AddToDictionary(tags, tank.PhdTagWeightInVacuum, E_INV_PARAM.WeightInVacuum)
            AddToDictionary(tags, tank.PhdTagMaxVolume, E_INV_PARAM.MaxVolume)
            AddToDictionary(tags, tank.PhdTagAvailableRoom, E_INV_PARAM.AvailableRoom)
        Next
        Logger.DebugFormat("Tag dictionary size: {0}", tags.Count)
    End Sub

    Private Function LoadBrowseCollections(ByVal tanks As ServiceDataSet.inventory_TanksDataTable,
                                           ByVal tags As Dictionary(Of String, E_INV_PARAM),
                                           ByVal dataAccess As UniformanceDataAccess.PHDDataLayer.PhdDataAccess,
                                           ByVal acqStartTime As DateTime,
                                           ByVal currentTime As DateTime,
                                           ByVal enableSnapshot As Boolean,
                                           ByVal sampleRate As TimeSpan,
                                           ByVal offset As TimeSpan,
                                           ByVal updateRecoverPeriod As TimeSpan,
                                           ByVal ignoreDST As Boolean) As BrowseCollections.BrowseCollectionsSet
        Logger.DebugFormat("tankTable size ={0}, " &
                           "Tag dictionary size: {1}, " &
                           "acqStartTime={2:dd.MM.yyyy HH:mm:ss}, " &
                           "currentTime={3:dd.MM.yyyy HH:mm:ss}, " &
                           "enableSnapshot={4}, " &
                           "sampleRate={5},",
                           tanks.Rows.Count,
                           tags.Count,
                           acqStartTime,
                           currentTime,
                           enableSnapshot,
                           sampleRate.ToString)
        Dim bcs As BrowseCollections.BrowseCollectionsSet = New BrowseCollections.BrowseCollectionsSet
        Dim taLastRecords As New ServiceDataSetTableAdapters.inventory_TanksDataMaxRecordTimestampTableAdapter
        Dim lastRecords As New ServiceDataSet.inventory_TanksDataMaxRecordTimestampDataTable
        Dim lastRecord As ServiceDataSet.inventory_TanksDataMaxRecordTimestampRow

        taLastRecords.Fill(lastRecords)
        For Each tank As ServiceDataSet.inventory_TanksRow In tanks
            lastRecord = lastRecords.FindByTankId(tank.TankId)
            PrepareTankRow(bcs, tank, lastRecord, acqStartTime, currentTime, dataAccess, enableSnapshot, sampleRate, offset, updateRecoverPeriod)
        Next
        Dim bc As BrowseCollections = IIf(enableSnapshot, bcs.Snap, bcs.Raw)
        Logger.DebugFormat("Fetching {0} groups of data ....", bc.tagGroups.Count)
        If enableSnapshot Then
            UniformanceDataAccess.PHDDataLayer.PhdDataAccess.TakeGroupSnapshot(bc, 0, ignoreDST)
        Else
            UniformanceDataAccess.PHDDataLayer.PhdDataAccess.BrowseGroupData(bc, 0, ignoreDST)
        End If
        Logger.Debug("Done!")
        Return bcs
    End Function

    Private Overloads Function SetTankData(ByVal tankData As ServiceDataSet.inventory_TanksDataRow,
                                 ByVal parameter As E_INV_PARAM,
                                 ByVal value As Object) As Boolean
        Select Case parameter
            Case E_INV_PARAM.AverageTemperature
                If IsNumeric(value) Then _
                tankData.AverageTemperature = value
            Case E_INV_PARAM.FreeWaterLevel
                If IsNumeric(value) Then _
                tankData.FreeWaterLevel = value
            Case E_INV_PARAM.FreeWaterVolume
                If IsNumeric(value) Then _
                tankData.FreeWaterVolume = value
            Case E_INV_PARAM.GrossObservableVolume
                If IsNumeric(value) Then _
                tankData.GrossObservableVolume = value
            Case E_INV_PARAM.GrossStandardVolume
                If IsNumeric(value) Then _
                tankData.GrossStandardVolume = value
            Case E_INV_PARAM.LiquidLevel
                If IsNumeric(value) Then _
                tankData.LiquidLevel = value
            Case E_INV_PARAM.NetStandardVolume
                If IsNumeric(value) Then _
                tankData.NetStandardVolume = value
            Case E_INV_PARAM.ObservableDensity
                If IsNumeric(value) Then _
                tankData.ObservableDensity = value
            Case E_INV_PARAM.ProductId
                If IsNumeric(value) Then _
                tankData.ProductId = value
            Case E_INV_PARAM.ProductLevel
                If IsNumeric(value) Then _
                tankData.ProductLevel = value
            Case E_INV_PARAM.ProductName
                tankData.ProductName = value
            Case E_INV_PARAM.ReferenceDensity
                If IsNumeric(value) Then _
                tankData.ReferenceDensity = value
            Case E_INV_PARAM.TotalObservableVolume
                If IsNumeric(value) Then _
                tankData.TotalObservableVolume = value
            Case E_INV_PARAM.WeightInAir
                If IsNumeric(value) Then _
                tankData.WeightInAir = value
            Case E_INV_PARAM.WeightInVacuum
                If IsNumeric(value) Then _
                    tankData.WeightInVacuum = value
            Case E_INV_PARAM.MaxVolume
                If IsNumeric(value) Then _
                    tankData.MaxVolume = value
            Case E_INV_PARAM.AvailableRoom
                If IsNumeric(value) Then _
                    tankData.AvailableRoom = value
            Case Else
                Return False
        End Select

        Return True
    End Function

    Private Overloads Function SetTankData(ByVal tankData As ServiceDataSet.inventory_ManualTanksDataRow,
                                 ByVal parameter As E_INV_PARAM,
                                 ByVal value As Object) As Boolean
        Select Case parameter
            Case E_INV_PARAM.AverageTemperature
                If IsNumeric(value) Then _
                tankData.AverageTemperature = value
            Case E_INV_PARAM.FreeWaterLevel
                If IsNumeric(value) Then _
                tankData.FreeWaterLevel = value
            Case E_INV_PARAM.FreeWaterVolume
                If IsNumeric(value) Then _
                tankData.FreeWaterVolume = value
            Case E_INV_PARAM.GrossObservableVolume
                If IsNumeric(value) Then _
                tankData.GrossObservableVolume = value
            Case E_INV_PARAM.GrossStandardVolume
                If IsNumeric(value) Then _
                tankData.GrossStandardVolume = value
            Case E_INV_PARAM.LiquidLevel
                If IsNumeric(value) Then _
                tankData.LiquidLevel = value
            Case E_INV_PARAM.NetStandardVolume
                If IsNumeric(value) Then _
                tankData.NetStandardVolume = value
            Case E_INV_PARAM.ObservableDensity
                If IsNumeric(value) Then _
                tankData.ObservableDensity = value
            Case E_INV_PARAM.ProductId
                If IsNumeric(value) Then _
                tankData.ProductId = value
            Case E_INV_PARAM.ProductLevel
                If IsNumeric(value) Then _
                tankData.ProductLevel = value
            Case E_INV_PARAM.ProductName
                tankData.ProductName = value
            Case E_INV_PARAM.ReferenceDensity
                If IsNumeric(value) Then _
                tankData.ReferenceDensity = value
            Case E_INV_PARAM.TotalObservableVolume
                If IsNumeric(value) Then _
                tankData.TotalObservableVolume = value
            Case E_INV_PARAM.WeightInAir
                If IsNumeric(value) Then _
                tankData.WeightInAir = value
            Case E_INV_PARAM.WeightInVacuum
                If IsNumeric(value) Then _
                tankData.WeightInVacuum = value
            Case E_INV_PARAM.MaxVolume
                If IsNumeric(value) Then _
                    tankData.MaxVolume = value
            Case E_INV_PARAM.AvailableRoom
                If IsNumeric(value) Then _
                    tankData.AvailableRoom = value
            Case Else
                Return False
        End Select

        Return True
    End Function

    Private Function EvaluateTimeStamp(ByVal phdTimeStamp As Date,
                                       ByVal requestTimeStamp As Date,
                                       ByVal enableSnapshot As Boolean,
                                       ByVal offset As TimeSpan,
                                       ByVal phdTagname As String,
                                       ByVal aboveMinConfidence As Boolean,
                                       ByVal tagDictionary As Dictionary(Of String, TimeSpan)) As Boolean
        If enableSnapshot Then
            If phdTimeStamp = requestTimeStamp Then Return True
        Else
            Dim startWindow As Date = requestTimeStamp - offset
            Dim endWindow As Date = requestTimeStamp + offset
            If phdTimeStamp < startWindow OrElse phdTimeStamp > endWindow Then Return False

            Dim currentTimeSpan As TimeSpan = requestTimeStamp - phdTimeStamp
            If tagDictionary.ContainsKey(phdTagname) Then
                Dim lastTimeSpan As TimeSpan = tagDictionary.Item(phdTagname)
                If Math.Abs(currentTimeSpan.TotalSeconds) < Math.Abs(lastTimeSpan.TotalSeconds) Then
                    If aboveMinConfidence Then
                        tagDictionary.Item(phdTagname) = currentTimeSpan
                        Return True
                    End If
                End If
            Else
                If aboveMinConfidence Then tagDictionary.Add(phdTagname, requestTimeStamp - phdTimeStamp)
                Return True
            End If
        End If
        Return False
    End Function

    Private Sub ProcessBrowseCollections(ByVal bcs As BrowseCollections.BrowseCollectionsSet,
                                         ByVal tanks As ServiceDataSet.inventory_TanksDataTable,
                                         ByVal acqTime As Date,
                                         ByVal tags As Dictionary(Of String, E_INV_PARAM),
                                         ByVal enableSnapshot As Boolean,
                                         ByVal timeWindowOffset As TimeSpan,
                                         Optional ByVal naCheck As Boolean = True)
        Logger.DebugFormat("tankTable size ={0}, " &
                           "Tag dictionary size: {1}, " &
                           "acqTime={2:dd.MM.yyyy HH:mm:ss}, " &
                           "enableSnapshot={3}, " &
                           "timeWindowOffset={4}," &
                           "naCheck={5}",
                           tanks.Rows.Count,
                           tags.Count,
                           acqTime,
                           enableSnapshot,
                           timeWindowOffset.ToString,
                           naCheck)

        Logger.Debug("Requesting tank data...")
        Dim taTanksData As New ServiceDataSetTableAdapters.inventory_TanksDataTableAdapter
        Dim tanksData As New ServiceDataSet.inventory_TanksDataDataTable
        Dim tankData As ServiceDataSet.inventory_TanksDataRow = Nothing
        Try
            taTanksData.Fill(tanksData, acqTime)
        Catch ex As Exception
            Logger.Error(ex)
        End Try

        Logger.Debug("Done!")

        Dim tGroup As BrowseCollections.TagGroup = Nothing

        Dim phdDataSet As DataSet = Nothing
        Dim phdDataTable As DataTable = Nothing
        Dim phdTagName As String = Nothing
        Dim phdValue As Object = Nothing

        Dim parameter As E_INV_PARAM = E_INV_PARAM.Unknown
        'Dim reportTime As Date

        Dim drsTanks() As ServiceDataSet.inventory_TanksRow = Nothing

        Dim bc As BrowseCollections = IIf(enableSnapshot, bcs.Snap, bcs.Raw)

        'DEBUG_TRACE("Tag groups found = {0}", bc.tagGroups.Count)

        'DEBUG_TRACE("Processing data...")

        Dim tagDictionary As Dictionary(Of String, TimeSpan)

        For Each pair As KeyValuePair(Of String, BrowseCollections.TagGroup) In bc.tagGroups.ToArray
            tGroup = pair.Value
            phdDataSet = tGroup.data

            If phdDataSet Is Nothing OrElse phdDataSet.Tables.Count = 0 Then Continue For

            phdDataTable = phdDataSet.Tables(0)

            tagDictionary = New Dictionary(Of String, TimeSpan)

            For Each phdDataRow As DataRow In phdDataTable.Rows
                phdTagName = phdDataRow(PHD_TAGNAME)

                If IsDBNull(phdDataRow.Item(PHD_TIMESTAMP)) = False AndAlso
                    EvaluateTimeStamp(phdDataRow.Item(PHD_TIMESTAMP),
                                      tGroup.EndTime - timeWindowOffset,
                                      enableSnapshot,
                                      timeWindowOffset,
                                      phdTagName,
                                      phdDataRow.Item(PHD_CONFIDENCE) >= tGroup.DataClass.GetMinConfidence,
                                      tagDictionary) AndAlso
                    IsDBNull(phdDataRow.Item(PHD_VALUE)) = False Then

                    phdValue = IIf(naCheck = True And phdDataRow.Item(PHD_CONFIDENCE) < tGroup.DataClass.GetMinConfidence, tGroup.DataClass.GetBadValString, phdDataRow.Item("Value"))

                    If Not tags.ContainsKey(phdTagName) Then Continue For

                    parameter = tags.Item(phdTagName)
                    'reportTime = phdDataRow.Item(PHD_TIMESTAMP)

                    drsTanks = tanks.Select(String.Format("PhdTag{0}='{1}'", GetInventoryParameterName(parameter), phdTagName))
                    If drsTanks Is Nothing OrElse drsTanks.Count = 0 Then Continue For

                    For Each tank As ServiceDataSet.inventory_TanksRow In drsTanks
                        tankData = tanksData.FindByTankIdRecordTimestamp(tank.TankId, tGroup.EndTime - timeWindowOffset)

                        If Not tankData Is Nothing Then
                            If Not SetTankData(tankData, parameter, phdValue) Then Continue For
                            tankData.LastUpdateTimestamp = Now
                        Else
                            tankData = tanksData.Newinventory_TanksDataRow
                            'Index part
                            tankData.TankId = tank.TankId
                            tankData.RecordTimestamp = tGroup.EndTime - timeWindowOffset
                            'End of index part

                            'Data part
                            tankData.ExciseStoreId = tank.ExciseStoreId
                            tankData.ParkId = tank.ParkId
                            tankData.LastUpdateTimestamp = Now
                            If Not SetTankData(tankData, parameter, phdValue) Then Continue For
                            'End of data part

                            tanksData.Rows.Add(tankData)
                        End If
                    Next
                End If
            Next

            If My.Settings.DEBUG_TANKS_PRINT_SET Then
                For Each key As String In tags.Keys
                    If tagDictionary.ContainsKey(key) Then
                        Logger.DebugFormat("Set reportTime={0:yyyy.MM.dd HH:mm:ss}, tagName={1}, timeSpan={2}",
                                           tGroup.EndTime - timeWindowOffset,
                                           key,
                                           tagDictionary.Item(key))
                    Else
                        Logger.DebugFormat("Set reportTime={0:yyyy.MM.dd HH:mm:ss}, tagName={1}, timeSpan=NO DATA",
                                           tGroup.EndTime - timeWindowOffset,
                                           key)
                    End If
                Next
            End If
        Next

        Dim rowsAdded As Integer = 0
        Dim rowsModified As Integer = 0

        For Each tankData In tanksData
            Select Case tankData.RowState
                Case DataRowState.Added
                    rowsAdded += 1
                Case DataRowState.Modified
                    rowsModified += 1
            End Select
        Next
        Logger.Debug("Updating data in RDBMS...")
        Logger.DebugFormat("Rows added={0}, Rows modified={1}", rowsAdded, rowsModified)
        taTanksData.Update(tanksData)
        Logger.Debug("Done!")
    End Sub

    Private Sub ProcessBrowseCollectionsManual(ByVal bcs As BrowseCollections.BrowseCollectionsSet,
                                         ByVal tanks As ServiceDataSet.inventory_TanksDataTable,
                                         ByVal acqTime As Date,
                                         ByVal tags As Dictionary(Of String, E_INV_PARAM),
                                         ByVal enableSnapshot As Boolean,
                                         ByVal timeWindowOffset As TimeSpan,
                                         Optional ByVal naCheck As Boolean = True)
        Logger.DebugFormat("tankTable size ={0}, " &
                           "Tag dictionary size: {1}, " &
                           "acqTime={2:dd.MM.yyyy HH:mm:ss}, " &
                           "enableSnapshot={3}, " &
                           "timeWindowOffset={4}," &
                           "naCheck={5}",
                           tanks.Rows.Count,
                           tags.Count,
                           acqTime,
                           enableSnapshot,
                           timeWindowOffset.ToString,
                           naCheck)

        Logger.Debug("Requesting tank data...")
        Dim taTanksData As New ServiceDataSetTableAdapters.inventory_ManualTanksDataTableAdapter
        Dim tanksData As New ServiceDataSet.inventory_ManualTanksDataDataTable
        Dim tankData As ServiceDataSet.inventory_ManualTanksDataRow = Nothing
        taTanksData.Fill(tanksData, acqTime)
        Logger.Debug("Done!")

        Dim tGroup As BrowseCollections.TagGroup = Nothing

        Dim phdDataSet As DataSet = Nothing
        Dim phdDataTable As DataTable = Nothing
        Dim phdTagName As String = Nothing
        Dim phdValue As Object = Nothing

        Dim parameter As E_INV_PARAM = E_INV_PARAM.Unknown
        'Dim reportTime As Date

        Dim drsTanks() As ServiceDataSet.inventory_TanksRow = Nothing
        Dim bc As BrowseCollections = IIf(enableSnapshot, bcs.Snap, bcs.Raw)

        Logger.DebugFormat("Tag groups found = {0}", bc.tagGroups.Count)
        Logger.Debug("Processing data...")

        Dim tagDictionary As Dictionary(Of String, TimeSpan)

        For Each pair As KeyValuePair(Of String, BrowseCollections.TagGroup) In bc.tagGroups.ToArray
            tGroup = pair.Value
            phdDataSet = tGroup.data

            If phdDataSet Is Nothing OrElse phdDataSet.Tables.Count = 0 Then Continue For

            phdDataTable = phdDataSet.Tables(0)
            tagDictionary = New Dictionary(Of String, TimeSpan)

            For Each phdDataRow As DataRow In phdDataTable.Rows
                phdTagName = phdDataRow(PHD_TAGNAME)

                If IsDBNull(phdDataRow.Item(PHD_TIMESTAMP)) = False AndAlso
                    EvaluateTimeStamp(phdDataRow.Item(PHD_TIMESTAMP),
                                      tGroup.EndTime - timeWindowOffset,
                                      enableSnapshot,
                                      timeWindowOffset,
                                      phdTagName,
                                      phdDataRow.Item(PHD_CONFIDENCE) >= tGroup.DataClass.GetMinConfidence,
                                      tagDictionary) AndAlso
                    IsDBNull(phdDataRow.Item(PHD_VALUE)) = False Then

                    phdValue = IIf(naCheck = True And phdDataRow.Item(PHD_CONFIDENCE) < tGroup.DataClass.GetMinConfidence, tGroup.DataClass.GetBadValString, phdDataRow.Item("Value"))

                    If Not tags.ContainsKey(phdTagName) Then Continue For

                    parameter = tags.Item(phdTagName)
                    'reportTime = phdDataRow.Item(PHD_TIMESTAMP)

                    drsTanks = tanks.Select(String.Format("PhdTag{0}='{1}'", GetInventoryParameterName(parameter), phdTagName))
                    If drsTanks Is Nothing OrElse drsTanks.Count = 0 Then Continue For

                    For Each tank As ServiceDataSet.inventory_TanksRow In drsTanks
                        tankData = tanksData.FindByTankIdRecordTimestamp(tank.TankId, tGroup.EndTime - timeWindowOffset)

                        If Not tankData Is Nothing Then
                            If tankData.IsManualSaveNull = False AndAlso tankData.ManualSave = True Then Continue For
                            If Not SetTankData(tankData, parameter, phdValue) Then Continue For
                            tankData.LastUpdateTimestamp = Now
                        Else
                            tankData = tanksData.Newinventory_ManualTanksDataRow
                            tankData.TankId = tank.TankId
                            tankData.RecordTimestamp = tGroup.EndTime - timeWindowOffset

                            tankData.ExciseStoreId = tank.ExciseStoreId
                            tankData.ParkId = tank.ParkId
                            tankData.LastUpdateTimestamp = Now
                            If Not SetTankData(tankData, parameter, phdValue) Then Continue For

                            tanksData.Rows.Add(tankData)
                        End If
                    Next
                End If
            Next

            If My.Settings.DEBUG_TANKS_PRINT_SET Then
                For Each key As String In tags.Keys
                    If tagDictionary.ContainsKey(key) Then
                        Logger.DebugFormat("Set reportTime={0:yyyy.MM.dd HH:mm:ss}, tagName={1}, timeSpan={2}", tGroup.EndTime - timeWindowOffset, key, tagDictionary.Item(key))
                    Else
                        Logger.DebugFormat("Set reportTime={0:yyyy.MM.dd HH:mm:ss}, tagName={1}, timeSpan=NO DATA", tGroup.EndTime - timeWindowOffset, key)
                    End If
                Next
            End If
        Next

        Dim rowsAdded As Integer = 0
        Dim rowsModified As Integer = 0

        For Each tankData In tanksData
            Select Case tankData.RowState
                Case DataRowState.Added
                    rowsAdded += 1
                Case DataRowState.Modified
                    rowsModified += 1
            End Select
        Next

        Logger.Debug("Updating data in RDBMS...")
        Logger.DebugFormat("Rows added={0}, Rows modified={1}", rowsAdded, rowsModified)
        taTanksData.Update(tanksData)
        Logger.Debug("Done!")
    End Sub

    Public Sub Iterate(ignoreDST As Boolean, minimumTankId As Integer, maximumTankId As Integer)
        Logger.Info("Synchronization started!")
        Try
            Dim serviceDS As ServiceDataSet = New ServiceDataSet
            Dim dataAccess As UniformanceDataAccess.PHDDataLayer.PhdDataAccess = Nothing
            With My.Settings
                dataAccess = New UniformanceDataAccess.PHDDataLayer.PhdDataAccess(.PHD_HOST,
                    .PHD_PORT,
                    .PHD_USERNAME,
                    .PHD_PASSWORD,
                    .PHD_APIVERSION,
                    .NT_USERNAME,
                    .NT_PASSWORD,
                    .MAX_ROWS,
                    .STRING_NA,
                    .STRING_BAD_VAL,
                    .MIN_CONFIDENCE)
            End With

            Dim tags As Dictionary(Of String, E_INV_PARAM) = New Dictionary(Of String, E_INV_PARAM)
            Dim bcs As BrowseCollections.BrowseCollectionsSet = Nothing
            Dim currentTime As Date = Now - My.Settings.SCAN_DELAY
            Dim acqStartTime As DateTime = currentTime - My.Settings.INSERT_RECOVER_PERIOD
            Dim xTime As DateTime = New DateTime(acqStartTime.Year, acqStartTime.Month, acqStartTime.Day, 0, 0, 0)
            While (xTime + My.Settings.SAMPLE_RATE) < acqStartTime
                xTime += My.Settings.SAMPLE_RATE
            End While
            acqStartTime = xTime

            LoadConfiguration(tags, serviceDS.inventory_Tanks, minimumTankId, maximumTankId)
            bcs = LoadBrowseCollections(serviceDS.inventory_Tanks,
                tags,
                dataAccess,
                acqStartTime,
                currentTime,
                My.Settings.ENABLE_SNAPSHOT,
                My.Settings.SAMPLE_RATE,
                My.Settings.DATA_TIME_WINDOW,
                My.Settings.UPDATE_RECOVER_PERIOD,
                ignoreDST)

            ProcessBrowseCollections(bcs,
                serviceDS.inventory_Tanks,
                acqStartTime,
                tags,
                My.Settings.ENABLE_SNAPSHOT,
                My.Settings.DATA_TIME_WINDOW,
                My.Settings.NA_CHECK)

            If My.Settings.PROCESS_MANUAL_DATA Then
                ProcessBrowseCollectionsManual(bcs,
                    serviceDS.inventory_Tanks,
                    acqStartTime,
                    tags,
                    My.Settings.ENABLE_SNAPSHOT,
                    My.Settings.DATA_TIME_WINDOW,
                    My.Settings.NA_CHECK)
            End If

            Logger.Info("Done!")
        Catch ex As Exception
            Logger.Error(ex)
        End Try

    End Sub
End Class
