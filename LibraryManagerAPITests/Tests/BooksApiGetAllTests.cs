using System.Net;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using AwesomeAssertions;
using LibraryManagerAPITests.Core;
using LibraryManagerAPITests.Models;

namespace LibraryManagerAPITests.Tests;

[AllureNUnit]
[AllureSuite("Books API")]
[TestFixture]
public class BooksApiGetAllTests : ApiTestBase
{
    [Test]
    [Category("Positive")]
    public void GetAllBooks_ShouldReturnOk()
    {
        // Arrange

        // Act
        var response = BooksApiClient.GetAll();

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            response.Data!.Should().BeOfType<List<Book>>();
        }
    }

    [Test]
    [Category("Positive")]
    public void GetAllBooksByTitle_ShouldReturnFilteredBooks()
    {
        // Arrange
        var id = RandomId();
        var createdBook = TestDataFactory.ValidBook(id);
        createdBook.Title = "BDD_Filter_Title";
        var createResponse = BooksApiClient.Create(createdBook);
        Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        RegisterBookForCleanup(id);

        // Act
        var response = BooksApiClient.GetAll("BDD_Filter_Title");

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data!.Any(x => x.Id == id), Is.True);
        }
    }

    [Test]
    [Category("Positive")]
    public void GetAllBooks_WithFilterThatHasNoMatches_ShouldReturnEmptyArray()
    {
        // Arrange
        var nonExistentTitle = $"NonExistent_{RandomId()}";

        // Act
        var response = BooksApiClient.GetAll(nonExistentTitle);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data!.Count, Is.EqualTo(0));
        }
    }

    [Test]
    [Category("Positive")]
    public void GetAllBooks_WithExactTitleMatch_ShouldReturnMatchingBooks()
    {
        // Arrange
        var id = RandomId();
        var exactTitle = $"ExactTitle_{id}";
        var createdBook = TestDataFactory.ValidBook(id);
        createdBook.Title = exactTitle;
        var createResponse = BooksApiClient.Create(createdBook);
        Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        RegisterBookForCleanup(id);

        // Act
        var response = BooksApiClient.GetAll(exactTitle);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data!.Any(x => x.Id == id && x.Title == exactTitle), Is.True);
        }
    }

    [Test]
    [Category("Positive")]
    public void GetAllBooks_FilterByTitleWithSpecialCharacters_ShouldReturnMatchingBooks()
    {
        // Arrange
        var id = RandomId();
        var titleWithSpecialChars = $"Book #1 \"Special\" (Edition)_{id}";
        var createdBook = TestDataFactory.ValidBook(id);
        createdBook.Title = titleWithSpecialChars;
        var createResponse = BooksApiClient.Create(createdBook);
        Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        RegisterBookForCleanup(id);

        // Act - Search by partial title with special chars
        var response = BooksApiClient.GetAll("Book #1");

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data!.Any(x => x.Title.Contains("Book #1")), Is.True);
        }
    }
}
