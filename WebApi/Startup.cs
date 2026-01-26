using System;
using System.IO;
using System.Reflection;
using Contract.Service;
using Contract.Service.Core;
using Contracts.Repository;
using Contracts.Repository.Base;
using Contracts.Service.Core;
using Contracts.Service.Pdf;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Model.Static;
using Repository;
using Repository.Base;
using Service;
using Service.Consumer;
using Service.Core;
using Service.Google;
using Service.Hub;
using Service.Pdf;
using WebApi.BackgroupJob;
using WebApi.HostedService;
using WebApi.Middleware;

namespace portal
{
    public class Startup
    {
        private readonly IHostApplicationLifetime _appLifetime;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            AppSettings.Ini(Configuration);
            Common.ReadNumberWebSerivce.SetDocSoWebserviceEndpoint(AppSettings.FixedValue.DocSoWebserviceEndpoint);
            services.AddHostedService<CacheManagerHostedService>();
            services.AddHostedService<RenewCacheBackgroundService>();
            if (!AppSettings.FixedValue.DisableConsummer)
            {
                services.AddHostedService<HoaDonPhatHanhConsumer>();
                services.AddHostedService<RemoteSigningConsummer>();
            }
            services.AddHostedService<BackgroundTaskService>();


            services.AddSingleton<ITaskQueueService, TaskQueueService>();
            services.AddSingleton<IConnectionStrings, ConnectionStrings>();
            // services.AddSingleton<IJwtTokenService, JwtTokenService>();
            //services.AddTransient<ICacheStoreVersionRepository, CacheStoreVersionRepository>();
            services.AddSingleton<IRepositoryWrapper, RepositoryWrapper>();

            services.AddSingleton<IExceptionService, Service.Core.ExceptionService>();
            services.AddSingleton<IJwtTokenService, JwtTokenService>();
            services.AddSingleton<IPdfService, PdfService>();


            services.AddSingleton<IServiceWrapper, ServiceWrapper>();

            AddSignalR(services);
            services.AddSingleton<ReCaptchaService, ReCaptchaService>();
            if (AppSettings.FixedValue.ShowSwaggerUI)
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ca EInvoice API", Version = "v1" });
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);

                    // Cấu hình để đọc các thuộc tính [Display] hoặc [Description]
                    c.UseAllOfToExtendReferenceSchemas();
                    c.EnableAnnotations();
                });
            }

        }
        private void AddSignalR(IServiceCollection services)
        {

            services.AddSignalR();

            var CorsWithOrigins = Configuration["CorsWithOrigins"].ToString().Split(",");
            services.AddCors(options =>
            {

                options.AddPolicy("ClientPermission", policy =>
                {
                    policy.AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithOrigins(CorsWithOrigins)
                        .AllowCredentials();
                });
            });
            services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
            services.AddSingleton<HoaDonPhatHanhHub>();
            services.AddSingleton<ProcessHub>();

            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IExceptionService exceptionService, IJwtTokenService tokenService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.ConfigureExceptionHandler(exceptionService, tokenService);
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Upload")),
                RequestPath = "/Upload"
            });
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Template")),
                RequestPath = "/Template"
            });

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Xml")),
                RequestPath = "/Xml"
            });
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "Pdf")),
                RequestPath = "/Pdf"
            });
            if (AppSettings.FixedValue.ShowSwaggerUI)
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = "swagger"; // Set the Swagger UI at the root URL
                });
            }
            app.UseRouting();
            app.UseCors("ClientPermission");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapHub<HoaDonPhatHanhHub>("hubs/hoa-don");
                endpoints.MapHub<ProcessHub>("hubs/process");
                endpoints.MapControllers();
                // endpoints.MapFallbackToFile("");
            });
            var check = env.IsDevelopment();
            // if (!env.IsDevelopment() && !AppSettings.FixedValue.ShowSwaggerUI)
            //     if (true)
            //     {
            //         app.UseSpa(spa =>
            //         {
            //             spa.Options.SourcePath = "ClientApp";

            //             if (env.IsDevelopment())
            //             {
            //                 spa.UseReactDevelopmentServer(npmScript: "start");
            //             }
            //         });
            //     }


        }
    }
}

