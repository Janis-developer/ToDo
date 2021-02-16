using System.Collections.Generic;

namespace ToDoApp.Models
{

    public class Rootobject
    {
        public List<ToDoItem> ToDoList { get; set; }
    }

    public class ToDoItem
    {
        public ToDoItem(string Name, bool Status)
        {
            this.Name = Name;
            this.Status = Status;
        }

        public ToDoItem() { }

        public ToDoItem(ToDoItem item)
        {
            Copy(item);
        }

        public void Copy(ToDoItem item)
        {
            this.Name = item.Name;
            this.Status = item.Status;
        }

        public string Name { get; set; }
        public bool Status { get; set; }
    }

    public delegate bool ToDoItemEventHandler(IToDoItemModifier sender, ToDoItem item);
}
