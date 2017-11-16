using System.Threading.Tasks;
using SimpleToDo.Repository.Contracts;
using SimpleToDo.Service.Contracts;
using Task = SimpleToDo.Model.Entities.Task;

namespace SimpleToDo.Service.Implementations
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public Task<Task> GetTaskById(int id)
        {
            return _taskRepository.GetTaskById(id);
        }

        public System.Threading.Tasks.Task CreateTask(Task task)
        {
            return _taskRepository.CreateTask(task);
        }

        public System.Threading.Tasks.Task UpdateTask(Task task)
        {
            return _taskRepository.UpdateTask(task);
        }

        public Task<bool> TaskExists(int taskId)
        {
            return _taskRepository.TaskExists(taskId);
        }

        public async Task<Task> RemoveTask(int id)
        {
            var task = await this.GetTaskById(id);

            await _taskRepository.RemoveTask(task);

            return task;
        }

        public async Task<Task> CompleteTask(int id)
        {
            var task = await this.GetTaskById(id);

            task.Done = true;

            await this.UpdateTask(task);

            return task;
        }

        public async Task<Task> UndoTask(int id)
        {
            var task = await this.GetTaskById(id);

            task.Done = false;

            await this.UpdateTask(task);

            return task;
        }
    }
}