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
        private readonly IRepository<PCInfo> _repository;

        public PCInfoController(IRepository<PCInfo> repository)
        {
            _repository = repository;
        }

        [Route("add")]
        public void Post([FromBody]PCInfo info)
        {
            _repository.Create(info);

            var item = _repository.GetById(1);
        }

        public string Get()
        {
            return "TEST DATA";
        }
    }
}
