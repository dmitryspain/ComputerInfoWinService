using ComputerInfo.DAL.Interfaces;
using ComputerInfo.Models.Models;
using ComputerInfoWebApi.Comparers;
using System;
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

        [Route("add")]
        public HttpResponseMessage Post([FromBody]PCInfo info)
        {
            var userNames = info.Users.Select(x => x.Name);
            var commonUsers = _userRepository.GetAllAsQueryable().Where(x => userNames.Contains(x.Name)).ToList();
            info.Users = info.Users.Except(commonUsers, new UserNameEqualityComparer()).ToList();

            var new_info = _infoRepository.GetSingle(x => x.RamLoad == info.RamLoad && x.CpuLoad == info.CpuLoad);
            if (new_info != null)
                new_info.DateTime = DateTime.Now;

            var infoId = _infoRepository.AddOrUpdate(new_info ?? info);
            
            if (commonUsers.Any())
            {
                commonUsers.ForEach(x => x.PCInfoId = infoId);
                commonUsers.ForEach(x => _userRepository.Update(x));
            }

            return Request.CreateResponse(HttpStatusCode.Created);
        }
    }
}
