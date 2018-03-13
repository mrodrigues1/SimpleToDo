using System.Threading.Tasks;
using Task = SimpleToDo.Model.Entities.Task;

namespace SimpleToDo.Repository.Contracts
{
    public interface ITaskRepository
    {
        Task<Task> GetTaskById(int id);
        Task CreateTask(Task task);
        Task UpdateTask(Task task);
        Task<bool> TaskExists(int taskId);
        Task RemoveTask(Task task);
    }
}