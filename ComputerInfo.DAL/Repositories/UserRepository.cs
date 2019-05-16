using ComputerInfo.DAL.EF;
using ComputerInfo.DAL.Interfaces;
using ComputerInfo.DAL.Repositories.Base;
using ComputerInfo.Models.Models;

namespace ComputerInfo.DAL.Repositories
{
    public class UserRepository : BaseRepository<User>, IRepository<User>
    {
        public UserRepository(PCInfoContext context)
            : base(context)
        {
        }
    }
}
