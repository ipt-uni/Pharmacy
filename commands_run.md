```bash
# Init
dotnet new webapp --auth Individual -uld
# Mysql
dotnet remove package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microting.EntityFrameworkCore.MySql # For latest dotnet 10 and support of mariadb
```
