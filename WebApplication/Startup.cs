using GalleryWebApplication.Helpers;
using GalleryWebApplication.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace GalleryWebApplication
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // Zamiana w URL wszystkich liter na ma³e.
            services.AddRouting(opt => opt.LowercaseUrls = true);

            // Dependency injection - Inverse of Control. Mapowanie interfejsu IGalleryService na obiekt GalleryService.
            services.AddTransient<IPhotosService, PhotosService>();

            // Dependency injection - Inverse of Control. Mapowanie interfejsu IAccountService na obiekt AccountService.
            services.AddTransient<IAccountService, AccountService>();

            // Dependency injection - Inverse of Control.
            services.AddTransient<SessionHelper>();

            // Sesja.
            services.AddSession(opt =>
            {
                // Nazwa cookies dla sesji.
                opt.Cookie.Name = string.Format(".Gallery.Session.{0}", Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", ""));

                // Po jakim czasie sesja zotanie wyczyszczona. Ka¿dy dostêp do sesji resetuje limit czasu.
                opt.IdleTimeout = TimeSpan.FromMinutes(25);
            });

            // Token AddAntiforgery dla formularzy.
            services.AddAntiforgery(opt =>
            {
                // Nazwa cookies dla sesji.
                opt.Cookie.Name = string.Format(".Gallery.Antiforgery.{0}", Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", ""));
            });

            services.AddControllers(options =>
            {
                var noContentFormatter = options.OutputFormatters.OfType<HttpNoContentOutputFormatter>().FirstOrDefault();
                if (noContentFormatter != null)
                {
                    noContentFormatter.TreatNullValueAsNoContent = false;
                }
            });
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
                app.UseExceptionHandler("/error");
                app.UseStatusCodePagesWithReExecute("/error");
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseSession();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Gallery}/{action=Index}/{id?}");
            });
        }
    }
}