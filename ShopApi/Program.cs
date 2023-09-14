
using EmailService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopApi.Model.Identity;

using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

using Microsoft.AspNetCore.HttpOverrides;
using ShopDB;
using OrderDB;

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
//var connectionString = config.GetSection("DemoApp")["ConnectionString"];
string connectString = String.Empty;

if (builder.Environment.IsDevelopment())
{
    connectString = builder.Configuration["ConnectionStrings:DeveloperX01"]!;
   // Console.WriteLine(connectString);
}else{
    connectString = builder.Configuration.GetSection("ConnectString").Value!;


}

var connectStringShop = connectString + "database=ShopDB;";
var connectStringOrder = connectString + "database=OrderDB;";
var connectStringAppIdentity = connectString + "database=AppIdentityDB;";
builder.Services.AddDbContext<AppIdentityDbContext>(
    options => options.UseMySql(connectStringAppIdentity, Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.33-mysql"))
);

builder.Services.AddDbContext<ShopDbContext>(
    options => options
        .UseMySql(connectStringShop, new MySqlServerVersion(new Version(8, 0, 11)))

);

builder.Services.AddDbContext<OrderDbContext>(
    options => options
        .UseMySql(connectStringOrder, new MySqlServerVersion(new Version(8, 0, 11)))

);


builder.Services.AddIdentity<UserIdentityX01, IdentityRole>()
    .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<AppIdentityDbContext>()
       .AddDefaultTokenProviders();

//------------------------ jwt --------------------------------

var mySecurityKey = new SymmetricSecurityKey(
              Encoding.UTF8.GetBytes(builder.Configuration.GetSection("IdentityX01:TokenX01-Key").Value!)
              );
string[]? audence = builder.Configuration.GetSection("Authentication:Schemes:JwtBearer:Audiences").Get<string[]>();
string joinedString = String.Empty;
if (audence != null)
{
    joinedString = audence.Aggregate((prev, current) => prev + "," + current);
}

builder.Services.AddAuthentication().AddJwtBearer
    (options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration.GetSection("Authentication:Schemes:JwtBearer:Issuer").Value,
            ValidAudience = joinedString,
            IssuerSigningKey = mySecurityKey
        };
    });

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
// for nginx
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});


app.UseRouting();

// if (app.Environment.IsDevelopment()){



    app.UseSwagger();


    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");

    });

//}
app.UseCors();
app.UseAuthentication();   // добавление middleware аутентификации 
app.UseAuthorization();   // добавление middleware авторизации 






//ProductEndpoints.Map(app) ;
app.MapControllers();

app.Run();




