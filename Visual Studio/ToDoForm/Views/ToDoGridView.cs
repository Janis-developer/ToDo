using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ToDoApp.Models;

namespace ToDoApp.Views
{
    /// <summary>
    /// Specific View. GridView.
    /// </summary>
    class ToDoGridView : IToDoView
    {
        // control provided by windows system
        private readonly DataGridView dgv;

        // colors to mark valid and invalid input
        private readonly Color okColor = Color.Empty;
        private readonly Color failColor = Color.Red;

        // c-tor
        public ToDoGridView(DataGridView dgv)
        {
            this.dgv = dgv;
            this.dgv.CellEndEdit += new DataGridViewCellEventHandler(this.CellEndEdit);
            this.dgv.UserDeletedRow += new DataGridViewRowEventHandler(this.UserDeletedRow);
        }

        // this if only one of the ways to implement approvement from validating object.
        // can use intreface and keep a ref to validator. JG.
        public event ToDoItemEventHandler OnToDoItemEdit;
        public event ToDoItemEventHandler OnToDoItemAdd;
        public event ToDoItemEventHandler OnToDoItemDelete;

        private void CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var row = dgv.Rows[e.RowIndex];
            if (row != null)
            {
                ToDoItem item = (ToDoItem)row.Tag;
                if (item == null)
                {
                    if (row.Cells[0].Value == null)
                        row.Cells[0].Value = false;

                    item = new ToDoItem((string)row.Cells[1].Value, (bool)row.Cells[0].Value);
                    row.Tag = item;
                    //backend.AddItem(item) :
                    var approved = OnToDoItemAdd?.Invoke(this, item);
                    if (approved.HasValue && approved.Value)
                    {
                        Debug.WriteLine("Adding approved");
                    }
                    else
                    {
                        row.Tag = null;
                    }
                }
                else
                {
                    ToDoItem oldValue = new ToDoItem(item);
                    item.Name = (string)row.Cells[1].Value;
                    item.Status = (bool)row.Cells[0].Value;
                    //backend.EditItem(item) :
                    var approved = OnToDoItemEdit?.Invoke(this, item);
                    if(approved.HasValue && approved.Value)
                    {
                        Debug.WriteLine("Changes approved");
                    }
                    else
                    {
                        item.Copy(oldValue);
                    }
                }
            }
        }

        private void UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            if(e.Row.Tag != null)
                OnToDoItemDelete?.Invoke(this, e.Row.Tag as ToDoItem); 
        }

        void IToDoView.Appended(ToDoItem toDoItem)
        {
            DataGridViewRow row = new DataGridViewRow();

            row.CreateCells(dgv, new object[] { toDoItem.Status, toDoItem.Name });

            row.Tag = toDoItem;

            dgv.Rows.Add(row);
        }
        void IToDoView.Removed(ToDoItem toDoItem)
        {
        }
        void IToDoView.Modified(ToDoItem toDoItem)
        {
        }

        void IToDoView.Clear()
        {
            dgv.Rows.Clear();
        }

        void IToDoView.ClearAndFill(IEnumerable<ToDoItem> toDoList)
        {
            ((IToDoView)this).Clear();

            foreach(var item in toDoList)
            {
                ((IToDoView)this).Appended(item);
            }

        }

        void IToDoItemModifier.EditedItem(ToDoItem item, bool success)
        {
            HighlightItem(item, success);
        }

        void IToDoItemModifier.AddedItem(ToDoItem item, bool success)
        {
            HighlightItem(item, success);
        }

        void HighlightItem(ToDoItem item, bool ok)
        {
            foreach(DataGridViewRow row in dgv.Rows)
            { 
                if(row.Tag == item)
                {
                    row.Cells[1].Style.BackColor = ok ? okColor : failColor;
                    break;
                }
            }
        }

        void IToDoItemModifier.DeletedItem(ToDoItem item, bool v)
        {
            if(v)
                Debug.WriteLine($"delete completed for: {item.Name}");
            else
                Debug.WriteLine($"delete FAILED for: {item.Name}");
        }
    }
}
