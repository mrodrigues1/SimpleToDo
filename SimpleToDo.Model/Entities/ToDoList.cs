using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimpleToDo.Model.Entities
{
    public class ToDoList
    {
        public int ListId { get; set; }

        [Required, MaxLength(250)]
        public string Name { get; set; }

        public virtual List<Task> Tasks { get; set; }
    }
}