var builder = DistributedApplication.CreateBuilder(args);

var service = builder
    .AddContainer("servicename", "mockserver/mockserver")
    .WithHttpEndpoint(port: 1080, targetPort: 1080, "http");

var web = builder
    .AddProject<Projects.ColinCCook_WebApi>("webapi")
    .WithReference(service.GetEndpoint("http"));

builder.Build().Run();
