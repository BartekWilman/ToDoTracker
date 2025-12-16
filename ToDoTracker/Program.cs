using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoTracker
{
    enum TaskStatus { Active, Done }
    enum Priority { Low, Medium, High }
    class TodoItem
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Title { get; private set; }
        public TaskStatus Status { get; private set; } = TaskStatus.Active;
        public Priority Priority { get; private set; } = Priority.Medium;
        public DateTime? Due { get; private set; }

        public TodoItem(string title, Priority priority = Priority.Medium, DateTime? due = null)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentNullException("Title is required");
            Title = title.Trim();
            Priority = priority;
            Due = due;
        }
        public void MarkDone()
        {
            Status = TaskStatus.Done;
        }
        public void ShowInfo()
        {
            if (Due != null)
            {
                Console.WriteLine($"Nazwa: {Title}\nStatus: {Status}\nPriorytet: {Priority}\nTermin: {Due}\n\n");
            }
            else
            {
                Console.WriteLine($"Nazwa: {Title}\nStatus: {Status}\nPriorytet: {Priority}\n\n");
            }
        }
    }
    class TodoService
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
            if(!list.Any()) return;
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
            MarkDone(array[n-1].Id);
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
                DateTime data = Program.GiveMeDate();
                Add(title, priorytet, data);
            }
            else
            {
                Add(title, priorytet);
            }
        }
    }


        internal class Program
        {

        public static DateTime GiveMeDate()
        {
            Console.Write("Podaj rok: ");
            int year = int.Parse(Console.ReadLine());
            Console.Write("Podaj miesiąc: ");
            int month = int.Parse(Console.ReadLine());
            Console.Write("Podaj dzień: ");
            int day = int.Parse(Console.ReadLine());

            return new DateTime(year, month, day);
        }
        public static void ShowMenu()
            {
                Console.WriteLine("TO DO TRACKER\n1. Pokaz zadania\n2. Dodaj zadanie\n3. Ukończ zadanie\n4. Zakończ działanie programu");
            }
            public static string Prompt()
            {
                Console.Write($"Wybierz funkcję ");
                return Console.ReadLine();
            }
        public static void MainLoop()
        {
            var service = new TodoService();
            bool flag = true;
            string prompt;
            while (flag)
            {
                Console.Clear();
                ShowMenu();
                prompt = Prompt();
                if (prompt == null) continue;
                switch (prompt)
                {
                    case "1":
                        service.GiveMeLists();
                        break;
                    case "2":
                        service.MakeAndAdd();
                        break;
                    case "3":
                        service.CompleteTaskFromList(); break;
                    case "4":
                        Console.WriteLine("NARA ;)");
                        Console.ReadKey();
                        flag = false;
                        break;
                    default:
                        Console.WriteLine("wybierz opcję z menu");
                        Console.ReadKey();
                        break;
                }
            }
        }
            static void Main(string[] args)
            {
                MainLoop();
            }
        }
    }

