### Library Manager API Documentation

---

#### Overview

Simple REST API for managing books. All endpoints are rooted at **`/api/books`**. Responses use standard HTTP status codes and JSON payloads.

---

#### 1. Get all books

**Endpoint**  
`GET /api/books?title={title}`

**Query string parameters**
- **title** — *string, optional* — Returns books that contain the input in their title.

**Example request**
```http
GET /api/books?title=Test
```

**Example response**
**Status:** `200 OK`  
```json
[
  {
    "Id": 1,
    "Title": "TestTitle1",
    "Description": "TestDescription1",
    "Author": "TestAuthor1"
  },
  {
    "Id": 2,
    "Title": "TestTitle2",
    "Description": "TestDescription2",
    "Author": "TestAuthor2"
  },
  {
    "Id": 3,
    "Title": "TestTitle3",
    "Description": "TestDescription3",
    "Author": "TestAuthor3"
  }
]
```

---

#### 2. Get a book

**Endpoint**  
`GET /api/books/{id}`

**Example request**
```http
GET /api/books/6
```

**Example response**
**Status:** `200 OK`  
```json
{
  "Id": 6,
  "Author": "TestAuthor",
  "Title": "TestTitle",
  "Description": "TestDescription"
}
```

---

#### 3. Add a book

**Endpoint**  
`POST /api/books`

**Body fields**
- **Id** — *positive integer*  
- **Author** — *string, max 30 characters*  
- **Title** — *string, max 100 characters*  
- **Description** — *string, optional*

**Example request**
```http
POST /api/books
Content-Type: application/json

{
  "Id": 6,
  "Author": "TestAuthor",
  "Title": "TestTitle",
  "Description": "TestDescription"
}
```

**Example response**
**Status:** `200 OK`  
```json
{
  "Id": 6,
  "Author": "TestAuthor",
  "Title": "TestTitle",
  "Description": "TestDescription"
}
```

---

#### 4. Update a book

**Endpoint**  
`PUT /api/books/{id}`

**Body fields**
- **Id** — *positive number*  
- **Author** — *string, max 30 characters*  
- **Title** — *string, max 100 characters*  
- **Description** — *string, optional*

**Example request**
```http
PUT /api/books/6
Content-Type: application/json

{
  "Id": 6,
  "Author": "TestAuthor",
  "Title": "TestTitle",
  "Description": "TestDescription"
}
```

**Example response**
**Status:** `200 OK`  
```json
{
  "Id": 6,
  "Author": "TestAuthor",
  "Title": "TestTitle",
  "Description": "TestDescription"
}
```


---

#### 5. Delete a book

**Endpoint**  
`DELETE /api/books/{id}`

**Example request**
```http
DELETE /api/books/6
```

**Example response**
**Status:** `204 No Content`

---

#### Error responses

All error responses use this JSON format:

```json
{
  "Message": "Book with id 1 not found!"
}
```

---