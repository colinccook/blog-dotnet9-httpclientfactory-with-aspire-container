var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHttpClient("servicename", httpClient =>
{
    // ServiceDiscovery takes this URI and swaps it out
    httpClient.BaseAddress = new("http://servicename");
});

var app = builder.Build();

app.MapGet("/foo", async (IHttpClientFactory httpClientFactory) =>
{
    var httpClientServiceName = httpClientFactory.CreateClient("servicename");
    var responseServiceName = await httpClientServiceName.GetAsync("/bar");

    if (!responseServiceName.IsSuccessStatusCode)
    {
        return Results.BadRequest();
    }

    return Results.Ok();
});

app.UseDeveloperExceptionPage();

app.Run();