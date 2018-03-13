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

        public IQueryable<List> GetToDoLists()
        {
            return _context.List;
        }

        public Task<List> GetToDoListById(int id)
        {
            return _context
                .List
                .Include(x => x.Tasks)
                .SingleOrDefaultAsync(m => m.ListId == id);
        }

        public Task CreateToDoList(List list)
        {
            _context.Add(list);
            return _context.SaveChangesAsync();
        }

        public Task UpdateToDoList(List list)
        {
            _context.Update(list);
            return _context.SaveChangesAsync();
        }

        public async Task RemoveToDoList(List list)
        {
            _context.List.Remove(list);
            await _context.SaveChangesAsync();
        }

        public Task<bool> ToDoListExists(int id)
        {
            return _context.List.AnyAsync(e => e.ListId == id);
        }
    }
}