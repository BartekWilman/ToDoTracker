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
        private List<TodoItem> _items = new List<TodoItem>();
        public TodoItem Add(string title, Priority priority = Priority.Medium, DateTime? due = null)
        {
            var item = new TodoItem(title, priority, due);
            _items.Add(item);
            return item;
        }
        public bool MarkDone(Guid id)
        {
            var item = _items.FirstOrDefault(x => x.Id == id);
            if (item == null) return false;
            item.MarkDone();
            return true;
        }
        public IEnumerable<TodoItem> List(TaskStatus? status = null)
        {
            if (status == null)
            {
                return _items;
            }
            return _items.Where(x => x.Status == status.Value);
        }

        public void ShowList(IEnumerable<TodoItem> items)
        {
            int i = 1;
            foreach (var item in items)
            {
                Console.WriteLine($"[{i}]");
                item.ShowInfo();
                i++;
            }
        }

        public void GiveMeLists()
        {
            IEnumerable<TodoItem> list;
            Console.Clear();
            Console.WriteLine("1. Wszystkie\n2. Aktywne\n3. Ukończone\nPokaz mi: ");
            string show = Console.ReadLine();
            switch (show)
            {
                case "2":
                    list = List(TaskStatus.Active);
                    break;
                case "3":
                    list = List(TaskStatus.Done);
                    break;
                default:
                    list = List();
                    break;
            }
            ShowList(list);
            Console.ReadKey();
        }
        public void CompleteTaskFromList()
        {
            IEnumerable<TodoItem> list;
            Console.Clear();
            list = List(TaskStatus.Active);
            if (!list.Any()) return;
            ShowList(list);
            TodoItem[] array = list.ToArray();
            Console.Write("Podaj numer zadania, które chcesz odhaczyć: ");
            var input = Console.ReadLine();
            if (!int.TryParse(input, out int n) || n < 1 || n > array.Length)
            {
                Console.WriteLine("Nieprawidłowy numer.");
                Console.ReadKey();
                return;
            }
            MarkDone(array[n - 1].Id);
            Console.ReadKey();
        }
        public void MakeAndAdd()
        {
            Console.Clear();
            Console.Write("Podaj tytul zadania: ");
            string title = Console.ReadLine();
            string pri;
            Priority priorytet;
            while (true)
            {
                Console.WriteLine("Podaj priorytet\n1. Niski\n2. Sredni\n3. Wysoki");
                pri = Console.ReadLine();

                if (pri == "1") { priorytet = Priority.Low; break; }
                if (pri == "2") { priorytet = Priority.Medium; break; }
                if (pri == "3") { priorytet = Priority.High; break; }

                Console.WriteLine("Wybierz poziom priorytetu wpisując od 1 do 3");
            }
            Console.WriteLine("Konretny termin?\n1. Tak\n2. Nie");
            string chceszDate = Console.ReadLine();
            if (chceszDate == "1")
            {
                DateTime data = ConsoleUi.GiveMeDate();
                Add(title, priorytet, data);
            }
            else
            {
                Add(title, priorytet);
            }
        }
    }
}
