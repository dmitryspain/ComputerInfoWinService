using ComputerInfo.Models.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
