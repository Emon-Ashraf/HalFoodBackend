# HalFood Backend

An ASP.NET Core API for the HalFood Android application. It provides accounts, shop and product management, baskets, and ordering workflows for restaurants and butchers.

## Features

- Register and sign in with JWT-based authentication.
- Register shops and manage their product listings.
- Add products to a basket, update quantities, and place orders.
- Browse shop products and retrieve customer or shop orders.

## Tech Stack

C#, ASP.NET Core 8, Entity Framework Core, PostgreSQL, and Swagger.

## Getting Started

Install the .NET 8 SDK and provision a PostgreSQL database. Configure `ConnectionStrings__DefaultConnection`, `Jwt__Key`, `Jwt__Issuer`, and `Jwt__Audience` in your local environment. Apply the included migrations with an EF Core 8 CLI tool, then start the development host:

```sh
dotnet ef database update --project DAL --startup-project HalFoodBackend
dotnet run --project HalFoodBackend
```

Use the development URL printed at startup to open `/swagger`.
