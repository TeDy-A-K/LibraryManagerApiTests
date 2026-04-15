using LibraryManagerAPITests.Config;
using RestSharp;

namespace LibraryManagerAPITests.Core;

public static class RestClientFactory
{
    public static RestClient Create(ApiSettings settings)
    {
        var options = new RestClientOptions(settings.BaseUrl)
        {
            Timeout = TimeSpan.FromSeconds(settings.TimeoutSeconds)
        };

        return new RestClient(options);
    }
}
