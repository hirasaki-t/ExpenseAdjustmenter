using Domain;
using Domain.Models.Expenses;
using Domain.Models.TraveringInformations;
using Domain.Models.Users;
using Domain.Repositories;
using Domain.Services;
using Domain.Storages;
using Moq;
using Usecase.TraveringExpenses;

namespace UnitTest.Usecase.TraveringExpenses;

public class TraveringExpenseAdderTest
{
    [Test]
    public async Task 領収書不要のケースで旅費交通費経費精算の追加に成功すること()
    {
        var traveringInformationRepository = new Mock<ITraveringInformationRepository>();
        var expenseRepository = new Mock<IExpenseRepository>();
        var userRepository = new Mock<IUserRepository>();
        var loginUserGetter = new Mock<ILoginUserGetter>();
        var storage = new Mock<IStorage>();
        var user = new User(new("山田 太郎"), new("yamada@google.com"), true, true);

        loginUserGetter.Setup(x => x.GetMailAsync()).ReturnsAsync("yamada@google.com");
        userRepository.Setup(x => x.GetAsync(user.Mail)).ReturnsAsync(user);

        await new TraveringExpenseAdder(traveringInformationRepository.Object, expenseRepository.Object, userRepository.Object, loginUserGetter.Object, storage.Object)
            .AddAsync(new DateOnly(2022, 12, 14), "現場業務", "初台", "東京", Ulid.NewUlid().ToString(), null, null, 500, null);

        traveringInformationRepository.Verify(x => x.AddAsync(It.IsAny<TraveringInformation>()));
        expenseRepository.Verify(x => x.AddAsync(It.IsAny<Expense>()));
        storage.Invoking(x => x.Verify(x => x.CreateFolderAsync(It.IsAny<string>()))).Should().Throw<MockException>();
        storage.Invoking(x => x.Verify(x => x.SaveAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>()))).Should().Throw<MockException>();
    }

    [Test]
    public async Task 領収書必要のケースで旅費交通費経費精算の追加に成功すること()
    {
        var traveringInformationRepository = new Mock<ITraveringInformationRepository>();
        var expenseRepository = new Mock<IExpenseRepository>();
        var userRepository = new Mock<IUserRepository>();
        var loginUserGetter = new Mock<ILoginUserGetter>();
        var storage = new Mock<IStorage>();
        var user = new User(new("山田 太郎"), new("yamada@google.com"), true, true);
        var stream = new MemoryStream();

        loginUserGetter.Setup(x => x.GetMailAsync()).ReturnsAsync("yamada@google.com");
        userRepository.Setup(x => x.GetAsync(user.Mail)).ReturnsAsync(user);

        await new TraveringExpenseAdder(traveringInformationRepository.Object, expenseRepository.Object, userRepository.Object, loginUserGetter.Object, storage.Object)
            .AddAsync(new DateOnly(2022, 12, 14), "現場業務", "初台", "東京", Ulid.NewUlid().ToString(), "電子", stream, 500, null);

        traveringInformationRepository.Verify(x => x.AddAsync(It.IsAny<TraveringInformation>()));
        expenseRepository.Verify(x => x.AddAsync(It.IsAny<Expense>()));
        storage.Verify(x => x.CreateFolderAsync("領収書"));
        storage.Verify(x => x.SaveAsync("領収書", It.IsAny<string>(), stream));
    }

    [Test]
    public async Task ユーザーの取得に失敗した場合に例外になること()
    {
        var userRepository = new Mock<IUserRepository>();
        var loginUserGetter = new Mock<ILoginUserGetter>();
        
        loginUserGetter.Setup(x => x.GetMailAsync()).ReturnsAsync("yamada@google.com");
        userRepository.Setup(x => x.GetAsync(It.IsAny<Mail>())).ReturnsAsync((User?)null);

        await FluentActions.Invoking(() => new TraveringExpenseAdder(
            new Mock<ITraveringInformationRepository>().Object,
            new Mock<IExpenseRepository>().Object,
            userRepository.Object,
            loginUserGetter.Object,
            new Mock<IStorage>().Object).AddAsync(new DateOnly(), "", null, null, "", null, null, 0, null))
            .Should().ThrowAsync<DomainException>().WithMessage("対象のユーザーが存在しません。");
    }
}