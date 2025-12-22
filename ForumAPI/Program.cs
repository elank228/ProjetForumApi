using ForumAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using ForumApi.DbContext;
using System.Globalization;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;
using ForumApi.Settings;
using ForumApi.Extensions;
using ForumApi.Data;
using ForumApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;




// Locale
builder.Services.AddLocalization(options => { options.ResourcesPath = "Resources"; });
builder.Services.Configure<RequestLocalizationOptions>(
    options =>
    {
        var supportedCultures = CultureInfo.GetCultures(CultureTypes.AllCultures)
              .Where(c => c.Name.StartsWith("fr") || c.Name.StartsWith("en"))
              .ToList();

        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures = supportedCultures;
    }
);

builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, Role>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
builder.Services.Configure<MailSettings>(configuration.GetSection("Mail"));
builder.Services.Configure<StorageSettings>(configuration.GetSection("Storage"));

var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>();

builder.Services.Configure<DataProtectionTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromDays(jwtSettings.ExpirationInDays));

builder.Services.AddCors(options =>
{
    options.AddPolicy("EnableCORS", builder =>
    {
        builder.AllowAnyOrigin()
           .AllowAnyHeader()
           .AllowAnyMethod()
           .WithExposedHeaders("Content-Disposition");
    });
});

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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
              new string[] { }
         }
    });
});

builder.Services.AddAuth(jwtSettings);

builder.Services.AddHttpClient();

builder.Services.AddControllers(o =>
{
    o.AllowEmptyInputInBodyModelBinding = true;
})
.AddViewLocalization()

.AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddLogging(options =>
{
    options.AddDebug();
    options.AddConsole();
});

// Dependency injections
builder.Services.AddHttpContextAccessor();
builder.Services.AddServices();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
    IdentityModelEventSource.ShowPII = true;
}

app.UseCors("EnableCORS");

// Locale
var localizeOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(localizeOptions.Value);

app.UseRouting();

app.UseAuth();
app.UseAuthorization();


app.UseMiddleware<LoadUserInfosMiddleware>();
app.UseMiddleware<ApplicationExceptionMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await IdentitySeed.SeedRolesAsync(services);
}

app.Run();