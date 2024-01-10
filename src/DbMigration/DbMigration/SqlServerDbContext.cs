using DbMigration.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DbMigration;

public class SqlServerDbContext : DbContext
{
    /// <summary>経費精算</summary>
    public DbSet<Expense> Expenses => Set<Expense>();

    /// <summary>交通費情報</summary>
    public DbSet<TraveringInformation> TraveringInformations => Set<TraveringInformation>();

    /// <summary>諸経費情報</summary>
    public DbSet<SundryInformation> SundryInformations => Set<SundryInformation>();

    /// <summary>経費種別</summary>
    public DbSet<ExpenseType> ExpenseTypes => Set<ExpenseType>();

    /// <summary>区分</summary>
    public DbSet<Category> Categories => Set<Category>();

    /// <summary>承認履歴</summary>
    public DbSet<ApproveHistory> ApproveHistories => Set<ApproveHistory>();

    /// <summary>ユーザー</summary>
    public DbSet<User> Users => Set<User>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var ulidConverter = new UlidConverter();
        var dateOnlyConverter = new DateOnlyConverter();

        modelBuilder.Entity<Expense>(x =>
        {
            x.HasKey(x => x.Id);
            x.Property(x => x.Id).HasConversion(ulidConverter);
            x.Property(x => x.UserId).HasConversion(ulidConverter);
            x.Property(x => x.Date).HasConversion(dateOnlyConverter);
            x.Property(x => x.ReceiptId).HasConversion(ulidConverter);
            x.HasMany<ApproveHistory>().WithOne().HasForeignKey(x => x.ExpenseId);
        });

        modelBuilder.Entity<TraveringInformation>(x =>
        {
            x.HasKey(x => x.ExpenseId);
            x.Property(x => x.ExpenseId).HasConversion(ulidConverter);
            x.Property(x => x.CategoryId).HasConversion(ulidConverter);
            x.HasOne<Expense>().WithOne().HasForeignKey<TraveringInformation>(x => x.ExpenseId);
        });

        modelBuilder.Entity<SundryInformation>(x =>
        {
            x.HasKey(x => x.ExpenseId);
            x.Property(x => x.ExpenseId).HasConversion(ulidConverter);
            x.Property(x => x.ExpenseTypeId).HasConversion(ulidConverter);
            x.HasOne<Expense>().WithOne().HasForeignKey<SundryInformation>(x => x.ExpenseId);
        });

        modelBuilder.Entity<ExpenseType>(x =>
        {
            x.HasKey(x => x.Id);
            x.Property(x => x.Id).HasConversion(ulidConverter);
            x.HasIndex(x => x.Name).IsUnique();
            x.HasMany<SundryInformation>().WithOne().HasForeignKey(x => x.ExpenseTypeId);
            x.HasData(
                new ExpenseType { Id = Ulid.NewUlid(), Name = "会議費", Details = "1人当たりの金額が「5,000円（税込）」以下", IsReceipt = true, IsActive = true },
                new ExpenseType { Id = Ulid.NewUlid(), Name = "接待費", Details = "1人当たりの金額が「5,001円（税込）」以上", IsReceipt = true, IsActive = true },
                new ExpenseType { Id = Ulid.NewUlid(), Name = "通信費", Details = null, IsReceipt = true, IsActive = true },
                new ExpenseType { Id = Ulid.NewUlid(), Name = "事務用品購入費", Details = null, IsReceipt = true, IsActive = true },
                new ExpenseType { Id = Ulid.NewUlid(), Name = "書籍購入費", Details = null, IsReceipt = true, IsActive = true },
                new ExpenseType { Id = Ulid.NewUlid(), Name = "ハードウェア購入費", Details = null, IsReceipt = true, IsActive = true },
                new ExpenseType { Id = Ulid.NewUlid(), Name = "ソフトウェア購入費", Details = null, IsReceipt = true, IsActive = true },
                new ExpenseType { Id = Ulid.NewUlid(), Name = "福利厚生", Details = null, IsReceipt = true, IsActive = true },
                new ExpenseType { Id = Ulid.NewUlid(), Name = "雑貨", Details = "消耗品など", IsReceipt = false, IsActive = true }
            );
        });

        modelBuilder.Entity<Category>(x =>
        {
            x.HasKey(x => x.Id);
            x.Property(x => x.Id).HasConversion(ulidConverter);
            x.HasIndex(x => x.Name).IsUnique();
            x.HasMany<TraveringInformation>().WithOne().HasForeignKey(x => x.CategoryId);
            x.HasData(
                new Category { Id = Ulid.NewUlid(), Name = "電車", Details = null, IsReceipt = false, IsActive = true },
                new Category { Id = Ulid.NewUlid(), Name = "バス", Details = null, IsReceipt = false, IsActive = true },
                new Category { Id = Ulid.NewUlid(), Name = "タクシー", Details = null, IsReceipt = true, IsActive = true },
                new Category { Id = Ulid.NewUlid(), Name = "その他", Details = null, IsReceipt = true, IsActive = true },
                new Category { Id = Ulid.NewUlid(), Name = "高速代", Details = null, IsReceipt = false, IsActive = true },
                new Category { Id = Ulid.NewUlid(), Name = "宿泊費", Details = null, IsReceipt = true, IsActive = true }
            );
        });

        modelBuilder.Entity<ApproveHistory>(x =>
        {
            x.HasKey(x => x.Id);
            x.Property(x => x.Id).HasConversion(ulidConverter);
            x.Property(x => x.UserId).HasConversion(ulidConverter);
        });

        modelBuilder.Entity<User>(x =>
        {
            x.HasKey(x => x.Id);
            x.Property(x => x.Id).HasConversion(ulidConverter);
            x.HasIndex(x => x.Mail).IsUnique();
            x.HasMany<Expense>().WithOne().HasForeignKey(x => x.UserId);
        });
    }

    /// <summary>UlidのDB保存用コンバーター</summary>
    private class UlidConverter : ValueConverter<Ulid, string>
    {
        private static readonly ConverterMappingHints defaultHints = new(size: 26);

        public UlidConverter(ConverterMappingHints? mappingHints = null)
            : base(
                convertToProviderExpression: x => x.ToString(),
                convertFromProviderExpression: x => Ulid.Parse(x),
                mappingHints: defaultHints.With(mappingHints))
        { }
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