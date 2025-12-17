using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoTracker
{
    internal enum TaskStatus { Active, Done }

    internal enum Priority { Low, Medium, High }
    internal class TodoItem
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Title { get; private set; }
        public TaskStatus Status { get; private set; } = TaskStatus.Active;
        public Priority Priority { get; private set; } = Priority.Medium;
        public DateTime? Due { get; private set; }

        public TodoItem(string title, Priority priority = Priority.Medium, DateTime? due = null)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentNullException("Title is required", nameof(title));
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
                Console.WriteLine($"Id: {Id}\nNazwa: {Title}\nStatus: {Status}\nPriorytet: {Priority}\nTermin: {Due}\n\n");
            }
            else
            {
                Console.WriteLine($"Id: {Id}\nNazwa: {Title}\nStatus: {Status}\nPriorytet: {Priority}\n\n");
            }
        }
    }
}
