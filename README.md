# Task_JODBE

Web Application for User Management
Overview
This project is a web application developed using .NET MVC, Entity Framework, HTML, jQuery, AngularJS, CSS, and SQL Database. The application is designed to allow an admin user to manage users through three key functionalities:

View Users: Admin can view a list of users with details including photo, name, email, mobile number, and password.
Add Users: Admin can manually add new users to the system by entering user information.
Import Users from Excel: Admin can upload a provided Excel file containing user data, and the system will automatically generate passwords for the users (excluding images).
The application has been tested thoroughly using Postman to ensure all endpoints are functioning correctly.

Features

1. Login Page:
A secure login page for the admin user.
Requires email and password for authentication.
Admin credentials are stored in the SQL Database.





2. User List Page:
Displays a list of users including:
Photo
Name
Email
Mobile Number
Password (password is displayed in a masked format for security)
Users can be searched and filtered if required.



3. Manual User Addition Page:
A form for adding new users by inputting:
Name
Email
Mobile Number
Password (password is generated automatically if left blank)
Upon form submission, user data is stored in the SQL Database.



4. Excel Import Page:
Admin can upload an Excel file containing user data.
The system reads the data and populates the users' information into the database.
Password generation occurs automatically during import.
Images from the Excel file are ignored as per the task requirements.
The import process has been optimized to complete within two minutes for handling large datasets.
Setup Instructions
To run the project locally, follow these steps:

Prerequisites
Visual Studio with .NET MVC and Entity Framework installed.
SQL Server or any compatible database server.
Postman (for testing API endpoints).
1. Clone the Repository
Use the following command to clone the repository:
bash
Copy code
git clone https://github.com/yourusername/repository-name.git

2. Install Dependencies
Navigate to the project folder and restore NuGet packages:
bash
Copy code
dotnet restore


3. Set Up the Database
Create a new SQL Server database for the project.
Add the admin user credentials to the database.
Apply the necessary migrations to set up the database schema.
bash
Copy code
dotnet ef database update


4. Admin Login Credentials
Before logging into the admin panel, create an admin user in the database using the following SQL query:
sql
Copy code
INSERT INTO Users (Email, Password)
VALUES ('admin@example.com', 'password123'); -- Replace with desired credentials
Ensure the password is hashed before saving it in the database if your system uses hashing for password storage.




5. Run the Application
Start the application using Visual Studio or by running:
bash
Copy code
dotnet run
Access the website through your browser at http://localhost:5000.
API Endpoints
The following API endpoints have been created and tested using Postman:

POST /api/login: Authenticates the admin and returns a token.
GET /api/users: Fetches a list of all users in the system.
POST /api/users: Adds a new user.
POST /api/import: Handles the import of users from an Excel file.
Testing with Postman:
The API has been thoroughly tested using Postman.
Ensure the Authorization header is set with the correct token when making requests.
Performance Optimization
The Excel Import Page has been optimized to ensure that the process of importing one million users is completed within two minutes.
The import process only handles textual data and excludes images to maintain performance.
Conclusion
This web application provides a simple interface for managing users, including manual additions and mass imports via Excel. The system is optimized for handling large datasets, and all functionalities have been rigorously tested to meet the requirements of the task.

Additional Notes:
Security Considerations: Ensure that all user passwords are hashed and salted before being stored in the database.
Performance: Importing large datasets from Excel can be resource-intensive. Optimizations have been applied to ensure the process completes within the required time.
