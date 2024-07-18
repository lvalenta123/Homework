Hash Diff by Luboš Valenta
===

The assignment
---

 - Provide 2 HTTP endpoints that accepts base64-encoded JSON 
 - The provided JSON data needs to be diff-ed and the results shall be available on a third end point
 - The results shall provide the following info in JSON format ( structure is up to you)

Must haves
---

 - [x] Solution written in C# - strongly preferred is to use .NET CORE ( NET 6.0 )
 - [x] Provide a test client (can be command line, integration test …..) to call the API 
 - [x] Documentation in code
 - [x] Describe limitations of the solution

How to run
---

1. Clone the repository
2. Copy db_password.txt from the email to the project root (next to the docker-compose.yml)
3. Run Docker compose with docker-compose.yml
4. Swagger is enabled on the address localhost:8001/swagger

How to run test client
---

1. Navigate to Test client folder in the project root
2. Run HashDiff.TestClient.exe 

Test client
---

Test client is located in the solution root folder under "Test Client" folder. To run it simple execute HashDiff.TestClient.exe.
The test client will build, (re)create, start, and attaches to containers for a service. Then it will guide the user
with the diff process.

Limitations of the solution
---

- The solution requires Docker compose be to installed
- Test client runs into a bug where secrets with DB password are not provided. As a workaround, I've created special Docker compose file. The solution uses correct file with secrets.
- For some reason I've named the application the way I've named it, and I've realized too late.

Improvements
---

- Based on requirements, authorization and authentication
- Based on requirements, it might be benefical to provide both parts in one request without Id
  - That way the API can return it's Id that can be used to pool for the result
- Based on requirements, error handling might differ. Instead of clean use facing API, API could expose, for example, ProblemDetail if it is consumed by other services