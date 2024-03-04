# User Management Technical Exercise

The exercise is an ASP.NET Core web application backed by Entity Framework Core, which faciliates management of some fictional users.
We recommend that you use [Visual Studio (Community Edition)](https://visualstudio.microsoft.com/downloads) or [Visual Studio Code](https://code.visualstudio.com/Download) to run and modify the application. 

**The application uses an in-memory database, so changes will not be persisted between executions.**

## Additional Packages
### Logging
I am using **log4net** for logging. Version `2.0.15` is installed using Package Manager for `UserManagement.Services` and `UserManagement.Web` projects. 
Logger configuration is saved in the `log4net.config` file. For this application `MemoryAppender` was chosen for
simplicity. Classes where logging is required load the configuration with the line `[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]`.

### Database Migrations
Since the models and the context are already created, code first approach was chosen.  
The following packages were installed for `UserManagement.Data` project:  
* Microsoft.EntityFrameworkCore 7.0.5
* Microsoft.EntityFrameworkCore.SqlServer 7.0.5
* Microsoft.Extensions.Configuration 7.0.0
* Microsoft.Extensions.Configuration.Json 7.0.0  

And for `UserManagement.Web` project:
* Microsoft.EntityFrameworkCore.Design 7.0.5  

The next steps are as follows:  
* Create the database in Microsoft SQL Server Management Studio (MSSMS)
* Update `appsettings.json` to include the connection string to the created database
* Update the `DataContext` `onConfiguring` method to no longer use the in memory database, but instead read the connection string from the `appsettings.json` file and use this string to establish a connection to the database.
* Open **Package Manager Console** in Visual Studio and make sure it is set to run on the `UserManagement.Data` project
* Create the first migration by typing `Add-Migration Initial` in the **Package Manager Console**
* Once the migration has been created successfully push the changes to the database using `Update-Database` command

## The Exercise
Complete as many of the tasks below as you can. These are split into 3 levels of difficulty 
* **Standard** - Functionality that is common when working as a web developer
* **Advanced** - Slightly more technical tasks and problem solving
* **Expert** - Tasks with a higher level of problem solving and architecture needed

### 1. Filters Section (Standard)

The users page contains 3 buttons below the user listing - **Show All**, **Active Only** and **Non Active**. Show All has already been implemented. Implement the remaining buttons using the following logic:
* Active Only – This should show only users where their `IsActive` property is set to `true`
* Non Active – This should show only users where their `IsActive` property is set to `false`

### 2. User Model Properties (Standard)

Add a new property to the `User` class in the system called `DateOfBirth` which is to be used and displayed in relevant sections of the app.

### 3. Actions Section (Standard)

Create the code and UI flows for the following actions
* **Add** – A screen that allows you to create a new user and return to the list
* **View** - A screen that displays the information about a user
* **Edit** – A screen that allows you to edit a selected user from the list  
* **Delete** – A screen that allows you to delete a selected user from the list

Each of these screens should contain appropriate data validation, which is communicated to the end user.

### 4. Data Logging (Advanced)

Extend the system to capture log information regarding primary actions performed on each user in the app.
* In the **View** screen there should be a list of all actions that have been performed against that user. 
* There should be a new **Logs** page, containing a list of log entries across the application.
* In the Logs page, the user should be able to click into each entry to see more detail about it.
* In the Logs page, think about how you can provide a good user experience - even when there are many log entries.

### 5. Extend the Application (Expert)

Make a significant architectural change that improves the application.
Structurally, the user management application is very simple, and there are many ways it can be made more maintainable, scalable or testable.
Some ideas are:
* Re-implement the UI using a client side framework connecting to an API. Use of Blazor is preferred, but if you are more familiar with other frameworks, feel free to use them.
* Update the data access layer to support asynchronous operations.
* Implement authentication and login based on the users being stored.
* Implement bundling of static assets.
* Update the data access layer to use a real database, and implement database schema migrations.

## Additional Notes

* Please feel free to change or refactor any code that has been supplied within the solution and think about clean maintainable code and architecture when extending the project.
* If any additional packages, tools or setup are required to run your completed version, please document these thoroughly.
