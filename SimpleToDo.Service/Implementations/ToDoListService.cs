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

        public Task Create(ToDoList toDoList) =>
            _toDoListRepository.Create(toDoList);

        public Task Update(ToDoList toDoList) =>
            _toDoListRepository.Update(toDoList);

        public async Task<string> Remove(int id)
        {
            var list = await this.FindToDoListById(id);

            await _toDoListRepository.Remove(list);

            return list.Name;
        }

        public Task<bool> Exists(int id) =>
            _toDoListRepository.Exists(id);
    }
}