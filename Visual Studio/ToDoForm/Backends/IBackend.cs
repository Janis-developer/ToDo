namespace ToDoApp.Backends
{
    using Models;

    interface IBackend
    {
        public void Save();

        public Rootobject Load();

        public bool AddHandler(IToDoItemModifier sender, ToDoItem item);
        public bool EditHandler(IToDoItemModifier sender, ToDoItem item);
        public bool DeleteHandler(IToDoItemModifier sender, ToDoItem item);

    }
}
