namespace CollectingProductionDataSystem.GetTransactions
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
            this.GetTransactionsServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.GetTransactionsServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // GetTransactionsServiceProcessInstaller
            // 
            this.GetTransactionsServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.GetTransactionsServiceProcessInstaller.Password = null;
            this.GetTransactionsServiceProcessInstaller.Username = null;
            // 
            // GetTransactionsServiceInstaller
            // 
            this.GetTransactionsServiceInstaller.Description = "Windows service to transfer transactions data from Automated Reporting System to " +
    "Collecting Primary Data System";
            this.GetTransactionsServiceInstaller.DisplayName = "CPDS GetTransactions";
            this.GetTransactionsServiceInstaller.ServiceName = "GetTransactions";
            this.GetTransactionsServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.GetTransactionsServiceProcessInstaller,
            this.GetTransactionsServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller GetTransactionsServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller GetTransactionsServiceInstaller;
    }
}