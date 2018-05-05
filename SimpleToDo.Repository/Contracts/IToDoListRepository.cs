using System.Linq;
using System.Threading.Tasks;
using SimpleToDo.Model.Entities;
using Task = System.Threading.Tasks.Task;

namespace SimpleToDo.Repository.Contracts
{
    public interface IToDoListRepository
    {
        IQueryable<ToDoList> ToDoLists();
        Task<ToDoList> FindToDoListById(int id);
        Task CreateToDoList(ToDoList toDoList);
        Task UpdateToDoList(ToDoList toDoList);
        Task RemoveToDoList(ToDoList toDoList);
        Task<bool> ToDoListExists(int id);
    }
}