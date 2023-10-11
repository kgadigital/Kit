using KitAplication.Data;
using KitAplication.Interface;
using KitAplication.Repository;
using KitAplication.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using KitAplication.Handler;
using Microsoft.EntityFrameworkCore.Metadata;
using static System.Net.Mime.MediaTypeNames;

namespace KitAplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<SqlContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("SQL")));
            // Register the repository
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<ISystemService, SystemService>();
            builder.Services.AddScoped<ILinkService, LinkService>();
            builder.Services.AddScoped<IChatSettingsService, ChatSettingService>();
            builder.Services.AddScoped<IMessageService, MessageService>();
            builder.Services.AddScoped<IChatAIService, ChatAIService>();
            builder.Services.AddScoped<IFileHandler,FileHandler>();


            builder.Services.AddLogging();

            //add HTTP in openai services
            builder.Services.AddHttpClient<ChatAIService>();
            //Authentication is responsible for providing the ClaimsPrincipal for authorization to make permission decisions against.
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); //Set time to the user is loggd out
                    options.SlidingExpiration = true;
                    options.AccessDeniedPath = "/Account/AccessDenied";
                });

            builder.Services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireClaim("IsAuthenticated", "true")
                    .Build();
            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.SameSite = SameSiteMode.None;
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });

            builder.Services.AddAntiforgery(options =>
            {
                options.SuppressXFrameOptionsHeader = true;
            });

            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();
            var app = builder.Build();

            //disable caching of sensitive pages
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Content-Security-Policy", "frame-ancestors 'self' * file:;");
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                context.Response.Headers["Pragma"] = "no-cache";
                context.Response.Headers["Expires"] = "0";
                await next();
            });

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("AllowAllOrigins");
           
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}