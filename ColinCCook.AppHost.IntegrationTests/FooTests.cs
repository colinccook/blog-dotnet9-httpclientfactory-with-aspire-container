using System.Net;
using System.Text;
using System.Text.Json;
using NUnit.Framework;

namespace ColinCCook.AppHost.IntegrationTests;

public class FooTests
{
    [TestCase]
    public async Task ReturnsBadRequest_When_BarServiceCallIsNotSuccessful()
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
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [TestCase]
    public async Task ReturnsOK_When_BarServiceCallIsSuccessful()
    {
        Assert.Inconclusive("Test not yet implemented. Need to set the expectation within mockserver to return a successful response");

        //     // Arrange
        //     var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.ColinCCook_AppHost>();
        //     appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        //     {
        //         clientBuilder.AddStandardResilienceHandler();
        //     });
        //     await using var app = await appHost.BuildAsync();
        //     var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();
        //     await app.StartAsync();

        //     // MockService expectation
        //     var httpClientMockServer = app.CreateHttpClient("servicename", "endpointname");
        //     await resourceNotificationService.WaitForResourceAsync("servicename", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));

        //     var expectation = new
        //     {
        //         httpRequest = new
        //         {
        //             method = "GET",
        //             path = "/bar"
        //         },
        //         httpResponse = new
        //         {
        //             statusCode = barStatusCode
        //         }
        //     };

        //     var json = JsonSerializer.Serialize(expectation);
        //     var content = new StringContent(json, Encoding.UTF8, "application/json");
        //     await httpClientMockServer.PutAsync("/mockserver/expectation", content);

        //     // Act
        //     var httpClient = app.CreateHttpClient("webapi");
        //     await resourceNotificationService.WaitForResourceAsync("webapi", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));
        //     var response = await httpClient.GetAsync("/foo");

        //     // Assert
        //     Assert.That(response.StatusCode, Is.EqualTo(fooStatusCode));

        //     // Verify MockService expectation
        //     var verification = new
        //     {
        //         httpRequest = new
        //         {
        //             method = "GET",
        //             path = "/bar"
        //         }
        //     };
        //     var verificationJson = JsonSerializer.Serialize(verification);
        //     var verificationContent = new StringContent(verificationJson, Encoding.UTF8, "application/json");
        //     var verificationResponse = await httpClientMockServer.PutAsync("/mockserver/verify", verificationContent);
        //     Assert.That(verificationResponse.StatusCode, Is.EqualTo(HttpStatusCode.Accepted));

        //     await httpClientMockServer.PutAsync("/mockserver/clear?type=EXPECTATIONS", null);
        // }
    }
}
