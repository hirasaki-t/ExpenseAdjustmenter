using Domain.Services;
using Microsoft.Graph;

namespace Infrastructure.MicrosoftGraphs;

public class LoginUserGetter : ILoginUserGetter
{
    private readonly GraphServiceClient graphServiceClient;

    public LoginUserGetter(GraphServiceClient graphServiceClient)
    {
        this.graphServiceClient = graphServiceClient;
    }

    public async Task<string> GetMailAsync() =>
        (await graphServiceClient.Me.Request().Select(x => x.Mail).GetAsync()).Mail;

    public async Task<string> GetNameAsync() =>
        (await graphServiceClient.Me.Request().Select(x => x.DisplayName).GetAsync()).DisplayName;
}