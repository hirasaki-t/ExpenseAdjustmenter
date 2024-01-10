using DbMigration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

await CreateHostBuilder(args).Build().RunAsync();

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((context, services) =>
        {
            services.AddDbContext<SqlServerDbContext>();
        });