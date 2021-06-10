using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using NetCars.Areas.Home.Models.Db.Account;
using NetCars.Context;
using NetCars.Infrastructure.Settings;
using NetCars.Services;
using NetCars.Services.Interfaces;
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
using Microsoft.AspNetCore.Authentication.Facebook;

namespace NetCars
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
            var dbType = Configuration["dbType"];
            if (dbType == "Sqlite")
            {
                services.AddDbContext<NetCarsContext>(builder =>
                {
                    builder.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
                });
            }
            else
            {
                services.AddDbContext<NetCarsContext>(builder =>
                {
                    //builder.UseMySql(Configuration.GetConnectionString("DefaultConnection"));
                    builder.UseMySql(Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(8, 0, 21)));
                });
            }

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = FacebookDefaults.AuthenticationScheme;
                //options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = "855139225098776";
                facebookOptions.AppSecret = "43d6ecaf6e15548ef6017f85c143eb70";
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/account/facebook-login";
            });

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
            }).AddEntityFrameworkStores<NetCarsContext>().AddDefaultTokenProviders();

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
            services.AddScoped<ICarService, CarService>();
            services.AddScoped<ICloudinaryService, CloudinaryService>();

            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));

            //services.AddCors();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseCors(options => options.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader());

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

                //Tu jesli bedziemy dodawac jakies inne obszary:

                //endpoints.MapAreaControllerRoute(
                //           name: "dashboardArea",
                //           areaName: "dashboard",
                //           pattern: "{area=dashboard}/{controller=Home}/{action=index}/{id?}");


            });

            
        }
    }
}