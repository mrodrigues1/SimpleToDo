using System.Linq;
using System.Threading.Tasks;
using SimpleToDo.Model.Entities;
using Task = System.Threading.Tasks.Task;

namespace SimpleToDo.Repository.Contracts
{
    public interface IToDoListRepository
    {
        IQueryable<ToDoList> ToDoLists();
        Task<ToDoList> FindById(int id);
        Task Create(ToDoList toDoList);
        Task Update(ToDoList toDoList);
        Task Remove(ToDoList toDoList);
        Task<bool> Exists(int id);
    }
}