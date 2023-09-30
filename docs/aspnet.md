## Asp net core
- cd shopapi
- dotnet run 
- dotnet publish -c release -o published
- 
      Microsoft.AspNetCore.Routing.Matching.AmbiguousMatchException: The request matched multiple endpoints. Matc
- dotnet  user-jwts create
- dotnet  user-jwts print db3caef7 --show-all
- dotnet  user-jwts key
- dotnet user-secrets list 

## Action Result
 * https://learn.microsoft.com/ru-ru/aspnet/core/tutorials/first-web-api?view=aspnetcore-7.0&tabs=visual-studio
 * https://code-maze.com/net-core-web-development-part6/
 - put : return NoContent();
 - create:
  1. !!  return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
  2.  return CreatedAtRoute("OwnerById", new { id = createdOwner.Id }, createdOwner);
  - DELETE :  return NoContent();
   
## ASPNETCORE_ENVIRONMENT
  - "ASPNETCORE_ENVIRONMENT": "Development" 
  - "ASPNETCORE_ENVIRONMENT": "Production"
  - Production: значение по умолчанию, если DOTNET_ENVIRONMENT и ASPNETCORE_ENVIRONMENT не заданы.
  - if (app.Environment.IsDevelopment()){}
## aspnet7.0 docker for  dockerfile
  -docker pull mcr.microsoft.com/dotnet/aspnet

##   Photo api

- https://code-maze.com/upload-files-dot-net-core-angular/
- https://makolyte.com/aspnetcore-receive-a-file-in-the-request/
- https://code-maze.com/file-upload-aspnetcore-mvc/
