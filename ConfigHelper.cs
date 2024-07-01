using System;

public static class ConfigHelper
{
    private static IConfigurationRoot config;

    public static ConfigHelper()
    {
        config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }

    public static string GetBaseUrl()
    {
        var currentEnvironment = config["CurrentEnvironment"];
        return config[$"Environments:{currentEnvironment}:BaseUrl"];
    }
}