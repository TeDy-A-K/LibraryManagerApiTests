using LibraryManagerAPITests.Config;
using Microsoft.Extensions.Configuration;

namespace LibraryManagerAPITests.Core;

public static class ConfigurationProvider
{
    private static readonly Lazy<ApiSettings> _apiSettings = new(LoadApiSettings);

    public static ApiSettings GetApiSettings()
    {
        return _apiSettings.Value;
    }

    private static ApiSettings LoadApiSettings()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(TestContext.CurrentContext.TestDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .Build();

        var settings = configuration.GetSection("ApiSettings").Get<ApiSettings>();

        if (settings is null)
        {
            throw new InvalidOperationException("ApiSettings section is missing from appsettings.json.");
        }

        return settings;
    }
}
