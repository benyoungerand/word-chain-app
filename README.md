An application used to process the dictionary given as part of the word-chain challange into a json file with relationships (JSON Formatter project).

This JSON was then used to populate a neo4j database - which was then called to find the shortest path.

Please ask Ben for variables needed in Commands.cs

Below is a visualisation of how data is stored in the database, I just thought it looked pretty cool
![image](https://user-images.githubusercontent.com/91882366/149289642-29e46fee-340a-4145-8159-2974528d3416.png)


### Running

The .NET application is a wrapper used to query the neo4j database.
1. Get credentials from Ben to put into the variables at the top of Commands.cs
2. Set AdamsCodeChallange.ConsoleApp as the startup application in Visual Studio
3. Run the application!

