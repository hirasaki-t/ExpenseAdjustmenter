using Domain;
using Domain.Models.Expenses;
using Domain.Models.SundryInformations;
using Domain.Models.Users;
using Domain.Repositories;
using Domain.Services;
using Domain.Storages;
using Moq;
using Usecase.SundryExpenses;

namespace UnitTest.Usecase.SundryExpenses;

public class SundryExpenseAdderTest
{
    [Test]
    public async Task 領収書不要のケースで諸経費精算の追加に成功すること()
    {
        var sundryInformationRepository = new Mock<ISundryInformationRepository>();
        var expenseRepository = new Mock<IExpenseRepository>();
        var userRepository = new Mock<IUserRepository>();
        var loginUserGetter = new Mock<ILoginUserGetter>();
        var storage = new Mock<IStorage>();
        var user = new User(new("山田 太郎"), new("yamada@google.com"), true, true);

        loginUserGetter.Setup(x => x.GetMailAsync()).ReturnsAsync("yamada@google.com");
        userRepository.Setup(x => x.GetAsync(user.Mail)).ReturnsAsync(user);

        await new SundryExpenseAdder(sundryInformationRepository.Object, expenseRepository.Object, userRepository.Object, loginUserGetter.Object, storage.Object)
            .AddAsync(new DateOnly(2022, 12, 14), Ulid.NewUlid().ToString(), null, 4, 500, "紙/持参", null);

        sundryInformationRepository.Verify(x => x.AddAsync(It.IsAny<SundryInformation>()));
        expenseRepository.Verify(x => x.AddAsync(It.IsAny<Expense>()));
        storage.Invoking(x => x.Verify(x => x.CreateFolderAsync(It.IsAny<string>()))).Should().Throw<MockException>();
        storage.Invoking(x => x.Verify(x => x.SaveAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>()))).Should().Throw<MockException>();
    }

    [Test]
    public async Task 領収書必要のケースで諸経費精算の追加に成功すること()
    {
        var sundryInformationRepository = new Mock<ISundryInformationRepository>();
        var expenseRepository = new Mock<IExpenseRepository>();
        var userRepository = new Mock<IUserRepository>();
        var loginUserGetter = new Mock<ILoginUserGetter>();
        var storage = new Mock<IStorage>();
        var user = new User(new("山田 太郎"), new("yamada@google.com"), true, true);
        var stream = new MemoryStream();

        loginUserGetter.Setup(x => x.GetMailAsync()).ReturnsAsync("yamada@google.com");
        userRepository.Setup(x => x.GetAsync(user.Mail)).ReturnsAsync(user);

        await new SundryExpenseAdder(sundryInformationRepository.Object, expenseRepository.Object, userRepository.Object, loginUserGetter.Object, storage.Object)
            .AddAsync(new DateOnly(2022, 12, 14), Ulid.NewUlid().ToString(), null, 4, 500, "電子", stream);

        sundryInformationRepository.Verify(x => x.AddAsync(It.IsAny<SundryInformation>()));
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

        await FluentActions.Invoking(() => new SundryExpenseAdder(
            new Mock<ISundryInformationRepository>().Object,
            new Mock<IExpenseRepository>().Object,
            userRepository.Object,
            loginUserGetter.Object,
            new Mock<IStorage>().Object).AddAsync(new DateOnly(), "", null, 1, 5000, null, null))
            .Should().ThrowAsync<DomainException>().WithMessage("対象のユーザーが存在しません。");
    }
}