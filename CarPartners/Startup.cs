using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CarPartners.Areas.Home.Models.Db.Account;
using CarPartners.Context;
using CarPartners.Infrastructure.Settings;
using CarPartners.Services;
using CarPartners.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CarPartners
{
    public class Startup
    {
        //Wstrzykniecie pliku konfiguracyjnego
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment env)
        {
            var config = new ConfigurationBuilder();
            config.AddJsonFile("secrets.json");
            Configuration = config.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<NetCarsContext>(builder =>
            {
                //builder.UseMySql(Configuration.GetConnectionString("DefaultConnection"));
                //builder.UseMySql(Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(8, 0, 21)));
                builder.UseSqlite("CarPartners=NetCars.db");
            });

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 2;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            }).AddEntityFrameworkStores<NetCarsContext>();

            services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, options =>
            {
                // scie�ka do logowania
                options.LoginPath = "/home/account/login";
            });

            //services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, options =>
            //{
            //    options.LoginPath = "/dashboard/account/login";
            //});

            services.AddRazorPages();
            services.AddControllersWithViews();

            services.AddOptions();
            services.AddAuthorization();

            // Tu wstrzykiwanie zaleznosci
            services.AddScoped<IHomeService, HomeService>();

            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var cultureInfo = new CultureInfo("pl-PL");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            if (env.IsProduction())
            {
                app.UseExceptionHandler("/blad");
            }

            app.UseStatusCodePagesWithReExecute("/error/{0}");

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                           name: "default",
                           areaName: "home",
                           pattern: "{controller=Home}/{action=index}/{id?}");

                //Tu je�li b�dziemy dodawa� jakie� inne obszary:

                //endpoints.MapAreaControllerRoute(
                //           name: "dashboardArea",
                //           areaName: "dashboard",
                //           pattern: "{area=dashboard}/{controller=Home}/{action=index}/{id?}");
            });
        }
    }
}