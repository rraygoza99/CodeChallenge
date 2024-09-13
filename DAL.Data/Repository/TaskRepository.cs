using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using DAL.Data.Interface;
using DAL.Model.Models;
using System.Threading.Tasks;

namespace DAL.Data.Repository
{
    public class TaskRepository : ITaskRepository
    {
        protected DbContext _context;
        public TaskRepository(TaskContext context){
            _context= context;
        }

        public IEnumerable<TaskEntity> GetAllTasks()
        {
            return _context.Set<TaskEntity>().AsEnumerable();
        }
        public TaskEntity GetTaskById(int taskId)
        {
            var entity =_context.Set<TaskEntity>().Where(a => a.Id == taskId).FirstOrDefault();
            if (entity == null)
                throw new Exception("Task not found");
            return entity;
        }

        public TaskEntity CreateTask(TaskEntity task)
        {
            EntityEntry dbEntityEntry = _context.Entry(task);
            _context.Set<TaskEntity>().Add(task);
            _context.SaveChanges();
            return _context.Set<TaskEntity>().AsEnumerable().FirstOrDefault(task);
        }
        public TaskEntity UpdateTask(TaskEntity task)
        {
            var entity = _context.Set<TaskEntity>().Where(a=> a.Id == task.Id).FirstOrDefault();
            if (entity == null)
                throw new Exception("Task not found");
            entity.Name = task.Name;
            entity.Description = task.Description;
            entity.DueDate = task.DueDate;
            EntityEntry dbEntry = _context.Entry(entity);
            _context.Set<TaskEntity>().Update(entity);
            _context.SaveChanges();
            return _context.Set<TaskEntity>().Where(a => a.Id == task.Id).First();
        }

        public IEnumerable<TaskEntity> ChangeTaskState(int taskId, bool state)
        {
            var entity = _context.Set<TaskEntity>().Where(a => a.Id == taskId).FirstOrDefault();
            if (entity == null)
                throw new Exception("Task not found");
            entity.IsCompleted = state;
            EntityEntry dbEntry = _context.Entry(entity);
            _context.Set<TaskEntity>().Update(entity);
            _context.SaveChanges();
            return _context.Set<TaskEntity>().AsEnumerable();
        }
    }
}
