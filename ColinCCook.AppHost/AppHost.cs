var builder = DistributedApplication.CreateBuilder(args);

var mockServerContainer = builder
    .AddContainer("mockserver", "mockserver/mockserver")
    .WithHttpEndpoint(port: 1080, targetPort: 1080, name: "mockserver");

var mockServerContainerEndpoint = mockServerContainer
    .GetEndpoint("mockserver");

var web = builder
    .AddProject<Projects.ColinCCook_WebApi>("webapi")
    .WithReference(mockServerContainerEndpoint);

builder.Build().Run();
