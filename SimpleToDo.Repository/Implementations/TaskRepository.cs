using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SimpleToDo.Model.Entities;
using SimpleToDo.Repository.Contracts;
using Task = SimpleToDo.Model.Entities.Task;

namespace SimpleToDo.Repository.Implementations
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ToDoDbContext _context;

        public TaskRepository(ToDoDbContext context)
        {
            _context = context;
        }

        public Task<Task> GetTaskById(int id)
        {
            return _context.Task
            .Include(t => t.ToDoList)
            .SingleOrDefaultAsync(m => m.TaskId == id);
        }

        public System.Threading.Tasks.Task CreateTask(Task task)
        {
            _context.Task.Add(task);
            return _context.SaveChangesAsync();
        }

        public System.Threading.Tasks.Task UpdateTask(Task task)
        {
            _context.Update(task);
            return _context.SaveChangesAsync();
        }

        public Task<bool> TaskExists(int taskId)
        {
            return _context.Task.AnyAsync(x => x.TaskId == taskId);
        }

        public System.Threading.Tasks.Task RemoveTask(Task task)
        {            
            _context.Task.Remove(task);
            return _context.SaveChangesAsync();
        }        
    }
}