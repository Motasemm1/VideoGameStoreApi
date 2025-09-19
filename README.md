## 🎮 VideoGameStoreApi

A fully-featured **RESTful Web API** for a video game store, allowing users to register, browse games, place orders, and manage their purchases.  
The project follows **Clean Architecture** principles and demonstrates best practices for building scalable .NET applications.

---

## 📝 Overview

This API enables:

- 👤 **User registration & login** with JWT authentication  
- 🎮 **Browsing available games** (with Genres and Publishers)  
- 🛒 **Placing and managing orders** (linked to the logged-in user)  
- 🛠 **Admin panel features** for managing Games, Genres, and Publishers  

The solution is structured for **maintainability** and **future scalability**, making it an excellent reference for learning or production use.

---

## 🛠️ Tech Stack

- **ASP.NET 9** – RESTful API 
- **Entity Framework Core** – Code-First approach
- **SQL Server** – Database provider
- **JWT Authentication** – Secure login & token-based authorization
- **Role-based Authorization** – Separate permissions for Admins and Users

---

## 🚀 Features

### 🔑 Authentication & Authorization
- JWT-based login
- Role-based access (Admin/User)

### 🎮 Game Management
- CRUD for Games, Genres, and Publishers (Admin only)

### 🛒 Orders
- Place orders for games (linked to logged-in user)
- Retrieve order details and items

### ⚙ Infrastructure & Code Quality
- Clean Architecture (Controllers → Services → Repositories → DbContext)
- Centralized Error Handling Middleware
- Validation for incoming requests

---

## 📂 Project Structure

Web/
├── Controllers/ # User, VideoGame, Order, Genre, Publisher Controllers
├── Middlewares/ # Exception Middleware
Application/
├── DTOs/ # DTOs for Games, Users, Orders, Genres, Publishers
├── Interfaces/ # Service Interfaces
└── Services/ # Business Logic
Infrastructure/
├── Data/ # EF Core DbContext
├── Repositories/ # Repository Implementations
└── Interfaces/ # Repository Interfaces
Domain/
└── Entities/ # User, VideoGame, Order, Genre, Publisher, OrderItem

yaml
Copy code

---

## 🔑 JWT Authentication Example

Example JWT Payload:

```json
{
  "sub": "8b19c3e3-0a2d-4b11-95b4-8c2f55e0af93",
  "unique_name": "john_doe",
  "role": "Admin",
  "exp": 1734512390,
  "iss": "VideoGameStoreApi",
  "aud": "VideoGameApiUser"
}
```
sub → User ID (GUID)

unique_name → Username

role → Role (Admin / User)

exp → Expiration timestamp



##📬 API Testing
The API has been tested using:

Postman (primary testing tool)

Scalar (alternative API testing)

##📈 Future Improvements
💳 Payment Gateway integration

🧪 Unit & Integration Testing (xUnit)

🐳 Containerization with Docker

##🧪 Getting Started
1️⃣ Clone the Repository
bash

git clone https://github.com/Motasemm1/VideoGameStoreApi.git
cd VideoGameStoreApi

2️⃣ Update Configuration
Copy appsettings.example.json → appsettings.json

Update your SQL Server connection string and JWT token

3️⃣ Apply Migrations
bash

dotnet ef database update

4️⃣ Run the Project
bash

dotnet run

5️⃣ Test the Endpoints
Use Postman or Scalar to send requests.

