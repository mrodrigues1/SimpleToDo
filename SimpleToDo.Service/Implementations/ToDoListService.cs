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

        public Task<List<ToDoList>> ToDoLists() =>
            _toDoListRepository.ToDoLists().ToListAsync();

        public Task<ToDoList> FindToDoListById(int id) =>
            _toDoListRepository.FindToDoListById(id);

        public Task CreateToDoList(ToDoList toDoList) =>
            _toDoListRepository.CreateToDoList(toDoList);

        public Task UpdateToDoList(ToDoList toDoList) =>
            _toDoListRepository.UpdateToDoList(toDoList);

        public async Task<string> RemoveToDoList(int id)
        {
            var list = await this.FindToDoListById(id);

            await _toDoListRepository.RemoveToDoList(list);

            return list.Name;
        }

        public Task<bool> ToDoListExists(int id) =>
            _toDoListRepository.ToDoListExists(id);
    }
}