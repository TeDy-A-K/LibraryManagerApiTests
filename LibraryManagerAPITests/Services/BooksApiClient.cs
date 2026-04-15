using LibraryManagerAPITests.Models;
using RestSharp;

namespace LibraryManagerAPITests.Services;

public class BooksApiClient
{
    private readonly RestClient _client;
    private readonly string _booksEndpoint;

    public BooksApiClient(RestClient client, string booksEndpoint)
    {
        _client = client;
        _booksEndpoint = booksEndpoint;
    }

    public ApiResponse<List<Book>> GetAll(string? title = null)
    {
        var request = new RestRequest(_booksEndpoint, Method.Get);
        if (!string.IsNullOrWhiteSpace(title))
        {
            request.AddQueryParameter("title", title);
        }

        var response = _client.Execute<List<Book>>(request);
        return new ApiResponse<List<Book>>
        {
            StatusCode = response.StatusCode,
            Data = response.Data,
            Content = response.Content
        };
    }

    public ApiResponse<Book> GetById(int id)
    {
        var response = _client.Execute<Book>(new RestRequest($"{_booksEndpoint}/{id}", Method.Get));
        return new ApiResponse<Book>
        {
            StatusCode = response.StatusCode,
            Data = response.Data,
            Content = response.Content
        };
    }

    public ApiResponse<Book> Create(Book book)
    {
        var request = new RestRequest(_booksEndpoint, Method.Post).AddJsonBody(book);
        var response = _client.Execute<Book>(request);
        return new ApiResponse<Book>
        {
            StatusCode = response.StatusCode,
            Data = response.Data,
            Content = response.Content
        };
    }

    public ApiResponse<Book> Update(int id, Book book)
    {
        var request = new RestRequest($"{_booksEndpoint}/{id}", Method.Put).AddJsonBody(book);
        var response = _client.Execute<Book>(request);
        return new ApiResponse<Book>
        {
            StatusCode = response.StatusCode,
            Data = response.Data,
            Content = response.Content
        };
    }

    public ApiResponse<object> Delete(int id)
    {
        var response = _client.Execute(new RestRequest($"{_booksEndpoint}/{id}", Method.Delete));
        return new ApiResponse<object>
        {
            StatusCode = response.StatusCode,
            Content = response.Content
        };
    }
}
