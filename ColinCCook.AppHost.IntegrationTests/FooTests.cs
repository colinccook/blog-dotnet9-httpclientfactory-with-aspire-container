using System.Net;
using System.Text;
using System.Text.Json;
using NUnit.Framework;

namespace ColinCCook.AppHost.IntegrationTests;

public class FooTests
{
    [TestCase]
    public async Task ReturnsAccepted_When_BarServiceIsCalledSuccessfully()
    {
        // Arrange
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.ColinCCook_AppHost>();
        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });
        await using var app = await appHost.BuildAsync();
        var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();
        await app.StartAsync();

        // Act
        var httpClient = app.CreateHttpClient("webapi");
        await resourceNotificationService.WaitForResourceAsync("webapi", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));
        var response = await httpClient.GetAsync("/foo");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Accepted));
    }

    [TestCase]
    public async Task ReturnsNotFound_When_BarServiceIsCalledUnsuccefully()
    {
        // Arrange
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.ColinCCook_AppHost>();
        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });
        await using var app = await appHost.BuildAsync();
        var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();
        await app.StartAsync();

        // Arrange MockServer expecatations; clear any away and add a new one to return a 404
        var httpClientMockServer = app.CreateHttpClient("servicename", "endpointname");
        await resourceNotificationService.WaitForResourceAsync("servicename", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        var expectation = new
        {
            httpRequest = new
            {
                method = "GET",
                path = "/bar"
            },
            httpResponse = new
            {
                statusCode = HttpStatusCode.NotAcceptable
            }
        };

        var json = JsonSerializer.Serialize(expectation);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        Assert.That((await httpClientMockServer.PutAsync("/mockserver/reset", null)).StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That((await httpClientMockServer.PutAsync("/mockserver/expectation", content)).StatusCode, Is.EqualTo(HttpStatusCode.Created));
        

        // Act
        var httpClient = app.CreateHttpClient("webapi");
        await resourceNotificationService.WaitForResourceAsync("webapi", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));
        var response = await httpClient.GetAsync("/foo");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}
