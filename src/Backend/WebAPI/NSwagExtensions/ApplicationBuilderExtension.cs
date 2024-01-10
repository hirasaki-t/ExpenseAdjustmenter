using NSwag.AspNetCore;

namespace WebAPI.NSwagExtensions;

/// <summary>アプリケーションビルダの拡張メソッド定義IF</summary>
public static class ApplicationBuilderExtension
{
    /// <summary>Microsoft Graphを使用したSwagger使用設定</summary>
    /// <param name="applicationBuilder">アプリケーションビルダ</param>
    /// <param name="clientId">クライアントID</param>
    /// <param name="graphScopes">Microsoft Graphのスコープ一覧</param>
    public static IApplicationBuilder UseSwagger(this IApplicationBuilder applicationBuilder, string clientId, string graphScopes)
    {
        applicationBuilder.UseOpenApi();
        applicationBuilder.UseSwaggerUi3(x =>
        {
            x.OAuth2Client = new OAuth2ClientSettings
            {
                ClientId = clientId,
                ClientSecret = string.Empty,
                UsePkceWithAuthorizationCodeGrant = true,
            };
            x.OAuth2Client.Scopes.Add(graphScopes);
            x.OAuth2Client.Scopes.Add("openid");
            x.OAuth2Client.Scopes.Add("profile");
        });
        return applicationBuilder;
    }
}