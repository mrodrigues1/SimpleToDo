using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleToDo.Model.Entities;
using SimpleToDo.Model.Extensions;
using SimpleToDo.Model.ViewModels;
using SimpleToDo.Service.Contracts;
using Task = SimpleToDo.Model.Entities.Task;

namespace SimpleToDo.Web.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IToDoListService _toDoListService;

        public TaskController(ITaskService taskService, IToDoListService toDoListService)
        {
            _taskService = taskService;
            _toDoListService = toDoListService;
        }

        // GET: Task
        public async Task<IActionResult> Index(int listId)
        {
            ToDoList toDoToDoList = await _toDoListService.FindById(listId);

            if (toDoToDoList == null)
                return NotFound();

            var taskIndexViewModel = new TaskIndexViewModel
            {
                Id = listId,
                ListName = toDoToDoList.Name,
                ToDoTasks = toDoToDoList.Tasks.Where(t => t.Done == false).OrderByDescending(x => x.Id),
                CompletedTasks = toDoToDoList.Tasks.Where(t => t.Done).OrderByDescending(x => x.Id)
            };

            return View(taskIndexViewModel);
        }

        // GET: Task/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var task = await _taskService.GetTaskById(id.Value);

            if (task == null)
                return NotFound();

            return View(task);
        }

        // GET: Task/Create
        public IActionResult Create(int listId)
        {
            return View(new TaskCreateEditViewModel
            {
                TodoListId = listId
            });
        }

        // POST: Task/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ToDoListId,Name,Description,Done")] Task task)
        {
            if (!ModelState.IsValid)
                return View(
                    new TaskCreateEditViewModel
                    {
                        TodoListId = task.ToDoListId,
                        Name = task.Name,
                        Description = task.Description
                    });

            await _taskService.CreateTask(task);

            this.AddAlertSuccess($"{task.Name} created.");

            return RedirectToAction(nameof(Index), new { listId = task.ToDoListId });

        }

        // GET: Task/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var task = await _taskService.GetTaskById(id.Value);

            if (task == null)
                return NotFound();

            return View(new TaskCreateEditViewModel
            {
                Id = task.Id,
                TodoListId = task.ToDoListId,
                Name = task.Name,
                Description = task.Description,
                Done = task.Done
            });
        }

        // POST: Task/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ToDoListId,Name,Description,Done")] Task task)
        {
            if (id != task.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(
                    new TaskCreateEditViewModel
                    {
                        Id = task.Id,
                        TodoListId = task.ToDoListId,
                        Name = task.Name,
                        Description = task.Description,
                        Done = task.Done
                    });

            try
            {
                await _taskService.UpdateTask(task);
            }
            catch (DbUpdateConcurrencyException)
            {
                var taskExists = await _taskService.TaskExists(task.Id);
                if (!taskExists)
                {
                    return NotFound();
                }

                throw;
            }

            this.AddAlertSuccess($"{task.Name} updated.");
            return RedirectToAction(nameof(Index), new { listId = task.ToDoListId });

        }

        // GET: Task/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var task = await _taskService.GetTaskById(id.Value);

            if (task == null)
                return NotFound();

            return View(task);
        }

        // POST: Task/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _taskService.RemoveTask(id);

            this.AddAlertSuccess($"{task.Name} removed.");

            return RedirectToAction(nameof(Index), new { listId = task.ToDoListId });
        }


        public async Task<IActionResult> CompleteTask(int id)
        {
            var task = await _taskService.CompleteTask(id);

            this.AddAlertSuccess($"{task.Name} completed.");

            return RedirectToAction(nameof(Index), new { listId = task.ToDoListId });
        }

        public async Task<IActionResult> UndoTask(int id)
        {
            var task = await _taskService.UndoTask(id);

            this.AddAlertSuccess($"{task.Name} undone.");

            return RedirectToAction(nameof(Index), new { listId = task.ToDoListId });
        }
    }
}
