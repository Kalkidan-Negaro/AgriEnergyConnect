
# AgriEnergyConnect Application Setup Guide

## Introduction
Welcome to AgriEnergyConnect! This guide will walk you through the steps to set up and run the application locally on your machine.

**Unifying the Agricultural Ecosystem:**

AgriEnergy Connect bridges the gap between farmers and employees, fostering a collaborative environment.

**Employee Empowerment:**

- Effortlessly register and manage farmer profiles, ensuring accurate data for efficient operations.
- Seamless product filtering simplifies the process of adding new products to farmer profiles.

**Enhanced Farmer Integration:**

- Empower farmers with a centralized platform for their agricultural needs.
- Facilitate easy product data management through intuitive interfaces.

**Harnessing the Power of C#:**

Built upon the robust foundation of C#, AgriEnergy Connect delivers:
- **Scalability:** Adaptable to your growing agricultural network.
- **Performance:** Efficient data management for a seamless user experience.
- **Security:** A secure environment for sensitive farmer and product information.

**Getting Started:**

For a seamless setup, please refer to the detailed instructions within the Getting Started section of this readme.

**Embrace the Future of Agriculture:**

AgriEnergy Connect is your gateway to a more connected, efficient, and sustainable agricultural future. Let's cultivate a thriving ecosystem together!


## Prerequisites
Before you begin, ensure you have the following installed:
- [ ] .NET SDK (version 4.0 or higher)
- [ ] SQL Server (or SQL Server Express)
- [ ] IDE (Visual Studio, Visual Studio Code, etc.)

## Setting Up the Database
1. **Create Database:**
   ```sql
   CREATE DATABASE AgriEnergyConnect;
   USE AgriEnergyConnect;
   ```
2. **Create Tables:**
   ```sql
   CREATE TABLE Farmers (
       FarmerID INT PRIMARY KEY IDENTITY(1,1),
       Name NVARCHAR(100) NOT NULL,
       ContactInfo NVARCHAR(100),
       Address NVARCHAR(255)
   );
   
   CREATE TABLE Products (
       ProductID INT PRIMARY KEY IDENTITY(1,1),
       FarmerID INT,
       Name NVARCHAR(100) NOT NULL,
       Category NVARCHAR(50),
       ProductionDate DATE,
       FOREIGN KEY (FarmerID) REFERENCES Farmers(FarmerID)
   );
   
   CREATE TABLE Employee (
       EmployeeID INT PRIMARY KEY IDENTITY(1,1),
       Name NVARCHAR(50) NOT NULL,
       PasswordHash NVARCHAR(255) NOT NULL,
       Role NVARCHAR(20) NOT NULL CHECK (Role IN ('Farmer', 'Employee')),
       FarmerID INT,
       FOREIGN KEY (FarmerID) REFERENCES Farmers(FarmerID)
   );
   ```
3. **Insert Sample Data:**
   ```sql
   INSERT INTO Farmers (Name, ContactInfo, Address) VALUES
   ('John Doe', 'john.doe@example.com', '123 Farm Lane'),
   ('Jane Smith', 'jane.smith@example.com', '456 Greenfield Road'),
   ('Michael Brown', 'michael.brown@example.com', '789 Orchard Street'),
   ('Emily Wilson', 'emily.wilson@example.com', '101 Farm Road');
   
   INSERT INTO Products (FarmerID, Name, Category, ProductionDate) VALUES
   (1, 'Organic Apples', 'Fruit', '2023-05-01'),
   (2, 'Free-range Eggs', 'Poultry', '2023-05-10'),
   (3, 'Fresh Milk', 'Dairy', '2023-05-05'),
   (4, 'Organic Vegetables', 'Vegetables', '2023-06-15');
   
   INSERT INTO Employee (Name, PasswordHash, Role, FarmerID) VALUES
   ('John Doe', 'hashed_password_1', 'Farmer', 1),
   ('Jane Smith', 'hashed_password_2', 'Farmer', 2),
   ('Alice Johnson', 'hashed_password_3', 'Employee', NULL),
   ('Bob Brown', 'hashed_password_4', 'Employee', NULL),
   ('Michael Brown', 'hashed_password_5', 'Farmer', 3),
   ('Emily Wilson', 'hashed_password_6', 'Farmer', 4);
   ```
   
## Configuring Connection Strings

  {
   "ConnectionStrings": {
    "AgriEnergyConnect": "Server=labVMH8OX\\SQLEXPRESS;Database=AgriEnergyConnect;Trusted_Connection=True;TrustServerCertificate=True;"
  }
  }
  ```

## Running the Application
1. **Clone the Repository:**
   ```bash
   git clone https:[https://github.com/Kalkidan-Negaro/AgriEnergyConnect]
   cd AgriEnergyConnect
   ```
2. **Build and Run the Application:**
   - Open the solution in your preferred IDE (Visual Studio, VS Code).
   - Restore dependencies and build the solution.
   - Set the startup project to your main application (e.g., `AgriEnergyConnect.Web`).
   - Press F5 or use the IDE's debugging tools to run the application.

## Accessing the Application
- Once the application is running locally, open a web browser and navigate to `http://localhost:5000`
  
## Additional Notes
- For production deployment, ensure to configure appropriate security measures and use environment-specific settings.
- Refer to the application's documentation for more detailed functionality and customization options.
