# Bug Report

**Bug ID:** `API-BUG-002`

**Title:** Book Author field validation incorrectly rejects exactly 30 characters

**Environment Name:** `Local`

**Priority:** `Medium`

**Severity:** `Medium`

## Preconditions
- API specification states: "Author should not exceed 30 characters"
- The intention is to allow Author field up to and including 30 characters

## Steps to Reproduce

### Request Details

**Request URL:**
```http
POST /api/books
```

**Request Body:**
```json
{
  "Id": 12345,
  "Title": "Book Title",
  "Description": "Book Description",
  "Author": "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
}
```

*Note: Author field contains exactly 30 characters (30 'A's)*

### Actual Response
```json
{
  "Message": "Book.Author should not exceed 30 characters!\r\nParameter name: Book.Author"
}
```

**Status Code:** `400 BadRequest`

### Expected Response
```json
{
  "Id": 12345,
  "Title": "Book Title",
  "Description": "Book Description",
  "Author": "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAA"
}
```

**Status Code:** `200 OK`
