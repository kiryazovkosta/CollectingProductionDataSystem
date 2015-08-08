<System.ComponentModel.RunInstaller(True)> Partial Class ProjectInstaller
    Inherits System.Configuration.Install.Installer

    'Installer overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Component Designer
    'It can be modified using the Component Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Phd2SqlInventoryServiceProcessInstaller = New System.ServiceProcess.ServiceProcessInstaller()
        Me.Phd2SqlInventoryServiceInstaller = New System.ServiceProcess.ServiceInstaller()
        '
        'Phd2SqlInventoryServiceProcessInstaller
        '
        Me.Phd2SqlInventoryServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem
        Me.Phd2SqlInventoryServiceProcessInstaller.Password = Nothing
        Me.Phd2SqlInventoryServiceProcessInstaller.Username = Nothing
        '
        'Phd2SqlInventoryServiceInstaller
        '
        Me.Phd2SqlInventoryServiceInstaller.Description = "Collecting Primary Data System: Reads inventory data from Uniformance PHD and wri" & _
    "tes it to MS SQL database."
        Me.Phd2SqlInventoryServiceInstaller.DisplayName = "CPDS PHD 2 SQL Inventory Synchronization Service"
        Me.Phd2SqlInventoryServiceInstaller.ServiceName = "CPDS PHD 2 SQL Inventory Synchronization Service"
        Me.Phd2SqlInventoryServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic
        '
        'ProjectInstaller
        '
        Me.Installers.AddRange(New System.Configuration.Install.Installer() {Me.Phd2SqlInventoryServiceProcessInstaller, Me.Phd2SqlInventoryServiceInstaller})

    End Sub
    Friend WithEvents Phd2SqlInventoryServiceProcessInstaller As System.ServiceProcess.ServiceProcessInstaller
    Friend WithEvents Phd2SqlInventoryServiceInstaller As System.ServiceProcess.ServiceInstaller

End Class
