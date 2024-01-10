using Domain;
using Domain.Models.Categories;
using Domain.Repositories;
using Domain.Services;
using Moq;
using Usecase.Categories;

namespace UnitTest.Usecase.Categories;

public class CategoryAdderTest
{
    [Test]
    public async Task 交通区分の追加に成功すること()
    {
        var categoryRepository = new Mock<ICategoryRepository>();
        var categoryDuplicateChecker = new Mock<ICategoryDuplicateChecker>();

        categoryDuplicateChecker.Setup(x => x.DuplicatedAsync(It.IsAny<Category>())).ReturnsAsync(false);

        await new CategoryAdder(categoryRepository.Object, categoryDuplicateChecker.Object).AddAsync("交通区分名", null, false);

        categoryRepository.Verify(x => x.AddAsync(It.IsAny<Category>()));
        categoryRepository.Verify(x => x.CommitAsync());
    }

    [Test]
    public async Task 重複する交通区分名を追加しようとすると例外になること()
    {
        var categoryDuplicateChecker = new Mock<ICategoryDuplicateChecker>();

        categoryDuplicateChecker.Setup(x => x.DuplicatedAsync(It.IsAny<Category>())).ReturnsAsync(true);

        await FluentActions.Invoking(() => new CategoryAdder(new Mock<ICategoryRepository>().Object, categoryDuplicateChecker.Object).AddAsync("電車", null, false))
            .Should().ThrowAsync<DomainException>().WithMessage("交通区分名[電車]はすでに登録されています。");
    }
}