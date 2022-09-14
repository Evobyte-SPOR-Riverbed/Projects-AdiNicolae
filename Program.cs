using Drinktionary.Cache;
using Drinktionary.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Drinktionary.Services;

namespace Drinktionary;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        // Add authentication service.
        builder.Services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = ConfigurationManager.AppSetting["JWT:ValidIssuer"],
                ValidAudience = ConfigurationManager.AppSetting["JWT:ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSetting["JWT:Secret"]))
            };
        });

        // Add HTTP Context Accessor.
        builder.Services.AddHttpContextAccessor();

        // Add services.
        builder.Services.AddScoped<ICacheService, CacheService>();
        builder.Services.AddSingleton<IUriService, UriService>(sp =>
        {
            IHttpContextAccessor accessor = sp.GetRequiredService<IHttpContextAccessor>();
            HttpRequest request = accessor.HttpContext.Request;
            string uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
            return new UriService(uri);
        });

        builder.Services.AddSwaggerGen();

        // Add database context.
        builder.Services.AddDbContext<DatabaseContext>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            // The default HSTS value is 30 days. You may want to change this for production
            // scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        app.UseStaticFiles();
        app.UseRouting();
        app.UseSwagger();
        app.UseSwaggerUI();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller}/{action=Index}/{id?}"
        );

        app.MapFallbackToFile("index.html");

        app.Run();
    }
}