using Data.Models;
using Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Service.MainTaskService
{
    public interface IMainTaskService
    {
        IEnumerable<MainTask> GetAll();
        MainTask GetBy(int id);
        bool Add(MainTask mainTask);
        bool Update(MainTask mainTask);
        bool Delete(MainTask mainTask);
    }

    public class MainTaskService : IMainTaskService
    {
        private IMainTaskRepository _mainTaskRepository;
        private ISubTaskRepository _subTaskRepository;

        public MainTaskService(IMainTaskRepository mainTaskRepository, ISubTaskRepository subTaskRepository, bool initialization = true)
        {
            _mainTaskRepository = mainTaskRepository;
            _subTaskRepository = subTaskRepository;
            if (initialization)
            {
                _mainTaskRepository = new MainTaskRepository();
                _subTaskRepository = new SubTaskRepository();
                
            }
        }

        public IEnumerable<MainTask> GetAll()
        {
            return _mainTaskRepository.GetAll();
        }

        public MainTask GetBy(int id)
        {
            return _mainTaskRepository.FindBy(id);
        }

        public bool Add(MainTask mainTask)
        {
            return _mainTaskRepository.Add(mainTask) > 0;
        }

        public bool Update(MainTask mainTask)
        {
            return _mainTaskRepository.Update(mainTask) > 0;
        }

        public bool Delete(MainTask mainTask)
        {
            var subtasks = _subTaskRepository.FindByMainProjectId(mainTask.Id);
            if (subtasks.Any())
            {
                foreach (var subtask in subtasks)
                {
                    var _sub = new SubTask { Id = subtask.Id };
                    _subTaskRepository.Delete(_sub);
                }
            }
            return _mainTaskRepository.Delete(mainTask) > 0;
        }
    }
}