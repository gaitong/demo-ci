using Data.Models;
using Data.Repository;
using Service.MainTaskService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Viewmodel;

namespace WebApi.Controllers
{
    [RoutePrefix("api/subTasks")]
    public class SubTaskController : ApiController
    {
        private ISubTaskService _subTaskService;

        private readonly IMainTaskRepository _mainTaskRepository;
        private readonly ISubTaskRepository _subTaskRepository;

        public SubTaskController()
        {
            _mainTaskRepository = new MainTaskRepository();
            _subTaskRepository = new SubTaskRepository();

            _subTaskService = new SubTaskService(_mainTaskRepository, _subTaskRepository);
        }

        [HttpGet]
        public IEnumerable<SubTask> GetAll()
        {
            return _subTaskService.GetAll();
        }

        [HttpGet]
        [Route("{id}")]
        public SubTask GetBy(int id)
        {
            return _subTaskService.GetBy(id);
        }

        [HttpGet]
        [Route("getByMainTaskId/{mainTaskId}")]
        public IEnumerable<SubTask> GetByMainTaskId(int mainTaskId)
        {
            return _subTaskService.GetByMainTaskId(mainTaskId);
        }

        [HttpPost]
        public bool Add(SubTaskInput subTaskInput)
        {
            var subTask = new SubTask
            {
                MainTaskId = subTaskInput.MainTaskId,
                Detail = subTaskInput.Detail
            };
            return _subTaskService.Add(subTask);
        }

        [HttpPut]
        [Route("{id}")]
        public bool Update(int id, int mainTaskId, string detail)
        {
            var subtask = new SubTask { Id = id, MainTaskId = mainTaskId, Detail = detail };
            return _subTaskService.Update(subtask);
        }

        [HttpDelete]
        [Route("{id}")]
        public bool Delete(int id)
        {
            var subTask = new SubTask { Id = id };
            return _subTaskService.Delete(subTask);
        }

        [HttpDelete]
        [Route("deleteByMainTaskId/{mainTaskId}")]
        public bool DeleteByMainTaskId(int mainTaskId)
        {
            return _subTaskService.DeleteByMainTaskId(mainTaskId);
        }

        [HttpPut]
        [Route("active")]
        public bool Active(int id, bool active)
        {
            return _subTaskService.Active(id, active);
        }
    }
}
