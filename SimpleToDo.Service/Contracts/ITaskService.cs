using System.Threading.Tasks;
using Task = SimpleToDo.Model.Entities.Task;

namespace SimpleToDo.Service.Contracts
{
    public interface ITaskService
    {
        Task<Task> GetTaskById(int id);
        System.Threading.Tasks.Task CreateTask(Task task);
        System.Threading.Tasks.Task UpdateTask(Task task);
        Task<bool> TaskExists(int taskId);
        Task<Task> RemoveTask(int id);
        Task<Task> CompleteTask(int id);
        Task<Task> UndoTask(int id);
    }
}