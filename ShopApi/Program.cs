
using EmailService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopApi.Model.Identity;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

var emailConfig = builder.Configuration.
 GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();

if (emailConfig != null)
{
    builder.Services.AddSingleton(emailConfig);
    builder.Services.AddScoped<IEmailSender, EmailSender>();
}


var urls = builder.Configuration.GetSection("CorsPolicy").Get<string[]>()!;

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins(urls)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
});

builder.Services.AddControllers();

var connectStringShop = builder.Configuration.GetSection("ConnectString").Value + "database=ShopDB;";
var connectStringAppIdentity = builder.Configuration.GetSection("ConnectString").Value + "database=AppIdentityDB;";


builder.Services.AddDbContext<AppIdentityDbContext>(
    options => options.UseMySql(connectStringAppIdentity, new MySqlServerVersion(new Version(8, 0, 11))
));

// затем подключаем сервисы Identity
builder.Services.AddIdentity<UserIdentityX01, IdentityRole>()
    .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<AppIdentityDbContext>()
       .AddDefaultTokenProviders();

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(
    opt =>
{
    opt.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "s.x-01.ru API",
        Version = "v2",
        Description = "ASP.NET Core Web API for mebel cluster CMS",

        Contact = new OpenApiContact
        {
            Name = "GitHub",
            Url = new Uri("https://github.com/igoradigey01/webapi")
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",

        }
    });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});


var app = builder.Build();

app.UseRouting();

app.UseSwagger();


app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");

});


app.UseCors();
app.UseAuthentication();   // добавление middleware аутентификации 
app.UseAuthorization();   // добавление middleware авторизации 



app.Map("/hi", async context => await context.Response.WriteAsync("Hello x-01.ru"));


//ProductEndpoints.Map(app) ;
app.MapControllers();

app.Run();




