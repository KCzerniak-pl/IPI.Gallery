using GalleryWebApi.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

namespace GalleryWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Zamiana w URL wszystkich liter na ma³e.
            services.AddRouting(opt => opt.LowercaseUrls = true);

            // Autentykacja z wykorzystaniem JWT.
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(jwt =>
                {
                    var secret = Encoding.ASCII.GetBytes(Configuration["JwtConfig:Secret"]);

                    jwt.RequireHttpsMetadata = false;
                    jwt.SaveToken = true;
                    jwt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secret),
                        ValidateLifetime = true,
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ClockSkew = TimeSpan.FromMinutes(1)
                    };
                });

            // Ustawienie zarz¹dzania uprawnieniami/rolami.
            // Je¿eli nie potrzeba zarz¹dznia rolami mo¿na wykorzystaæ "AddDefaultIdentity".
            services.AddIdentity<Database.Entities.GalleryUser, IdentityRole>().AddEntityFrameworkStores<Database.AuthDbContext>().AddDefaultTokenProviders();

            // Konfiguracja kontekstu po³¹czenia z baz¹ (wymagane jest dodanie referencji do utworzonej wczeœniej biblioteki "Database").
            // Wymaga dodatkowych bibliotek: Microsoft.EntityFrameworkCore, Microsoft.EntityFrameworkCore.SqlServer.
            services.AddDbContext<Database.GalleryDbContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("GalleryDatabase")));
            services.AddDbContext<Database.AuthDbContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("GalleryDatabase")));

            // Swagger.
            // Wymaga dodatkowych bibliotek: Swashbuckle.AspNetCore.Swagger, Swashbuckle.AspNetCore.SwaggerGen, Swashbuckle.AspNetCore.SwaggerUI.
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Gallery API", Version = "v1" });

                // JWT - konfiguracja dla Swaggera.
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Please insert JWT token into field",
                    Name = "Authorization",
                    BearerFormat = "JWT",
                    Scheme = JwtBearerDefaults.AuthenticationScheme.ToLowerInvariant(),
                    Type = SecuritySchemeType.Http,
                    In = ParameterLocation.Header,
                });
                c.OperationFilter<AuthResponsesOperationFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Swagger.
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gallery API v1");
            });

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            UpgradeDatabase(app);
        }

        private void UpgradeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var galleryDbContext = serviceScope.ServiceProvider.GetService<Database.GalleryDbContext>();
                var authDbContext = serviceScope.ServiceProvider.GetService<Database.AuthDbContext>();

                if (galleryDbContext != null && galleryDbContext.Database != null)
                {
                    galleryDbContext.Database.Migrate();
                }

                if (authDbContext != null && authDbContext.Database != null)
                {
                    authDbContext.Database.Migrate();
                }
            }
        }
    }
}
