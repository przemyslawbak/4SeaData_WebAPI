using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            services.AddTransient<INodeParser, NodeParser>();
            services.AddTransient<IGeoAreaFinder, GeoAreaFinder>();
            services.AddTransient<IMemoryAccess, MemoryAccess>();
            services.AddTransient<IExceptionProcessor, ExceptionProcessor>();
            services.AddTransient<DataRepository>();
            services.AddMemoryCache();
            services.AddMvc();
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
