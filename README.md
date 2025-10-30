# Blazor Application Suite

This repository now provides a single entry point for all of the sample applications that ship with it.  Open the `BlazorSuite.sln` solution in Visual Studio or `dotnet` tooling to work with any of the contained projects without switching repositories.

## Included projects

| Project | Path | Description |
| --- | --- | --- |
| **BlazorWeatherApp** | `BlazorWeatherApp/BlazorWeatherApp.csproj` | Weather dashboard demonstrating data fetching and reusable components. |
| **BulkyBookWeb** | `Bulky/BulkyWeb/BulkyBookWeb.csproj` | ASP.NET Core MVC storefront backed by supporting class library projects for data access, models, and utilities. |
| **BulkyBook.DataAccess** | `Bulky/Bulky.DataAccess/BulkyBook.DataAccess.csproj` | Entity Framework Core data layer used by the BulkyBook web application. |
| **BulkyBook.Models** | `Bulky/Bulky.Models/BulkyBook.Models.csproj` | Shared model classes for the BulkyBook ecosystem. |
| **BulkyBook.Utility** | `Bulky/Bulky.Utility/BulkyBook.Utility.csproj` | Utility helpers and constants for BulkyBook. |
| **PmsApi** | `PmsApi/PmsApi.csproj` | Web API for the project management system sample. |
| **YumBlazor** | `YumBlazor/YumBlazor.csproj` | Recipe management demo built with Blazor. |
| **LearnBlazor** | `LearnBlazor/LearnBlazor.csproj` | Collection of learning-focused Blazor components and helpers. |
| **TodoList** | `TodoList/TodoList.csproj` | Simple todo tracking application showcasing CRUD operations. |

## Getting started

1. Restore dependencies and build the desired project:
   ```bash
   dotnet build BlazorSuite.sln
   ```
   Building the solution compiles every sample against the exact same project files that existed before the consolidation, so functionality and reference graphs remain unchanged.
2. Run a specific sample by invoking `dotnet run` from its project directory, e.g.:
   ```bash
   cd BlazorWeatherApp
   dotnet run
   ```

Each project retains its original structure and configuration, so existing documentation and tutorials continue to apply.
