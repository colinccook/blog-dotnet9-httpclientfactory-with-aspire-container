Problem:

The `/foo` endpoint is throwing because I'm attempting to perform a GET without a valid BaseUri.

The BaseUri should be present, as it's an environment variable called `services__servicename__endpointname__0` with a value of `http://localhost:1080`.

The IHttpClientFactory is being passed `servicename`, but is failing to pick up the value.

`AppHost.cs` is defining a Docker container and aliasing it as `servicename`. I have linked it to the WebApi with `.WithReference`.

From my understanding, Aspire's Service Discovery should be picking up the environment variable, and then the IHttpClientFactory should use it.

I've created a single NUnit test that is currently failing.

I have created an `/environment-variables` endpoint that confirms that the enviornment variable mentioned above is definitely present in runtime.

(When I've figured it out, I'll actually set the expecation on mockserver to return a response when '/bar' is actually hit)