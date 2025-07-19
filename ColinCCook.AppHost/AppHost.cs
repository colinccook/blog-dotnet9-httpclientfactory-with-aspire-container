var builder = DistributedApplication.CreateBuilder(args);

var service = builder
    .AddContainer("servicename", "mockserver/mockserver")
    .WithHttpEndpoint(port: 1080, targetPort: 1080, "endpointname")
    .WithBindMount("./config", "/config/", isReadOnly: true)
    // this code doesn't seem to be working for me, container just hangs on startup
    // .WithContainerFiles("/config", [
    //         new ContainerFile
    //         {
    //             Name = "expectation.json",
    //             Contents = """
    //           [
    //             {
    //               "httpRequest": {
    //                 "method": "GET",
    //                 "path": "/bar"
    //               },
    //               "httpResponse": {
    //                 "statusCode": 200
    //               },
    //               "times": {
    //                 "unlimited": true
    //               }
    //             }
    //           ]        
    //           """,
    //         }
    //     ])
    .WithEnvironment("MOCKSERVER_INITIALIZATION_JSON_PATH", "/config/expectation.json");

var web = builder
    .AddProject<Projects.ColinCCook_WebApi>("webapi")
    .WithReference(service.GetEndpoint("endpointname"));

builder.Build().Run();
