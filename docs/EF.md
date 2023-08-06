# EF создать модель классов из субд mysql

 0. nuget packages
   - Pomelo.EntityFrameworkCore.MySql
   - Microsoft.EntityFrameworkCore.Tools
   - Microsoft.EntityFrameworkCore.Design
   - install https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-2.0.9-windows-x64-installer
 1.  зависимости см   
  - https://www.nuget.org/packages/Pomelo.EntityFrameworkCore.MySql/7.0.0   
  -  https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql  

 2.  
 - dotnet ef dbcontext scaffold "Database=ShopDB;server=localhost;port=3306;UserId=root;Password=123456;" "Pomelo.EntityFrameworkCore.MySql" 



## help ef
 - https://learn.microsoft.com/ru-ru/ef/core/managing-schemas/scaffolding/?tabs=dotnet-core-cli
 - https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql

## user-secrets 
1. use
   1. set
 - dotnet user-secrets init
 - dotnet user-secrets set ConnectionStrings:DeveloperX01 "server=127.0.0.1;port=3306;UserId=root;Password=123456;"
   2. get 
 - var movieApiKey = builder.Configuration["ConnectionStrings:DeveloperX01"];
 - var moviesApiKey = _config["ConnectionStrings:DeveloperX01"]; 
 2. help
  - https://learn.microsoft.com/ru-ru/aspnet/core/security/app-secrets?view=aspnetcore-7.0&tabs=windows

 ## docker-compose secrets
  - https://nvd.codes/post/use-docker-secrets-in-asp-net-core/ 

## help dbcontext
 - https://learn.microsoft.com/ru-ru/ef/core/dbcontext-configuration/ 