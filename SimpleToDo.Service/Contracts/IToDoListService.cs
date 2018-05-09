using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleToDo.Model.Entities;
using Task = System.Threading.Tasks.Task;

namespace SimpleToDo.Service.Contracts
{
    public interface IToDoListService
    {
        Task<List<ToDoList>> ToDoLists();
        Task<ToDoList> FindById(int id);
        Task Create(ToDoList toDoList);
        Task Update(ToDoList toDoList);
        Task<string> Remove(int id);
        Task<bool> Exists(int id);
    }
}