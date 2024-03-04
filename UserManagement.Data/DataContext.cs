using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UserManagement.Models;

namespace UserManagement.Data;

public class DataContext : DbContext, IDataContext
{
    public DataContext() => Database.EnsureCreated();

    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    /*    protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseInMemoryDatabase("UserManagement.Data.DataContext");*/

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //Read the connection string from appsettings.json file
        var builder = new ConfigurationBuilder()
                                      .SetBasePath(Directory.GetCurrentDirectory())
                                      .AddJsonFile("appsettings.json");
        var config = builder.Build();
        var connectionString = config.GetConnectionString("UserManagementConnection");

        //Use the connection string to connect to the SQL server
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder model)
        => model.Entity<User>().HasData(new[]
        {
            new User { Id = 1, Forename = "Peter", Surname = "Loew", Email = "ploew@example.com", DateOfBirth = new DateTime(1991, 03, 14), IsActive = true },
            new User { Id = 2, Forename = "Benjamin Franklin", Surname = "Gates", Email = "bfgates@example.com", DateOfBirth = new DateTime(1971, 07, 24), IsActive = true },
            new User { Id = 3, Forename = "Castor", Surname = "Troy", Email = "ctroy@example.com", DateOfBirth = new DateTime(1956, 11, 11), IsActive = false },
            new User { Id = 4, Forename = "Memphis", Surname = "Raines", Email = "mraines@example.com", DateOfBirth = new DateTime(1975, 10, 25), IsActive = true },
            new User { Id = 5, Forename = "Stanley", Surname = "Goodspeed", Email = "sgodspeed@example.com", DateOfBirth = new DateTime(1999, 01, 05), IsActive = true },
            new User { Id = 6, Forename = "H.I.", Surname = "McDunnough", Email = "himcdunnough@example.com", DateOfBirth = new DateTime(1983, 07, 22), IsActive = true },
            new User { Id = 7, Forename = "Cameron", Surname = "Poe", Email = "cpoe@example.com", DateOfBirth = new DateTime(2008, 12, 19), IsActive = false },
            new User { Id = 8, Forename = "Edward", Surname = "Malus", Email = "emalus@example.com", DateOfBirth = new DateTime(2010, 04, 08), IsActive = false },
            new User { Id = 9, Forename = "Damon", Surname = "Macready", Email = "dmacready@example.com", DateOfBirth = new DateTime(1998, 06, 24), IsActive = false },
            new User { Id = 10, Forename = "Johnny", Surname = "Blaze", Email = "jblaze@example.com", DateOfBirth = new DateTime(2000, 10, 03), IsActive = true },
            new User { Id = 11, Forename = "Robin", Surname = "Feld", Email = "rfeld@example.com", DateOfBirth = new DateTime(1966, 09, 30), IsActive = true },
        });

    public DbSet<User>? Users { get; set; }

    public IQueryable<TEntity> GetAll<TEntity>() where TEntity : class
        => base.Set<TEntity>();

    public void Create<TEntity>(TEntity entity) where TEntity : class
    {
        base.Add(entity);
        SaveChanges();
    }

    public new void Update<TEntity>(TEntity entity) where TEntity : class
    {
        base.Update(entity);
        SaveChanges();
    }

    public void Delete<TEntity>(TEntity entity) where TEntity : class
    {
        base.Remove(entity);
        SaveChanges();
    }
}
