using Domain;
using Domain.Models.TraveringInformations;

namespace UnitTest.Domain.Models.TraveringInformations;

public class EndSectionTest
{
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("　")]
    public void 到着地を空にすることができない(string empty)
    {
        FluentActions.Invoking(() => new EndSection(empty))
            .Should().Throw<DomainException>().WithMessage("到着地を空にすることはできません。");
    }

    [Test]
    public void 到着地を生成できる()
    {
        var endSection = new EndSection("到着地");
        endSection.ToString().Should().Be("到着地");
        endSection.Should().Be(new EndSection("到着地"));
    }
}