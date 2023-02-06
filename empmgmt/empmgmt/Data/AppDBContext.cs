using DocumentFormat.OpenXml.Spreadsheet;
using empmgmt.Models.db;
using Microsoft.EntityFrameworkCore;

namespace empmgmt.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<login_user> login_user { get; set; }
        public DbSet<employee> employee { get; set; }
        public DbSet<skills> skills { get; set; }
        public DbSet<view_employee> view_employee { get; set; }
    }
}
