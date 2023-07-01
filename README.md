## Conduit Api
![Screenshot from 2022-05-03 17-58-34](https://user-images.githubusercontent.com/40242609/166452994-46249abf-d9d4-4660-97ab-759f15e6b8d4.png)

Conduit is a fullstack blogging application. This was created to demonstrate a fully fledged fullstack application built with ASP.NET Core (with feature orientation) including CRUD operations, authentication, routing and pagination.

You can find the frontend project for Conduit at https://github.com/RupeshGhosh10/ConduitWeb/

### Build With
- ASP.NET Web API
- PostgreSql

### How it works
- AutoMapper: Enables mapping of Data Transfer Objects (DTOs) to Domain Models.
- Entity Framework: Used for object-relational mapping (ORM) to interact with the database.
- JWT Authentication: Implements JSON Web Token (JWT) for user authentication and authorization.
- Swagger: Provides an interactive API documentation and exploration interface.
- Unique Generated Slugs: Each article in the Conduit API is assigned a unique generated slug. Slugs provide human-readable and SEO-friendly URLs for articles.
- Pagination Support: The API supports pagination by utilizing the limit and offset parameters. These parameters allow you to retrieve a specific number of records (limit) starting from a given position (offset).


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
