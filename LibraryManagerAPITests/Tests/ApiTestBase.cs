using System.Text.Json;
using LibraryManagerAPITests.Core;
using LibraryManagerAPITests.Models;
using LibraryManagerAPITests.Services;

namespace LibraryManagerAPITests.Tests;

public abstract class ApiTestBase
{
    private readonly List<int> _createdBookIds = new List<int>();

    protected BooksApiClient BooksApiClient { get; private set; }

    [SetUp]
    public void SetUpBase()
    {
        var apiSettings = ConfigurationProvider.GetApiSettings();
        var client = RestClientFactory.Create(apiSettings);
        BooksApiClient = new BooksApiClient(client, apiSettings.BooksEndpoint);
    }

    [TearDown]
    public void TearDownBase()
    {
        foreach (var id in _createdBookIds.Distinct())
        {
            BooksApiClient.Delete(id);
        }

        _createdBookIds.Clear();
    }

    protected void RegisterBookForCleanup(int id)
    {
        _createdBookIds.Add(id);
    }

    protected static int RandomId()
    {
        return Random.Shared.Next(1, 1000000);
    }

    protected static ErrorResponse? ParseError(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return null;
        }

        return JsonSerializer.Deserialize<ErrorResponse>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    //TODO: Consider using a logging framework for better log management and output formatting, improve response logging in a more central place.
    protected static void LogResponse<T>(ApiResponse<T> response, string testName)
    {
        TestContext.WriteLine($"\n=== Response Log for: {testName} ===");
        TestContext.WriteLine($"Status Code: {response.StatusCode}");
        TestContext.WriteLine($"Response Body: {response.Content ?? "No content"}");
        TestContext.WriteLine("=== End Response Log ===\n");
    }
}
