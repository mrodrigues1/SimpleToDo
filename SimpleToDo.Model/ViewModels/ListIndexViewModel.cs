using System.Collections.Generic;
using SimpleToDo.Model.Entities;

namespace SimpleToDo.Model.ViewModels
{
    public class ListIndexViewModel
    {
        public IEnumerable<List> ToDoLists { get; set; }
        public IEnumerable<string> GridColumns { get; set; }

        public ListIndexViewModel()
        {
            GridColumns = new List<string>
            {
                nameof(List.Name).ToLower(),
                "Actions"
            };
        }
    }

    public class Column
    {
        public string Key { get; set; }
        public string Name { get; set; }
    }
}