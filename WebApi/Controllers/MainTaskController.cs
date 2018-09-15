using Data.Models;
using Data.Repository;
using Service.MainTaskService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
namespace WebApi.Controllers
{
    [RoutePrefix("api/maintasks")]
    public class MainTaskController : ApiController
    {
        private IMainTaskService _mainTaskService;

        private IMainTaskRepository _mainTaskRepository;
        private ISubTaskRepository _subTaskRepository;

        public MainTaskController()
        {
            _mainTaskRepository = new MainTaskRepository();
            _subTaskRepository = new SubTaskRepository();

            _mainTaskService = new MainTaskService(_mainTaskRepository, _subTaskRepository);
        }

        [HttpGet]
        public IEnumerable<MainTask> GetAll()
        {
            return _mainTaskService.GetAll();
        }

        [HttpGet]
        [Route("{id:int}")]
        public MainTask GetBy(int id)
        {
            return _mainTaskService.GetBy(id);
        }

        [HttpPost]
        public bool Add(string name)
        {
            var mainTask = new MainTask { Name = name };
            return _mainTaskService.Add(mainTask);
        }

        [HttpPut]
        [Route("{id:int}")]
        public bool Update(MainTask mainTask)
        {
            return _mainTaskService.Update(mainTask);
        }

        [HttpDelete]
        public bool Delete(int id)
        {
            var mainTask = new MainTask { Id = id };
            return _mainTaskService.Delete(mainTask);

        }
    }
}
