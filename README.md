# Using HttpClientFactory to call a Docker container within Aspire

I spent a long while trying to understand why a C# project was not hitting a Docker container properly via `IHttpClientFactory` in a .NET Aspire solution.

As far as I could tell, everything was set up correctly. The connection between the container and the service was configured in my `AppHost.cs` file, and an environment variable named `services__servicename__endpointname__0` with a value like `http://localhost:1080` was being correctly set in my application. My `IHttpClientFactory` was also being created using a named service.

However, the `BaseAddress` on the `HttpClient` was not being resolved to the container's address.

It turns out that, while `builder.AddServiceDefaults()` is essential, I also needed to explicitly register a named `HttpClient` that uses Aspire's service discovery URI format. This allows Aspire to correctly map the service name to the container's endpoint at runtime.

```csharp
builder.Services.AddHttpClient("httpclientname", httpClient =>
{
    // Aspire's Service Discovery intercepts this special URI format 
    // and replaces it with the correct address for the container.
    httpClient.BaseAddress = new("http://_endpointname.servicename");
});
```

## How This Demo Project Works

I created this demo project to solidify my learnings.

### Prerequisites

*   .NET 9 SDK (or newer)
*   Docker Desktop

### Getting Started

You can run the entire solution, including the mock server in a container, by running the Aspire host project from your terminal:

```bash
dotnet run --project ColinCCook.AppHost/ColinCCook.AppHost.csproj
```

### Architecture

Here is the basic architecture of the solution:

<img width="2244" height="630" alt="image" src="https://github.com/user-attachments/assets/a64d25c9-2c43-4705-98b7-8c4be8c24b0e" />

### Step 1: Consumer calls the FooService

The system under test is the `ColinCCook.WebApi` project. It exposes a single GET `/foo` endpoint.

While it's possible to call this endpoint directly after running the app, I've also written an integration test in the `ColinCCook.AppHost.IntegrationTests` project that does the same thing for your convenience.

### Step 2: The Foo Service calls the Bar service

The `/foo` endpoint injects an `IHttpClientFactory` and requests a named client called `httpclientname`. This client is configured in `Program.cs` to use the special Aspire service discovery address.

It then calls the Bar service's GET `/bar` endpoint.

### Step 3: The Bar service responds

The "Bar service" is a mock HTTP service (`mockserver`) running in a Docker container. I've used an expectation to set up the required HTTP contract.

By default, it will always return an `Accepted` (202) response. The integration test that covers the negative path will override this expectation to return a different response.

### Step 4: The Foo service responds

Depending on the Bar service's response, the Foo service will respond with either an `Accepted` or `NotFound` response. Both of these scenarios are covered in the integration tests.


