using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Api.Configurations
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(o =>
            {
                o.CustomSchemaIds(t => t.FullName);
                o.SwaggerDoc("v1",
                    new OpenApiInfo
                        {Title = "TMP API", Version = "v1"});
            });

            return services;
        }

        public static IApplicationBuilder UseSwashbuckleSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(o =>
            {
                o.SwaggerEndpoint("/swagger/v1/swagger.json", "TMP API V1");
                o.RoutePrefix = string.Empty;
            });

            return app;
        }
    }
}