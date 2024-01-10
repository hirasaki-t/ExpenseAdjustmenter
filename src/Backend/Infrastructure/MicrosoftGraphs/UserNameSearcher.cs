using Domain.Models.Users;
using Domain.Services;
using Microsoft.Graph;

namespace Infrastructure.MicrosoftGraphs;

public class UserNameSearcher : IUserNameSearcher
{
    private readonly GraphServiceClient graphServiceClient;

    public UserNameSearcher(GraphServiceClient graphServiceClient)
    {
        this.graphServiceClient = graphServiceClient;
    }

    public async Task<UserName?> SearchAsync(Mail mail)
    {
        var searchUser = await graphServiceClient.Users.Request().Filter($"mail eq '{mail.Value}'").Select("displayName").GetAsync();
        return searchUser.Any() ? new UserName(searchUser.Single().DisplayName) : null;
    }
}