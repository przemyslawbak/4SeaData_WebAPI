using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using WebAPI.DAL;
using WebAPI.Services;

namespace WebAPI
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
            AddServiceProxyHttpClients(services);

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration["Data:4Sea_Server:ConnectionString"]));
            services.AddSingleton<IUpdatingProgress, UpdatingProgress>();
            services.AddTransient<IHostedService, TimeHostedUpdater>();
            services.AddTransient<IUpdaterService, UpdaterService>();
            services.AddTransient<IUpdateInitializer, UpdateInitializer>();
            services.AddTransient<IDataAccessService, DataAccessService>();
            services.AddTransient<IDataProcessor, DataProcessor>();
            services.AddTransient<IHttpClientProvider, HttpClientProvider>();
            services.AddTransient<IStringParser, StringParser>();
            services.AddTransient<IUpdatedVesselFactory, UpdatedVesselFactory>();
            services.AddTransient<IScrapper, Scrapper>();
            services.AddTransient<INodeProcessor, NodeProcessor>();
            services.AddTransient<IGeoAreaFinder, GeoAreaFinder>();
            services.AddTransient<IMemoryAccess, MemoryAccess>();
            services.AddTransient<IExceptionProcessor, ExceptionProcessor>();
            services.AddTransient<INodeCreator, NodeCreator>();
            services.AddTransient<ISqlQueryBuilder, SqlQueryBuilder>();
            services.AddTransient<IProxyProvider, ProxyProvider>();
            services.AddTransient<IADORepository, ADORepository>();
            services.AddTransient<EFRepository>();
            services.AddTransient<EFStatRepository>();
            services.AddMemoryCache();
            services.AddMvc();
        }

        private void AddServiceProxyHttpClients(IServiceCollection services)
        {
            ProxyProvider proxyProvider = new ProxyProvider();
            List<string> proxies = proxyProvider.GetProxies();
            foreach (string proxy in proxies)
            {
                string user = Configuration["Proxy:User"];
                string pass = Configuration["Proxy:Pass"];
                string uri = $"http://{proxy}";

                services.AddHttpClient(proxy).ConfigurePrimaryHttpMessageHandler(() =>
                {
                    return new HttpClientHandler
                    {
                        Proxy = new WebProxy
                        {
                            Address = new Uri(uri),
                            BypassProxyOnLocal = false,
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential(userName: user, password: pass)
                        },
                        UseProxy = true
                    };
                });
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }
            else
            {
                //TODO: routing
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
