using System.Text.Json;
using ToDoApp.Models;

namespace ToDoApp.Backends
{
    class JsonFileBackend : IBackend
    {
        private Rootobject rootObj;
        private readonly string filename;
        private readonly JsonSerializerOptions options = new JsonSerializerOptions();

        public JsonFileBackend(string filename)
        {
            this.filename = filename;
            options.WriteIndented = true;
        }

        Rootobject IBackend.Load()
        {
            return rootObj = Load(filename);
        }

        void IBackend.Save()
        {
            Save(rootObj, filename);
        }

        public void Save(Rootobject rootObj, string filename)
        {
            using var stream = new System.IO.StreamWriter(filename);

            string jsonStr = System.Text.Json.JsonSerializer.Serialize<Rootobject>(rootObj, options);

            stream.Write(jsonStr);
        }

        static public Rootobject Load(string filename)
        {
            using var stream = new System.IO.StreamReader(filename);

            string jsonStr = stream.ReadToEnd();

            Rootobject rootObj = (Rootobject)System.Text.Json.JsonSerializer.Deserialize(jsonStr, typeof(Rootobject));

            return rootObj;
        }

        bool IBackend.AddHandler(IToDoItemModifier sender, ToDoItem item)
        {
            if (IsValid(item))
            {
                rootObj.ToDoList.Add(item);
                ((IBackend)this).Save();
                sender.AddedItem(item, true);
                return true;
            }
            else
            {
                sender.AddedItem(item, false);
                return false;
            }
        }

        bool IBackend.EditHandler(IToDoItemModifier sender, ToDoItem item)
        {
            if (IsValid(item) && rootObj.ToDoList.Contains(item)) 
            {
                ((IBackend)this).Save();
                sender.EditedItem(item, true);
                return true;
            }
            else
            {
                sender.EditedItem(item, false);
                return false;
            }
        }

        bool IBackend.DeleteHandler(IToDoItemModifier sender, ToDoItem item)
        {
            if(rootObj.ToDoList.Contains(item))
            {
                rootObj.ToDoList.Remove(item);
                ((IBackend)this).Save();
                sender.DeletedItem(item, true);
                return true;
            }
            else
            {
                sender.DeletedItem(item, false);
                return false;
            }
        }

        bool IsValid(ToDoItem item)
        {
            if(item == null)
            {
                return false;
            }

            return !string.IsNullOrWhiteSpace(item.Name);
        }
    }
}
