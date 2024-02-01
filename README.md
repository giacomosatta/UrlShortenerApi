# WebAPI in C# .NET for URL Shortening

This repository contains a simple WebAPI built with C# .NET for shortening URLs. It takes an input URL, saves it to a local database, and returns a shortened URL with a randomly generated code.

## How it Works

1. **Shortening URL**: When a POST request is made to `/api/shorten`, the API takes a URL as input and validates its format. If the URL is valid, it generates a unique random code and creates a shortened URL. This shortened URL is then saved to the local database.

2. **Accessing Shortened URL**: Users can access the shortened URL by making a GET request to `/api/{code}` where `{code}` is the randomly generated code associated with the shortened URL. The API redirects the request to the original long URL stored in the database.

## Endpoints

- **POST /api/shorten**: Shortens a given URL and returns the shortened URL.

- **GET /api/{code}**: Redirects to the original URL associated with the provided code.

## Installation and Setup

1. Clone this repository to your local machine.

2. Ensure you have .NET Core SDK installed.

3. Update the connection string in `appsettings.json` with your local database details.

4. Navigate to the project directory and run the following commands:

   ```bash
   dotnet restore
   dotnet build
   dotnet run


## Technologies Used

   1. C# .NET
   2. Entity Framework Core
   3. ASP.NET Core
   4. Microsoft SQL Server (Local Database)


