CREATE DATABASE UserManagementDB;

CREATE TABLE Users
(
    UserID INT IDENTITY(1,1) PRIMARY KEY, 
    Name NVARCHAR(100),  
    Email NVARCHAR(100) UNIQUE,  
    MobileNumber NVARCHAR(20),  
    password NVARCHAR(max),
    passwordHash VARBINARY(MAX),
    passwordSalt VARBINARY(MAX),
    CreatedAt DATETIME DEFAULT GETDATE(),  
    UpdatedAt DATETIME,  
    Photo NVARCHAR(MAX)  
);


CREATE TABLE AdminUsers
(
    AdminID INT IDENTITY(1,1) PRIMARY KEY,  
    Email NVARCHAR(100) UNIQUE NOT NULL,    
    password NVARCHAR(max),
    passwordHash VARBINARY(MAX),
    passwordSalt VARBINARY(MAX),     
    CreatedAt DATETIME DEFAULT GETDATE()    
);





