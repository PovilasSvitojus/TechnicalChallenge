using System;
using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;

namespace UserManagement.Data.Tests;

public class UserServiceTests
{
    [Fact]
    public void GetAll_WhenContextReturnsEntities_MustReturnSameEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = service.GetAll();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeSameAs(users);
    }

    [Fact]
    public void FilterByActive_WhenNoActiveUsersExist_ReturnsEmpty()
    {
        //Arrange. Also set the first user to be inactive
        var service = CreateService();
        var users = SetupUsers();
        users.ElementAt(0).IsActive = false;

        //Act
        var result = service.FilterByActive(true);

        //Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void FilterByActive_WhenOneActiveUserExists_ReturnsThatUser()
    {
        //Arrange
        var service = CreateService();
        var users = SetupUsers();

        //Act
        var result = service.FilterByActive(true);

        //Assert
        result.Should().Equal(users.ElementAt(0));
    }

    [Fact]
    public void FilterByActive_WhenMultipleActiveUsersExists_ReturnsAllOfThem()
    {
        //Arrange. Also set the second user to be active
        var service = CreateService();
        var users = SetupUsers();
        users.ElementAt(1).IsActive = true;

        //Act
        var result = service.FilterByActive(true);

        //Assert
        result.Should().Equal(users);
    }

    [Fact]
    public void FilterByActive_WhenNoInactiveUsersExist_ReturnsEmpty()
    {
        //Arrange. Also set second user to be active
        var service = CreateService();
        var users = SetupUsers();
        users.ElementAt(1).IsActive = true;

        //Act
        var result = service.FilterByActive(false);

        //Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void FilterByActive_WhenOneInactiveUserExists_ReturnsThatUser()
    {
        //Arrange
        var service = CreateService();
        var users = SetupUsers();

        //Act
        var result = service.FilterByActive(false);

        //Assert
        result.Should().Equal(users.ElementAt(1));
    }

    [Fact]
    public void FilterByActive_WhenMultipleInactiveUsersExists_ReturnsAllOfThem()
    {
        //Arrange. Also set first user to be inactive
        var service = CreateService();
        var users = SetupUsers();
        users.ElementAt(0).IsActive = false;

        //Act
        var result = service.FilterByActive(false);

        //Assert
        result.Should().Equal(users);
    }

    private IQueryable<User> SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com", DateOnly dateOfBirth = new DateOnly(), bool isActive = true)
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
        }.AsQueryable();

        _dataContext
            .Setup(s => s.GetAll<User>())
            .Returns(users);

        return users;
    }

    private readonly Mock<IDataContext> _dataContext = new();
    private UserService CreateService() => new(_dataContext.Object);
}
