using log4net.Core;
using UserManagement.Models;

namespace UserManagement.Web.Models.Users;

public class UserLogViewModel
{
    public User User { get; set; } = new();
    public List<LoggingEvent> UserEvents { get; set; } = new();
}
