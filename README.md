# G5Store

The purpose of this project is to enable companies to sell products to their customers via the internet by using our application. There are two different roles in the application; sellers and customers. Salespeople in the company who use this application will be able to handle products, coupons, system users and orders. The customers will be able to log in, easily search/filter for existing products, add products to the shopping cart, use coupons, create orders and finally receive a confirmation email for their created order.



## Table of contents

* [1. Technologies used](#1. Technologies used)

* [2. Local development setup](#2. Local development setup)

* [3. Team](#3. Team)



## 1. Technologies used
Project is created using:
* .NET Core 3.1

* Blazor

* ASP.NET MVC

* Swagger

* GitHub Action CI/CD

* Entity Framework

* Microsoft Azure
	
	* App service
	* Database server
	* Azure container registry
	* Blobstorage
	
* Docker
	
* MSSQL
	
* MSTest
	
	
## 2. Local development setup
We use Visual studio 2019 as IDE (for both front- and backend), Microsoft SQL Server Management Studio for browsing the database, Postman for making calls to our API and Docker Desktop for testing our Dockerfiles locally. Before you can run the projects you will need the `appsettings.Development.json`-files for both front- and backend. This is because they are included in our gitignore-file. These will be placed in the root folder of each project. The development appsettings for frontend project contains the url for the locally running API while the development appsettings for the backend project contains the connection string to the local database. Below are the two files that will be needed before running the projects. 

Please make sure to paste in your connection string to the local database in the backend appsettings-file. Furthermore, make sure that the ApiHostUrl matches the url to your local API. In order to know what url your API will be running on navigate to `Backend -> Properties -> launchSettings.json`, then check if your sslPort matches the port of `ApiHostUrl` (44339 in this case) in the frontend appsettings-file. If your sslPort does not match the port defined in `appsettings.Development.json` then you need to replace "44339" with your sslPort defined in `launchSettings.json`. For instance, if your sslPort would be "44350" then your appsetting-file would define the ApiHostUrl like this instead: `"ApiHostUrl": "https://localhost:44350/"`.



`appsettings.Development.json` (Frontend)

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "ApiHostUrl": "https://localhost:44339/"
}
```

`appsettings.Development.json` (Backend)

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "ConnectionStrings": {
    "StoreDatabase": "LOCAL_SQL_SERVER_CONNECTION_STRING_GOES_HERE"
  }
}

```



After you have placed these files in the projects you will need to update the database before running the API. In order to do so, navigate to the package manager console in Visual Studio and run this command in the backend project:

```bash
Update-Database
```

This will create the local database and insert some seeded data which are defined in the backend-class `StoreDbContext.cs`.



## 3. Team

#### Product owner

| [![Stephan Johansen](.\Documentation\Images\Profile avatars\Stephan.png)](https://github.com/skjohansen) |
| :----------------------------------------------------------: |
|      [Stephan Johansen](https://github.com/skjohansen)       |

#### Testers

| ![Leila Ershad](.\Documentation\Images\Profile avatars\Default.png) | ![Anna Niemelä](.\Documentation\Images\Profile avatars\Default.png) | ![Monirul Azam](.\Documentation\Images\Profile avatars\Default.png) | ![Jolanta Barkauskaite](.\Documentation\Images\Profile avatars\Default.png) | ![Sara Kristensen](.\Documentation\Images\Profile avatars\Default.png) |
| :----------------------------------------------------------: | :----------------------------------------------------------: | :----------------------------------------------------------: | :----------------------------------------------------------: | :----------------------------------------------------------: |
|                         Leila Ershad                         |                         Anna Niemelä                         |                         Monirul Azam                         |                     Jolanta Barkauskaite                     |                       Sara Kristensen                        |

#### Developers

| [![André Morad](.\Documentation\Images\Profile avatars\Andre.png)](https://github.com/AndreMorad) | [![Nor Shiervani](.\Documentation\Images\Profile avatars\Nor.png)](https://github.com/norshiervani) | [![Ahmad Yassin](.\Documentation\Images\Profile avatars\Ahmad.png)](https://github.com/akyassin) | [![Micael Wollter](.\Documentation\Images\Profile avatars\Mikael.png)](https://github.com/aohzork) | [![Irvin Perez](.\Documentation\Images\Profile avatars\Irvin.png)](https://github.com/Irvper77) |
| :----------------------------------------------------------: | :----------------------------------------------------------: | :----------------------------------------------------------: | :----------------------------------------------------------: | :----------------------------------------------------------: |
|         [André Morad](https://github.com/AndreMorad)         |       [Nor Shiervani](https://github.com/norshiervani)       |         [Ahmad Yassin](https://github.com/akyassin)          |         [Micael Wollter](https://github.com/aohzork)         |          [Irvin Perez](https://github.com/Irvper77)          |

