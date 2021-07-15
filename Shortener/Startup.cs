using Domain.Repositories;
using Infrastructure.Configuration.EFCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace Shortener
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var orm = Configuration.GetValue<ORM>("ORM");
            switch (orm)
            {
                case ORM.EFCore:
                    ConfigureWithEFCore(services);
                    break;
                case ORM.Dapper:
                    ConfigureWithDapper(services);
                    break;
                default:
                    throw new NotImplementedException();
            }
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Shortener", Version = "v1" });
            });
        }

        private void ConfigureWithEFCore(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(Configuration["EFCore:ConnectionString"]);
            });

            services.AddTransient<ILinkRepository, Infrastructure.Repositories.EFCore.LinkRepository>();
        }

        private void ConfigureWithDapper(IServiceCollection services)
        {
            services.AddTransient<ILinkRepository, Infrastructure.Repositories.Dapper.LinkRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var serviceScopeFactory = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                try
                {
                    var dbContext = serviceScopeFactory.ServiceProvider.GetRequiredService<DataContext>();
                    dbContext.Database.EnsureCreated();
                }
                catch (InvalidOperationException e)
                {
                    // When using Dapper the DataContext isn't registered so an exception is thrown when trying
                    // to get the DataContext scoped service.
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
            }
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shortener v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public enum ORM
        {
            EFCore = 0,
            Dapper = 1
        }
    }
}
