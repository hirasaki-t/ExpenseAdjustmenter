using Domain.Models.Users;

namespace UnitTest.Domain.Models.Users;

public class UserTest
{
    [Test]
    public void 正しい情報で生成される()
    {
        var id = new UserId();
        var name = new UserName("ユーザー名");
        var mail = new Mail("メールアドレス");
        var user = new User(id, name, mail, false, true);

        user.Should().BeEquivalentTo(new
        {
            Id = id,
            Name = name,
            Mail = mail,
            IsAdmin = false,
            IsActive = true
        });
    }

    [Test]
    public void 管理者フラグが更新されること()
    {
        var user = new User(new("山田 太郎"), new("yamada@google.com"), false, true);
        user.IsAdmin.Should().BeFalse();
        user.UpdateIsAdmin(true);
        user.IsAdmin.Should().BeTrue();
        user.UpdateIsAdmin(false);
        user.IsAdmin.Should().BeFalse();
    }

    [Test]
    public void 有効フラグが更新されること()
    {
        var user = new User(new("山田 太郎"), new("yamada@google.com"), true, false);
        user.IsActive.Should().BeFalse();
        user.UpdateIsActive(true);
        user.IsActive.Should().BeTrue();
        user.UpdateIsActive(false);
        user.IsActive.Should().BeFalse();
    }
}