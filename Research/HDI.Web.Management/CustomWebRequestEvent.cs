using System;
using System.Collections.Generic;
using System.Text;

using System.Web.Management;
using System.Web;

namespace HDI.Web.Management
{

  /// <summary>
  /// Custom health monitoring event class extending WebRequestEvent
  /// </summary>
  public class CustomWebRequestEvent : System.Web.Management.WebRequestEvent
  {

    private String msgCreated;
    private String msgRaised;

    /// <summary>
    /// Constructor with arguments for the base class.
    /// </summary>
    /// <param name="msg">Event message</param>
    /// <param name="eventSource">Source of the event.</param>
    /// <param name="eventCode">Unique code representing the event.</param>
    public CustomWebRequestEvent(String msg, Object eventSource, Int32 eventCode)
      : base(msg, eventSource, eventCode)
    {

      msgCreated = String.Format("Event created at: {0}", EventTime.ToString());

    }

    /// <summary>
    /// Constructor with arguments for the base class.
    /// </summary>
    /// <param name="msg">Event message</param>
    /// <param name="eventSource">Source of the event.</param>
    /// <param name="eventCode">Unique code representing the event.</param>
    /// <param name="eventDetailCode">Unique code representing the event detail.</param>
    public CustomWebRequestEvent(String msg, Object eventSource, Int32 eventCode, Int32 eventDetailCode)
      : base(msg, eventSource, eventCode, eventDetailCode)
    {

      msgCreated = String.Format("Event created at: {0}", EventTime.ToString());

    }

    /// <summary>
    /// Called when the event is raised.
    /// </summary>
    /// <remarks></remarks>
    public override void Raise()
    {

      msgRaised = String.Format("Event raised at: {0}", EventTime.ToString());
      //Store your custom event information etc. here.

      base.Raise();
    }

    /// <summary>
    /// Formats the event information for the provider.
    /// </summary>
    /// <param name="formatter">Standard formatter for health monitoring events.</param>
    public override void FormatCustomEventDetails(System.Web.Management.WebEventFormatter formatter)
    {

      //Format our custom event information.
      formatter.AppendLine(String.Empty);

      formatter.AppendLine("*** Start of custom event information");

      formatter.AppendLine(msgCreated);
      formatter.AppendLine(msgRaised);

      formatter.AppendLine("*** End of custom event information");

    }

  }

}
