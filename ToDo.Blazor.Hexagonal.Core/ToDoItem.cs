namespace ToDo.Blazor.Hexagonal.Core;

public class ToDoItem
{
    public int Id { get; set; }

    public required string Description { get; set; }

    public required bool IsComplete { get; set; }

    public required DateTime DateCreated { get; set; }
}
