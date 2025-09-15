using ToDo.Blazor.Hexagonal.Core;
using ToDo.Blazor.Hexagonal.Core.Interfaces;

namespace ToDo.Blazor.Hexagonal.Infrastructure
{
    public class ToDoService(IFileService fileService) : IToDoService
    {
        private List<ToDoItem> _toDoItems = [];

        public List<ToDoItem> Get()
        {
            _toDoItems = fileService.ReadFromFile();
            return _toDoItems;
        }

        public ToDoItem Get(int id)
        {
            return _toDoItems.First(x => x.Id == id);
        }

        public List<ToDoItem> Add(ToDoItem toDoItem)
        {
            _toDoItems.Add(toDoItem);
            fileService.SaveToFile(_toDoItems);
            return _toDoItems;
        }

        public List<ToDoItem> Toggle(int id)
        {
            var toDoItemToUpdate = Get(id);

            if (toDoItemToUpdate != null)
            {
                toDoItemToUpdate.IsComplete = !toDoItemToUpdate.IsComplete;
                fileService.SaveToFile(_toDoItems);
            }

            return _toDoItems;
        }

        public List<ToDoItem> Delete(int id)
        {
            var toDoItemToRemove = Get(id);

            if (toDoItemToRemove != null)
            {
                _toDoItems.Remove(Get(id));
                fileService.SaveToFile(_toDoItems);
            }

            return _toDoItems;
        }
    }
}
