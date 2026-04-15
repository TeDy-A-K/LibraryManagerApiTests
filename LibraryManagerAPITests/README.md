# LibraryManagerAPITests

API test automation framework for the Library Manager service.

## Stack
- .NET 10 | NUnit | RestSharp | Allure

## Documentation
- [BDD Test Scenarios](LibraryManagerAPITests/Docs/ManualBDDScenarios.md)
- [Found Bugs](LibraryManagerAPITests/Docs/Bugs)

## Quick Start
```bash
dotnet test LibraryManagerAPITests/LibraryManagerAPITests.csproj
```

## Run by Category
```bash
dotnet test LibraryManagerAPITests/LibraryManagerAPITests.csproj --filter "Category=Positive"
dotnet test LibraryManagerAPITests/LibraryManagerAPITests.csproj --filter "Category=Negative"
```

## Allure Reports
```bash
dotnet test LibraryManagerAPITests/LibraryManagerAPITests.csproj
allure serve LibraryManagerAPITests/bin/Debug/net10.0/allure-results
```
