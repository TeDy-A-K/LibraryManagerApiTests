# Bug Report

**Bug ID:** `API-BUG-001`

**Title:** Book Author field returns null in API response when valid author data is submitted

**Environment Name:** `Local`

**Priority:** `High`

**Severity:** `High`

## Preconditions

## Steps to Reproduce

### Request Details

**Request URL:**
```http
POST /api/books
```

**Request Body:**
```json
{
  "Id": 497267,
  "Title": "Title_497267",
  "Description": "Description_497267",
  "Author": "John Doe"
}
```

### Actual Response
```json
{
  "Id": 497267,
  "Title": "Title_497267",
  "Description": "Description_497267",
  "Author": null
}
```

### Expected Response
```json
{
  "Id": 497267,
  "Title": "Title_497267",
  "Description": "Description_497267",
  "Author": "John Doe"
}
```