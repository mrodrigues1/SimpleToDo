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

        public Task<List<List>> ToDoLists()
        {
            return _toDoListRepository.ToDoLists().ToListAsync();
        }

        public Task<List> FindToDoListById(int id)
        {
            return _toDoListRepository.FindToDoListById(id);
        }

        public Task CreateToDoList(List list)
        {
            return _toDoListRepository.CreateToDoList(list);
        }

        public Task UpdateToDoList(List list)
        {
            return _toDoListRepository.UpdateToDoList(list);
        }

        public async Task<string> RemoveToDoList(int id)
        {
            var list = await this.FindToDoListById(id);

            await _toDoListRepository.RemoveToDoList(list);

            return list.Name;
        }

        public Task<bool> ToDoListExists(int id)
        {
            return _toDoListRepository.ToDoListExists(id);
        }
    }
}