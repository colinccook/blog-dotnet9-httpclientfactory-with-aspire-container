Problem:

The IHttpClientFactory is not picking up the `mockserver` service. It is throwing a missing base URI exception.

`mockserver` should be picked up by ServiceDefaults in the WebApi project.

`AppHost.cs` is defining a Docker container and aliasing it as `mockserver`. I have linked it to the WebApi with `.WithReference`

I've created a single NUnit test that is currently failing.

The Aspire dashboard is telling me that there's an environment variable in the WebApi project called `services__mockserver__mockserver__0`, with a value of `http://localhost:1080`.

(When I've figured it out, I'll actually set the expecation on mockserver to return a response when '/bar' is actually hit)