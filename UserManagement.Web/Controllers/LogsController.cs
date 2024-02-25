using System.Linq;
using log4net.Core;
using UserManagement.Services.Interfaces;

namespace UserManagement.Web.Controllers;

[Route("logs")]
public class LogsController : Controller
{
    private readonly ILogService _logService;
    public LogsController(ILogService logService) => _logService = logService;

    [HttpGet]
    public ViewResult List()
    {
        List<LoggingEvent> logs = new List<LoggingEvent>();
        logs = _logService.GetFullLogs();

        return View(logs);
    }

    [HttpGet("{id}")]
    public ViewResult ViewLog(int id)
    {
        LoggingEvent logEvent = _logService.GetFullLogs().ElementAt(id);

        return View(logEvent);
    }
}
