
using System.Reflection;
using BirdWatching.API.Middleware.Exceptions;
using BirdWatching.Common.Entities;
using BirdWatching.Common.Validators;
using BirdWatching.DataPersistence.Repositories;
using BirdWatching.Interfaces;
using BirdWatching.Services;

namespace BirdWatching.API.Helpers
{
    public static class StartupConfigHelper
    {
        public static void ConfigureAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();

            services.AddSingleton<IBirdWatchingRepositoryAsync, BirdWatchingRepositoryMemoryAsync>();
            services.AddSingleton<IEntityValidator<Bird>, BirdValidator>();
            services.AddScoped<IBirdWatchingServiceAsync, BirdWatchingServiceAsync>();

            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            ConfigureSwagger(services);
        }

        public static void ConfigureApp(this WebApplication app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.ConfigureExceptionHandler();
            app.UseHttpsRedirection();
            app.UseRouting();

            UseSwagger(app);

            app.MapControllers();
        }

        private static void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
                {
                    Version = "v1",
                    Title = "Bird Watching API",
                    Description = "A simple example ASP.NET Core Web API"
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            }
            );
        }
        private static void UseSwagger(WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bird Watching V1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
