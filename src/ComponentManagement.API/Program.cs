using ComponentManagement.Infrastructure.Persistence;
using ComponentManagement.Infrastructure.Services;
using ComponentManagement.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using ComponentManagement.Application.Users.Commands;
using ComponentManagement.Infrastructure.Repositories;
using ComponentManagement.Infrastructure.BackgroundJobs;
using ComponentManagement.Infrastructure.Realtime;
using ComponentManagement.Application.Notifications.Interfaces;
using ComponentManagement.Infrastructure.Notifications;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services
    .AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });


builder.Services.AddSignalR();

// Add PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(LoginCommand).Assembly));

// Add Services

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IComponentRepository, ComponentRepository>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IAppDbContext, AppDbContext>();
builder.Services.AddScoped<IComponentActivityRepository, ComponentActivityRepository>();
builder.Services.AddScoped<IPartRepository, PartRepository>();
builder.Services.AddScoped<IAPLRepository, APLRepository>();
builder.Services.AddScoped<IAPLPartRepository, APLPartRepository>();
builder.Services.AddScoped<IPartComponentAPLRepository, PartComponentAPLRepository>();
builder.Services.AddScoped<IComponentCustomPartRepository, ComponentCustomPartRepository>();
builder.Services.AddScoped<IHistoricalRepository, HistoricalRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddDataProtection();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUnitRepository, UnitRepository>();
builder.Services.AddScoped<IComponentLifetimeRepository, ComponentLifetimeRepository>();  
builder.Services.AddScoped<INotificationBroadcaster, SignalRNotificationBroadcaster>();

builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

builder.Services.AddHttpClient<FonnteWhatsappNotificationService>();

builder.Services.AddScoped<IWhatsappNotificationService>(sp =>
    sp.GetRequiredService<FonnteWhatsappNotificationService>());

builder.Services.AddScoped<ILifetimeNotificationService>(sp =>
    sp.GetRequiredService<FonnteWhatsappNotificationService>());

builder.Services.AddHostedService<LifetimeMonitoringWorker>();






// Add CORS
// ✅ CORS configuration for secure cookie-based auth
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173") // ganti sesuai port React kamu
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // ✅ wajib untuk cookie
    });
});


// Add Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Component Management API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
    c.CustomSchemaIds(type => type.FullName);
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
        ),
        NameClaimType = ClaimTypes.Name,
        RoleClaimType = ClaimTypes.Role
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var roleHeader = context.Request.Headers["x-user-role"].FirstOrDefault();
            string? cookieName = null;

            if (!string.IsNullOrEmpty(roleHeader))
            {
                // 🔹 Gunakan role dari header
                cookieName = $"access_token_{roleHeader}";
            }
        var cookies = context.Request.Cookies;

        if (cookies.Any())
        {
            // Ambil cookie yang terakhir dibuat
            var latestToken = cookies.Last().Value;
            context.Token = latestToken;
        }

            // Jika tidak ada role di header, fallback ke role default
            var accessToken = !string.IsNullOrEmpty(cookieName)
                ? context.Request.Cookies[cookieName]
                : context.Request.Cookies.FirstOrDefault(c => c.Key.StartsWith("access_token_")).Value;

            if (!string.IsNullOrEmpty(accessToken))
            {
                context.Token = accessToken;
                Console.WriteLine($"[JWT] Token aktif: {cookieName ?? "default"} ✅");
            }
            else
            {
                Console.WriteLine($"[JWT] Tidak ada token ditemukan untuk role {roleHeader ?? "(default)"} ❌");    
            }

            return Task.CompletedTask;
        },

        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"[JWT ERROR] {context.Exception.Message}");
            return Task.CompletedTask;
        }
    };
});



var app = builder.Build();

// Configure middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("AllowFrontend");
app.MapHub<NotificationHub>("/hubs/notifications");
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();