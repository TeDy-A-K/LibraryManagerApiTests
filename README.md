# LibraryManagerAPITests

API test automation framework for the Library Manager service.

## Stack
- .NET 10
- NUnit
- RestSharp
- Allure

## Project
- `LibraryManagerAPITests/LibraryManagerAPITests.csproj`
- `LibraryManagerAPITests/Docs/ManualBDDScenarios.md`

## Run tests
```bash
dotnet test LibraryManagerAPITests/LibraryManagerAPITests.csproj
```

## Run by category
```bash
dotnet test LibraryManagerAPITests/LibraryManagerAPITests.csproj --filter "Category=Positive"
dotnet test LibraryManagerAPITests/LibraryManagerAPITests.csproj --filter "Category=Negative"
```

## Allure
```bash
dotnet test LibraryManagerAPITests/LibraryManagerAPITests.csproj
allure serve LibraryManagerAPITests/bin/Debug/net10.0/allure-results
```
