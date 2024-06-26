**To run this solution follow below steps on powershell command**
```
git clone https://github.com/sumangadikrindi/fakeblog.git
# Define the content for the .env file
$env_content = @"
SQL_SA_PASSWORD=P@**w0rd
FAKEBLOG_Users_SQLDBName=UsersDb
FAKEBLOG_Blogs_SQLDBName=BlogsDb
"@

# Write the content to a .env file
$env_content | Out-File -FilePath .env
docker compose up -d --build

#Now you should be able to test the solution at http://localhost:80/swagger

```

**Below I tried to express what I have implemented so far, to show case my understanding and skill in various .net development skills and knowledge on different patterns and technologies:**

```
FakeBlog.sln/
├── FakeBlog.Users.Api/ 
│
│       - Repository pattern for DB Access
│       - CQRS using MetiatR to implement proto service
│       - AutoMapper to conversion between different types easily
│
│        grpc://localhost.5051/CreateUser (Author registration)
│        grpc://localhost.5051/GetUserDetails
│
│       - Own database - Connection string from environment variables configured through docker compose
│                      - DB migration on app startup
│
│        Dockerfile
│
├── FakeBlog.Blogss.Api/ 
│
│       - Repository pattern for DB Access
│       - CQRS using MetiatR to implement proto service
│       - AutoMapper to conversion between different types easily
│
│        grpc://localhost.5052/CreateBlog 
│        grpc://localhost.5052/GetBlog
│
│       - Own database - Connection string from environment variables configured through docker compose
│                      - DB migration on app startup
│
│        Dockerfile
│
├── FakeBlog.Blogss.Api/ 
│
│       - Grpc Service channel registration in dependency injection container
│       - Access service channels based on need
│       - AutoMapper to conversion between different types easily
│
│        http://localhost.80/Author/RegisterAuthor - POST
│        http://localhost.80/Author/{id} - GET
│        http://localhost.80/Blog - POST 
│        http://localhost.80/Blog/{id} - GET - Optional Heder to request for Author details of the blog.
│
│
│       - Grpc service reference to both blog, and user services - grpc service addresses from environment variables configured through docker compose
│
│        Dockerfile
│
├── compose.yml - Docker compose file 
│        - Reads all sensitive information from .env file alone, and sent environment variables in each container as required.
│       - Also spins an SQL Server container to hold test data - Connection credentials from .env file
│       
└── [Improvements possible]
        - Secret fetching from .env files to be replaced with fetching from security vault for production kind solutions
        - RabbitMQ kind of messaging service spinning throgh Docker compose
        - Push events on Author registration, and Blog post creation
        - Create notifier service (Background service) to consume events and send notifications.
        - Response Caching
        - Unit testing of Command and Request Handlers, to veify base functionality and as wel to check even creation
```
