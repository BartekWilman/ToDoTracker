using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;

namespace ToDoTracker
{
    internal class TodoStorage
    {
        private const string path = "tasks.json";
        private JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented  = true};
        public void Save(TodoService serv)
        {
            TodoItem[] array = serv.GetList().ToArray();
            string json = JsonSerializer.Serialize(array, options);
            File.WriteAllText(path, json);
        }
        public List<TodoItem> Load()
        {
            if (!File.Exists(path))
            {
                return new List<TodoItem>();
            }
            string jsonFromFile = File.ReadAllText(path);
            List<TodoItem> toLoad = JsonSerializer.Deserialize<List<TodoItem>>(jsonFromFile);
            if(toLoad == null)
            {
                return new List<TodoItem>();
            }
            return toLoad;
        }
    }
}
