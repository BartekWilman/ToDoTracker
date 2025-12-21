using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoTracker
{
    internal static class ConsoleUi
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
        public static void LoadFromFile(TodoStorage stor, TodoService serv)
        {
            serv.LoadItems(stor.Load());
        }
        public static void MainLoop()
        {
            var service = new TodoService();
            var staorage = new TodoStorage();
            LoadFromFile(staorage, service );
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
                        staorage.Save(service);
                        break;
                    case "3":
                        service.CompleteTaskFromList(); 
                        staorage.Save(service);
                        break;
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
    }
}
