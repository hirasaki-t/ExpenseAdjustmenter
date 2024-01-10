using Domain.Models.ApproveHistories;
using Domain.Models.Categories;
using Domain.Models.Expenses;
using Domain.Models.ExpenseTypes;
using Domain.Models.SundryInformations;
using Domain.Models.TraveringInformations;
using Domain.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Repositories.EFCore.Contexts;

public abstract class ContextBase : DbContext
{
    /// <summary>経費精算</summary>
    public DbSet<Expense> Expenses => Set<Expense>();

    /// <summary>旅費交通費情報</summary>
    public DbSet<TraveringInformation> TraveringInformations => Set<TraveringInformation>();

    /// <summary>諸経費情報</summary>
    public DbSet<SundryInformation> SundryInformations => Set<SundryInformation>();

    /// <summary>承認履歴</summary>
    public DbSet<ApproveHistory> ApproveHistories => Set<ApproveHistory>();

    /// <summary>交通区分</summary>
    public DbSet<Category> Categories => Set<Category>();

    /// <summary>経費種別</summary>
    public DbSet<ExpenseType> ExpenseTypes => Set<ExpenseType>();

    /// <summary>ユーザー</summary>
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var dateOnlyConverter = new DateOnlyConverter();

        modelBuilder.Entity<Expense>(x =>
        {
            x.HasKey(x => x.Id);
            x.Property(x => x.Id).HasConversion(x => x.Value.ToString(), x => new(x));
            x.Property(x => x.UserId).HasConversion(x => x.Value.ToString(), x => new(x));
            x.Property(x => x.Date).HasConversion(dateOnlyConverter);
            x.Property(x => x.Amount).HasConversion(x => x.Value, x => new(x));
            x.Property(x => x.SubmissionMethod).HasConversion(x => x == null ? null : x.Value, x => x == null ? null : new(x));
            x.Property(x => x.ReceiptId).HasConversion(x => x == null ? null : x.Value.ToString(), x => x == null ? null : new(x));
            x.HasOne<TraveringInformation>().WithOne().HasForeignKey<TraveringInformation>(x => x.ExpenseId);
            x.HasOne<SundryInformation>().WithOne().HasForeignKey<SundryInformation>(x => x.ExpenseId);
        });

        modelBuilder.Entity<TraveringInformation>(x =>
        {
            x.HasKey(x => x.ExpenseId);
            x.Property(x => x.ExpenseId).HasConversion(x => x.Value.ToString(), x => new(x));
            x.Property(x => x.CategoryId).HasConversion(x => x.Value.ToString(), x => new(x));
            x.Property(x => x.StartSection).HasConversion(x => x == null ? null : x.Value, x => x == null ? null : new(x));
            x.Property(x => x.EndSection).HasConversion(x => x == null ? null : x.Value, x => x == null ? null : new(x));
            x.Property(x => x.WorkName).HasConversion(x => x.Value, x => new(x));
        });

        modelBuilder.Entity<SundryInformation>(x =>
        {
            x.HasKey(x => x.ExpenseId);
            x.Property(x => x.ExpenseId).HasConversion(x => x.Value.ToString(), x => new(x));
            x.Property(x => x.ExpenseTypeId).HasConversion(x => x.Value.ToString(), x => new(x));
            x.Property(x => x.ParticipationNumber).HasConversion(x => x.Value, x => new(x));
        });

        modelBuilder.Entity<ApproveHistory>(x =>
        {
            x.HasKey(x => x.Id);
            x.Property(x => x.Id).HasConversion(x => x.Value.ToString(), x => new(x));
            x.Property(x => x.ExpenseId).HasConversion(x => x.Value.ToString(), x => new(x));
            x.Property(x => x.UserId).HasConversion(x => x.Value.ToString(), x => new(x));
            x.Property(x => x.Date);
            x.Property(x => x.Comment);
            x.Property(x => x.Status).HasConversion(x => x.Value, x => new(x));
        });

        modelBuilder.Entity<Category>(x =>
        {
            x.HasKey(x => x.Id);
            x.Property(x => x.Id).HasConversion(x => x.Value.ToString(), x => new(x));
            x.Property(x => x.Name).HasConversion(x => x.Value, x => new(x));
        });

        modelBuilder.Entity<ExpenseType>(x =>
        {
            x.HasKey(x => x.Id);
            x.Property(x => x.Id).HasConversion(x => x.Value.ToString(), x => new(x));
            x.Property(x => x.Name).HasConversion(x => x.Value, x => new(x));
        });

        modelBuilder.Entity<User>(x =>
        {
            x.HasKey(x => x.Id);
            x.Property(x => x.Id).HasConversion(x => x.Value.ToString(), x => new(x));
            x.Property(x => x.Name).HasConversion(x => x.Value, x => new(x));
            x.Property(x => x.Mail).HasConversion(x => x.Value, x => new(x));
        });
    }

    /// <summary>DateOnly型のDB保存用のコンバーター</summary>
    private class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
    {
        public DateOnlyConverter()
        : base(
            convertToProviderExpression: x => x.ToDateTime(TimeOnly.MinValue),
            convertFromProviderExpression: x => DateOnly.FromDateTime(x))
        { }
    }
}