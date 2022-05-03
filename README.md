## Conduit Api
![Screenshot from 2022-05-03 17-58-34](https://user-images.githubusercontent.com/40242609/166452994-46249abf-d9d4-4660-97ab-759f15e6b8d4.png)

Conduit is a fullstack blogging application. This was created to demonstrate a fully fledged fullstack application built with ASP.NET Core (with feature orientation) including CRUD operations, authentication, routing and pagination.

Frontend Project: https://github.com/RupeshGhosh10/ConduitWeb/

### Build With
- ASP.NET Web API
- PostgreSql

### How it works
- AutoMapper for mapping DTO with Domain Model
- EntityFramework for ORM
- JWT Authentication
- Swagger via Swashbuckle.AspNetCore

### How to use
To clone and run this application, you'll need git, dotnet cli and postgresql installed on your computer. From your command line:

```bash
# clone this repository
$ git clone https://github.com/RupeshGhosh10/ConduitApi/

# install dependencies
$ dotnet restore

# perform migrations
$ dotnet ef --startup-project Conduit.Api/Conduit.Api.csproj database update

# run api
$ cd Conduit.Api
$ dotnet watch

# open https://localhost:5001/swagger/index.html on broswer
```
