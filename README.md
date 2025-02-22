```markdown
# Calculator

The Calc project is a simple calculator web application built using ASP.NET Core. It provides a user-friendly interface for performing basic arithmetic operations and an API for managing calculation records. The application is divided into three main components: the UI, the API, and the database.

## Overview

The Calc project is structured into three main components:

1. **UI**: Built using ASP.NET Core Razor Pages, HTML, CSS, JavaScript, and Bootstrap. It provides the user interface for performing calculations and viewing the history of calculations.
2. **API**: Built using ASP.NET Core Web API and Entity Framework Core. It handles the backend logic and data storage.
3. **Database**: A SQL Server instance that stores calculation records.

### Project Structure

- **Calculator.API**: Contains the API logic, controllers, DTOs, and database contexts.
- **Calculator.UI**: Contains the Razor Pages, views, and static assets for the UI.
- **Calculator.Database**: Contains SQL Server database project and SQL scripts.
- **Calculator.ErrorHandler**: Contains error handling logic.
- **Calculator.MathLib**: Contains the mathematical logic for evaluating expressions.
- **Calculator.APITests**: Contains unit tests for the API.
- **Calculator.MathLibTests**: Contains unit tests for the MathLib.

### Technologies Used

- **Frontend**: ASP.NET Core Razor Pages, HTML, CSS, JavaScript, Bootstrap
- **Backend**: ASP.NET Core Web API, Entity Framework Core, SQL Server
- **Database**: SQL Server
- **Libraries and Packages**:
  - Entity Framework Core
  - Swashbuckle.AspNetCore (for Swagger/OpenAPI integration)
  - Bootstrap (for UI styling)
  - jQuery Validation (for client-side form validation)
  - Serilog (for logging)

## Features

1. **Calculator UI**:
   - **Display of Calculations**: Shows the current calculation expression and result.
   - **Input Buttons**: Buttons for digits (0-9), operators (+, -, *, /), decimal point, and clear.
   - **Calculation**: Button to perform the calculation of the entered expression.
   - **History**: Section that displays the history of past calculations.
   - **Integers Only Option**: Checkbox to restrict calculations to integer results only.

2. **API**:
   - **Calculation Expression Records**: Endpoints to create, retrieve, update, and delete calculation expression records.
   - **Calculation Endpoint**: Endpoint to perform calculations based on the provided expression.
   - **Load Last Calculations**: Endpoint to retrieve the last 'n' calculation records.

3. **Database**:
   - **Calculation Expression Record Storage**: SQL Server database to store calculation expression records.
   - **Entity Framework Core**: Used for database context and migrations.

## Getting started

### Requirements

- **ASP.NET Core SDK**: SDK for building .NET applications.
- **SQL Server**: Relational database management system.

### Quickstart

1. **Clone the repository**:
   ```sh
   git clone <repository-url>
   cd calculator
   ```

2. **Set up the database**:
   - Ensure SQL Server is running.
   - Update the connection string in `appsettings.json` to point to your SQL Server instance.
   - Run the migrations to set up the database schema:
     ```sh
     dotnet ef database update --project Calculator.API
     ```

3. **Run the application**:
   - Start the API:
     ```sh
     dotnet run --project Calculator.API
     ```
   - Start the UI:
     ```sh
     dotnet run --project Calculator.UI
     ```

4. **Access the application**:
   - Open your browser and navigate to `https://localhost:5001` to access the calculator UI.

### License

The project is proprietary (not open source). All rights reserved.

```
