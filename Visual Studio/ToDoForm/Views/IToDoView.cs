using System.Collections.Generic;
using ToDoApp.Models;

namespace ToDoApp.Views
{
    internal interface IToDoView : IToDoReadOnlyView, IToDoItemModifier
    {
        void Clear();
        void ClearAndFill(IEnumerable<ToDoItem> toDoList);
        public event ToDoItemEventHandler OnToDoItemEdit;
        public event ToDoItemEventHandler OnToDoItemAdd;
        public event ToDoItemEventHandler OnToDoItemDelete;

        void Appended(ToDoItem toDoItem);
        void Removed(ToDoItem toDoItem);
        void Modified(ToDoItem toDoItem);
        // I'm using using the 'lean' methodology
        // but feel free to extend interface when needed. JG.
    }
}