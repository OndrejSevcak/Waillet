**This file serves as an entry point for AI agents working on this repository**

## About this project

This repository contains the Waillet application, a web-based crypto wallet system built using Blazor for the frontend and ASP.NET Core Web API for the backend. The application allows users to create a separate wallet for each asset, move assets between wallets and make transfers to different accounts.

The project is structured into two main parts:
- **Waillet.Blazor**: The client-side Blazor application for user interaction.
- **WailletAPI**: The server-side ASP.NET Core API handling business logic, data access, and authentication.

Key technologies: C#, .NET, Entity Framework Core, SQL Server, JWT for authentication, and Blazor WebAssembly.

## Basic rules (AI agents)

1. **Git operations**
   - Always create a new branch for any changes or features.
   - Use descriptive commit messages following the format: "feat: add new feature" or "fix: resolve issue".
   - Do not commit directly to the main branch; use pull requests for code review.
   - Before committing, ensure all tests pass and code is linted.
   - Squash commits when merging to keep history clean.

2. **README**
   - Update the README.md file whenever adding new features, dependencies, or setup instructions.
   - Include clear setup steps, prerequisites, and usage examples.
   - Keep it concise but comprehensive; use sections for installation, configuration, and API documentation.

3. **Coding conventions**
   - Follow C# coding standards: use PascalCase for classes/methods, camelCase for variables, and proper indentation.
   - Use meaningful variable and method names; avoid abbreviations.
   - Implement proper error handling with try-catch blocks and custom Result classes.
   - Ensure code is asynchronous where appropriate, especially for I/O operations.
   - Use dependency injection for services and repositories.

4. **Blazor specific rules**
   - Use Razor components with proper separation of concerns: logic in .cs files, UI in .razor.
   - Avoid direct DOM manipulation; use Blazor's data binding and event handling.
   - Optimize performance by using @key directives in loops and minimizing re-renders.
   - Follow component naming conventions: e.g., WalletComponent.razor.

5. **Services and cache**
   - Implement services as interfaces and classes in the Services folder.
   - Use caching (e.g., in-memory or distributed) for frequently accessed data to improve performance.
   - Ensure services are registered in Program.cs with appropriate lifetimes (scoped, singleton).
   - Handle cache invalidation properly, especially for user-specific data.

## Technical documentation

Refer to the docs/ folder for detailed specifications:
- ProductSpecification.md: High-level product requirements and features.

For API documentation, see the controllers in WailletAPI/Controllers/ and the Swagger setup in Program.cs.

## Important files (do not change without explicit permission)

- **WailletAPI/Data/database_schema.sql**: Core database schema; changes require migration scripts.
- **WailletAPI/appsettings.json**: Configuration settings; sensitive data should be in environment variables.
- **Waillet.Blazor/Program.cs**: Application startup; modifications affect the entire app.
- **WailletAPI/Program.cs**: API startup configuration.
- **WailletAPI/Configuration/JwtSettings.cs**: JWT configuration; handle securely.

## Security

- Never hardcode sensitive information like API keys or passwords
- Validate all user inputs to prevent injection attacks.
- Use HTTPS in production and enable CORS appropriately.
- Regularly update dependencies for security patches.
- Log security events but avoid logging sensitive data.