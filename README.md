# CardRegistrationAPI

# Create SQL Server container

1. Install Docker
2. Run docker command to create SQL Server container, docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong@Passw0rd" -p 1433:1433 --name sql1 -h sql1 -d mcr.microsoft.com/mssql/server:2019-latest

# Create database schema:
1. Open CardRegistrationAPI.sln solution
2. In Package Manager Console, Run update-data from Infrastructure project

# Start Rest API
1. Open CardRegistrationAPI.sln solution.
2. Click F5 to start the API service.

# API routes:

Note: replace port number in base url

Get cards for user:
- HttpGet
- https://localhost:44388/api/v1/users/{userid}/cards

Get all cards:
- HttpGet
- https://localhost:44388/api/v1/cards

Get single card for user:
- HttpGet
- https://localhost:44388/api/v1/users/{userId}/cards/{cardId}

create card:
- HttpPost
- https://localhost:44388/api/v1/users/{userId}/cards
- sample payload
```
{
 "cardNumber": 4012888888881881,
 "cvc": 123,
 "expiryMonth": 9,
 "expiryYear": 2022,
 "userId": 3
}
```

## Technologies used

- Dot Net Framework 3.1
- Clean Code Architecture
- CQRS
- MediatR
- EF Core
- SQL Server
- Docker
- EF Core Migration
- Dependency Injection
- SOLID
- CreditCardValidator

Integration tests:
- xUnit
- FluentAssertion
- EF In Memory DB
- microsoft.aspnetcore.mvc.testing


Unit tests:
- xUnit
- Moq
- FluentAssertion
- Microsoft.NET.Test.Sdk


