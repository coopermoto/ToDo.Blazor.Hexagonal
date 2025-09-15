namespace ToDo.Blazor.Hexagonal.Core.Interfaces;

public interface IFileService
{
    List<ToDoItem> ReadFromFile();

    void SaveToFile(List<ToDoItem> toDoItems);
}
