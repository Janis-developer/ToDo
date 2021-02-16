using ToDoApp.Models;

namespace ToDoApp
{
    public interface IToDoItemModifier
    {
        public void EditedItem(ToDoItem item, bool v);
        public void AddedItem(ToDoItem item, bool v);
        public void DeletedItem(ToDoItem item, bool v);
    }
}