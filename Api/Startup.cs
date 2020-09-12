using System;
using Api.Configurations;
using Api.Database;
using Api.Interfaces;
using Api.Services;
using Api.Subscribers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddControllers();
            services.AddSwagger();
            
            services.AddSingleton<IConnectionMultiplexer>(x =>
                ConnectionMultiplexer.Connect(Configuration.GetConnectionString("Redis"))
            );

            services.AddDbContext<Context>(o => o.UseSqlServer(Configuration.GetConnectionString("SqlServer")));

            services.AddHostedService<RedisSubscriberOne>();
            services.AddHostedService<RedisSubscriberTwo>();
            
            services.AddTransient<ISomeService, SomeImplementation>();
            
            services.AddTransient<ICache, RedisCache>();
            // services.AddTransient<ICache, MemoryCache>();
            // services.AddTransient<ICache, DatabaseCache>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwashbuckleSwagger();
            app.UseRouting();
            app.UseAuthorization();

            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetService<Context>();
            Console.WriteLine("Starting migration...");
            context.Database.Migrate();
            Console.WriteLine("Completed migration.");

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}