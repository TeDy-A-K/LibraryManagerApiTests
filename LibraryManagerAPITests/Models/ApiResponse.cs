using System.Net;

namespace LibraryManagerAPITests.Models;

public class ApiResponse<T>
{
    public required HttpStatusCode StatusCode { get; init; }
    public T? Data { get; init; }
    public string? Content { get; init; }
}
