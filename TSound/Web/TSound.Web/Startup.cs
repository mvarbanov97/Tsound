using CloudinaryDotNet;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSound.Data;
using TSound.Data.Models;
using TSound.Data.UnitOfWork;
using TSound.Services;
using TSound.Services.Contracts;
using TSound.Web.MappingConfiguration;
using TSound.Services.Providers;
using TSound.Services.External.Spotify;
using TSound.Data.Seeder.Seeding;
using System.Web.Http;
using TSound.Services.External;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication;
using TSound.Services.External.SpotifyAuthorization;

namespace TSound.Web
{
    public static class ServiceProviderExtensions
    {
        public static IServiceCollection AddControllersAsServices(this IServiceCollection services,
           IEnumerable<Type> controllerTypes)
        {
            foreach (var type in controllerTypes)
            {
                services.AddTransient(type);
            }

            return services;
        }
    }

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
            services.AddDbContext<TSoundDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<User>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true; //Default.
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.DefaultLockoutTimeSpan = new TimeSpan(0, 5, 0); // Default = 5 min
                options.Lockout.MaxFailedAccessAttempts = 3; // Default = 5 attempts
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredUniqueChars = 1; // Default = 1 unique char
            })
            .AddRoles<Role>()
            .AddUserManager<UserManager<User>>()
            .AddEntityFrameworkStores<TSoundDbContext>();


            // Cloudinary Authentication
            var cloudinaryAccount = new CloudinaryDotNet.Account(
                this.Configuration["Cloudinary:CloudName"],
                this.Configuration["Cloudinary:ApiKey"],
                this.Configuration["Cloudinary:ApiSecret"]);
            var cloudinary = new Cloudinary(cloudinaryAccount);
            services.AddSingleton(cloudinary);

            // Register Logic Services
            services.AddSingleton<HttpClient>(new HttpClient());
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddTransient<IPlaylistService, PlaylistService>();
            services.AddTransient<ISongService, SongService>();
            services.AddTransient<IGenreService, GenreService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAlbumService, AlbumService>();
            services.AddTransient<IArtistService, ArtistService>();

            services.AddScoped<ISpotifyAuthService, SpotifyAuthService>();
            services.AddTransient<IUserAccountsService, UserAccountsService>();
            services.AddTransient<IAccountsService, AccountsService>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IApplicationCloudinary, ApplicationCloudinary>();

            services.AddControllersAsServices(typeof(Startup).Assembly.GetExportedTypes()
                .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
                .Where(t => typeof(ApiController).IsAssignableFrom(t)
                    || t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase)));

            services.AddHttpClient();

            services.AddAuthentication()
                    .AddSpotify(spotifyOptions =>
                    {
                        spotifyOptions.ClientId = $"{this.Configuration["Spotify:ClientId"]}";
                        spotifyOptions.ClientSecret = $"{this.Configuration["Spotify:ClientSecret"]}";
                        spotifyOptions.CallbackPath = "/callback";
                        spotifyOptions.UserInformationEndpoint = "https://api.spotify.com/v1/me";
                        spotifyOptions.SaveTokens = true;
                        spotifyOptions.Scope.Add("playlist-read-private playlist-modify-private user-read-email user-read-private");
                        spotifyOptions.Events.OnRemoteFailure = (context) =>
                         {
                             // Handle failed login attempts here
                             return Task.CompletedTask;
                         };
                        spotifyOptions.ClaimActions.MapJsonKey("urn:spotify:profilepicture", "profilepicture", "url");
                    });

            services.AddAutoMapper(cfg => cfg.AddProfile<AutomapperProfile>());

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<TSoundDbContext>();

                if (env.IsDevelopment())
                {
                    dbContext.Database.Migrate();
                }
                else if (env.IsProduction())
                {
                    dbContext.Database.Migrate();
                }

                if (!dbContext.Users.Any())
                {
                    new TSoundDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
                }
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
