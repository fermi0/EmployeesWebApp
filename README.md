A simple practice project for integrating EmployeesAPI to EmployeesWebApp using .NET core. This project utilize Backend logic for operations in client-side such as utilizing Data Transfer Object(DTO), repository pattern, filters, pagination and SQL server. This project depends upon EmployeesAPI for Models, StaffDBContext and DTOs.

To run this project, clone the repository and also [EmployeesAPI](https://github.com/fermi6-626/EmployeesAPI.git) repository, open it in Visual Studio or VSCode. You have to add reference from API to the EmployeesWebApp:

>1. Right-click on your solution then select Add>Existing Project
>2. right-click on EmployeesWebApp and select Add>Project Reference>EmployeesAPI

#### Pre-requisite:
	1. NuGet manager
	2. Microsoft.EntityFrameworkCore.SqlServer
	3. Microsoft.EntityFrameworkCore.Design
	4. Microsoft.EntityFrameworkCore.Tools

If those packages didn't add automatically then, from NuGet manager Add those packages to the project.

The API project uses SQL 2022 for database, which works only in LAN environment, you should create a database, and edit the connection string to your IP in API. After creating a database name of your choice, run from the root of your project:

`dotnet ef migrations <name of migration>`

`dotnet ef database update`

The above command runs the migration update to create the database table using the reference in API's Models. The first value are NULL, to edit the table, right-click the Tables folder in the SSMS latest version, and click on **edit top 200 rows**. To show the db tables, click on **select top 1000 rows**. You can use my already created database i.e. Employees.bak in API repo if you prefer. To restore you must have SSMS 20.1.10.0 installed.

Best way to test the web app is to publish the API and run it independently. The web app will call the API via URI, in this case `http://localhost:5000`.

To publish the API, in visual studio right-click on the root of the project i.e. EmployeesAPI and select "publish", and select "Folder". Browse where to publish the API, and click on finish, then close. To edit the settings of publish click on **Show all settings** where you can select self-containment and win-x64, then select the connection string you want to use at runtime and migrate during publish. Click on "Publish" to now publish as an app. In command line you can achieve this with following command(run from the root of API project):

`dotnet publish -r win-x64 -p:PublishSingleFile=true --self-contained true -o 'path\to\output'`

You can now navigate to the folder that the API is deployed, and run it as an exe file. If you want to test it in another machine you can transfer the published folder to the target machine, and install The [.NET Core Hosting Bundle](https://dotnet.microsoft.com/permalink/dotnetcore-current-windows-runtime-bundle-installer) in target machine. Now run the api.exe to run the program. The program runs on `https://localhost:5000`.

Run the web app from visual studio:

`dotnet watch run`

The web app runs on `https://localhost:5001`. If you used my database, the first page should look like this:
![image](https://github.com/fermi6-626/EmployeeWebApp/assets/93081133/53eeebb2-41f3-4334-9161-5dadc8189e9e)


The **Details** page takes the Id from the json and finds the id in API then returns the GetID() function from the API to the webapp via the services>EmployeeService.cs and handled via the HomeController in web app:
![image](https://github.com/fermi6-626/EmployeeWebApp/assets/93081133/86fd2b7b-1f06-4725-b63f-7d28fb91520c)


If you click on **Back to list**, it redirects to homepage. If you click on **Edit**, the page should look like this:
![image](https://github.com/fermi6-626/EmployeeWebApp/assets/93081133/5551041d-9284-4bbd-8837-f853f28613e7)

**Save** will save and redirects back to the Details page.

**Search** performs a database search where name = Fname and only show the searched results:
![image](https://github.com/fermi6-626/EmployeeWebApp/assets/93081133/70f7cffd-36b1-4b26-a74b-652d9a2b7461)

Clicking on **Home** should get you back in the homepage which is a paginated view of all employees. If you click on **Create**, This page should show up:
![image](https://github.com/fermi6-626/EmployeeWebApp/assets/93081133/eff15fbf-984b-4bc3-a26a-cdfaad5a6c2e)

After you fill the forms and click on **Create**, you are redirected to the homepage. There you may have to navigate to next page to view the employee you just created.

**Delete** will ask for confirmation handled via different action method, the page should look like this:
![image](https://github.com/fermi6-626/EmployeeWebApp/assets/93081133/f72958b8-13cf-4515-854a-aee927ebd28f)


Clicking **Delete** will redirect to homepage.

If you want to publish the web app too, you can run the command from the root of the project(EmployeeWebApp):

`dotnet publish -r win-x64 -p:PublishSingleFile=true --self-contained true -o 'path\to\output'`
