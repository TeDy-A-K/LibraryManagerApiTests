using System.Net;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using LibraryManagerAPITests.Core;

namespace LibraryManagerAPITests.Tests;

[AllureNUnit]
[AllureSuite("Books API")]
[TestFixture]
public class BooksApiGetByIdTests : ApiTestBase
{
    [Test]
    [Category("Positive")]
    public void GetBookById_WhenExists_ShouldReturnBook()
    {
        // Arrange
        var id = RandomId();
        var createResponse = BooksApiClient.Create(TestDataFactory.ValidBook(id));
        Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        RegisterBookForCleanup(id);

        // Act
        var response = BooksApiClient.GetById(id);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data!.Id, Is.EqualTo(id));
        }
    }

    [Test]
    [Category("Negative")]
    public void GetBookById_WhenNotExists_ShouldReturnNotFound()
    {
        // Arrange
        var id = RandomId();

        // Act
        var response = BooksApiClient.GetById(id);
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
    public void GetBookById_WithZeroId_ShouldReturnNotFound()
    {
        // Arrange
        var id = 0;

        // Act
        var response = BooksApiClient.GetById(id);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    [Category("Negative")]
    public void GetBookById_WithNegativeId_ShouldReturnNotFoundOrBadRequest()
    {
        // Arrange
        var id = -1;

        // Act
        var response = BooksApiClient.GetById(id);

        // Assert
        Assert.That(response.StatusCode, Is.AnyOf(HttpStatusCode.NotFound, HttpStatusCode.BadRequest));
    }
}
