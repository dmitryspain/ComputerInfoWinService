using ComputerInfo.DAL.EF;
using ComputerInfo.DAL.Interfaces;
using ComputerInfo.DAL.Repositories.Base;
using ComputerInfo.Models.Models;

namespace ComputerInfo.DAL.Repositories
{
    public class PCInfoRepository : BaseRepository<PCInfo>, IRepository<PCInfo>
    {
        public PCInfoRepository(PCInfoContext context)
            :base(context)
        {
        }
    }
}
