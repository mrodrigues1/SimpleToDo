using System.Threading.Tasks;
using Task = SimpleToDo.Model.Entities.Task;

namespace SimpleToDo.Repository.Contracts
{
    public interface ITaskRepository
    {
        Task<Task> GetTaskById(int id);
        System.Threading.Tasks.Task CreateTask(Task task);
        System.Threading.Tasks.Task UpdateTask(Task task);
        Task<bool> TaskExists(int taskId);
        System.Threading.Tasks.Task RemoveTask(Task task);
    }
}