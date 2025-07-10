var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHttpClient("httpclientname", httpClient =>
{
    // Service Discovery takes this URI format and creates the appropriate address
    httpClient.BaseAddress = new("http://_endpointname.servicename");
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // add Swashbuckle.AspNetCore

var app = builder.Build();

app.MapGet("/foo", async (IHttpClientFactory httpClientFactory) =>
{
    var httpClientServiceName = httpClientFactory.CreateClient("httpclientname");
    var responseServiceName = await httpClientServiceName.GetAsync("/bar");

    if (!responseServiceName.IsSuccessStatusCode)
    {
        return Results.BadRequest();
    }

    return Results.Ok();
});

app.UseDeveloperExceptionPage();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();