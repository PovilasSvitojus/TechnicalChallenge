using System.Collections.Generic;
using System.Linq;
using log4net.Core;
using UserManagement.Models;
using UserManagement.Services.Interfaces;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]

namespace UserManagement.Services.Implementations;
internal class LogService : ILogService
{
    private static readonly log4net.ILog logger = log4net.LogManager.GetLogger("UserController");

    /// <summary>
    /// Returns all log events from the current session.
    /// </summary>
    /// <returns>
    /// A full list of log4net.Core.LoggingEvent from the current session.
    /// </returns>
    public List<LoggingEvent> GetFullLogs()
    {
        var appender = log4net.LogManager.GetRepository().GetAppenders().OfType<log4net.Appender.MemoryAppender>().Single(); // Assumes exactly one MemoryAppender
        var events = appender.GetEvents();

        return events.ToList<LoggingEvent>();
    }

    /// <summary>
    /// Returns a list of logged actions involving the specified user.
    /// </summary>
    /// <param name="id">
    /// ID of the user.
    /// </param>
    /// <returns>
    /// A list of log4net.Core.LoggingEvent associated with the user.
    /// </returns>
    public List<LoggingEvent> GetUserLog(int id)
    {
        List<LoggingEvent> userLogs = new List<LoggingEvent>();
        var appender = log4net.LogManager.GetRepository().GetAppenders().OfType<log4net.Appender.MemoryAppender>().Single(); // Assumes exactly one MemoryAppender
        var events = appender.GetEvents();
        foreach( var logEvent in events)
        {
            User user = (User)logEvent.LookupProperty("user");
            if(user != null && user.Id == id)
            {
                userLogs.Add(logEvent);
            }
        }

        return userLogs;
    }
}
