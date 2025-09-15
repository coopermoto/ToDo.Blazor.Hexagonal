using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ToDo.Blazor.Hexagonal.Core;
using ToDo.Blazor.Hexagonal.Core.Interfaces;

namespace ToDo.Blazor.Hexagonal.Infrastructure
{
    public class FileService(IConfiguration configuration) : IFileService
    {
        private readonly string _dataFile = configuration["SampleDataFile"]
            ?? throw new ArgumentNullException("SampleDataFile");

        public List<ToDoItem> ReadFromFile()
        {
            if (!File.Exists(_dataFile))
            {
                return [];
            }

            string json = File.ReadAllText(_dataFile);
            return JsonConvert.DeserializeObject<List<ToDoItem>>(json) ?? [];
        }

        public void SaveToFile(List<ToDoItem> toDoItems)
        {
            string json = JsonConvert.SerializeObject(toDoItems);
            File.WriteAllText(_dataFile, json);
        }
    }
}
