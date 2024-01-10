using Domain;
using Domain.Models.Categories;
using Domain.Repositories;
using Domain.Services;
using Moq;
using Usecase.Categories;

namespace UnitTest.Usecase.Categories;

public class CategoryUpdaterTest
{
    [Test]
    public async Task 交通区分の更新に成功すること()
    {
        var categoryRepository = new Mock<ICategoryRepository>();
        var categoryDuplicateChecker = new Mock<ICategoryDuplicateChecker>();
        var expenseType = new Category(new CategoryName("電車"), null, true);

        categoryRepository.Setup(x => x.GetAsync(It.IsAny<CategoryId>())).ReturnsAsync(expenseType);
        categoryDuplicateChecker.Setup(x => x.DuplicatedAsync(It.IsAny<Category>())).ReturnsAsync(false);

        await new CategoryUpdater(categoryRepository.Object, categoryDuplicateChecker.Object).UpdateAsync(Ulid.NewUlid().ToString(), "新幹線", "", false, true);

        categoryRepository.Verify(x => x.UpdateAsync(expenseType));
        categoryRepository.Verify(x => x.CommitAsync());
    }

    [Test]
    public async Task 交通区分IDから経費種別が取得できない場合は例外になること()
    {
        var categoryRepository = new Mock<ICategoryRepository>();

        categoryRepository.Setup(x => x.GetAsync(It.IsAny<CategoryId>())).ReturnsAsync((Category?)null);

        await FluentActions.Invoking(() => new CategoryUpdater(categoryRepository.Object, new Mock<ICategoryDuplicateChecker>().Object).UpdateAsync(Ulid.NewUlid().ToString(), "バス", null, true, true))
           .Should().ThrowAsync<DomainException>().WithMessage("対象の交通区分が存在しません。");
    }

    [Test]
    public async Task 重複する交通区分名に更新しようとすると例外になること()
    {
        var categoryRepository = new Mock<ICategoryRepository>();
        var categoryDuplicateChecker = new Mock<ICategoryDuplicateChecker>();
        var expenseType = new Category(new CategoryName("バス"), null, false);

        categoryRepository.Setup(x => x.GetAsync(It.IsAny<CategoryId>())).ReturnsAsync(expenseType);
        categoryDuplicateChecker.Setup(x => x.DuplicatedAsync(It.IsAny<Category>())).ReturnsAsync(true);

        await FluentActions.Invoking(() => new CategoryUpdater(categoryRepository.Object, categoryDuplicateChecker.Object).UpdateAsync(Ulid.NewUlid().ToString(), "バス", null, true, true))
           .Should().ThrowAsync<DomainException>().WithMessage("交通区分名[バス]はすでに登録されています。");
    }
}