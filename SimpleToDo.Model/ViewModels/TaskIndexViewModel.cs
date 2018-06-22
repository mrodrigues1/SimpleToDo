using System.Collections.Generic;
using SimpleToDo.Model.Entities;

namespace SimpleToDo.Model.ViewModels
{
    public class TaskIndexViewModel
    {
        public int Id { get; set; }
        public string ListName { get; set; }
        public IEnumerable<Task> ToDoTasks { get; set; }

        public IEnumerable<Task> CompletedTasks { get; set; }

        public Task DefaultTask => new Task();
    }
}