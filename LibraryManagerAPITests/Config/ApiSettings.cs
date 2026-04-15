namespace LibraryManagerAPITests.Config;

public class ApiSettings
{
    public required string BaseUrl { get; init; }
    public required string BooksEndpoint { get; init; }
    public int TimeoutSeconds { get; init; }
}
