# Order Delicious 🍽️

A food delivery web application built with ASP.NET and PostgreSQL, supporting multi-branch restaurants, menu browsing, cart management, and order placement for customers, restaurant staff, and admins.

## Screenshots/Demo

Coming soon

## Features

- Multi-branch restaurant listings with address-based location
- Menu browsing by category
- Cart management
- Order placement and tracking
- Role-based access (Customer, Employee, Admin)
- Customer reviews

## Tech Stack

- **Backend:** ASP.NET Core, C#
- **Database:** PostgreSQL, EF Core (migrations)

## Architecture

The project uses a layered architecture built around clear separation of concerns:

- **Domain layer** contains the core business entities and (would learn more to include     rules so i can apply DDD ).
- **Application layer** handles use cases and business logic such as authentication, categories, items, and other features.
- **Infrastructure layer** is responsible for data access, persistence, and integration with PostgreSQL through EF Core.
- **API layer** exposes the functionality through controllers and endpoints.

In short, requests flow from the API layer into the application logic, then through the infrastructure layer to interact with the database and return the result.

## Database Design

### High-Level ERD

> A simplified overview of the core domain model. Implementation details such as keys, indexes, constraints, and join entities are omitted for readability.

![ER Diagram](DOCS/Diagrams/Order%20Delicious.drawio.png)

## How To Run

### Prerequisites

- .NET SDK 8.x
- PostgreSQL 18.4

### Setup

\`\`\`bash
git clone <https://github.com/Abdulluh11235/order-delicious.git>
cd order-delicious
dotnet restore
dotnet ef database update
dotnet run
\`\`\`

## License
