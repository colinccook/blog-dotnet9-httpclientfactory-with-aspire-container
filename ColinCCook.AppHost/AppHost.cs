var builder = DistributedApplication.CreateBuilder(args);

var service = builder
    .AddContainer("servicename", "mockserver/mockserver")
    .WithHttpEndpoint(port: 1080, targetPort: 1080, name: "endpointname");

var endpoint = service
    .GetEndpoint("endpointname");

var web = builder
    .AddProject<Projects.ColinCCook_WebApi>("webapi")
    .WithReference(endpoint);

builder.Build().Run();
