# [BookStore](http://bookwebstore.somee.com/)

BookStore is an e-commerce web built with ASP.NET Core MVC, using Entity Framework Core and Microsoft SQL Server as the backend technologies. The project uses ASP.NET Core Identity for authentication and authorization, and follows the repository and unit of work pattern for data access. The project is structured using n-tier architecture, with separate projects for the Presentation, Entity, Data Access, and Business Logic.
# Project Description

BookStore allows user to browse and purchase books, while store can sell their products through the platform. The project consists of three roles: User, Librarian, and Admin. User can add book to the  cart, view and update the cart, and complete orders via Stripe payment processing. Librarian can add product, edit or delete existing product, and manage orders. Admins can create account, assign permissions to users and lock/unlock account.

Default admin account : admin@gmail.com 
                        Admin@123
# Project Structure

The BookStore project follows a typical n-tier architecture structure with the following layers:

1. **Presentation Layer**: This layer represents the user interfaces, such as MVC or API controllers, views, or command-line interfaces. It interacts with the application through use cases, typically via dependency injection.

2. **BusinessLogic Layer**: This layer contains the application logic and orchestrates the flow of data. It defines use cases or application services that encapsulate the business rules and acts as an intermediary between the Presentation and DataAccess layers.

3. **DataAccess Layer**: This layer is responsible for interacting with the database and performing CRUD (create, read, update, delete) operations on the data. The DataAccess Layer in the BookStore project is implemented as a separate project and uses ASP.NET Core EF Core for ORM (object-relational mapping) and MSSQL for database management.

4. **Entity Layer**: This layer contains Entity, classes and functions that are used across the application

# Technologies Used 
- **Programming Language**: C# 11 (.NET 7)
- **Web Framework**: ASP.NET Core MVC
- **Database**: Entity Framework Core SQL Server 7.0.13 (ORM)
- **Identity Provider**: Microsoft AspNetCore Identity 7.0.13
- **Front-End Libraries**: Bootstrap, jQuery, DataTables, Ajax
- **Several external technologies**: Toastr, Stripe
# License
The BookStore project is open-source and released under the [MIT License](https://opensource.org/licenses/MIT). You are free to use, modify, and distribute the codebase as per the license terms.






  
