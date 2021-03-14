using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SportsStore.Models;

namespace SportsStore
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
            services.AddDbContext<ApplicationDbContext>(options => 
                options.UseSqlServer(Configuration["Data:ConnectionString"]));
            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(Configuration["IdentityData:ConnectionString"]));
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IProductRepository, EFProductRepository>();
            services.AddTransient<IOrderRepository, EFOrderRepository>();

            services.AddScoped(sp => SessionCart.GetCart(sp));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc(opts => opts.EnableEndpointRouting = false);

            services.AddMemoryCache();
            services.AddSession();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();
            app.UseMvc(routes => {
                routes.MapRoute
                (
                    name: null,
                    template: "{category}/Page{productPage:int}",
                    defaults: new { controller = "Product", action = "List" }
                );
                routes.MapRoute
                (
                    name: null,
                    template: "Page{productPage:int}",
                    defaults: new { controller = "Product", action = "List", productPage = 1 }
                );
                routes.MapRoute
                (
                    name: null,
                    template: "{category}",
                    defaults: new {controller = "Product", action = "List", productPage = 1 }
                );
                routes.MapRoute
                (
                    name: null,
                    template: "",
                    defaults: new { controller = "Product", action = "List", productPage = 1 }
                );

                routes.MapRoute(name: null, template: "{controller}/{action}/{id?}");
            });
            SeedData.EnsurePopulated(app);
            IdentitySeedData.EnsurePopulated(app);
        }
    }
}
