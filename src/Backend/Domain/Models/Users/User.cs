namespace Domain.Models.Users;

public class User
{
    public User(UserName name, Mail mail, bool isAdmin, bool isActive) : this(new UserId(), name, mail, isAdmin, isActive) { }

    public User(UserId id, UserName name, Mail mail, bool isAdmin, bool isActive)
    {
        Id = id;
        Name = name;
        Mail = mail;
        IsAdmin = isAdmin;
        IsActive = isActive;
    }

    public UserId Id { get; }

    public UserName Name { get; }

    public Mail Mail { get; }

    public bool IsAdmin { get; private set; }

    public bool IsActive { get; private set; }

    public void UpdateIsAdmin(bool isAdmin) => IsAdmin = isAdmin;

    public void UpdateIsActive(bool isActive) => IsActive = isActive;
}