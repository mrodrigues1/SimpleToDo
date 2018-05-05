using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleToDo.Model.Entities;
using Task = System.Threading.Tasks.Task;

namespace SimpleToDo.Service.Contracts
{
    public interface IToDoListService
    {
        Task<List<ToDoList>> ToDoLists();
        Task<ToDoList> FindToDoListById(int id);
        Task CreateToDoList(ToDoList toDoList);
        Task UpdateToDoList(ToDoList toDoList);
        Task<string> RemoveToDoList(int id);
        Task<bool> ToDoListExists(int id);
    }
}