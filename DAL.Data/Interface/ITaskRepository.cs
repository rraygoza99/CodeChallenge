using DAL.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data.Interface
{
    public interface ITaskRepository
    {
        public IEnumerable<TaskEntity> GetAllTasks();
        public TaskEntity GetTaskById(int taskId);
        public TaskEntity UpdateTask(TaskEntity task);
        public TaskEntity CreateTask(TaskEntity task);
        public IEnumerable<TaskEntity> ChangeTaskState(int taskId, bool state);


    }
}
