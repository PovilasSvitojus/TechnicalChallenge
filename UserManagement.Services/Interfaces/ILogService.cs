using System.Collections.Generic;
using log4net.Core;

namespace UserManagement.Services.Interfaces;
public interface ILogService
{

    /// <summary>
    /// Returns all log events from the current session.
    /// </summary>
    /// <returns>
    /// A full list of log4net.Core.LoggingEvent from the current session.
    /// </returns>
    public List<LoggingEvent> GetFullLogs();

    /// <summary>
    /// Returns a list of logged actions involving the specified user.
    /// </summary>
    /// <param name="id">
    /// ID of the user.
    /// </param>
    /// <returns>
    /// A list of log4net.Core.LoggingEvent associated with the user.
    /// </returns>
    List<LoggingEvent> GetUserLog(int id);
}
