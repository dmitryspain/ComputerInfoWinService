using ComputerInfo.Models.Models;
using System.Data.Entity;

namespace ComputerInfo.DAL.EF
{
    public class PCInfoContext : DbContext
    {
        public PCInfoContext(string connectionString)
            :base(connectionString)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<PCInfoContext>());
        }

        public DbSet<PCInfo> PCInfos { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
