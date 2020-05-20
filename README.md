This is an ASP.NET CORE REST API which will consume the AGL test service and output a response json
.NET Core API is hosted on Azure Cloud.

##  1. .NET Core API

This application connects with external API http://agl-developer-test.azurewebsites.net/people.json to get list of owners with pets. Then it transforms data (using LINQ) and returns JSON response with gender of owners with list of cat names in alphabetical order.

The API documentation is implemented using SWAGGER 

This application has been hosted in Azure App services for demo:

The end points are 

 - GET (gender of owners with list of cat names)
		 - https://aglcatownerserviceramya.azurewebsites.net/api/pets)
- Swagger
		- https://aglcatownerserviceramya.azurewebsites.net/swagger/index.html

It is developed using:

 - .NET Core with Visual Studio 2019
 - MediatR,  CQRS & Dependecy Injection
 - Tests are written using Xunit 
 - Documentation is supported by  SWAGGER**
 - **Logging** for Error, Information and Trace logs.

To run this application on local machine:

 - Clone this solution and run using VS 2019.
 - It will run on localhost using port 17733 - [http://localhost:17733/](http://localhost:17733/)
 - Navigate to URL to get JSON response with gender of owners with list of cat names   - [http://localhost:17733/api/pets](http://localhost:17733/api/pets)
 - Swagger End point - [http://localhost:17733/swagger](http://localhost:17733/swagger)



Json Response : 
[{"ownerGender":"Male","catNames":["Garfield","Jim","Max","Tom"]},{"ownerGender":"Female","catNames":["Garfield","Simba","Tabby"]}]

