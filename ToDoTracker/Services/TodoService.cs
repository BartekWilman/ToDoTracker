using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoTracker;

namespace ToDoTracker
{
    internal class TodoService
    {
        //lista Toditem
        private List<TodoItem> _items = new List<TodoItem>();
        //KONSTRUKTOR
        public TodoItem Add(string title, Priority priority = Priority.Medium, DateTime? due = null)
        {
            var item = new TodoItem(title, priority, due);
            _items.Add(item);
            return item;
        }
        //odhaczenie jako done
        public bool MarkDone(Guid id)
        {
            var item = _items.FirstOrDefault(x => x.Id == id);
            if (item == null) return false;
            item.MarkDone();
            return true;
        }
        //funkcja bool usunięcia TodoItem(Guid Id)
        public bool RemoveItem(Guid id)
        {
            var item = _items.FirstOrDefault(x => x.Id == id);
            if (item == null) return false;
            _items.Remove(item);
            return true;
        }

        //nadpisanie listy (atrybutu)
        public void LoadItems(List<TodoItem> items)
        {
            _items = items;
        }
        //pobranie listy (atrybutu)
        public List<TodoItem> GetList()
        {
            return _items;
        }
    }
}
