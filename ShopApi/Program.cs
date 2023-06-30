
using EmailService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopApi.Model.Identity;

var builder = WebApplication.CreateBuilder(args);

var emailConfig = builder.Configuration.
 GetSection("EmailConfiguration")
                .Get<EmailConfiguration>() ;

if (emailConfig != null)
{
    builder.Services.AddSingleton(emailConfig);
    builder.Services.AddScoped<IEmailSender, EmailSender>();
}


builder.Services.AddControllers();
//   var authConfig = Configuration.GetSection("Auth");
var connectStringShop = Environment.GetEnvironmentVariable("ConnectString") + "database=ShopDB;";
var connectStringAppIdentity = Environment.GetEnvironmentVariable("ConnectString") + "database=AppIdentityDB;";
//  var serverVersion = new MySqlServerVersion(new Version(8, 0,21));

// Replace 'YourDbContext' with the name of your own DbContext derived class.
// builder.Services.AddDbContext<MyShopDbContext>(
//     options => options
//         .UseMySql(connectStringShop, new MySqlServerVersion(new Version(8, 0, 11)))

// );

builder.Services.AddDbContext<AppIdentityDbContext>(
    options => options.UseMySql(connectStringAppIdentity, new MySqlServerVersion(new Version(8, 0, 11))
));

// затем подключаем сервисы Identity
builder.Services.AddIdentity<UserIdentityX01, IdentityRole>()
    .AddRoles<IdentityRole>()                      //31.12.21
        .AddEntityFrameworkStores<AppIdentityDbContext>()
       .AddDefaultTokenProviders();

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();   // добавление middleware аутентификации 
app.UseAuthorization();   // добавление middleware авторизации 
app.UseCors();


app.Map("/hi", async context => await context.Response.WriteAsync("Hello METANIT.COM!"));


//ProductEndpoints.Map(app) ;
app.MapControllers();

app.Run();




