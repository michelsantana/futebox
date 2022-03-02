using Dapper.FluentMap;
using Dapper.FluentMap.Dommel;
using Dommel;
using FluentMigrator.Runner;
using Futebox.DB;
using Futebox.DB.Interfaces;
using Futebox.DB.Migrations;
using Futebox.Interfaces.DB;
using Futebox.Services;
using Futebox.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Futebox.DB.Mappers;
using System;
using System.IO;
using Quartz;
using Quartz.Core;
using Quartz.Impl;

namespace Futebox
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Settings.BackendRoot = Directory.GetCurrentDirectory();
            Settings.ApplicationsRoot = Configuration.GetValue<string>("ApplicationsRoot");
            Settings.ApplicationHttpBaseUrl = Configuration.GetValue<string>("ApplicationHttpBaseUrl");
            Settings.RobotEndpointBaseUrl = Configuration.GetValue<string>("RobotEndpointBaseUrl");
            Settings.TelegramBotToken = DotEnv.Get("TELEGRAM_BOT_TOKEN");
            Settings.TelegramNotifyUserId = DotEnv.Get("TELEGRAM_NOTIFY_USERID");
            Settings.DEBUGMODE = DotEnv.Get("DEFAULT_DEBUGMODE") == "true";
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddControllers();
            services.AddHttpClient();
            services.AddLogging(config => config.AddDebug().AddConsole());

            services.AddSingleton<IDatabaseConfig, DatabaseConfig>((_) => new DatabaseConfig(Configuration.GetValue<string>("DatabaseName")));
            services.AddSingleton<ISchedulerService, SchedulerService>();
            services.AddSingleton<IQueueService, QueueService>();
            services.AddSingleton<IBrowserService, BrowserService>();

            services.AddSingleton<IHttpHandler, HttpHandler>();
            services.AddSingleton<ICacheHandler, CacheHandler>();

            services.AddSingleton<IGerenciamentoTimesService, GerenciamentoTimesService>();
            services.AddSingleton<IFootstatsService, FootstatsService>();
            services.AddSingleton<IClassificacaoService, ClassificacaoService>();
            services.AddSingleton<IPartidasService, PartidasService>();
            services.AddSingleton<IFutebotService, FutebotService>();
            services.AddSingleton<IProcessoService, ProcessoService>();
            services.AddSingleton<IRodadaService, RodadaService>();
            services.AddSingleton<IAgendamentoService, AgendamentoService>();
            services.AddSingleton<INotifyService, NotifyService>();
            //services.AddSingleton<ISubProcessoRepositorio, SubProcessoRepositorio>();
            services.AddSingleton<IProcessoRepositorio, ProcessoRepositorio>();
            services.AddSingleton<ITimeRepositorio, TimeRepositorio>();

            services.AddSingleton<IInstagramService, InstagramService>();
            services.AddSingleton<IYoutubeService, YoutubeService>();

            services.AddSingleton(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
            services.AddQuartz(_ =>
            {
                _.UseMicrosoftDependencyInjectionScopedJobFactory();
            });

            ConfigureMigrations(services);
            ConfigureFluentMapper();
        }

        private void ConfigureMigrations(IServiceCollection services)
        {
            services.AddFluentMigratorCore()
               .ConfigureRunner(rb => rb
                   .AddSQLite()
                   .WithGlobalConnectionString($"{Configuration.GetValue<string>("DatabaseName")}")
                   .ScanIn(typeof(DbSchemaTime).Assembly).For.Migrations())
               .AddLogging(lb => lb.AddFluentMigratorConsole())
               .BuildServiceProvider(false)
               .GetRequiredService<IMigrationRunner>()
               .MigrateUp();
        }

        private void ConfigureFluentMapper()
        {
            RegisterMappings.Register();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });
        }
    }
}
