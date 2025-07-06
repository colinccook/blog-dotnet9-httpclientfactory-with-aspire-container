var builder = DistributedApplication.CreateBuilder(args);

var service = builder
    .AddContainer("servicename", "mockserver/mockserver")
    .WithHttpEndpoint(port: 1080, targetPort: 1080, "endpointname");

var web = builder
    .AddProject<Projects.ColinCCook_WebApi>("webapi")
    .WithReference(service.GetEndpoint("endpointname"));

builder.Build().Run();
