using System;
using System.Linq;
using log4net.Core;
using Microsoft.AspNetCore.Mvc.Rendering;
using UserManagement.Models;
using UserManagement.Services.Interfaces;
using UserManagement.Web.Models.Logs;

namespace UserManagement.Web.Controllers;

[Route("logs")]
public class LogsController : Controller
{
    private readonly ILogService _logService;
    public LogsController(ILogService logService) => _logService = logService;

    [HttpGet]
    public ViewResult List(DateTime? dateFrom, DateTime? dateTo, int? userId, int? severityId)
    {
        List<LoggingEvent> logs = new List<LoggingEvent>();
        List<User> users = new List<User>();
        List<Level> severityLevels = new List<Level>();
        logs = _logService.GetFullLogs();

        //Get all unique users that have a log entry associated with them
        //Also get all unique severity levels
        foreach (var logEvent in logs)
        {
            User user = (User)logEvent.LookupProperty("user");
            if (user != null && !users.Exists(u => u.Id == user.Id))
            {
                users.Add(user);
            }
            if (!severityLevels.Exists(l => l == logEvent.Level))
            {
                severityLevels.Add(logEvent.Level);
            }
        }

        //Generate the lists for both dropdowns in the filter section
        List<SelectListItem> userOptions = new List<SelectListItem>();
        userOptions = users.Select(u => new SelectListItem
        {
            Value = u.Id.ToString(),
            Text = u.Forename + " " + u.Surname,
            Selected = u.Id == userId
        }).ToList();

        //Id is index in the list.
        List<SelectListItem> severityOptions = new List<SelectListItem>();
        severityOptions = severityLevels.Select(l => new SelectListItem
        {
            Value = severityLevels.FindIndex(lvl => lvl.DisplayName == l.DisplayName).ToString(),
            Text = l.DisplayName,
            Selected = severityLevels.FindIndex(lvl => lvl.DisplayName == l.DisplayName) == severityId
        }).ToList();

        //Apply the filters
        if (userId != null)
        {
            logs = _logService.GetUserLog((int)userId);
        }
        if (dateFrom != null)
        {
            logs = logs.Where(l => l.TimeStamp > dateFrom).ToList();
        }
        if( dateTo != null)
        {
            logs = logs.Where(l => l.TimeStamp < dateTo).ToList();
        }
        if (severityId != null)
        {
            logs = logs.Where(l => l.Level == severityLevels.ElementAt((int)severityId)).ToList();
        }

        var model = new LogListViewModel
        {
            Events = logs,
            UserOptions = userOptions,
            SeverityOptions = severityOptions
        };

        return View(model);
    }

    [HttpGet("{id}")]
    public ViewResult ViewLog(int id)
    {
        LoggingEvent logEvent = _logService.GetFullLogs().ElementAt(id);

        return View(logEvent);
    }
}
