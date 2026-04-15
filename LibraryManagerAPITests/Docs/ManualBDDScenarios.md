# Library Manager API - BDD Manual Test Scenarios

Base URL: `http://localhost:9000`  
Resource: `/api/books`

## GET /api/books

### @Positive Get all books
**Scenario:** Retrieve all books successfully  
**Given** the Library Manager API is available  
**When** a client sends `GET /api/books`  
**Then** the response status code is `200 OK`  
**And** the response body is a JSON array of books

### @Positive Filter books by title
**Scenario Outline:** Retrieve books filtered by title fragment  
**Given** at least one book exists with title containing `BDD_Filter_Title`  
**When** a client sends `GET /api/books?title=BDD_Filter_Title`  
**Then** the response status code is `200 OK`  
**And** each returned book title contains `BDD_Filter_Title`

**Examples:**
    | BDD_Filter_Title |
    | Test |
	| Title |

## GET /api/books/{id}

### @Positive Get existing book by id
**Scenario:** Retrieve one existing book  
**Given** a book exists with id `X`  
**When** a client sends `GET /api/books/X`  
**Then** the response status code is `200 OK`  
**And** the response body contains that book id

### @Negative Get missing book by id
**Scenario:** Try to retrieve a non-existing book  
**Given** no book exists with id `Y`  
**When** a client sends `GET /api/books/Y`  
**Then** the response status code is `404 Not Found`  
**And** the response body contains a JSON error with `Message: "Book with id Y not found!"`

## POST /api/books

### @Positive Add book with valid payload
**Scenario:** Create a new book with valid data  
**Given** no book exists with id `Y`  
**When** a client sends `POST /api/books` with a valid payload  
**Then** the response status code is `200 OK`  
**And** the response body contains the created book

### @Positive Add book with valid payload without optional params
**Scenario:** Create a new book without optional description field  
**Given** no book exists with id `Y`  
**When** a client sends `POST /api/books` with required fields only (Id, Author, Title)  
**Then** the response status code is `200 OK`  
**And** the response body contains the created book with Description as null or empty

### @Negative Add book with missing required params
**Scenario Outline:** Try to create a book with missing required fields  
**Given** no book exists with id `X`  
**When** a client sends `POST /api/books` with missing `MissingField`  
**Then** the response status code is `400 Bad Request`

**Examples:**
    | MissingField |
    | Id |
    | Author |
    | Title |

### @Negative Add book with duplicate id
**Scenario:** Try to create a book with an existing id  
**Given** a book already exists with id `X`  
**When** a client sends `POST /api/books` with the same id `X`  
**Then** the response status code is `400 Bad Request`

### @Negative Add book with author length > 30
**Scenario:** Try to create a book with too long author value  
**Given** no book exists with id `X`  
**When** a client sends `POST /api/books` and `Author` length is `31`  
**Then** the response status code is `400 Bad Request`

### @Negative Add book with title length > 100
**Scenario:** Try to create a book with too long title value  
**Given** no book exists with id `X`  
**When** a client sends `POST /api/books` and `Title` length is `101`  
**Then** the response status code is `400 Bad Request`

### @Positive Add book with author exactly 30 characters (boundary)
**Scenario:** Create a book with author at maximum valid length  
**Given** no book exists with id `X`  
**When** a client sends `POST /api/books` with `Author` length of exactly `30` characters  
**Then** the response status code is `200 OK`  
**And** the response body contains the created book with author length of 30

### @Positive Add book with title exactly 100 characters (boundary)
**Scenario:** Create a book with title at maximum valid length  
**Given** no book exists with id `X`  
**When** a client sends `POST /api/books` with `Title` length of exactly `100` characters  
**Then** the response status code is `200 OK`  
**And** the response body contains the created book with title length of 100

### @Negative Add book with empty author
**Scenario:** Try to create a book with empty author field  
**Given** no book exists with id `X`  
**When** a client sends `POST /api/books` with `Author: ""`  
**Then** the response status code is `400 Bad Request`

### @Negative Add book with empty title
**Scenario:** Try to create a book with empty title field  
**Given** no book exists with id `X`  
**When** a client sends `POST /api/books` with `Title: ""`  
**Then** the response status code is `400 Bad Request`

### @Negative Add book with whitespace-only author
**Scenario:** Try to create a book with whitespace-only author field  
**Given** no book exists with id `X`  
**When** a client sends `POST /api/books` with `Author: "   "` (3 spaces)  
**Then** the response status code is `400 Bad Request`

### @Negative Add book with whitespace-only title
**Scenario:** Try to create a book with whitespace-only title field  
**Given** no book exists with id `X`  
**When** a client sends `POST /api/books` with `Title: "   "` (3 spaces)  
**Then** the response status code is `400 Bad Request`

### @Negative Add book with invalid ID
**Scenario Outline:** Try to create a book with invalid ID values  
**Given** no book exists with id `InvalidId`  
**When** a client sends `POST /api/books` with `Id: InvalidId`  
**Then** the response status code is `400 Bad Request`

**Examples:**
    | InvalidId |
    | 0 |
    | -1 |

### @Positive Add book with special characters
**Scenario Outline:** Create a book with special characters in fields  
**Given** no book exists with id `X`  
**When** a client sends `POST /api/books` with special characters  
**Then** the response status code is `200 OK`  
**And** the response body contains the created book with special characters preserved

**Examples:**
    | Author | Title |
    | O'Brien | Book #1 |
    | Jean-Paul | A "Famous" Book |
    | Author | Book (Edition) |

## PUT /api/books/{id}

### @Positive Update existing book
**Scenario:** Update an existing book with valid data  
**Given** a book exists with id `X`  
**When** a client sends `PUT /api/books/X` with valid updated payload  
**Then** the response status code is `200 OK`  
**And** the response body reflects updated values

### @Positive Update existing book without optional params
**Scenario:** Update an existing book with required fields only  
**Given** a book exists with id `X`  
**When** a client sends `PUT /api/books/X` with required fields only (Id, Author, Title)  
**Then** the response status code is `200 OK`  
**And** the response body reflects updated values with Description as null or empty

### @Negative Update book with missing required params
**Scenario Outline:** Try to update a book with missing required fields  
**Given** a book exists with id `X`  
**When** a client sends `PUT /api/books/X` with missing `MissingField`  
**Then** the response status code is `400 Bad Request`

**Examples:**
    | MissingField |
    | Id |
    | Author |
    | Title |

### @Negative Update missing book
**Scenario:** Try to update a non-existing book  
**Given** no book exists with id `Y`  
**When** a client sends `PUT /api/books/Y` with valid payload  
**Then** the response status code is `404 Not Found`  
**And** the response body contains a JSON error with `Message: "Book with id Y not found!"`

### @Negative Update book with author length > 30
**Scenario:** Try to update existing book with too long author value  
**Given** a book exists with id `X`  
**When** a client sends `PUT /api/books/X` and `Author` length is `31`  
**Then** the response status code is `400 Bad Request`

### @Negative Update book with title length > 100
**Scenario:** Try to update existing book with too long title value  
**Given** a book exists with id `X`  
**When** a client sends `PUT /api/books/X` and `Title` length is `101`  
**Then** the response status code is `400 Bad Request`

## DELETE /api/books/{id}

### @Positive Delete existing book
**Scenario:** Delete one existing book  
**Given** a book exists with id `X`  
**When** a client sends `DELETE /api/books/X`  
**Then** the response status code is `204 No Content`

### @Negative Delete missing book
**Scenario:** Try to delete non-existing book  
**Given** no book exists with id `Y`  
**When** a client sends `DELETE /api/books/Y`  
**Then** the response status code is `404 Not Found`  
**And** the response body contains a JSON error with `Message: "Book with id Y not found!"`
