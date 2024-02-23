using System.Collections.Generic;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;

public interface IUserService 
{
    /// <summary>
    /// Return users by active state.
    /// </summary>
    /// <param name="isActive">
    /// True for active users. False for inactive users.
    /// </param>
    /// <returns>
    /// A filtered list of users.
    /// </returns>
    IEnumerable<User> FilterByActive(bool isActive);

    /// <summary>
    /// Returns all users.
    /// </summary>
    /// <returns>
    /// A full list of users.
    /// </returns>
    IEnumerable<User> GetAll();

    /// <summary>
    /// Returns a single user by id.
    /// </summary>
    /// <param name="id">
    /// ID of the user to be returned.
    /// </param>
    /// <returns>
    /// A single user object with the specified id.
    /// </returns>
    User? GetById(int id);

    /// <summary>
    /// Creates a new user entry in the database.
    /// </summary>
    /// <param name="user">
    /// User object to be added to the database.
    /// </param>
    /// <returns>
    /// User object of a newly created user.
    /// </returns>
    User CreateUser(User user);

    /// <summary>
    /// Updates an user entry in the database.
    /// </summary>
    /// <param name="user">
    /// Updated user object to be changed in the database.
    /// </param>
    /// <returns>
    /// User object of a newly updated user.
    /// </returns>
    User UpdateUser(User user);

    /// <summary>
    /// Deletes an user entry from the database.
    /// </summary>
    /// <param name="id">
    /// Id of the user to be deleted.
    /// </param>
    void DeleteUser(int id);

}
