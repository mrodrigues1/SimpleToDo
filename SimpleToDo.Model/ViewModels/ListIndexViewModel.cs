using System.Collections.Generic;
using SimpleToDo.Model.Entities;

namespace SimpleToDo.Model.ViewModels
{
    public class ListIndexViewModel
    {
        public IEnumerable<List> ToDoLists { get; set; }
        public IEnumerable<Column> GridColumns { get; set; }

        public ListIndexViewModel()
        {
            GridColumns = new List<Column>
            {
                new Column { Key = nameof(List.Name), Name = nameof(List.Name) },
                new Column { Key = string.Empty, Name = string.Empty }
            };
        }
    }

    public class Column
    {
        public string Key { get; set; }
        public string Name { get; set; }
    }
}