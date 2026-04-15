using System.Net;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using LibraryManagerAPITests.Core;

namespace LibraryManagerAPITests.Tests;

[AllureNUnit]
[AllureSuite("Books API")]
[TestFixture]
public class BooksApiDeleteTests : ApiTestBase
{
    [Test]
    [Category("Positive")]
    public void DeleteBook_WhenExists_ShouldReturnNoContent()
    {
        // Arrange
        var id = RandomId();
        var createResponse = BooksApiClient.Create(TestDataFactory.ValidBook(id));
        Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        // Act
        var response = BooksApiClient.Delete(id);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
    }

    [Test]
    [Category("Negative")]
    public void DeleteBook_WhenNotExists_ShouldReturnNotFound()
    {
        // Arrange
        var id = RandomId();

        // Act
        var response = BooksApiClient.Delete(id);
        var error = ParseError(response.Content);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(error?.Message, Is.EqualTo(string.Format(ErrorMessages.BookNotFoundTemplate, id)));
        }
    }

    [Test]
    [Category("Negative")]
    public void DeleteBook_WhenAlreadyDeleted_ShouldReturnNotFound()
    {
        // Arrange
        var id = RandomId();
        var createResponse = BooksApiClient.Create(TestDataFactory.ValidBook(id));
        Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var firstDelete = BooksApiClient.Delete(id);
        Assert.That(firstDelete.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

        // Act
        var secondDelete = BooksApiClient.Delete(id);
        var error = ParseError(secondDelete.Content);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(secondDelete.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(error?.Message, Is.EqualTo(string.Format(ErrorMessages.BookNotFoundTemplate, id)));
        }
    }

    [Test]
    [Category("Negative")]
    public void DeleteBook_WithZeroId_ShouldReturnNotFound()
    {
        // Arrange
        var id = 0;

        // Act
        var response = BooksApiClient.Delete(id);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    [Category("Negative")]
    public void DeleteBook_WithNegativeId_ShouldReturnNotFoundOrBadRequest()
    {
        // Arrange
        var id = -1;

        // Act
        var response = BooksApiClient.Delete(id);

        // Assert
        Assert.That(response.StatusCode, Is.AnyOf(HttpStatusCode.NotFound, HttpStatusCode.BadRequest));
    }
}
