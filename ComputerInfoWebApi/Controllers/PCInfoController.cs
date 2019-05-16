using ComputerInfo.DAL.Interfaces;
using ComputerInfo.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ComputerInfoWebApi.Controllers
{
    [RoutePrefix("api/pcinfo")]
    public class PCInfoController : ApiController
    {
        private readonly IRepository<PCInfo> _infoRepository;
        private readonly IRepository<User> _userRepository;

        public PCInfoController(IRepository<PCInfo> repository, IRepository<User> userRepository)
        {
            _infoRepository = repository;
            _userRepository = userRepository;
        }

        bool isNewUser(IEnumerable<string> usersFromRequest, IEnumerable<string> usersFromDb)
        {
            foreach (var userName in usersFromDb)
            {
                if (!usersFromRequest.Contains(userName))
                    return true;
            }

            return false;
        }

        [Route("add")]
        public void Post([FromBody]PCInfo info)
        {
            //info.Users.ForEach(user => _userRepository.Create(user));
            

            var asd = _infoRepository.GetAllAsQueryable()
                .Where(x => x.CpuLoad != info.CpuLoad && x.RamLoad != info.RamLoad);

            if(asd == null)
            {
                _infoRepository.Create(info);
            }
            else
            {

            }

            var item = _infoRepository.GetById(3);
        }
    }
}
