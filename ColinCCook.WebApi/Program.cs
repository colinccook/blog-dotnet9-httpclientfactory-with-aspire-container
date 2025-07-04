var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var app = builder.Build();

app.MapGet("/foo", async (IHttpClientFactory httpClientFactory) =>
{
    var httpClient = httpClientFactory.CreateClient("mockserver");
    var response = await httpClient.GetAsync("/bar");

    if (!response.IsSuccessStatusCode)
    {
        return Results.BadRequest();
    }

    return Results.Ok();
});

app.UseDeveloperExceptionPage();

app.Run();