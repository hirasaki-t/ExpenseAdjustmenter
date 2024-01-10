using WebAPI;

CreateHostBuilder().Build().Run();

static IHostBuilder CreateHostBuilder()
        => Host.CreateDefaultBuilder()
#if DEBUG
            .UseEnvironment("Development")
#elif STAGING
            .UseEnvironment("Staging")
#elif RELEASE
            .UseEnvironment("Production")
#endif
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());