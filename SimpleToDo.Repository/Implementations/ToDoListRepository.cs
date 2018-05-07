using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SimpleToDo.Model.Entities;
using SimpleToDo.Repository.Contracts;
using Task = System.Threading.Tasks.Task;

namespace SimpleToDo.Repository.Implementations
{
    public class ToDoListRepository : IToDoListRepository
    {
        private readonly ToDoDbContext _context;

        public ToDoListRepository(ToDoDbContext context)
        {
            _context = context;
        }

        public IQueryable<ToDoList> ToDoLists() =>
            _context.List;


        public Task<ToDoList> FindToDoListById(int id) =>
            _context
                .List
                .Include(x => x.Tasks)
                .SingleOrDefaultAsync(m => m.ListId == id);

        public Task CreateToDoList(ToDoList toDoList)
        {
            _context.Add(toDoList);
            return _context.SaveChangesAsync();
        }

        public Task UpdateToDoList(ToDoList toDoList)
        {
            _context.Update(toDoList);
            return _context.SaveChangesAsync();
        }

        public async Task RemoveToDoList(ToDoList toDoList)
        {
            _context.List.Remove(toDoList);
            await _context.SaveChangesAsync();
        }

        public Task<bool> ToDoListExists(int id) =>
            _context.List.AnyAsync(e => e.ListId == id);
    }
}