using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleToDo.Model.Entities;
using Task = System.Threading.Tasks.Task;

namespace SimpleToDo.Service.Contracts
{
    public interface IToDoListService
    {
        Task<List<List>> ToDoLists();
        Task<List> FindToDoListById(int id);
        Task CreateToDoList(List list);
        Task UpdateToDoList(List list);
        Task<string> RemoveToDoList(int id);
        Task<bool> ToDoListExists(int id);
    }
}