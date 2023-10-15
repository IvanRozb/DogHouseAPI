# DogHouseAPI
DogHouseAPI is a simple project designed for uploading and retrieving information about dogs. 
It is built using the ONION architecture and incorporates essential design patterns such as the Repository pattern, Dependency Injection pattern, and Facade Pattern. 
The project is thoroughly tested with unit tests to ensure reliability and functionality.

## Table of Contents

- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Configuration](#configuration)
- [Usage](#usage)

## Prerequisites

Before you can run this project, you'll need to have the following tools and dependencies installed on your system:

- [.NET 7](https://dotnet.microsoft.com/download/dotnet/7.0)

## Installation
To run DogHouseAPI on your local machine, follow these steps:

1. **Install .NET 7**: If you don't have .NET 7 installed, download and install it from the official [Microsoft .NET website](https://dotnet.microsoft.com/download/dotnet/7.0).

2. **Clone the Repository**: Use the following command to clone the project repository to your local machine:

   ```bash
   git clone https://github.com/IvanRozb/DogHouseAPI
   
2. **Navigate to the Project Directory**: Change your working directory to the project folder:

   ```bash
   cd DogHouseAPI/Web/
   
3. **Start the Application**: Launch the application in watch mode using the following command:

   ```bash
   dotnet watch run

## Configuration

To configure this project, edit a `appsettings.json` file in the project root and set the following variables:

- `ConnectionStrings`:
  - `DefaultConnection`: Set the connection string to your own database.
- `ApiVersion`: The API version must begin with "Dogshouseservice.Version." This is important because if the API version doesn't follow this format, an error will be thrown when you access the /ping endpoint.
- `RateLimit`:
  - `PermitLimit`: Specifies the number of permits or requests allowed within a defined time window. By default, the permit limit is set to 10, meaning a client can make up to 10 requests within the specified time window.
  - `TimeWindow`: Represents the time window in which the permit limit applies. The value is set to 1 second, so the permit limit of 10 requests is allowed within a 1-second time window.
  - `AutoReplenishment`: This is a boolean value set to true.
    It indicates whether the rate limiter should automatically replenish permits after the time window elapses.
    When set to true, the rate limiter will replenish available permits, allowing clients to continue making requests after the defined time window has passed.
    If set to false, no automatic permit replenishment occurs, and clients must wait until the next time window to regain permits.

## Usage

### Swagger Documentation

When you start the project following the installation guide, you can access the Swagger documentation. Here's an overview of the API endpoints:

### Dogs

**GET `/dogs`**

This endpoint retrieves a list of dogs and supports the following query parameters:

- `attribute`: Sort by attribute (e.g., name, color, tail_length, or weight).
- `order`: Sort order (asc or desc).
- `pageNumber`: Page number for pagination (must be higher than 0).
- `pageSize`: Number of items per page (must be higher than 0).

If you don't specify these parameters, the endpoint returns all dogs. For example:

```bash
curl -X 'GET' \
  'http://localhost:5142/dogs' \
  -H 'accept: */*'
```

Response:

```
[
  {
    "name": "Bailey",
    "color": "sable",
    "tail_length": 11,
    "weight": 25
  },
  // More dog objects...
]
```

To apply sorting or pagination, set both `attribute `and `order` for sorting, and `pageNumber` and `pageSize` for pagination:

```bash
curl -X 'GET' \
  'http://localhost:5142/dogs?attribute=name&order=asc&pageNumber=1&pageSize=1' \
  -H 'accept: */*'
```

Response: 
```
[
  {
    "name": "Bailey",
    "color": "sable",
    "tail_length": 11,
    "weight": 25
  }
]

```

* The acceptable values for `attribute` are `name`, `color`, `tail_length`, and `weight`. If you provide an inappropriate value, a Bad Request Error (400) will be returned with the message: "attribute must be name, color, tail_length, or weight."

* The acceptable values for `order` are `asc` or `desc`. An inappropriate value will result in a Bad Request Error (400) with the message: "order must be asc or desc."

* `pageNumber` and `pageSize` must be higher than 0. If not, a Bad Request Error (400) will be sent with the respective message.

**POST `/dog`**

This endpoint allows you to create a new dog. All query parameters are required:

* `name` and `color` must be strings with a maximum limit of 50 characters.
* `tail_length` and `weight` must be numbers between 0.01 and 1000.

You cannot create a dog if a dog with the same name already exists; otherwise, an error will be returned.

Example:
```bash
curl -X 'POST' \
  'http://localhost:5142/dog' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "name": "Ashley",
  "color": "red",
  "tail_length": 3,
  "weight": 10
}'
```

Response:

```
{
  "name": "Ashley",
  "color": "red",
  "tail_length": 3,
  "weight": 10
}
```

### Ping

This section contains one endpoint:


**GET `/ping`**

This endpoint returns the current version of the API. You can configure it in `appsettings.json`. See [Configuration](#configuration).

Example:

```bash
curl -X 'GET' \
  'http://localhost:5142/ping' \
  -H 'accept: */*'
```

Response:

```
Dogshouseservice.Version1.0.1
```
