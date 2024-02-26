using log4net.Core;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UserManagement.Web.Models.Logs;

public class LogListViewModel
{
    public List<LoggingEvent> Events { get; set; } = new();
    public List<SelectListItem> UserOptions { get; set; } = new();
    public List<SelectListItem> SeverityOptions { get; set; } = new();
}
