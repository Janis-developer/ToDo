using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace ToDoApp
{
    using Models;
    using Views;
    using Backends;

    public partial class ToDoMainForm : Form
    {
        private IToDoView currentView;
        private IBackend backend;
        private readonly string defaultFileName = "todo.json";
        private readonly string defaultTemplate = "{ \"ToDoList\": [] }";

        public ToDoMainForm()
        {
            InitializeComponent();

            InstantiateInterfaces();
        }

        private void InstantiateInterfaces()
        {
            // in big project we would use some IoC container, but here just instantiate
            backend = new JsonFileBackend(defaultFileName);

            currentView = new ToDoGridView(this.dgvToDo);
            currentView.OnToDoItemAdd += (sender, item) => backend.AddHandler(sender, item);
            currentView.OnToDoItemEdit += (sender, item) => backend.EditHandler(sender, item); ;
            currentView.OnToDoItemDelete += (sender, item) => backend.DeleteHandler(sender, item); ;
        }

        private void LoadData()
        {
            Rootobject rootObj = backend.Load();

            UpdateView(rootObj);
        }

        private void UpdateView(Rootobject rootObj)
        {
            currentView.ClearAndFill(rootObj.ToDoList);
        }

        private void ToDoMainForm_Load(object sender, EventArgs e)
        {
            LoadDefaultList();
        }

        private void LoadDefaultList()
        {
            if (!System.IO.File.Exists(defaultFileName))
            {
                using var s = System.IO.File.CreateText(defaultFileName);
                s.Write(defaultTemplate);
            }
            LoadData();
        }

    }
}
