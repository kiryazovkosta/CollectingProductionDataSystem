namespace CollectingProductionDataSystem.Infrastructure.Log
{
    using System.Web.Management;

    internal class CustomWebAuthenticationFailureAuditEvent : WebAuthenticationFailureAuditEvent
    {
        public CustomWebAuthenticationFailureAuditEvent(string msg, object eventSource, string userName) : base(msg, eventSource,
         WebEventCodes.WebExtendedBase + WebEventCodes.AuditFormsAuthenticationFailure, WebEventCodes.AuditFormsAuthenticationFailure, userName)
        {

        }
    }
}