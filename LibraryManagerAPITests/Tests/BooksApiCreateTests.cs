using System.Net;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using AwesomeAssertions;
using LibraryManagerAPITests.Core;

namespace LibraryManagerAPITests.Tests;

[AllureNUnit]
[AllureSuite("Books API")]
[TestFixture]
public class BooksApiCreateTests : ApiTestBase
{
    [Test]
    [Category("Positive")]
    [AllureIssue("API-BUG-001")]
    public void AddBook_WithValidPayload_ShouldReturnCreatedBook()
    {
        // Arrange
        var id = RandomId();
        var payload = TestDataFactory.ValidBook(id);

        // Act
        var response = BooksApiClient.Create(payload);
        RegisterBookForCleanup(id);

        // Assert
        LogResponse(response, nameof(AddBook_WithValidPayload_ShouldReturnCreatedBook));
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(response.Data, Is.Not.Null);
        response.Data.Should().BeEquivalentTo(payload);
    }

    [Test]
    [Category("Positive")]
    [AllureIssue("API-BUG-001")]
    public void AddBook_WithValidPayloadWithoutOptionalDescription_ShouldReturnCreatedBook()
    {
        // Arrange
        var id = RandomId();
        var payload = TestDataFactory.ValidBookWithoutDescription(id);

        // Act
        var response = BooksApiClient.Create(payload);
        RegisterBookForCleanup(id);

        // Assert
        LogResponse(response, nameof(AddBook_WithValidPayloadWithoutOptionalDescription_ShouldReturnCreatedBook));
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            response.Data.Should().BeEquivalentTo(payload);
        }
    }

    [Test]
    [Category("Negative")]
    [TestCase("Id")]
    [TestCase("Author")]
    [TestCase("Title")]
    public void AddBook_WithMissingRequiredField_ShouldReturnBadRequest(string missingField)
    {
        // Arrange
        var id = RandomId();
        var payload = TestDataFactory.MissingField(id, missingField);

        // Act
        var response = BooksApiClient.Create(payload);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    [Category("Negative")]
    public void AddBook_WithExistingId_ShouldReturnBadRequest()
    {
        // Arrange
        var id = RandomId();
        var payload = TestDataFactory.ValidBook(id);
        var firstCreate = BooksApiClient.Create(payload);
        Assert.That(firstCreate.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        RegisterBookForCleanup(id);

        // Act
        var response = BooksApiClient.Create(payload);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    [Category("Negative")]
    public void AddBook_WithAuthorLongerThan30_ShouldReturnBadRequest()
    {
        // Arrange
        var id = RandomId();
        var payload = TestDataFactory.WithAuthorLength(id, 31);

        // Act
        var response = BooksApiClient.Create(payload);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    [Category("Negative")]
    public void AddBook_WithTitleLongerThan100_ShouldReturnBadRequest()
    {
        // Arrange
        var id = RandomId();
        var payload = TestDataFactory.WithTitleLength(id, 101);

        // Act
        var response = BooksApiClient.Create(payload);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    [Category("Positive")]
    [AllureIssue("API-BUG-002")]
    public void AddBook_WithAuthorExactly30Characters_ShouldReturnCreatedBook()
    {
        // Arrange
        var id = RandomId();
        var payload = TestDataFactory.WithAuthorLength(id, 30);

        // Act
        var response = BooksApiClient.Create(payload);
        RegisterBookForCleanup(id);

        // Assert
        LogResponse(response, nameof(AddBook_WithAuthorExactly30Characters_ShouldReturnCreatedBook));
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
    public void AddBook_WithTitleExactly100Characters_ShouldReturnCreatedBook()
    {
        // Arrange
        var id = RandomId();
        var payload = TestDataFactory.WithTitleLength(id, 100);

        // Act
        var response = BooksApiClient.Create(payload);
        RegisterBookForCleanup(id);

        // Assert
        LogResponse(response, nameof(AddBook_WithTitleExactly100Characters_ShouldReturnCreatedBook));
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data!.Title.Length, Is.EqualTo(100));
        }
    }

    [Test]
    [Category("Negative")]
    public void AddBook_WithEmptyAuthor_ShouldReturnBadRequest()
    {
        // Arrange
        var id = RandomId();
        var payload = TestDataFactory.WithEmptyAuthor(id);

        // Act
        var response = BooksApiClient.Create(payload);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    [Category("Negative")]
    public void AddBook_WithEmptyTitle_ShouldReturnBadRequest()
    {
        // Arrange
        var id = RandomId();
        var payload = TestDataFactory.WithEmptyTitle(id);

        // Act
        var response = BooksApiClient.Create(payload);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    [Category("Negative")]
    public void AddBook_WithWhitespaceOnlyAuthor_ShouldReturnBadRequest()
    {
        // Arrange
        var id = RandomId();
        var payload = TestDataFactory.WithWhitespaceOnlyAuthor(id);

        // Act
        var response = BooksApiClient.Create(payload);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    [Category("Negative")]
    public void AddBook_WithWhitespaceOnlyTitle_ShouldReturnBadRequest()
    {
        // Arrange
        var id = RandomId();
        var payload = TestDataFactory.WithWhitespaceOnlyTitle(id);

        // Act
        var response = BooksApiClient.Create(payload);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    [Category("Negative")]
    public void AddBook_WithZeroId_ShouldReturnBadRequest()
    {
        // Arrange
        var payload = TestDataFactory.WithZeroId();

        // Act
        var response = BooksApiClient.Create(payload);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    [Category("Negative")]
    public void AddBook_WithNegativeId_ShouldReturnBadRequest()
    {
        // Arrange
        var payload = TestDataFactory.WithNegativeId();

        // Act
        var response = BooksApiClient.Create(payload);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    [Category("Positive")]
    [AllureIssue("API-BUG-001")]
    public void AddBook_WithSpecialCharacters_ShouldReturnCreatedBook()
    {
        // Arrange
        var id = RandomId();
        var payload = TestDataFactory.WithSpecialCharacters(id);
        payload.Id = id;

        // Act
        var response = BooksApiClient.Create(payload);
        RegisterBookForCleanup(id);

        // Assert
        LogResponse(response, nameof(AddBook_WithSpecialCharacters_ShouldReturnCreatedBook));
        using (Assert.EnterMultipleScope())
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data!.Author, Is.EqualTo(payload.Author));
            Assert.That(response.Data.Title, Is.EqualTo(payload.Title));
        }
    }
}
