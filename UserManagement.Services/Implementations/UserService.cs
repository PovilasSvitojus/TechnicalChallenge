using System;
using System.Collections.Generic;
using System.Linq;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;
    public UserService(IDataContext dataAccess) => _dataAccess = dataAccess;

    /// <summary>
    /// Return users by active state.
    /// </summary>
    /// <param name="isActive">
    /// True for active users. False for inactive users.
    /// </param>
    /// <returns>
    /// A filtered list of users.
    /// </returns>
    public IEnumerable<User> FilterByActive(bool isActive)
    {
        var filteredUsers = _dataAccess.GetAll<User>().Where(user => user.IsActive == isActive);
        return filteredUsers;
    }

    /// <summary>
    /// Returns all users.
    /// </summary>
    /// <returns>
    /// A full list of users.
    /// </returns>
    public IEnumerable<User> GetAll() => _dataAccess.GetAll<User>();

    /// <summary>
    /// Returns a single user by id.
    /// </summary>
    /// <param name="id">
    /// ID of the user to be returned.
    /// </param>
    /// <returns>
    /// A single user object with the specified id.
    /// </returns>
    public User? GetById(int id)
    {
        //If the user can be found return the user, otherwise return null
        try
        {
            User user = _dataAccess.GetAll<User>().First(u => u.Id == id);
            return user;
        }
        catch (InvalidOperationException)
        {
            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// Creates a new user entry in the database.
    /// </summary>
    /// <param name="user">
    /// User object to be added to the database.
    /// </param>
    /// <returns>
    /// User object of a newly created user.
    /// </returns>
    public User CreateUser(User user)
    {
        _dataAccess.Create<User>(user);
        return user;
    }

    /// <summary>
    /// Updates an user entry in the database.
    /// </summary>
    /// <param name="user">
    /// Updated user object to be changed in the database.
    /// </param>
    /// <returns>
    /// User object of a newly updated user.
    /// </returns>
    public User UpdateUser(User user)
    {
        _dataAccess.Update<User>(user);
        return user;
    }

    /// <summary>
    /// Deletes an user entry from the database.
    /// </summary>
    /// <param name="id">
    /// Id of the user to be deleted.
    /// </param>
    public void DeleteUser(int id) {
        User? userToDelete = GetById(id);
        if (userToDelete != null)
        {
            _dataAccess.Delete<User>(userToDelete);
        }
    }

    
}
