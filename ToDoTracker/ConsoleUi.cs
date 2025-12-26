using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoTracker
{
    internal static class ConsoleUi
    {
        //przefiltrowanie listy
        public static IEnumerable<TodoItem> List(TodoService service,TaskStatus? status = null)
        {
            if (status == null)
            {
                return service.GetList();
            }
            return service.GetList().Where(x => x.Status == status.Value);
        }
        //wypisanie elementów listy z numeracją
        public static void ShowList(IEnumerable<TodoItem> items)
        {
            int i = 1;
            foreach (var item in items)
            {
                Console.WriteLine($"[{i}]");
                item.ShowInfo();
                i++;
            }
        }
        //wyświetlanie przefiltrowanych list
        public static void GiveMeLists(TodoService serv)
        {
            IEnumerable<TodoItem> list;
            Console.Clear();
            Console.WriteLine("1. Wszystkie\n2. Aktywne\n3. Ukończone\nPokaz mi: ");
            string show = Console.ReadLine();
            switch (show)
            {
                case "2":
                    list = List(serv, TaskStatus.Active);
                    break;
                case "3":
                    list = List(serv, TaskStatus.Done);
                    break;
                default:
                    list = List(serv);
                    break;
            }
            ShowList(list);
            Console.ReadKey();
        }
        //stworzenie TodoItem i dodanie go do listy (atrybutu)
        public static void MakeAndAdd(TodoService serv)
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
                serv.Add(title, priorytet, data);
            }
            else
            {
                serv.Add(title, priorytet);
            }
        }
        private static bool PickItemAndExecute(
            TodoService serv,
            bool allowDecline,
            string prompt,
            Action<TodoItem> action,
            TaskStatus? status = null)
        {
            var list = List(serv, status).ToList();
            if (list.Count == 0)
            {
                Console.WriteLine("Lista nie zawiera elementów");
                return false;
            }
            ShowList(list);
            TodoItem[] array = list.ToArray();
            Console.WriteLine(prompt);
            var input = Console.ReadLine();
            if (allowDecline)
            {
                if (input == "0")
                {
                    Console.WriteLine("Anulowano");
                    return false;
                }

            }
            if (!int.TryParse(input, out int n) || n < 1 || n > array.Length)
            {
                Console.WriteLine("Nieprawidłowy numer.");
                Console.ReadKey();
                return false;
            }
            action(array[n - 1]);
            return true;
        }
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
            Console.WriteLine("TO DO TRACKER\n1. Pokaz zadania\n2. Dodaj zadanie\n3. Ukończ zadanie\n4. Usuń zadanie\n0. Zakończ działanie programu");
        }
        public static string Prompt()
        {
            Console.Write($"Wybierz funkcję ");
            return Console.ReadLine();
        }
        public static void LoadFromFile(TodoStorage stor, TodoService serv)
        {
            serv.LoadItems(stor.Load());
        }
        public static void MainLoop()
        {
            var service = new TodoService();
            var storage = new TodoStorage();
            LoadFromFile(storage, service );
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
                        GiveMeLists(service);
                        break;
                    case "2":
                        MakeAndAdd(service);
                        storage.Save(service);
                        break;
                    case "3":
                        PickItemAndExecute(
                            service,
                            allowDecline: true,
                            prompt: "Podaj numer zadania do ukończenia (0 = anuluj): ",
                            action: item => service.MarkDone(item.Id),
                            status: TaskStatus.Active
                            );
                        storage.Save(service);
                        break;
                    case "4":
                        bool del = PickItemAndExecute(
                            service,
                            allowDecline: true,
                            prompt: "Podaj numer zadania do usunięcia",
                            action: item => service.RemoveItem(item.Id)
                            );
                        if (del) storage.Save(service);
                        break;
                    case "0":
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
    }
}