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
