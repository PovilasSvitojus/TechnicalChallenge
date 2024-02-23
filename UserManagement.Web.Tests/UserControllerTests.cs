using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NuGet.ContentModel;
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

    [Fact]
    public void ViewUser_WhenCalledWithId0_CallsUserServiceGetByIdMethodWithId0()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers();

        //Act
        var result = controller.ViewUser(0);

        //Assert
        _userService.Verify(s => s.GetById(0), Times.Exactly(1));
    }

    [Fact]
    public void ViewUser_WhenCalledWithId0_ReturnsUserWithId0()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers();
        _userService.Setup(s => s.GetById(0)).Returns(users[0]);

        //Act
        var result = controller.ViewUser(0);

        //Assert
        result.Model.Should().BeOfType<User>()
            .Which.Id.Should().Be(0);
    }

    [Fact]
    public void ViewUser_WhenCalledWithId2_ReturnsNullAndDirectsToErrorView()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers();

        //Act
        var result = controller.ViewUser(2);

        //Assert
        result.Model.Should().BeNull();
        result.ViewName.Should().Be("Error");
    }

    [Fact]
    public void CreateUser_WhenCalled_ReturnsCreateUserView()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers();

        //Act
        var result = controller.CreateUser();

        //Assert
        //Null ViewName points to the view with the same name as the function.
        result.ViewName.Should().BeNull();
    }

    [Fact]
    public void Create_WhenCalledWithAUser_CallsUserServiceCreateUserMethod()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers();

        //Act
        var result = controller.Create(users[0]);

        //Assert
        _userService.Verify(s => s.CreateUser(users[0]), Times.Exactly(1));
    }

    [Fact]
    public void Create_WhenCalledWithAUser_RedirectsToTheListView()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers();

        //Act
        var result = (RedirectToActionResult)controller.Create(users[0]);

        //Assert
        result.ActionName.Should().Be("List");
    }

    [Fact]
    public void EditUser_WhenCalledWithId0_CallsUserServiceGetByIdMethodWithId0()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers();

        //Act
        var result = controller.EditUser(0);

        //Assert
        _userService.Verify(s => s.GetById(0), Times.Exactly(1));
    }

    [Fact]
    public void EditUser_WhenCalledWithId0_PassesUserWithId0ToTheView()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers();
        _userService.Setup(s => s.GetById(0)).Returns(users[0]);

        //Act
        var result = controller.EditUser(0);

        //Assert
        result.Model.Should().BeOfType<User>()
            .Which.Id.Should().Be(0);
    }

    [Fact]
    public void EditUser_WhenCalledWithId2_ReturnsDirectsToErrorView()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers();

        //Act
        var result = controller.EditUser(2);

        //Assert
        result.ViewName.Should().Be("Error");
    }

    [Fact]
    public void Edit_WhenCalledWithId0AndAUser_CallsUserServiceUpdateUserMethod()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers();

        //Act
        var result = controller.Edit(0, users[0]);

        //Assert
        _userService.Verify(s => s.UpdateUser(users[0]), Times.Exactly(1));
    }

    [Fact]
    public void Edit_WhenCalledWithAUser_RedirectsToTheListView()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers();

        //Act
        var result = (RedirectToActionResult)controller.Edit(0, users[0]);

        //Assert
        result.ActionName.Should().Be("List");
    }

    [Fact]
    public void DeleteUser_WhenCalledWithId0_CallsUserServiceGetByIdMethodWithId0()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers();

        //Act
        var result = controller.DeleteUser(0);

        //Assert
        _userService.Verify(s => s.GetById(0), Times.Exactly(1));
    }

    [Fact]
    public void DeleteUser_WhenCalledWithId0_PassesUserWithId0ToTheView()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers();
        _userService.Setup(s => s.GetById(0)).Returns(users[0]);

        //Act
        var result = controller.DeleteUser(0);

        //Assert
        result.Model.Should().BeOfType<User>()
            .Which.Id.Should().Be(0);
    }

    [Fact]
    public void DeleteUser_WhenCalledWithId2_ReturnsDirectsToErrorView()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers();

        //Act
        var result = controller.DeleteUser(2);

        //Assert
        result.ViewName.Should().Be("Error");
    }

    [Fact]
    public void Delete_WhenCalledWithId0_CallsUserServiceDeleteUserMethod()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers();

        //Act
        var result = controller.Delete(0);

        //Assert
        _userService.Verify(s => s.DeleteUser(0), Times.Exactly(1));
    }

    [Fact]
    public void Delete_WhenCalledWithId0_RedirectsToTheListView()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers();

        //Act
        var result = (RedirectToActionResult)controller.Delete(0);

        //Assert
        result.ActionName.Should().Be("List");
    }


    private User[] SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com", DateTime dateOfBirth = new DateTime(), bool isActive = true)
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
