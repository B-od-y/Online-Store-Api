                  Online-Store-ASP.Net Web API Project 🛒📲
 
Project Desciption 📜:
  This project is an Online Store Management System developed using ASP.NET Web API, ADO.NET,
  and SQL Server.
  The solution is built using the 3-tier architecture pattern: DAL (Data Access Layer),
  BAL (Business Access Layer), and API (Presentation Layer).

  Technologies Used 💻:
    ASP.net Web API ⛓️‍💥
    ADO.net ⛓️
    Sql Server 🗃️
    3-Tier Architecture (DAL - BAL - APi) 🔩

  Database Design 🗄️ :
  Database Name: OnLineShop_DB

  Main Tables:
    - Customers         : Stores customer information
    - Orders            : Manages customer orders
    - OrderItems        : Lists items included in each order
    - OrderStatus       : Tracks status of orders (Pending, Completed, etc.)
    - Payments          : Stores payment details
    - ProductCatalog    : Contains product details
    - ProductCategory   : Categorizes products
    - Reviews           : Customer feedback on products
    - Shipping          : Handles shipping details
    - ShippingStatus    : Indicates the current shipping status

  Table Relationships 🔑:
      - OrderItems linked to Orders (via OrderID) and ProductCatalog (via ProductID)
      - Orders linked to Customers and OrderStatus
      - ProductCatalog linked to ProductCategory and Reviews
    - Orders also linked to Payments and Shipping


    
