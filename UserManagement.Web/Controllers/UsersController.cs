using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;
using log4net.Core;
using UserManagement.Services.Interfaces;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private static readonly log4net.ILog logger = log4net.LogManager.GetLogger("UserController");

    private readonly IUserService _userService;
    private readonly ILogService _logService;
    public UsersController(IUserService userService, ILogService logService)
    {
        _userService = userService;
        _logService = logService;
    }

    [HttpGet]
    public ViewResult List(bool? isActive)
    {
        IEnumerable<UserListItemViewModel> items;

        if (isActive == true)
        {
            items = _userService.FilterByActive(true).Select(p => new UserListItemViewModel
            {
                Id = p.Id,
                Forename = p.Forename,
                Surname = p.Surname,
                Email = p.Email,
                DateOfBirth = p.DateOfBirth,
                IsActive = p.IsActive
            });
        }
        else if (isActive == false)
        {
            items = _userService.FilterByActive(false).Select(p => new UserListItemViewModel
            {
                Id = p.Id,
                Forename = p.Forename,
                Surname = p.Surname,
                Email = p.Email,
                DateOfBirth = p.DateOfBirth,
                IsActive = p.IsActive
            });
        }
        else
        {
            items = _userService.GetAll().Select(p => new UserListItemViewModel
            {
                Id = p.Id,
                Forename = p.Forename,
                Surname = p.Surname,
                Email = p.Email,
                DateOfBirth = p.DateOfBirth,
                IsActive = p.IsActive
            });

        }

        var model = new UserListViewModel
        {
            Items = items.ToList()
        };

        return View(model);
    }

    [HttpGet("{id}")]
    public ViewResult ViewUser(int id)
    {
        List<LoggingEvent> userLogs = new List<LoggingEvent>();

        User? user = _userService.GetById(id);
        if (user == null)
        {
            //Clear the user property so that it doesn't get assigned to the wrong user
            log4net.ThreadContext.Properties["user"] = null;
            logger.Info($"Attempted to view User with id = {id}, which could not be found");

            return View("Error");
        }

        log4net.ThreadContext.Properties["user"] = user;
        logger.Info($"User {user.Forename} {user.Surname} was viewed");

        userLogs = _logService.GetUserLog(id);

        var model = new UserLogViewModel
        {
            User = user,
            UserEvents = userLogs
        };

        return View(model);
    }

    [Route("CreateUser")]
    public ViewResult CreateUser()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(User user)
    {
        //Assume that a newly created user is active
        user.IsActive = true;
        var result = _userService.CreateUser(user);

        log4net.ThreadContext.Properties["user"] = user;
        logger.Info($"User {user.Forename} {user.Surname} was created");

        return RedirectToAction(nameof(List));
    }

    [Route("EditUser")]
    public ViewResult EditUser(int id)
    {
        User? user = _userService.GetById(id);
        if (user == null)
        {
            //Clear the user property so that it doesn't get assigned to the wrong user
            log4net.ThreadContext.Properties["user"] = null;
            logger.Info($"Attempted to update User with id = {id}, which could not be found");

            return View("Error");
        }
        return View(user);
    }

    [Route("Edit")]
    [HttpPut("{id}")]
    public IActionResult Edit(int id, User user)
    {
        var result = _userService.UpdateUser(user);

        //Using Debug here just for demonstration purposes
        log4net.ThreadContext.Properties["user"] = user;
        logger.Debug($"User {user.Forename} {user.Surname} was updated");

        return RedirectToAction(nameof(List));
    }

    [Route("DeleteUser")]
    public ViewResult DeleteUser(int id)
    {
        User? user = _userService.GetById(id);
        if (user == null)
        {
            //Clear the user property so that it doesn't get assigned to the wrong user
            log4net.ThreadContext.Properties["user"] = null;
            logger.Info($"Attempted to delete User with id = {id}, which could not be found");

            return View("Error");
        }
        return View(user);
    }

    [Route("Delete")]
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        User? user = _userService.GetById(id);
        if (user == null)
        {
            //Clear the user property so that it doesn't get assigned to the wrong user
            log4net.ThreadContext.Properties["user"] = null;
            logger.Info($"Attempted to delete User with id = {id}, which could not be found");

            return View("Error");
        }

        _userService.DeleteUser(id);

        log4net.ThreadContext.Properties["user"] = user;
        logger.Info($"User {user.Forename} {user.Surname} was deleted");

        return RedirectToAction(nameof(List));
    }
}
