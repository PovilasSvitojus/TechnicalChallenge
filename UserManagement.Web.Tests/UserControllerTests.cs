using System;
using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;
using UserManagement.WebMS.Controllers;

namespace UserManagement.Data.Tests;

public class UserControllerTests
{
    [Fact]
    public void List_WhenCalledWithNullParameter_CallsUserServiceGetAllMethod()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers();

        // Act
        var result = controller.List(null);

        // Assert
        _userService.Verify(s => s.GetAll(), Times.Exactly(1));
    }

    [Fact]
    public void List_WhenServiceReturnsUsers_ModelMustContainUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = controller.List(null);

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().BeEquivalentTo(users);
    }

    [Fact]
    public void List_WhenCalledWithTrue_CallsUserServiceFilterByActiveMethod()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers();

        // Act
        var result = controller.List(true);

        // Assert
        _userService.Verify(s => s.GetAll(), Times.Exactly(0));
        _userService.Verify(s => s.FilterByActive(true), Times.Exactly(1));
    }

    [Fact]
    public void List_WhenCalledWithTrue_ReturnsOneActiveUser()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers();
        var activeUsers = users.Where(u => u.IsActive == true);

        // Act
        var result = controller.List(true);

        // Assert
        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().HaveCount(1)
            .And.BeEquivalentTo(activeUsers);
    }

    [Fact]
    public void List_WhenCalledWithFalse_CallsUserServiceFilterByActiveMethod()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers();

        // Act
        var result = controller.List(false);

        // Assert
        _userService.Verify(s => s.GetAll(), Times.Exactly(0));
        _userService.Verify(s => s.FilterByActive(false), Times.Exactly(1));
    }

    [Fact]
    public void List_WhenCalledWithFalse_ReturnsOneInactiveUser()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers();
        var inactiveUsers = users.Where(u => u.IsActive == false);

        // Act
        var result = controller.List(false);

        // Assert
        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().HaveCount(1)
            .And.BeEquivalentTo(inactiveUsers);
    }



    private User[] SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com", DateOnly dateOfBirth = new DateOnly(), bool isActive = true)
    {
        var users = new[]
        {
            new User
            {
                Forename = forename,
                Surname = surname,
                Email = email,
                DateOfBirth = dateOfBirth,
                IsActive = isActive
            },
             new User
            {
                Forename = forename,
                Surname = surname,
                Email = email,
                DateOfBirth = dateOfBirth,
                IsActive = !isActive
            }
        };

        _userService
            .Setup(s => s.GetAll())
            .Returns(users);
        _userService
            .Setup(s => s.FilterByActive(true))
            .Returns(users.Where(u => u.IsActive == true));
        _userService
            .Setup(s => s.FilterByActive(false))
            .Returns(users.Where(u => u.IsActive == false));

        return users;
    }

    private readonly Mock<IUserService> _userService = new();
    private UsersController CreateController() => new(_userService.Object);
}
