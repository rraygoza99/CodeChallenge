using DAL.Data;
using DAL.Data.Repository;
using DAL.Model.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace UnitTesting
{
    public class RepositoryTests
    {
        private readonly Mock<TaskContext> _mockContext;
        private readonly Mock<DbSet<TaskEntity>> _mockSet;
        private readonly TaskRepository _repo;
        public RepositoryTests()
        {
            var options = new DbContextOptionsBuilder<TaskContext>()
            .UseInMemoryDatabase(databaseName: "taskAPI")
            .Options;
            _mockContext = new Mock<TaskContext>(options);
            _mockSet = new Mock<DbSet<TaskEntity>>();
            _repo = new TaskRepository(_mockContext.Object);
        }

        [Fact]
        public void GetAllTasks_ShouldReturnAllRows()
        {
            var tasks = new List<TaskEntity>()
            {
                new TaskEntity { Id = 1, Name = "Task 1", Description = "Test Desc 1", DueDate = DateTime.Now, IsCompleted = false },
                new TaskEntity { Id = 2, Name = "Task 2", Description = "Test Desc 1", DueDate = DateTime.Now, IsCompleted = false }
            }.AsQueryable();

            _mockSet.As<IQueryable<TaskEntity>>().Setup(m => m.Provider).Returns(tasks.Provider);
            _mockSet.As<IQueryable<TaskEntity>>().Setup(m => m.Expression).Returns(tasks.Expression);
            _mockSet.As<IQueryable<TaskEntity>>().Setup(m => m.ElementType).Returns(tasks.ElementType);
            _mockSet.As<IQueryable<TaskEntity>>().Setup(m => m.GetEnumerator()).Returns(tasks.GetEnumerator());

            _mockContext.Setup(c => c.Set<TaskEntity>()).Returns(_mockSet.Object);

            var result = _repo.GetAllTasks();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
        [Fact]
        public void CreateTask_ShouldAddTaskAndReturnIt()
        {
            // Arrange
            var newTask = new TaskEntity { Id = 1, Name = "New Task" };
            var tasks = new List<TaskEntity> { newTask }.AsQueryable();

            _mockSet.As<IQueryable<TaskEntity>>().Setup(m => m.Provider).Returns(tasks.Provider);
            _mockSet.As<IQueryable<TaskEntity>>().Setup(m => m.Expression).Returns(tasks.Expression);
            _mockSet.As<IQueryable<TaskEntity>>().Setup(m => m.ElementType).Returns(tasks.ElementType);
            _mockSet.As<IQueryable<TaskEntity>>().Setup(m => m.GetEnumerator()).Returns(tasks.GetEnumerator());

            _mockSet.Setup(m => m.Add(It.IsAny<TaskEntity>())).Callback<TaskEntity>(t => newTask = t);
            _mockContext.Setup(c => c.Set<TaskEntity>()).Returns(_mockSet.Object);
            _mockContext.Setup(c => c.SaveChanges()).Returns(1);

            // Act
            var result = _repo.CreateTask(new TaskEntity { Name = "New Task" });

            // Assert
            _mockSet.Verify(m => m.Add(It.IsAny<TaskEntity>()), Times.Once);
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
            Assert.Equal(result.Id, result.Id);
            Assert.Equal(result.Name, result.Name);
        }
        [Fact]
        public void UpdateTask_ShouldUpdateTaskDetails()
        {
            // Arrange
            var taskToUpdate = new TaskEntity { Id = 1, Name = "Old Task", Description = "Old Description" };
            var updatedTask = new TaskEntity { Id = 1, Name = "Updated Task", Description = "Updated Description", DueDate = DateTime.Now };
            var tasks = new List<TaskEntity> { taskToUpdate }.AsQueryable();

            _mockSet.As<IQueryable<TaskEntity>>().Setup(m => m.Provider).Returns(tasks.Provider);
            _mockSet.As<IQueryable<TaskEntity>>().Setup(m => m.Expression).Returns(tasks.Expression);
            _mockSet.As<IQueryable<TaskEntity>>().Setup(m => m.ElementType).Returns(tasks.ElementType);
            _mockSet.As<IQueryable<TaskEntity>>().Setup(m => m.GetEnumerator()).Returns(tasks.GetEnumerator());

            _mockContext.Setup(c => c.Set<TaskEntity>()).Returns(_mockSet.Object);
            _mockContext.Setup(c => c.SaveChanges()).Returns(1);

            // Act
            var result = _repo.UpdateTask(updatedTask);

            // Assert
            _mockSet.Verify(m => m.Update(It.IsAny<TaskEntity>()), Times.Once);
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
            Assert.Equal(updatedTask.Name, result.Name);
            Assert.Equal(updatedTask.Description, result.Description);
        }
        [Fact]
        public void UpdateTask_ShouldThrowException_WhenTaskNotFound()
        {
            // Arrange
            var taskToUpdate = new TaskEntity { Id = 1, Name = "Updated Task" };
            var tasks = new List<TaskEntity>().AsQueryable();

            _mockSet.As<IQueryable<TaskEntity>>().Setup(m => m.Provider).Returns(tasks.Provider);
            _mockSet.As<IQueryable<TaskEntity>>().Setup(m => m.Expression).Returns(tasks.Expression);
            _mockSet.As<IQueryable<TaskEntity>>().Setup(m => m.ElementType).Returns(tasks.ElementType);
            _mockSet.As<IQueryable<TaskEntity>>().Setup(m => m.GetEnumerator()).Returns(tasks.GetEnumerator());

            _mockContext.Setup(c => c.Set<TaskEntity>()).Returns(_mockSet.Object);

            // Act
            var exception = Assert.Throws<Exception>(() => _repo.UpdateTask(taskToUpdate));

            //Assert
            Assert.Equal("Task not found", exception.Message);
        }
    }
}
