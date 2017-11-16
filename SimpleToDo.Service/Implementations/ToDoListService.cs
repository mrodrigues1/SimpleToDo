using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SimpleToDo.Model.Entities;
using SimpleToDo.Repository.Contracts;
using SimpleToDo.Service.Contracts;
using Task = System.Threading.Tasks.Task;

namespace SimpleToDo.Service.Implementations
{
    public class ToDoListService : IToDoListService
    {
        private readonly IToDoListRepository _toDoListRepository;

        public ToDoListService(IToDoListRepository toDoListRepository)
        {
            _toDoListRepository = toDoListRepository;
        }

        public Task<List<List>> GetToDoLists()
        {
            return _toDoListRepository.GetToDoLists().ToListAsync();
        }

        public Task<List> GetToDoListById(int id)
        {
            return _toDoListRepository.GetToDoListById(id);
        }

        public Task CreateToDoList(List list)
        {
            return _toDoListRepository.CreateToDoList(list);
        }

        public Task UpdateToDoList(List list)
        {
            return _toDoListRepository.UpdateToDoList(list);
        }

        public string RemoveToDoList(int id)
        {
            var list = this.GetToDoListById(id).Result;

            _toDoListRepository.RemoveToDoList(list);

            return list.Name;
        }

        public bool ToDoListExists(int id)
        {
            return _toDoListRepository.ToDoListExists(id);
        }
    }
}