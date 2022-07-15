using Logic.Models;
using System.Data.Entity;

namespace Logic.Services
{
    public class MyDBContext : DbContext
    {
        public MyDBContext() : base("(localDB)\\MSSQLLocalDB") { }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Report> Reports { get; set; }
    }
}