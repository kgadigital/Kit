# Developer Documentation - "KIT- Chatbot"

## 1. System Overview

The goal of this project is to develop a chatbot using MVC .NET and the OpenAI API, which will allow employees of Karlskoga Municipality to ask IT-related questions. The chatbot will be accessible through a popup chat window on the municipality's website.

The project includes an administrator login that enables administrators to create and manage multiple OpenAI systems, chat settings, and create links to the front-end chatbot. Each OpenAI system can be customized with a specific system name, OpenAI model, prefix for user question input, and system content text. Administrators can create system messages for each system with specific roles (assistant/user) and their content text to provide the chatbot with better instructions on how to handle different types of user interactions. The prefix is added to the beginning of the user's question to provide clearer instructions since the chatbot model may not always be attentive to the system content instructions.

The target audience for this chatbot is employees within the municipality who need quick and easy access to IT-related information. By providing a user-friendly chatbot interface, the goal is to reduce the workload on the IT department and improve efficiency in the workplace.
### Backend:

- **ASP.NET Core MVC**: This framework is used to implement server-side logic and handle HTTP requests. It forms the core of our web application.

- **SQL Server**: The relational database management system is employed for data storage. It stores and manages data efficiently.

- **Entity Framework Core**: It serves as the Object-Relational Mapping (ORM) tool for database access and management. It simplifies database operations.

### Frontend:

- **HTML, CSS, and JavaScript,Jquary**: These technologies are used to build the user interface components. They define the look and interactivity of our web application.

- **Razor Views**: Razor views are employed to render dynamic content and handle user interactions. They allow us to create dynamic web pages.

## 2. Development Environment

To set up the development environment and run the "KIT-chatbot" system, follow these steps:

### Prerequisites:

- Visual Studio IDE (2019 or later) with ASP.NET Core development tools installed. This is our development environment.

- SQL Server or SQL Server Express. It's used as our database server.

- .NET 6 SDK. This is the required SDK version for our project.

### Project Setup:

1. Clone the project repository from the version control system. This fetches the project source code.

2. Open the solution file in Visual Studio. We use Visual Studio as our development IDE.

3. Restore the NuGet packages used in the project. This ensures all necessary libraries are available.

4. Configure the database connection string in the `SqlContext.cs` file. Specify how the application connects to the database.(In the ClassLibrary)

6. Configure the OpenAi key string in the `appsettings.json` file. 

9. Run database migrations using Entity Framework Core:
   - Open the Package Manager Console.
   - Execute the command: `Update-Database`. This initializes and updates the database schema.

10. Build the solution. Compile the project to create the executable.

### Running the Application:

1.Press the "Run" button in Visual Studio. This starts the application.

3. The application will launch in the default web browser. You can access and interact with it here.

4. Run database migrations using Entity Framework Core:
   - Open the Package Manager Console.
   - Execute the command: `Update-Database`. This ensures the database schema is up to date.

## 3. System Functionality

The "Kit-chatbot" system provides the following functionalities:

 - Create and manage multiple OpenAI systems, chat settings, and create links to the front-end chatbot.
  - Each OpenAI system can be customized with a specific system name, OpenAI model, prefix for user question input, and system content text

  -  Administrators can create system messages for each system with specific roles (assistant/user) and their content text to provide the chatbot with better instructions on how to handle different types of user interactions

## 4. Code Structure and Organization

The codebase of the "KIT-Chatbot" system follows a structured organization to enhance readability and maintainability. Here is an overview of the key directories and files:

- **Models**: These classes define the structure of our data.

- **Controllers**: Implements backend logic and handles HTTP requests. These are responsible for processing user requests.

- **Views**: Contains Razor view templates for rendering HTML content. These define how the application's user interface is presented.

- **Scripts and Styles**: Contains JavaScript and CSS files for frontend functionality and styling. These enhance the user experience.

- **Services**: Implements additional business logic and interacts with the repository. These provide additional functionality.

- **Repository**: Interacts with the data. This is the layer responsible for data access.

- **Data**: Includes the DbContext class and database migration files. This manages the database and its schema.

## 5. Technologies Used in the Project

### Framework:

- .NET 6.0: The core framework for building the application, providing a robust and efficient runtime environment.

- ASP.NET Core MVC: Utilized to implement the Model-View-Controller architectural pattern, enabling the development of a structured and maintainable web application.

- Entity Framework Core: Serves as the Object-Relational Mapping (O/RM) tool, allowing cross-platform database access and management with ease.

- Bootstrap (CSS & JavaScript): The popular front-end framework used for responsive web design, ensuring a user-friendly interface.

- jQuery: A JavaScript library that simplifies handling client-side interactions and enhances user experience.

### NuGet Packages:

- Microsoft.EntityFrameworkCore (Version: 7.0.5): Essential for database access and management, providing seamless interaction with the underlying data.

- Microsoft.EntityFrameworkCore.SqlServer (Version: 7.0.5): Specific to SQL Server, this package enhances database connectivity and operations.

- Microsoft.EntityFrameworkCore.Tools (Version: 7.0.5): A set of tools that simplify database migrations and schema management.

- Microsoft.VisualStudio.Web.CodeGeneration.Design (Version: 6.0.15): A design-time package for code generation tasks, streamlining development.

-  Microsoft.Extensions.Logging Version: 7.0.0

- Newtonsoft.Json Verson:13.0.3

### API Integration
-  OpenAI API  Chat completion [https://platform.openai.com/docs/guides/chat]( https://platform.openai.com/docs/guides/chat)

### Graphics Design:

- Google Fonts: [https://fonts.google.com/](https://fonts.google.com/)

- Karlskoga Graphics Manual: [Download PDF](http://intranatet/download/18.387692b115eecfadc7d2ebde/1654699047473/grafisk-manual-karlskoga-kommun-riktlinje.pdf)

### Library of Scalable Vector Icons:

- Font Awesome

### Autenticaion and autorization setup

The application has a login system that is implemented using the AddAuthentication method in the program.cs file. It uses cookie-based authentication and sets the login path to /Account/Login. The expiration time for the user's session is set to 30 minutes, and a sliding expiration is enabled.

The application also has a custom authorization policy that is defined using the AddAuthorization method in the program.cs file. The policy requires that the user is authenticated and has a specific claim called "IsAuthenticated" with a value of "true". This policy is set as the default policy for the application.
With this setup, only authenticated users with the "IsAuthenticated" claim can access the admin sections of the application.
