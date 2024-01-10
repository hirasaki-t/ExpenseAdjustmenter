using Infrastructure.Repositories.EFCore.Contexts;
using System.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Web;
using Query.Handlers;
using Usecase.Users;
using WebAPI.Converters;
using WebAPI.Filters;
using WebAPI.NSwagExtensions;
using Infrastructure.Repositories.EFCore;
using Domain.Repositories;
using Domain.Services;
using Infrastructure.MicrosoftGraphs;
using Query.QueryServices;
using Usecase.ExpenseTypes;
using Usecase.Categories;
using Domain.Storages;
using Infrastructure.Storages;
using Usecase.TraveringExpenses;
using Usecase.SundryExpenses;
using Usecase.ApproveHistories;
using Usecase.Receipts;

namespace WebAPI;

public class Startup
{
    /// <summary>環境情報</summary>
    private readonly IWebHostEnvironment environment;

    /// <summary>設定情報</summary>
    private readonly IConfiguration configuration;

    public Startup(IWebHostEnvironment environment, IConfiguration configuration)
    {
        this.environment = environment;
        this.configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddResponseCompression();
        serviceCollection.AddSingleton(_ => new DatabaseConnectionString(configuration.GetConnectionString("ExpenseAdjustmenter")));
        serviceCollection.AddDbContext<SQLServerContext>();
        serviceCollection.AddScoped<ContextBase>(x => x.GetRequiredService<SQLServerContext>());
        serviceCollection.AddScoped<IDbConnection>(x => new SqlConnection(x.GetRequiredService<DatabaseConnectionString>().Value));

        serviceCollection.AddScoped<IUserNameSearcher, UserNameSearcher>();
        serviceCollection.AddScoped<ILoginUserGetter, LoginUserGetter>();

        serviceCollection.AddScoped<IUserRepository, EFCoreUserRepository>();
        serviceCollection.AddScoped<IUserDuplicateChecker, UserDuplicateChecker>();
        serviceCollection.AddScoped<UserQueryService>();
        serviceCollection.AddScoped<UserAdminChecker>();
        serviceCollection.AddScoped<UserAdder>();
        serviceCollection.AddScoped<UserUpdater>();

        serviceCollection.AddScoped<IExpenseTypeRepository, EFCoreExpenseTypeRepository>();
        serviceCollection.AddScoped<IExpenseTypeDuplicateChecker, ExpenseTypeDuplicateChecker>();
        serviceCollection.AddScoped<ExpenseTypeQueryService>();
        serviceCollection.AddScoped<ExpenseTypeAdder>();
        serviceCollection.AddScoped<ExpenseTypeUpdater>();

        serviceCollection.AddScoped<ICategoryRepository, EFCoreCategoryRepository>();
        serviceCollection.AddScoped<ICategoryDuplicateChecker, CategoryDuplicateChecker>();
        serviceCollection.AddScoped<CategoryQueryService>();
        serviceCollection.AddScoped<CategoryAdder>();
        serviceCollection.AddScoped<CategoryUpdater>();

        serviceCollection.AddScoped<IExpenseRepository, EFCoreExpenseRepository>();

        serviceCollection.AddScoped<ITraveringInformationRepository, EFCoreTraveringInformationRepository>();
        serviceCollection.AddScoped<TraveringExpenseQueryService>();
        serviceCollection.AddScoped<TraveringExpenseAdder>();
        serviceCollection.AddScoped<TraveringExpenseUpdater>();
        serviceCollection.AddScoped<TraveringExpenseDeleter>();

        serviceCollection.AddScoped<ISundryInformationRepository, EFCoreSundryInformationRepository>();
        serviceCollection.AddScoped<SundryExpenseQueryService>();
        serviceCollection.AddScoped<SundryExpenseAdder>();
        serviceCollection.AddScoped<SundryExpenseUpdater>();
        serviceCollection.AddScoped<SundryExpenseDeleter>();

        serviceCollection.AddScoped<IApproveHistoryRepository, EFCoreApproveHistoryRepository>();
        serviceCollection.AddScoped<ApproveHistoryQueryService>();
        serviceCollection.AddScoped<ApproveHistoryApplicationer>();
        serviceCollection.AddScoped<ApproveHistoryApprover>();
        serviceCollection.AddScoped<ApproveHistoryRejecter>();

        serviceCollection.AddScoped<ReceiptDownloader>();
        serviceCollection.AddScoped<ReceiptZipDownloader>();

        serviceCollection.AddScoped(_ => new DeadlineQueryService(Environment.GetEnvironmentVariable("Deadline")));

#if DEBUG
        serviceCollection.AddTransient<IStorage>(_ => new AzureStorage(configuration.GetValue<string>("AzureStorage:ConnectionString"), configuration.GetValue<string>("AzureStorage:StorageName")));
#else
        serviceCollection.AddTransient<IStorage>(_ => new AzureStorage(configuration.GetValue<string>("AzureStorage:ConnectionString"), configuration.GetValue<string>("AzureStorage:StorageName")));
#endif

        serviceCollection
            .AddControllers(options =>
            {
                options.Filters.Add<DomainExceptionFilter>();
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
            });
        serviceCollection.AddEndpointsApiExplorer();

        if (environment.IsDevelopment())
            serviceCollection.AddSwaggerDocumentWithGraphAuthentication(
                "経費精算システム API",
                configuration.GetValue<string>("MicrosoftGraph:Scopes"),
                configuration.GetValue<string>("AzureAd:AuthorizationUrl"),
                configuration.GetValue<string>("AzureAd:TokenUrl")
            );

        serviceCollection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(configuration.GetSection("AzureAd")).EnableTokenAcquisitionToCallDownstreamApi()
            .AddMicrosoftGraph(configuration.GetSection("MicrosoftGraph"))
            .AddInMemoryTokenCaches();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        Dapper.SqlMapper.AddTypeHandler(new DateOnlyHandler());
        app.UseResponseCompression();
        app.UseRouting();

        if (env.IsDevelopment())
        {
            app.UseSwagger(configuration.GetValue<string>("AzureAd:ClientId"), configuration.GetValue<string>("MicrosoftGraph:Scopes"));
        }

        app.UseHttpsRedirection();
        app.UseCors();
        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapFallbackToFile("index.html");
        });
    }
}