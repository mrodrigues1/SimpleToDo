using System.ComponentModel.DataAnnotations;

namespace SimpleToDo.Model.Entities
{
    public class Task
    {
        public int TaskId { get; set; }

        public int ListId { get; set; }

        [Required, MaxLength(500)]
        public string Name { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; }

        public bool Done { get; set; }

        public virtual ToDoList ToDoList { get; set; }
    }
}