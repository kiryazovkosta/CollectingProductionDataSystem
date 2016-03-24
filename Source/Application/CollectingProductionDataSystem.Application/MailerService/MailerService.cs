namespace CollectingProductionDataSystem.Application.MailerService
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net.Mail;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using log4net;

    public class MailerService : CollectingProductionDataSystem.Application.Contracts.IMailerService
    {
        private readonly ILog logger;
        private string smtpServer;

        public MailerService(ILog loggerParam)
        {
            this.logger = loggerParam;
            InitializeMailService();
        }

        /// <summary>
        /// Initializes the mail service.
        /// </summary>
        private void InitializeMailService()
        {

            // ToDo: refactore this
            var assembly = Assembly.GetEntryAssembly();
            Configuration configuration;
            if (assembly != null)
            {
                var exeFileName = assembly.Location;
                configuration = ConfigurationManager.OpenExeConfiguration(exeFileName);
                ConfigurationSectionGroup appSettingsGroup = configuration.GetSectionGroup("applicationSettings");
                ConfigurationSection appSettingsSection = appSettingsGroup.Sections[0];
                ClientSettingsSection settings = appSettingsSection as ClientSettingsSection;
                string toString = settings.Settings.Get("SMTP_TO").Value.ValueXml.InnerText;
                this.To = string.IsNullOrEmpty(toString) ? "nikolay.kostadinov@bmsys.eu, kosta.kiryazov@bmsys.eu" : toString;
                string fromString = settings.Settings.Get("SMTP_FROM").Value.ValueXml.InnerText;
                this.From = string.IsNullOrEmpty(fromString) ? "SAPO@bmsys.eu" : fromString;
                string smtpServerString = settings.Settings.Get("SMTP_SERVER").Value.ValueXml.InnerText;
                this.smtpServer = string.IsNullOrEmpty(smtpServerString) ? "192.168.7.195" : smtpServerString;
            }       
           
        }

        public string From { get; set; }

        public string To { get; set; }

        public string SmtpServer
        {
            get
            {
                return this.smtpServer;
            }
        }

        public void SendMail(string message, string title = "Message")
        {
            MailMessage mailMessage = new MailMessage(this.From, this.To, title, message);
            SmtpClient client = new SmtpClient(this.smtpServer);
            client.UseDefaultCredentials = true;
            try
            {
                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }
    }
}
