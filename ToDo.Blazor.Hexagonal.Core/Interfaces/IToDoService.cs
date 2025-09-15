namespace ToDo.Blazor.Hexagonal.Core.Interfaces;

public interface IToDoService
{
    List<ToDoItem> Get();

    ToDoItem Get(int id);

    List<ToDoItem> Add(ToDoItem toDoItem);

    List<ToDoItem> Toggle(int id);

    List<ToDoItem> Delete(int id);
}
