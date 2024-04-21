using Pulumi;
using Pulumi.Auth0;
using Pulumi.Auth0.Inputs;
using System.Collections.Generic;

return await Deployment.RunAsync(() =>
{

    // Create an Auth0 Connection to enable Google social login.
    var googleConnection = new Connection("google-oauth2-connection", new ConnectionArgs
    {
        Name = "google-oauth2",         // Unique name for the connection for referencing in your application
        Strategy = "google-oauth2", // Specify the strategy as "google-oauth2" for Google social login

        // Configuration options specific to the Google OAuth2 strategy
        Options = new ConnectionOptionsArgs
        {
            ClientId = "your-google-client-id",
            ClientSecret = "your-google-client-secret",

            // The permissions that your application will request from Google
            Scopes = { "email", "profile" },

        },

        // Indicates whether this connection can be used with Auth0 Development keys
        IsDomainConnection = false
    });

    // create Auth0 client
    var auth0Client = new Client("client", new ClientArgs
    {
        AllowedLogoutUrls =
        {
            "https://example.com/logout",
        },
        AllowedOrigins =
        {
            "https://example.com",
        },
        AppType = "regular_web",
        Callbacks =
        {
            "https://example.com/auth/callback",
        },
        JwtConfiguration = new ClientJwtConfigurationArgs
        {
            Alg = "RS256",
        },
    });

    // Export Client ID and Secret
    return new Dictionary<string, object?>
    {
        ["clientID"] = auth0Client.ClientId,
        ["clientSecret"] = auth0Client.ClientSecret
    };
});
