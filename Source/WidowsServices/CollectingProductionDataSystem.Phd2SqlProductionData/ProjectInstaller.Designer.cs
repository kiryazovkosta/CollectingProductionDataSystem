namespace CollectingProductionDataSystem.Phd2SqlProductionData
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.phd2SqlProductionDataServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.phd2SqlProductionDataServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // phd2SqlProductionDataServiceProcessInstaller
            // 
            this.phd2SqlProductionDataServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.phd2SqlProductionDataServiceProcessInstaller.Password = null;
            this.phd2SqlProductionDataServiceProcessInstaller.Username = null;
            // 
            // phd2SqlProductionDataServiceInstaller
            // 
            this.phd2SqlProductionDataServiceInstaller.Description = "Collecting Production Data System: Reads production systems data from Uniformance" +
    " PHD and writes it to MS SQL database.";
            this.phd2SqlProductionDataServiceInstaller.DisplayName = "CPDS PHD 2 SQL Production Data Synchronization Service";
            this.phd2SqlProductionDataServiceInstaller.ServiceName = "CPDS PHD 2 SQL Production Data Synchronization Service";
            this.phd2SqlProductionDataServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.phd2SqlProductionDataServiceProcessInstaller,
            this.phd2SqlProductionDataServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller phd2SqlProductionDataServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller phd2SqlProductionDataServiceInstaller;
    }
}