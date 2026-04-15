using System.Net;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using LibraryManagerAPITests.Core;

namespace LibraryManagerAPITests.Tests;

[AllureNUnit]
[AllureSuite("Books API")]
[TestFixture]
public class BooksApiUpdateTests : ApiTestBase
{
    [Test]
    [Category("Positive")]
    public void UpdateBook_WithValidPayload_ShouldReturnUpdatedBook()
    {
        // Arrange
        var id = RandomId();
        var original = TestDataFactory.ValidBook(id);
        var createResponse = BooksApiClient.Create(original);
        Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        RegisterBookForCleanup(id);

        var updated = TestDataFactory.ValidBook(id);
        updated.Author = "UpdatedAuthor";
        updated.Title = "UpdatedTitle";
        updated.Description = "UpdatedDescription";

        // Act
        var response = BooksApiClient.Update(id, updated);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data!.Author, Is.EqualTo(updated.Author));
            Assert.That(response.Data.Title, Is.EqualTo(updated.Title));
            Assert.That(response.Data.Description, Is.EqualTo(updated.Description));
        }
    }

    [Test]
    [Category("Positive")]
    public void UpdateBook_WithValidPayloadWithoutOptionalDescription_ShouldReturnUpdatedBook()
    {
        // Arrange
        var id = RandomId();
        var original = TestDataFactory.ValidBook(id);
        var createResponse = BooksApiClient.Create(original);
        Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        RegisterBookForCleanup(id);

        var updated = TestDataFactory.ValidBookWithoutDescription(id);
        updated.Author = "UpdatedAuthor";
        updated.Title = "UpdatedTitle";

        // Act
        var response = BooksApiClient.Update(id, updated);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data!.Author, Is.EqualTo(updated.Author));
            Assert.That(response.Data.Title, Is.EqualTo(updated.Title));
        }
    }

    [Test]
    [Category("Negative")]
    [TestCase("Id")]
    [TestCase("Author")]
    [TestCase("Title")]
    public void UpdateBook_WithMissingRequiredField_ShouldReturnBadRequest(string missingField)
    {
        // Arrange
        var id = RandomId();
        var original = TestDataFactory.ValidBook(id);
        var createResponse = BooksApiClient.Create(original);
        Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        RegisterBookForCleanup(id);

        var payload = TestDataFactory.MissingField(id, missingField);

        // Act
        var response = BooksApiClient.Update(id, payload);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    [Category("Negative")]
    public void UpdateBook_WhenNotExists_ShouldReturnNotFound()
    {
        // Arrange
        var id = RandomId();
        var payload = TestDataFactory.ValidBook(id);

        // Act
        var response = BooksApiClient.Update(id, payload);
        var error = ParseError(response.Content);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(error?.Message, Is.EqualTo($"Book with id {id} not found!"));
        }
    }

    [Test]
    [Category("Negative")]
    public void UpdateBook_WithAuthorLongerThan30_ShouldReturnBadRequest()
    {
        // Arrange
        var id = RandomId();
        var createResponse = BooksApiClient.Create(TestDataFactory.ValidBook(id));
        Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        RegisterBookForCleanup(id);

        var payload = TestDataFactory.WithAuthorLength(id, 31);

        // Act
        var response = BooksApiClient.Update(id, payload);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    [Category("Negative")]
    public void UpdateBook_WithTitleLongerThan100_ShouldReturnBadRequest()
    {
        // Arrange
        var id = RandomId();
        var createResponse = BooksApiClient.Create(TestDataFactory.ValidBook(id));
        Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        RegisterBookForCleanup(id);

        var payload = TestDataFactory.WithTitleLength(id, 101);

        // Act
        var response = BooksApiClient.Update(id, payload);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    [Category("Positive")]
    [AllureIssue("API-BUG-002")]
    public void UpdateBook_WithAuthorExactly30Characters_ShouldReturnUpdatedBook()
    {
        // Arrange
        var id = RandomId();
        var createResponse = BooksApiClient.Create(TestDataFactory.ValidBook(id));
        Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        RegisterBookForCleanup(id);

        var payload = TestDataFactory.WithAuthorLength(id, 30);

        // Act
        var response = BooksApiClient.Update(id, payload);

        // Assert
        LogResponse(response, nameof(UpdateBook_WithAuthorExactly30Characters_ShouldReturnUpdatedBook));
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data!.Author.Length, Is.EqualTo(30));
        }
    }

    [Test]
    [Category("Positive")]
    [AllureIssue("API-BUG-003")]
    public void UpdateBook_WithTitleExactly100Characters_ShouldReturnUpdatedBook()
    {
        // Arrange
        var id = RandomId();
        var createResponse = BooksApiClient.Create(TestDataFactory.ValidBook(id));
        Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        RegisterBookForCleanup(id);

        var payload = TestDataFactory.WithTitleLength(id, 10);

        // Act
        var response = BooksApiClient.Update(id, payload);

        // Assert
        LogResponse(response, nameof(UpdateBook_WithTitleExactly100Characters_ShouldReturnUpdatedBook));
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data!.Title.Length, Is.EqualTo(100));
        }
    }

    [Test]
    [Category("Negative")]
    public void UpdateBook_WithEmptyAuthor_ShouldReturnBadRequest()
    {
        // Arrange
        var id = RandomId();
        var createResponse = BooksApiClient.Create(TestDataFactory.ValidBook(id));
        Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        RegisterBookForCleanup(id);

        var payload = TestDataFactory.WithEmptyAuthor(id);

        // Act
        var response = BooksApiClient.Update(id, payload);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    [Category("Negative")]
    public void UpdateBook_WithEmptyTitle_ShouldReturnBadRequest()
    {
        // Arrange
        var id = RandomId();
        var createResponse = BooksApiClient.Create(TestDataFactory.ValidBook(id));
        Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        RegisterBookForCleanup(id);

        var payload = TestDataFactory.WithEmptyTitle(id);

        // Act
        var response = BooksApiClient.Update(id, payload);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    [Category("Negative")]
    public void UpdateBook_WithWhitespaceOnlyAuthor_ShouldReturnBadRequest()
    {
        // Arrange
        var id = RandomId();
        var createResponse = BooksApiClient.Create(TestDataFactory.ValidBook(id));
        Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        RegisterBookForCleanup(id);

        var payload = TestDataFactory.WithWhitespaceOnlyAuthor(id);

        // Act
        var response = BooksApiClient.Update(id, payload);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    [Category("Negative")]
    public void UpdateBook_WithWhitespaceOnlyTitle_ShouldReturnBadRequest()
    {
        // Arrange
        var id = RandomId();
        var createResponse = BooksApiClient.Create(TestDataFactory.ValidBook(id));
        Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        RegisterBookForCleanup(id);

        var payload = TestDataFactory.WithWhitespaceOnlyTitle(id);

        // Act
        var response = BooksApiClient.Update(id, payload);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    [Category("Negative")]
    public void UpdateBook_WithUrlIdMismatchBodyId_ShouldHandleAppropriately()
    {
        // Arrange
        var urlId = RandomId();
        var bodyId = RandomId();
        var createResponse = BooksApiClient.Create(TestDataFactory.ValidBook(urlId));
        Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        RegisterBookForCleanup(urlId);

        var payload = TestDataFactory.ValidBook(bodyId);
        payload.Id = bodyId;

        // Act
        var response = BooksApiClient.Update(urlId, payload);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    [Category("Positive")]
    public void UpdateBook_WithSpecialCharacters_ShouldReturnUpdatedBook()
    {
        // Arrange
        var id = RandomId();
        var createResponse = BooksApiClient.Create(TestDataFactory.ValidBook(id));
        Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        RegisterBookForCleanup(id);

        var payload = TestDataFactory.WithSpecialCharacters(id);
        payload.Id = id;

        // Act
        var response = BooksApiClient.Update(id, payload);

        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data!.Author, Is.EqualTo(payload.Author));
            Assert.That(response.Data.Title, Is.EqualTo(payload.Title));
        }
    }
}
