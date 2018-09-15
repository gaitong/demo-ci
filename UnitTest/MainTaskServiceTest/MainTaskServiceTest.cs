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

namespace UnitTest.MainTaskTest
{
    [TestFixture]
    public class MainTaskServiceTest
    {
        [Test]
        public void GivenNotHaveMainTaskInDatabase_WhenGetAll_ThenReturnNotNullAndCountEqualZero()
        {
            var expected = 0;
            Mock<IMainTaskRepository> mockMainTaskRepository = new Mock<IMainTaskRepository>();
            mockMainTaskRepository.Setup(mt => mt.GetAll()).Returns(new List<MainTask>());

            Mock<ISubTaskRepository> mockSubTaskRepository = new Mock<ISubTaskRepository>();

            var mainTaskService = new MainTaskService(mockMainTaskRepository.Object, mockSubTaskRepository.Object, false);

            var mainTasks = mainTaskService.GetAll();
            Assert.IsNotNull(mainTasks);
            Assert.AreEqual(expected, mainTasks.Count());
        }

        [Test]
        public void GivenHaveMainTaskInDatabase_WhenGetAll_ThenReturnNotNullAndCountMoreThanZero()
        {
            var expected = 0;
            Mock<IMainTaskRepository> mockMainTaskRepository = new Mock<IMainTaskRepository>();
            mockMainTaskRepository.Setup(mt => mt.GetAll()).Returns(GetMainTasks());

            Mock<ISubTaskRepository> mockSubTaskRepository = new Mock<ISubTaskRepository>();

            var mainTaskService = new MainTaskService(mockMainTaskRepository.Object, mockSubTaskRepository.Object, false);

            var mainTasks = mainTaskService.GetAll();
            Assert.NotNull(mainTasks);
            Assert.Less(expected, mainTasks.Count());
        }

        [Test]
        public void GivenGetMainTaskId_WhenFoundId_ThenReturnMainTaskDetail()
        {
            Mock<IMainTaskRepository> mockMainTaskRepository = new Mock<IMainTaskRepository>();
            mockMainTaskRepository.Setup(mt => mt.FindBy(It.IsAny<int>())).Returns((int id) => GetMainTasks().First(mt => mt.Id == id));

            Mock<ISubTaskRepository> mockSubTaskRepository = new Mock<ISubTaskRepository>();

            var mainTaskService = new MainTaskService(mockMainTaskRepository.Object, mockSubTaskRepository.Object, false);

            var maintaskInput = GetMainTasks().First();
            var mainTasks = mainTaskService.GetBy(maintaskInput.Id);
            Assert.NotNull(mainTasks);
            Assert.AreEqual(mainTasks.Id, maintaskInput.Id);
            Assert.AreEqual(mainTasks.Name, maintaskInput.Name);
        }

        [Test]
        public void GivenGetMainTaskId_WhenNotFoundId_ThenReturnNull()
        {
            Mock<IMainTaskRepository> mockMainTaskRepository = new Mock<IMainTaskRepository>();
            mockMainTaskRepository.Setup(mt => mt.FindBy(It.IsAny<int>())).Returns((int id) => GetMainTasks().FirstOrDefault(mt => mt.Id == id));

            Mock<ISubTaskRepository> mockSubTaskRepository = new Mock<ISubTaskRepository>();

            var mainTaskService = new MainTaskService(mockMainTaskRepository.Object, mockSubTaskRepository.Object, false);

            var mainTasks = mainTaskService.GetBy(0);
            Assert.IsNull(mainTasks);
        }

        [Test]
        public void GivenAddNewMainTask_WhenComplete_ThenReturnTrue()
        {
            Mock<IMainTaskRepository> mockMainTaskRepository = new Mock<IMainTaskRepository>();
            mockMainTaskRepository.Setup(mt => mt.Add(It.IsAny<MainTask>())).Returns(1);

            Mock<ISubTaskRepository> mockSubTaskRepository = new Mock<ISubTaskRepository>();

            var mainTaskService = new MainTaskService(mockMainTaskRepository.Object, mockSubTaskRepository.Object, false);

            var maintask = GetMainTasks().FirstOrDefault();
            var response = mainTaskService.Add(maintask);
            Assert.IsTrue(response);
        }

        [Test]
        public void GivenUpdateMainTask_WhenComplete_ThenReturnTrue()
        {
            Mock<IMainTaskRepository> mockMainTaskRepository = new Mock<IMainTaskRepository>();
            mockMainTaskRepository.Setup(mt => mt.Update(It.IsAny<MainTask>())).Returns(1);

            Mock<ISubTaskRepository> mockSubTaskRepository = new Mock<ISubTaskRepository>();

            var mainTaskService = new MainTaskService(mockMainTaskRepository.Object, mockSubTaskRepository.Object, false);

            var maintask = GetMainTasks().FirstOrDefault();
            var response = mainTaskService.Update(maintask);
            Assert.IsTrue(response);
        }

        [Test]
        public void GivenDeletteMainTaskNotHaveSubTask_WhenComplete_ThenReturnTrueAndVerifyDeleteSubTask()
        {
            Mock<IMainTaskRepository> mockMainTaskRepository = new Mock<IMainTaskRepository>();
            mockMainTaskRepository.Setup(mt => mt.Delete(It.IsAny<MainTask>())).Returns(1);

            Mock<ISubTaskRepository> mockSubTaskRepository = new Mock<ISubTaskRepository>();
            mockSubTaskRepository.Setup(mt => mt.FindByMainProjectId(It.IsAny<int>())).Returns(GetSubTasks());
            mockSubTaskRepository.Setup(mt => mt.Delete(It.IsAny<SubTask>())).Returns(1);

            var mainTaskService = new MainTaskService(mockMainTaskRepository.Object, mockSubTaskRepository.Object, false);

            var maintask = GetMainTasks().FirstOrDefault();
            var response = mainTaskService.Delete(maintask);
            Assert.IsTrue(response);

            mockSubTaskRepository.Verify(m => m.Delete(It.IsAny<SubTask>()), Times.Exactly(GetSubTasks().Count()));
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
