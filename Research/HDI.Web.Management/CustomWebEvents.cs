using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace HDI.Web.Management
{

  /// <summary>
  /// Custom HTTP module for raising CustomWebRequestEvent.
  /// </summary>
  /// <remarks></remarks>
  public class CustomWebEvents : System.Web.UI.Page, IHttpModule
  {

    public override void Dispose()
    {
      //Add your dispose functionality here.
    }

    /// <summary>
    /// Add handlers for BeginRequest and EndRequest
    /// </summary>
    public new void Init(System.Web.HttpApplication httpApp)
    {

      httpApp.BeginRequest += new EventHandler(httpApp_BeginRequest);
      httpApp.EndRequest += new EventHandler(httpApp_EndRequest);

    }

    /// <summary>
    /// Create instance of CustomWebRequestEvent and raise it.
    /// </summary>
    void httpApp_BeginRequest(object sender, EventArgs e)
    {
      try
      {
        Int32 eventCode = System.Web.Management.WebEventCodes.WebExtendedBase + 30;
        CustomWebRequestEvent customEvent = new CustomWebRequestEvent("Custom event start", this, eventCode);
        customEvent.Raise();
      }
      catch (Exception ex)
      {
        HttpApplication httpApp = sender as HttpApplication;
        httpApp.Context.Response.Output.WriteLine(ex.ToString());
      }
    }

    /// <summary>
    /// Create instance of CustomWebRequestEvent and raise it.
    /// </summary>
    void httpApp_EndRequest(object sender, EventArgs e)
    {
      try
      {
        Int32 eventCode = System.Web.Management.WebEventCodes.WebExtendedBase + 30;
        CustomWebRequestEvent customEvent = new CustomWebRequestEvent("Custom event end", this, eventCode);
        customEvent.Raise();
      }
      catch (Exception ex)
      {
        HttpApplication httpApp = sender as HttpApplication;
        httpApp.Context.Response.Output.WriteLine(ex.ToString());
      }
    }


  }
}
