var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var app = builder.Build();

app.MapGet("/foo", async (IHttpClientFactory httpClientFactory) =>
{
    var httpClient = httpClientFactory.CreateClient("servicename");
    var response = await httpClient.GetAsync("/bar");

    if (!response.IsSuccessStatusCode)
    {
        return Results.BadRequest();
    }

    return Results.Ok();
});

app.MapGet("/environment-variables", (ILoggerFactory loggerFactory) =>
{
    var logger = loggerFactory.CreateLogger("EnvironmentVariablesLogger");
    foreach (var env in Environment.GetEnvironmentVariables().Cast<System.Collections.DictionaryEntry>())
    {
        logger.LogInformation("{Key}={Value}", env.Key, env.Value);
    }
    return Results.Ok();
});

app.UseDeveloperExceptionPage();

app.Run();