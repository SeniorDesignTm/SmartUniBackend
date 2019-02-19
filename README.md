# SmartUniBackend


To have it working you have to recreate the database 

go to the command line type: dotnet ef migrations add InitialCreate then dotnet ef database update
once the database is made take the connection string and put it in the DataContext file at line 21 (Helper\DataContext.cs)

If all of that worked fine it means you have now a ms sql database on your computer 

and to initialize it uncomment the lines that starts from 40 to 60 in the UserService (Services\UserService.cs)

and then run the SmartUniTest from the green play icon (not the ISS Express).
