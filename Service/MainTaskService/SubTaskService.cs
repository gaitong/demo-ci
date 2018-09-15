using Data.Models;
using Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Service.MainTaskService
{
    public interface ISubTaskService
    {
        IEnumerable<SubTask> GetAll();
        SubTask GetBy(int id);
        IEnumerable<SubTask> GetByMainTaskId(int mainTaskId);
        bool Add(SubTask subtask);
        bool Update(SubTask subtask);
        bool Delete(SubTask subtask);
        bool DeleteByMainTaskId(int mainTaskId);
        bool Active(int id, bool active);
    }

    public class SubTaskService : ISubTaskService
    {
        private IMainTaskRepository _mainTaskRepository;
        private ISubTaskRepository _subTaskRepository;

        public SubTaskService(IMainTaskRepository mainTaskRepository, ISubTaskRepository subTaskRepository, bool initialization = true)
        {
            _mainTaskRepository = mainTaskRepository;
            _subTaskRepository = subTaskRepository;
            if (initialization)
            {
                _subTaskRepository = new SubTaskRepository();
                _mainTaskRepository = new MainTaskRepository();
            }
        }

        public IEnumerable<SubTask> GetAll()
        {
            return _subTaskRepository.GetAll();
        }

        public SubTask GetBy(int id)
        {
            return _subTaskRepository.FindBy(id);
        }

        public IEnumerable<SubTask> GetByMainTaskId(int mainTaskId)
        {
            return _subTaskRepository.FindByMainProjectId(mainTaskId);
        }

        public bool Add(SubTask subtask)
        {
            var mainTask = _mainTaskRepository.FindBy(subtask.MainTaskId);
            if(mainTask == null)
            {
                return false;
            }
            return _subTaskRepository.Add(subtask) > 0;
        }

        public bool Update(SubTask subtask)
        {
            return _subTaskRepository.Update(subtask) > 0;
        }

        public bool Delete(SubTask subtask)
        {
            return _subTaskRepository.Delete(subtask) > 0;
        }

        public bool DeleteByMainTaskId(int mainTaskId)
        {
            return _subTaskRepository.DeleteByMainTaskId(mainTaskId) > 0;
        }

        public bool Active(int id, bool active)
        {
            return _subTaskRepository.Active(id, active) > 0;
        }
    }
}