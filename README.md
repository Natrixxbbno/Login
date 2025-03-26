# Authentication Application

A Windows Forms application for user authentication and registration, developed in C#.

## Features

-   Modern and user-friendly interface
-   User authentication
-   New user registration
-   Complete input validation
-   Secure password storage (SHA256 hashing)
-   Account management (account deletion)
-   Validations for:
    -   Email
    -   Username (3-20 characters, letters, numbers, and underscore)
    -   Password (minimum 8 characters, uppercase and lowercase letters, numbers, and special characters)

## System Requirements

-   Windows 10 or newer
-   .NET Framework 6.0 or newer
-   PostgreSQL (included in project)

## Installation

1. Clone the repository
2. Open the solution in Visual Studio
3. Build and run the project

## Project Structure

-   `Form1.cs` - Main application form
-   `DatabaseManager.cs` - Database management
-   `Program.cs` - Application entry point

## Security

-   Passwords are stored using SHA256 hashing
-   Complete input validation
-   Protection against SQL injection
-   Secure session management

## Development

The project is developed using:

-   C# Windows Forms
-   PostgreSQL for data storage
-   .NET Framework 6.0

## License

This project is licensed under the MIT License.
