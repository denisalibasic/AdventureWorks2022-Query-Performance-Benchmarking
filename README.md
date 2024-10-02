# AdventureWorks Query Performance Benchmark
This repository demonstrates and benchmarks different query approaches in C# .NET 8 using the AdventureWorks2022 database. It compares the performance of the following query types:

- Entity Framework (EF) Queries
- Raw SQL in EF
- Stored Procedures (SP)
- Dapper

The project consists of a console application, an API, a Blazor WebApp, and Docker configurations for the database and RabbitMQ.

## Table of Contents
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Setup](#setup)
- [AdventureWorksQueryPerformance](#adventureworksqueryperformance)
- [AdventureWorksQueryPerformanceAPI](#adventureworksqueryperformanceapi)
- [WebApp](#webapp)
- [Database](#database)
- [RabbitMQ](#rabbitmq)
- [Usage](#usage)
  - [WebApp Functionality](#webapp-functionality)
  - [AdventureWorksQueryPerformance Output](#adventureworksqueryperformance-output)
- [Run Query Performance Benchmarks](#run-query-performance-benchmarks)
- [Contributions](#contributions)
- [License](#license)
- [Blog](#blog)

## Getting Started
### Prerequisites
- .NET 8 SDK
- Docker

### Setup
Instructions how to run the project locally
Clone the repository:
```bash
git clone https://github.com/denisalibasic/AdventureWorks2022-Query-Performance-Benchmarking.git
cd AdventureWorksQueryPerformance
 ```

Run SQL Server Database:
Navigate to the docker-compose file for the database and run:
```bash
docker-compose up -d
 ```

Run RabbitMQ:
Navigate to the docker-compose file for RabbitMQ and run:
```bash
docker-compose up -d
 ```

## AdventureWorksQueryPerformance
A console application built on .NET 8 that benchmarks various query methods. It performs the following actions:

- Executes the queries sequentially and records the execution time.
- Produces an HTML file with a bar chart visualizing the performance results.
- Publishes messages to RabbitMQ for each completed query.
- Returns results in the following format:
  ```bash
   public class TaskResult
  {
      public string TaskName { get; set; } = string.Empty;
      public long ElapsedMilliseconds { get; set; }
  }
   ```
## AdventureWorksQueryPerformanceAPI
A REST API built on .NET 8 that:

- Provides an endpoint /run-queries to trigger the query performance tests.
- Communicates with the AdventureWorksQueryPerformance console app via a QueryPerformanceApiAdapter and returns the performance results.

## WebApp
A Blazor WebApp built on .NET 8 that:

- Fetches messages from RabbitMQ and displays them on the screen.
- Sends a request to the AdventureWorksQueryPerformanceAPI to run the queries.
- Once the response is received, displays the results in a table format.

## Database
- Dockerized SQL Server using the image mcr.microsoft.com/mssql/server:2022-latest.
- Uses the AdventureWorks2022 database.

## RabbitMQ
- Dockerized RabbitMQ used for messaging between the applications.

## Usage
### WebApp Functionality
Fetch RabbitMQ Messages:
- On the WebApp interface, click the button to fetch messages from RabbitMQ.
- Messages are produced by the AdventureWorksQueryPerformance console app for each query executed.

Run Queries:
- Trigger the query execution by sending a request to the AdventureWorksQueryPerformanceAPI.
- The WebApp will display the performance results in a table once the response is received.

### AdventureWorksQueryPerformance Output
- HTML reports with bar charts displaying the query performance are generated in the bin folder after executing the queries.
- Each query result is published as a message to RabbitMQ, and these messages are displayed on the WebApp.

## Run Query Performance Benchmarks
You have two options to run the query performance benchmarks:
1. Run AdventureWorksQueryPerformance (Console Application)
   This option generates an HTML report with a bar chart of the query performance results.
   - ```bash
     cd AdventureWorksQueryPerformance
     dotnet run
     ```
   - The HTML file with the performance results will be generated in the bin folder.
2. Navigate to the AdventureWorksQueryPerformance console application folder and run it:
This option displays the results directly on a website.

Step 1: Run the AdventureWorksQueryPerformanceAPI to execute the queries and serve the results.
- Navigate to the AdventureWorksQueryPerformanceAPI project folder and start the API:
  ```bash
   cd AdventureWorksQueryPerformanceAPI
   dotnet run
  ```
- The API will start on http://localhost:5141.

Step 2: Run the Blazor WebApp to view the query results in a web interface.
- Navigate to the WebApp project folder and start the Blazor WebApp:
  ```bash
   cd WebApp
   dotnet run
  ```
- The WebApp will start on http://localhost:5014.

In the WebApp, you can trigger the query execution, fetch results from RabbitMQ, and view the performance results in a table.

## Contributions
Feel free to fork this repository, make improvements, and submit a pull request. For major changes, please open an issue to discuss what you'd like to change.

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Blog
Link to related blog post: TODO
