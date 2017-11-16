using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleToDo.Model.Entities;
using Task = System.Threading.Tasks.Task;

namespace SimpleToDo.Service.Contracts
{
    public interface IToDoListService
    {
        Task<List<List>> GetToDoLists();
        Task<List> GetToDoListById(int id);
        Task CreateToDoList(List list);
        Task UpdateToDoList(List list);
        string RemoveToDoList(int id);
        bool ToDoListExists(int id);
    }
}