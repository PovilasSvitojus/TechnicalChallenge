using System;
using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;

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
        User? user = _userService.GetById(id);
        if (user == null)
        {
            return View("Error");
        }
        return View(user);
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
        
        return RedirectToAction(nameof(List));
    }

    [Route("EditUser")]
    public ViewResult EditUser(int id)
    {
        User? user = _userService.GetById(id);
        if (user == null)
        {
            return View("Error");
        }
        return View(user);
    }

    [Route("Edit")]
    [HttpPut("{id}")]
    public IActionResult Edit(int id, User user)
    {
        var result = _userService.UpdateUser(user);

        return RedirectToAction(nameof(List));
    }

    [Route("DeleteUser")]
    public ViewResult DeleteUser(int id)
    {
        User? user = _userService.GetById(id);
        if (user == null)
        {
            return View("Error");
        }
        return View(user);
    }

    [Route("Delete")]
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _userService.DeleteUser(id);
        return RedirectToAction(nameof(List));
    }
}
