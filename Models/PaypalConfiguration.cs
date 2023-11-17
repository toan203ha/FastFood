using Microsoft.Extensions.Configuration;
using PayPal.Api;
using System;

public static class PaypalConfiguration
{
    // Variables for storing the clientID and clientSecret key
    public readonly static string ClientId;
    public readonly static string ClientSecret;

    // Constructor
    static PaypalConfiguration()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json") // Adjust the file path accordingly
            .Build();

        ClientId = configuration["Paypal:ClientId"];
        ClientSecret = configuration["Paypal:ClientSecret"];
    }

    // getting properties from IConfiguration
    public static IConfiguration GetConfig()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json") // Adjust the file path accordingly
            .Build();
        return configuration;
    }
    private static string GetAccessToken()
    {
        var config = GetConfig();
        string clientId = config["Paypal:ClientId"];
        string clientSecret = config["Paypal:ClientSecret"];

        // getting accesstoken from paypal
        string accessToken = new OAuthTokenCredential(clientId, clientSecret).GetAccessToken();
        return accessToken;
    }

    public static APIContext GetAPIContext()
    {
        // Create APIContext
        APIContext apiContext = new APIContext(GetAccessToken());

        // Convert IConfiguration to Dictionary<string, string>
        var configMap = GetConfig().AsEnumerable();
        var configDict = configMap.ToDictionary(x => x.Key, x => x.Value);

        // Assign configuration to apiContext
        apiContext.Config = configDict;

        return apiContext;
    }

}
