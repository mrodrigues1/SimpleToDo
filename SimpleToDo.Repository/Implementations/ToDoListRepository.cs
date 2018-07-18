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

        public IQueryable<ToDoList> ToDoLists() => _context.ToDoList.AsNoTracking();

        public Task<ToDoList> FindById(int id) =>
            _context
                .ToDoList
                .Include(x => x.Tasks)
                .SingleOrDefaultAsync(m => m.Id == id);

        public Task Create(ToDoList toDoList)
        {
            _context.Add(toDoList);
            return _context.SaveChangesAsync();
        }

        public Task Update(ToDoList toDoList)
        {
            _context.Update(toDoList);
            return _context.SaveChangesAsync();
        }

        public async Task Remove(ToDoList toDoList)
        {
            _context.ToDoList.Remove(toDoList);
            await _context.SaveChangesAsync();
        }

        public Task<bool> Exists(int id) => _context.ToDoList.AnyAsync(e => e.Id == id);
    }
}