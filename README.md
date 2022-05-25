# V-API
## Requirements
- OS Windows 
- Docker Desktop
- Functional WSL 2
- Visual Studio
## Instructions
- Download or Clone this project 
- In the folder VolvoApi open the terminal of your choise
- Run the following command to orchestrate the containers ```$> docker-compose up -d --build```
- Check if the contaiers are running with the following command ```$> docker ps```
- It shoud return something like this:
 
 ``` 
 $>docker ps
CONTAINER ID   IMAGE               COMMAND                  CREATED         STATUS         PORTS
   NAMES
b5f91785bf07   volvoapi_volvoapi   "dotnet VolvoApi.dll"    7 minutes ago   Up 7 minutes   443/tcp, 0.0.0.0:9009->80/tcp   volvoapi_volvoapi_1
3c596aad2926   adminer             "entrypoint.sh docke…"   7 minutes ago   Up 7 minutes   0.0.0.0:8080->8080/tcp          adminer
4d1266a17efb   postgres            "docker-entrypoint.s…"   7 minutes ago   Up 7 minutes   0.0.0.0:5432->5432/tcp          postgresdb 
```
- open the API Swagger [HERE!](http://localhost:9009/swagger/index.html)

![alt text](https://github.com/jjackbauer/V-API/blob/main/img/VAPISwagger.PNG?raw=true)

- The API Endpoints can be tested in this interface

- To Run the tests, open in the same folder ```VolvoApi.sln```
- In Visual Studio, run the integration tests
