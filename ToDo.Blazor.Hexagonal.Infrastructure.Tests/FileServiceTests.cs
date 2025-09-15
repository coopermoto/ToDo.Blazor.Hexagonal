using Microsoft.Extensions.Configuration;
using Moq;
using System.Text.Json;
using ToDo.Blazor.Hexagonal.Core;

namespace ToDo.Blazor.Hexagonal.Infrastructure.Tests;

[TestClass]
public sealed class FileServiceTests
{
    private Mock<IConfiguration>? _mockConfiguration;

    [TestInitialize]
    public void TestInitialize()
    {
        _mockConfiguration = new Mock<IConfiguration>();

        // Clean up any existing test files before each test
        var testFiles = new[] { "sampleData.json", "newfile.json", "nonexistentfile.json" };

        foreach (var file in testFiles)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }

        // Create a sample data file for testing
        var sampleData = new List<ToDoItem>
        {
            new() { Id = 1, Description = "Buy groceries", IsComplete = false, DateCreated = DateTime.Now },
            new() { Id = 2, Description = "Walk the dog", IsComplete = true, DateCreated = DateTime.Now },
            new() { Id = 3, Description = "Read a book", IsComplete = false, DateCreated = DateTime.Now }
        };

        File.WriteAllText("sampleData.json", JsonSerializer.Serialize(sampleData));
    }

    [TestMethod]
    public void ReadFromFile_ReturnsListOfToDoItems()
    {
        // Arrange
        _mockConfiguration?.Setup(config => config["SampleDataFile"]).Returns("sampleData.json");
        var fileService = new FileService(_mockConfiguration!.Object);

        // Act
        var results = fileService.ReadFromFile();

        // Assert
        Assert.IsNotNull(results);
        Assert.IsInstanceOfType(results, typeof(List<ToDoItem>));
    }

    [TestMethod]
    public void ReadFromFile_ReadsExptectedData()
    {
        // Arrange
        _mockConfiguration?.Setup(config => config["SampleDataFile"]).Returns("sampleData.json");
        var fileService = new FileService(_mockConfiguration!.Object);

        // Act
        var results = fileService.ReadFromFile();

        // Assert
        Assert.IsNotNull(results);
        Assert.AreEqual(3, results.Count);
        Assert.AreEqual("Buy groceries", results[0].Description);
        Assert.AreEqual("Walk the dog", results[1].Description);
        Assert.AreEqual("Read a book", results[2].Description);
    }

    [TestMethod]
    public void ReadFromFile_FileDoesNotExist_ReturnsEmptyList()
    {
        // Arrange
        _mockConfiguration?.Setup(config => config["SampleDataFile"]).Returns("nonexistentfile.json");
        var fileService = new FileService(_mockConfiguration!.Object);

        // Act
        var results = fileService.ReadFromFile();

        // Assert
        Assert.IsNotNull(results);
        Assert.AreEqual(0, results.Count);
    }

    [TestMethod]
    public void SaveToFile_CreatesFileIfNotExists()
    {
        // Arrange
        _mockConfiguration?.Setup(config => config["SampleDataFile"]).Returns("newfile.json");
        var fileService = new FileService(_mockConfiguration!.Object);
        var toDoItems = new List<ToDoItem>
        {
            new() { Id = 1, Description = "Test Item", IsComplete = false, DateCreated = DateTime.Now }
        };
        // Act
        fileService.SaveToFile(toDoItems);
        // Assert
        var results = fileService.ReadFromFile();
        Assert.AreEqual(1, results.Count);
        Assert.AreEqual("Test Item", results[0].Description);
    }

    [TestMethod]
    public void SaveToFile_SavesListOfToDoItems()
    {
        // Arrange
        _mockConfiguration?.Setup(config => config["SampleDataFile"]).Returns("sampleData.json");
        var fileService = new FileService(_mockConfiguration!.Object);
        var toDoItems = new List<ToDoItem>
        {
            new() { Id = 1, Description = "Test Item 1", IsComplete = false, DateCreated = DateTime.Now },
            new() { Id = 2, Description = "Test Item 2", IsComplete = true, DateCreated = DateTime.Now }
        };
        // Act
        fileService.SaveToFile(toDoItems);
        // Assert
        var results = fileService.ReadFromFile();
        Assert.AreEqual(2, results.Count);
        Assert.AreEqual("Test Item 1", results[0].Description);
        Assert.AreEqual("Test Item 2", results[1].Description);
    }
}
