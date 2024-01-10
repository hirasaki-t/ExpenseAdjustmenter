using NSwag;

namespace WebAPI.NSwagExtensions;

/// <summary>サービスコレクションの拡張メソッド定義IF</summary>
public static class ServiceCollectionExtension
{

    /// <summary>Microsoft Graphの認証と合わせてSwaggerDocumentを定義する</summary>
    /// <param name="serviceCollection">サービスコレクション</param>
    /// <param name="apiTitle">API名(Swagger上部に表示される)</param>
    /// <param name="graphScopes">Microsoft Graphのスコープの一覧</param>
    /// <param name="authorizationUrl">認証URL(Azure Portalから取得)</param>
    /// <param name="tokenUrl">トークン要求URL(Azure Portalから取得)</param>
    public static IServiceCollection AddSwaggerDocumentWithGraphAuthentication(
        this IServiceCollection serviceCollection,
        string apiTitle,
        string graphScopes,
        string authorizationUrl,
        string tokenUrl)
    {
        serviceCollection.AddSwaggerDocument(x =>
        {
            x.Title = apiTitle;
            x.AddSecurity("bearer", Enumerable.Empty<string>(), new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.OAuth2,
                Flow = OpenApiOAuth2Flow.AccessCode,
                ExtensionData = new Dictionary<string, object> { { "x-tokenName", "id_token" } },
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        Scopes = new Dictionary<string, string>
                    {
                        { graphScopes, "Microsoft Graph scopes" },
                        { "openid", "openid" },
                        { "profile", "profile" }
                    },
                        AuthorizationUrl = authorizationUrl,
                        TokenUrl = tokenUrl,
                    },
                },
                In = OpenApiSecurityApiKeyLocation.Header,
            });
        });

        return serviceCollection;
    }
}