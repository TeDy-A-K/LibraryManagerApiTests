using LibraryManagerAPITests.Models;

namespace LibraryManagerAPITests.Core;

public static class TestDataFactory
{
    public static Book ValidBook(int id)
    {
        return new Book
        {
            Id = id,
            Author = $"Author_{id}",
            Title = $"Title_{id}",
            Description = $"Description_{id}"
        };
    }

    public static Book WithAuthorLength(int id, int length)
    {
        return new Book
        {
            Id = id,
            Author = new string('A', length),
            Title = $"Title_{id}",
            Description = "Description"
        };
    }

    public static Book WithTitleLength(int id, int length)
    {
        return new Book
        {
            Id = id,
            Author = "Author",
            Title = new string('T', length),
            Description = "Description"
        };
    }

    public static Book ValidBookWithoutDescription(int id)
    {
        return new Book
        {
            Id = id,
            Author = $"Author_{id}",
            Title = $"Title_{id}",
            Description = null
        };
    }

    public static Book MissingField(int id, string missingField)
    {
        var book = new Book
        {
            Id = id,
            Author = $"Author_{id}",
            Title = $"Title_{id}",
            Description = "Description"
        };

        return missingField switch
        {
            "Id" => new Book { Author = book.Author, Title = book.Title, Description = book.Description },
            "Author" => new Book { Id = book.Id, Title = book.Title, Description = book.Description },
            "Title" => new Book { Id = book.Id, Author = book.Author, Description = book.Description },
            _ => book
        };
    }

    public static Book WithEmptyAuthor(int id)
    {
        return new Book
        {
            Id = id,
            Author = "",
            Title = $"Title_{id}",
            Description = "Description"
        };
    }

    public static Book WithEmptyTitle(int id)
    {
        return new Book
        {
            Id = id,
            Author = "Author",
            Title = "",
            Description = "Description"
        };
    }

    public static Book WithWhitespaceOnlyAuthor(int id)
    {
        return new Book
        {
            Id = id,
            Author = "   ",
            Title = $"Title_{id}",
            Description = "Description"
        };
    }

    public static Book WithWhitespaceOnlyTitle(int id)
    {
        return new Book
        {
            Id = id,
            Author = "Author",
            Title = "   ",
            Description = "Description"
        };
    }

    public static Book WithZeroId()
    {
        return new Book
        {
            Id = 0,
            Author = "Author",
            Title = "Title",
            Description = "Description"
        };
    }

    public static Book WithNegativeId(int negativeId = -1)
    {
        return new Book
        {
            Id = negativeId,
            Author = "Author",
            Title = "Title",
            Description = "Description"
        };
    }

    public static Book WithSpecialCharacters(int id)
    {
        return new Book
        {
            Id = id,
            Author = "O'Brien",
            Title = "Book #1 \"Special\" (Edition)",
            Description = "Contains @ and & symbols"
        };
    }
}
