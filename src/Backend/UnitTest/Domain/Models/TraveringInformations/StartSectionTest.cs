using Domain;
using Domain.Models.TraveringInformations;

namespace UnitTest.Domain.Models.TraveringInformations;

public class StartSectionTest
{
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("　")]
    public void 出発地を空にすることができない(string empty)
    {
        FluentActions.Invoking(() => new StartSection(empty))
            .Should().Throw<DomainException>().WithMessage("出発地を空にすることはできません。");
    }

    [Test]
    public void 出発地を生成できる()
    {
        var startSection = new StartSection("出発地");
        startSection.ToString().Should().Be("出発地");
        startSection.Should().Be(new StartSection("出発地"));
    }
}