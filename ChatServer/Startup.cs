using System;
using System.Net;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using ChatServer.Extensions;
using ChatServer.Hubs;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Core.Domain.Commands;
using WebApi.Infrastructure;
using WebApi.Infrastructure.Data;
using WebApi.Infrastructure.Data.Mapper;
using WebApi.Infrastructure.Helpers;
using WebApi.Infrastructure.Identity;

namespace ChatServer
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default"), b => b.MigrationsAssembly(typeof(AppIdentityDbContext).Assembly.FullName)));
            services.AddDbContext<GameDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default"), b => b.MigrationsAssembly(typeof(GameDbContext).Assembly.FullName)));

            services.ConfigureAuthentication(Configuration).ConfigureIdentity();

            services.ConfigureCors(Configuration);
            services.AddSignalR();
            services.AddAutoMapper(typeof(DataProfile));


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        
            services.RegisterSwagger();
            services.AddMediatR(typeof(CreatePlayerCommand).Assembly);
            var builder = new ContainerBuilder();
            builder.RegisterModule(new InfrastructureModule());
            builder.Populate(services);
            var container = builder.Build();
            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseExceptionHandler(
                builder =>
                {
                    builder.Run(
                        async context =>
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                            var error = context.Features.Get<IExceptionHandlerFeature>();
                            if (error != null)
                            {
                                context.Response.AddApplicationError(error.Error.Message);
                                await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                            }
                        });
                });
          
            app.UseExceptionHandler("/error");

            app.ConfigureSwagger();
            app.UseCors(Constants.Strings.CorsPolicy.Name);

            app.UseAuthentication();
            app.UseSignalR(options =>
            {
                options.MapHub<MessageHub>("/MessageHub");
            });
            InitializeDatabase(app);
            app.UseMvc();
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<GameDbContext>().Database.Migrate();
                scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>().Database.Migrate();
            }
        }
    }
}
