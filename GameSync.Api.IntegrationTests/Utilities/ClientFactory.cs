using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GameSync.Api.IntegrationTests.Utilities;

public static class ClientFactory
{
    public static HttpClient GetHttpClientWithMocks(WebApplicationFactory<Program> factory, Dictionary<Type, object> servicesToMock)
    {
        return factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(originalServices =>
            {
                foreach (var service in servicesToMock)
                {
                    if (service.Key.IsInterface)
                    {
                        originalServices.Replace(new ServiceDescriptor(service.Key, service.Value));
                    }
                }
            });
        })
        .CreateClient();
    }
}
