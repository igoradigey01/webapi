
using EmailService;

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




