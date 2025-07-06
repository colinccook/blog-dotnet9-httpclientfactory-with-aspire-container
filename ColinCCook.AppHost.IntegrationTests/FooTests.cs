namespace ColinCCook.AppHost.IntegrationTests;

public class FooTests
{
    [Test]
    public async Task ReturnsSuccessfully_When_ServiceIsCallingThirdPartyContainerProperly()
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
        var httpClientMockServer = app.CreateHttpClient("servicename", "endpointname");
        await resourceNotificationService.WaitForResourceAsync("servicename", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));
        var responseMockServer = await httpClientMockServer.GetAsync("/bar");

        var httpClient = app.CreateHttpClient("webapi");
        await resourceNotificationService.WaitForResourceAsync("webapi", KnownResourceStates.Running).WaitAsync(TimeSpan.FromSeconds(30));
        var response = await httpClient.GetAsync("/foo");

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}
