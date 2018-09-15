using Data.Models;
using Data.Repository;
using Moq;
using NUnit.Framework;
using Service.MainTaskService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.SubTaskServiceTest
{
    [TestFixture]
    public class SubTaskServiceTest
    {
        [Test]
        public void GivenNotHaveSubTaskInDatabase_WhenGetAll_ThenReturnNotNullAndCountEqualZero()
        {
            var expected = 0;
            Mock<ISubTaskRepository> mockSubTaskRepository = new Mock<ISubTaskRepository>();
            mockSubTaskRepository.Setup(method => method.GetAll()).Returns(new List<SubTask>());

            Mock<IMainTaskRepository> mockMainTaskRepository = new Mock<IMainTaskRepository>();

            var subTaskService = new SubTaskService(mockMainTaskRepository.Object, mockSubTaskRepository.Object, false);

            var subTasks = subTaskService.GetAll();
            Assert.NotNull(subTasks);
            Assert.AreEqual(expected, subTasks.Count());
        }

        [Test]
        public void GivenHaveSubTaskInDatabase_WhenGetAll_ThenReturnNotNullAndCountMoreThanZero()
        {
            var expected = 0;
            Mock<ISubTaskRepository> mockSubTaskRepository = new Mock<ISubTaskRepository>();
            mockSubTaskRepository.Setup(method => method.GetAll()).Returns(GetSubTasks());

            Mock<IMainTaskRepository> mockMainTaskRepository = new Mock<IMainTaskRepository>();

            var subTaskService = new SubTaskService(mockMainTaskRepository.Object, mockSubTaskRepository.Object, false);

            var subTasks = subTaskService.GetAll();
            Assert.NotNull(subTasks);
            Assert.Less(expected, subTasks.Count());
        }

        [Test]
        public void GivenGetById_WhenFoundId_ThenReturnSubTaskDetail()
        {
            Mock<ISubTaskRepository> mockSubTaskRepository = new Mock<ISubTaskRepository>();
            mockSubTaskRepository.Setup(method => method.FindBy(It.IsAny<int>())).Returns((int id)=>GetSubTasks().FirstOrDefault(st => st.Id == id));

            Mock<IMainTaskRepository> mockMainTaskRepository = new Mock<IMainTaskRepository>();

            var subTaskService = new SubTaskService(mockMainTaskRepository.Object, mockSubTaskRepository.Object, false);

            var subTaskInput = GetSubTasks().FirstOrDefault();
            var subTasks = subTaskService.GetBy(subTaskInput.Id);
            Assert.NotNull(subTasks);
            Assert.AreEqual(subTasks.Id, subTaskInput.Id);
            Assert.AreEqual(subTasks.Detail, subTaskInput.Detail);
            Assert.AreEqual(subTasks.MainTaskId, subTaskInput.MainTaskId);
            Assert.AreEqual(subTasks.Active, subTaskInput.Active);
        }

        [Test]
        public void GivenGetById_WhenNotFoundId_ThenReturnNull()
        {
            Mock<ISubTaskRepository> mockSubTaskRepository = new Mock<ISubTaskRepository>();
            mockSubTaskRepository.Setup(method => method.FindBy(It.IsAny<int>())).Returns((int id) => GetSubTasks().FirstOrDefault(st => st.Id == id));

            Mock<IMainTaskRepository> mockMainTaskRepository = new Mock<IMainTaskRepository>();

            var subTaskService = new SubTaskService(mockMainTaskRepository.Object, mockSubTaskRepository.Object, false);

            var subTasks = subTaskService.GetBy(0);
            Assert.IsNull(subTasks);
        }

        [Test]
        public void GivenGetByMainTaskId_WhenFoundId_ThenReturnListSubTask()
        {
            Mock<ISubTaskRepository> mockSubTaskRepository = new Mock<ISubTaskRepository>();
            mockSubTaskRepository.Setup(method => method.FindByMainProjectId(It.IsAny<int>())).Returns((int id) => GetSubTasks().Where(st => st.MainTaskId == id));

            Mock<IMainTaskRepository> mockMainTaskRepository = new Mock<IMainTaskRepository>();

            var subTaskService = new SubTaskService(mockMainTaskRepository.Object, mockSubTaskRepository.Object, false);

            var subTaskInput = GetSubTasks().FirstOrDefault();
            var subTasks = subTaskService.GetByMainTaskId(subTaskInput.MainTaskId);
            var expected = GetSubTasks().Where(st => st.MainTaskId == subTaskInput.MainTaskId).Count();
            Assert.NotNull(subTasks);
            Assert.AreEqual(expected, subTasks.Count());
        }

        [Test]
        public void GivenGetByMainTaskId_WhenNotFoundId_ThenReturnNotNullAndCountEqualZero()
        {
            var expected = 0;
            Mock<ISubTaskRepository> mockSubTaskRepository = new Mock<ISubTaskRepository>();
            mockSubTaskRepository.Setup(method => method.FindByMainProjectId(It.IsAny<int>())).Returns((int id) => GetSubTasks().Where(st => st.MainTaskId == id));

            Mock<IMainTaskRepository> mockMainTaskRepository = new Mock<IMainTaskRepository>();

            var subTaskService = new SubTaskService(mockMainTaskRepository.Object, mockSubTaskRepository.Object, false);

            var subTasks = subTaskService.GetByMainTaskId(0);
            Assert.NotNull(subTasks);
            Assert.AreEqual(expected, subTasks.Count());
        }

        [Test]
        public void GivenAddNewSubTask_WhenNotFoundMainTaskId_ThenReturnFalse()
        {
            Mock<ISubTaskRepository> mockSubTaskRepository = new Mock<ISubTaskRepository>();

            Mock<IMainTaskRepository> mockMainTaskRepository = new Mock<IMainTaskRepository>();
            mockMainTaskRepository.Setup(method => method.FindBy(It.IsAny<int>())).Returns((int id) => GetMainTasks().FirstOrDefault(mt => mt.Id == id));

            var subTaskService = new SubTaskService(mockMainTaskRepository.Object, mockSubTaskRepository.Object, false);

            var subtaskInput = GetSubTasks().FirstOrDefault();
            subtaskInput.Id = 0;
            var subTasks = subTaskService.Add(subtaskInput);
            Assert.IsFalse(subTasks);
        }

        [Test]
        public void GivenAddNewSubTask_WhenFoundMainTaskId_ThenReturnTrue()
        {
            Mock<ISubTaskRepository> mockSubTaskRepository = new Mock<ISubTaskRepository>();
            mockSubTaskRepository.Setup(st => st.Add(It.IsAny<SubTask>())).Returns(1);

            Mock<IMainTaskRepository> mockMainTaskRepository = new Mock<IMainTaskRepository>();
            mockMainTaskRepository.Setup(method => method.FindBy(It.IsAny<int>())).Returns((int id) => GetMainTasks().FirstOrDefault(mt => mt.Id == id));

            var subTaskService = new SubTaskService(mockMainTaskRepository.Object, mockSubTaskRepository.Object, false);

            var subtaskInput = GetSubTasks().FirstOrDefault();
            var subTasks = subTaskService.Add(subtaskInput);
            Assert.IsTrue(subTasks);
        }

        [Test]
        public void GivenUpdateDetailSubTask_WhenSuccess_ThenReturnTrue()
        {
            Mock<ISubTaskRepository> mockSubTaskRepository = new Mock<ISubTaskRepository>();
            mockSubTaskRepository.Setup(st => st.Update(It.IsAny<SubTask>())).Returns(1);

            Mock<IMainTaskRepository> mockMainTaskRepository = new Mock<IMainTaskRepository>();
            mockMainTaskRepository.Setup(method => method.FindBy(It.IsAny<int>())).Returns((int id) => GetMainTasks().FirstOrDefault(mt => mt.Id == id));

            var subTaskService = new SubTaskService(mockMainTaskRepository.Object, mockSubTaskRepository.Object, false);

            var subtaskInput = GetSubTasks().FirstOrDefault();
            var subTasks = subTaskService.Update(subtaskInput);
            Assert.IsTrue(subTasks);
        }

        [Test]
        public void GivenDeleteSubTask_WhenSuccess_ThenReturnTrue()
        {
            Mock<ISubTaskRepository> mockSubTaskRepository = new Mock<ISubTaskRepository>();
            mockSubTaskRepository.Setup(st => st.Delete(It.IsAny<SubTask>())).Returns(1);

            Mock<IMainTaskRepository> mockMainTaskRepository = new Mock<IMainTaskRepository>();
            mockMainTaskRepository.Setup(method => method.FindBy(It.IsAny<int>())).Returns((int id) => GetMainTasks().FirstOrDefault(mt => mt.Id == id));

            var subTaskService = new SubTaskService(mockMainTaskRepository.Object, mockSubTaskRepository.Object, false);

            var subtaskInput = GetSubTasks().FirstOrDefault();
            var subTasks = subTaskService.Delete(subtaskInput);
            Assert.IsTrue(subTasks);
        }

        [Test]
        public void GivenDeleteSubTaskByMainTaskId_WhenSuccess_ThenReturnTrue()
        {
            Mock<ISubTaskRepository> mockSubTaskRepository = new Mock<ISubTaskRepository>();
            mockSubTaskRepository.Setup(st => st.DeleteByMainTaskId(It.IsAny<int>())).Returns(1);

            Mock<IMainTaskRepository> mockMainTaskRepository = new Mock<IMainTaskRepository>();
 
            var subTaskService = new SubTaskService(mockMainTaskRepository.Object, mockSubTaskRepository.Object, false);

            var subtaskInput = GetSubTasks().FirstOrDefault();
            var subTasks = subTaskService.DeleteByMainTaskId(subtaskInput.MainTaskId);
            Assert.IsTrue(subTasks);
        }

        [Test]
        public void GivenActiveToEnable_WhenSuccess_ThenReturnTrue()
        {
            Mock<ISubTaskRepository> mockSubTaskRepository = new Mock<ISubTaskRepository>();
            mockSubTaskRepository.Setup(st => st.Active(It.IsAny<int>(),It.IsAny<bool>())).Returns(1);

            Mock<IMainTaskRepository> mockMainTaskRepository = new Mock<IMainTaskRepository>();

            var subTaskService = new SubTaskService(mockMainTaskRepository.Object, mockSubTaskRepository.Object, false);

            var subtaskInput = GetSubTasks().FirstOrDefault(st => !st.Active);
            subtaskInput.Active = true;
            var subTasks = subTaskService.Active(subtaskInput.Id, subtaskInput.Active);
            Assert.IsTrue(subTasks);
        }

        private List<MainTask> GetMainTasks()
        {
            return new List<MainTask>
            {
                new MainTask{ Id = 1, Name="task_A"},
                new MainTask{ Id = 2, Name="task_B"},
            };
        }

        private List<SubTask> GetSubTasks()
        {
            return new List<SubTask>
            {
                new SubTask{ Id = 1, Detail="sub_1", Active = false, MainTaskId = 1},
                new SubTask{ Id = 2, Detail="sub_2", Active = false, MainTaskId = 1},
                new SubTask{ Id = 3, Detail="sub_1", Active = true, MainTaskId = 2}
            };
        }
    }
}
