# Using HttpClientFactory to call a Docker container within Aspire

I spent a long while trying to understand why Csharp project was not hitting a Docker container properly via a `IHttpClientFactory`.

As far as I could tell, everything was set up correctly; the connection between container and service was set up successfully in my `AppHost.cs` file. An environmet variable called  `services__servicename__endpointname__0` with a value of `http://localhost:1080` was being set in my app. My `IHttpClientFactory` was being created using a named service.

But the BaseUri was not being set properly.

It turns out that, whilst I called, `builder.AddServiceDefaults();`, I still needed to add a named client specifically that mapped the servicename and endpoint name:

```csharp
builder.Services.AddHttpClient("httpclientname", httpClient =>
{
    // Service Discovery takes this URI format and creates the appropriate address
    httpClient.BaseAddress = new("http://_endpointname.servicename");
});
```

## How this demo project works

I created the demo project to test my learings.

Here is its basic architecture:

<img width="2244" height="630" alt="image" src="https://github.com/user-attachments/assets/a64d25c9-2c43-4705-98b7-8c4be8c24b0e" />

You can run this by running the Aspire host project under `ColinCCook.AppHost` project.

### Step 1: Consumer calls the FooService

The system under test is my `ColinCCook.WebApi` project. It exposes a single GET `/foo` endpoint.

Whilst it's possible to call this by running the app, I've written an integration test in the `ColinCCook.AppHost.IntegrationTests` project that does the same thing for your convenience.

### Step 2: The App Service calls the Bar service

The endpoint injects a IHttpClientFactory, and the code specifically asks for a named client called `httpclientname`. This is registered just above the endpoint code in the builder.

The Bar service is a mock http service. I've used `mockserver` with an expectation to set up the HTTP contract. It will return either an `Accepted` or `NotAcceptable` response.


