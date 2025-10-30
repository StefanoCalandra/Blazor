# Blazor Suite Improvement Roadmap

This guide lists pragmatic enhancements for each project in the consolidated solution and calls out overlap so every sample can demonstrate a distinct scenario.

## Cross-project differentiation opportunities

- **Weather demos are repeated** in **BlazorWeatherApp**, **LearnBlazor**, and **YumBlazor**. The weather service in BlazorWeatherApp now layers geolocation, caching, and option validation on top of its API client so it stands out as the resilient integration example.【F:BlazorWeatherApp/Components/Pages/Home.razor†L1-L205】【F:BlazorWeatherApp/Services/WeatherService.cs†L1-L139】 *LearnBlazor* reimagines its page as a streaming lab, while *YumBlazor* pivots to pantry-driven meal planning so each project highlights a distinct scenario.【F:LearnBlazor/Components/Pages/Weather.razor†L1-L199】【F:YumBlazor/Components/Pages/MealPlanner.razor†L1-L268】
  - Further differentiate *BlazorWeatherApp* by adding offline-ready caching (IndexedDB) and resiliency tests that simulate upstream failures.
  - Grow the *LearnBlazor* lab with comparative samples for error boundaries or UI virtualization so learners can toggle multiple advanced features.
  - Expand the *YumBlazor* planner to sync with user profiles so authenticated cooks can store personal pantry defaults.
- **Back-office CRUD patterns** appear in Bulky (products/categories) and YumBlazor (recipes, categories) but target different domains. To keep them unique, evolve Bulky toward enterprise-grade catalog management (bulk import/export, audit logging) while YumBlazor leans into social and personalization features.
- **Data-provider integration** can connect the front-end samples. For instance, letting TodoList or LearnBlazor consume the PmsApi endpoints demonstrates API consumption and draws clearer lines between API and UI projects.【F:PmsApi/Controllers/ProjectsController.cs†L15-L159】

## Project-specific suggestions

### BlazorWeatherApp
- Validate the API key and base URL at startup, cache responses, and support coordinate-based lookups so the app remains responsive even when users rely on geolocation.【F:BlazorWeatherApp/Components/Pages/Home.razor†L7-L205】【F:BlazorWeatherApp/Services/WeatherService.cs†L1-L139】
- Next, layer retry and circuit-breaker policies (e.g., Polly) around outbound calls to handle transient faults more gracefully.【F:BlazorWeatherApp/Services/WeatherService.cs†L68-L110】
- Add comparative analytics like hourly breakdowns or historical charts to distinguish it from the educational samples.

### LearnBlazor
- The weather page now acts as an interactive lab that demonstrates snapshot fetching, streaming updates, cancelable loops, and logging so learners can explore multiple async patterns in one place.【F:LearnBlazor/Components/Pages/Weather.razor†L1-L199】
- Introduce mini-labs for JavaScript interop and form validation, with toggles to enable/disable features at runtime so learners can experiment without leaving the page.
- Publish markdown-driven lessons that link to the components in `Components/Pages` so the app reads like a tutorial rather than a grab bag of demos.【F:LearnBlazor/Program.cs†L1-L28】

### YumBlazor
- The placeholder weather page has been replaced with a pantry-aware meal planner that reacts to focus preferences and inventory toggles, reinforcing the app’s culinary identity.【F:YumBlazor/Components/Pages/MealPlanner.razor†L1-L268】
- Capitalize on the existing ASP.NET Core Identity setup to deliver saved meal plans, collaborative recipe collections, and dietary preference filters.【F:YumBlazor/Program.cs†L1-L57】
- Add repository abstraction tests to guard against regressions in the custom data layer under `Repository/` as features grow.

### TodoList
- Tasks now persist to protected local storage, track due dates, and support tag filtering so the sample showcases state management beyond in-memory collections.【F:TodoList/Components/Pages/Todo.razor†L1-L245】【F:TodoList/TodoItem.cs†L1-L9】
- Consider wiring the app to consume PmsApi for collaborative project tasks, turning it into a lightweight companion to the backend sample.【F:PmsApi/Controllers/ProjectsController.cs†L15-L159】
- Add reminder notifications (email or toast) to demonstrate background scheduling with persistent state.

### PmsApi
- Document how to use the `include` query-string to shape responses so UI clients can fetch exactly the data they need; the controllers already support optional includes for tasks, manager, and category.【F:PmsApi/Controllers/ProjectsController.cs†L28-L84】
- Add projection endpoints that emit dashboards (e.g., overdue tasks per project) to better showcase aggregation scenarios beyond pure CRUD.
- Provide sample API clients or Postman collections that front-end projects can import to kickstart integration tests.

### Bulky (BulkyBook)
- Leverage the existing filtering logic in the product API to add advanced catalog tooling such as saved searches, scheduled imports, or AI-powered recommendations.【F:Bulky/BulkyWeb/Areas/Admin/Controllers/ProductController.cs†L36-L172】
- Introduce domain events or background jobs for image processing and inventory synchronization to differentiate it from the other CRUD apps.【F:Bulky/BulkyWeb/Areas/Admin/Controllers/ProductController.cs†L70-L132】
- Publish onboarding documentation that walks through the multi-project architecture (Web, DataAccess, Models, Utility) so contributors understand the layered design.【F:Bulky/BulkyWeb/Areas/Admin/Controllers/ProductController.cs†L19-L132】
