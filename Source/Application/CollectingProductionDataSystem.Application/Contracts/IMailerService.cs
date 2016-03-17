namespace CollectingProductionDataSystem.Application.Contracts
{
    using System;
    public interface IMailerService
    {
        string From { get; set; }
        void SendMail( string message, string title = "Message");
        string SmtpServer { get; }
        string To { get; set; }
    }
}
