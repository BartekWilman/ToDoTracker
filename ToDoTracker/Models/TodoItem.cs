using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ToDoTracker
{
    internal enum TaskStatus { Active, Done }

    internal enum Priority { Low, Medium, High }
    internal class TodoItem
    {
        public Guid Id { get; init; }
        public string Title { get; private set; }
        public TaskStatus Status { get; private set; } = TaskStatus.Active;
        public Priority Priority { get; private set; } = Priority.Medium;
        public DateTime? Due { get; private set; }

        public TodoItem(string title, Priority priority = Priority.Medium, DateTime? due = null)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title is required", nameof(title));
            Id = Guid.NewGuid();
            Title = title.Trim();
            Priority = priority;
            Due = due;
        }
        [JsonConstructor]
        public TodoItem(Guid id, string title, TaskStatus status, Priority priority, DateTime? due)
        {
            Id = id;
            Title = title;
            Status = status;
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
