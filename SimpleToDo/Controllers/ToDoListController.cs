using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleToDo.Model.Entities;
using SimpleToDo.Model.Extensions;
using SimpleToDo.Service.Contracts;

namespace SimpleToDo.Web.Controllers
{
    public class ToDoListController : Controller
    {
        private readonly IToDoListService _toDoListService;

        public ToDoListController(IToDoListService toDoListService)
        {
            _toDoListService = toDoListService;
        }

        // GET: ToDoList
        public async Task<IActionResult> Index()
        {
            return View(await _toDoListService.ToDoLists());
        }

        // GET: ToDoList/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var list = await _toDoListService.FindById(id.Value);

            if (list == null) return NotFound();

            return View(list);
        }

        // GET: ToDoList/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ToDoList/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] ToDoList toDoToDoList)
        {
            if (ModelState.IsValid)
            {
                await _toDoListService.Create(toDoToDoList);

                this.AddAlertSuccess($"{toDoToDoList.Name} created.");

                return RedirectToAction(nameof(Index));
            }

            return View(toDoToDoList);
        }

        // GET: ToDoList/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var list = await _toDoListService.FindById(id.Value);

            if (list == null) return NotFound();

            return View("Edit", list);
        }

        // POST: ToDoList/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] ToDoList toDoList)
        {
            if (id != toDoList.Id)
                return NotFound();

            if (!ModelState.IsValid) return View(toDoList);

            try
            {
                await _toDoListService.Update(toDoList);
            }
            catch (DbUpdateConcurrencyException)
            {
                bool todoExists = await _toDoListService.Exists(id);

                if (!todoExists)
                    return NotFound();
                else
                    throw;
            }

            this.AddAlertSuccess($"{toDoList.Name} updated.");
            return RedirectToAction(nameof(Index));
        }

        // GET: ToDoList/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var list = await _toDoListService.FindById(id.Value);

            if (list == null) return NotFound();

            return View(list);
        }

        // POST: ToDoList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string listName = await _toDoListService.Remove(id);

            this.AddAlertSuccess($"{listName} removed.");

            return RedirectToAction(nameof(Index));
        }
    }
}
