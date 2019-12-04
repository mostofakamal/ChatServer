# ChatServer
A real time chat Server

> CI : [![Build status](https://dev.azure.com/mostofacefalo/Test/_apis/build/status/ChatServer-CI)](https://dev.azure.com/mostofacefalo/Test/_build/latest?definitionId=1)

> [Azure Link]( https://chatserver-prod.azurewebsites.net)

## Description:

It supports real time chatting in groups. User can Create, List groups and then connect to a group and send message. 
Users has to register and be autheticated beforehand.

## Technology/Practices used

* ASPNET Core Web API with C#
* Swagger Api Documentation
* EntityFramework Core as ORM with Code first Migration
* MSSQL as DB Sotrage
* JWT token-based authetication with ASPNET Core Identity
* SignalR for real-time chatting functionality
* mediatR for implementation of mediator
* Repository and Specification pattern for db operations
* Azure Devops CI-CD integration 

## Solution architecture/ Description

> WebApi.Core

The solution contains a project named WebApi.Core which contains the domain and contracts for the solutions. I have used generic repository with specification pattern for db operations. For request response handling i have used the mediatR where requests are handled as command and the queries. Each of them are associated with respective handlers. This way the responsibilities are more clean and seperate.

> WebApi.Infrastructure

WebApi.Infrastructure contains the solution infrastructure related concerns e.g DB access, Authentication, Mapping , Identity User management.
It contains the implementations of the interfaces in WebApi.Core project. 
I have used EF Core Code first migration approach. 

For Authentication i have used JWT token claim-based authentication approach which is integrated as Bearer Token in the API for Authorized endpoints.

> ChatServer

It is the ASP.NET Core Web API acting as a chat server.Here i have used signalR for real time communication. 
The swagger documentation of the Web API contains all the details of the endpoints and request response.
Authorized endpoints require 'Bearer <token>' in the header of the request. 


This is hosted on Azure Web App service in the link provided above

This consumer of this Chat Server is a ChatClient which is a seperate Angular app and can be found [here](https://chatclient-prod.azurewebsites.net) . Repo Url: https://github.com/mostofakamal/ChatServerClient

## Performance optimization/ Scale out scope:

Since the Chat server is using SignalR therefore for more traffic we will require to scale out the app service but that will cause problem regarding signalR connection because then there will be mutiple servers so user connected to a specific server will not be in sync with others. To solve the issue we can follow two approaches:
* Azure SignalR Service
* Redis backplane 
Details of the process can be found in this [docs](https://docs.microsoft.com/en-us/aspnet/core/signalr/scale?view=aspnetcore-3.0) .
The User and token management can be extracted to a seperate application service e.g IdentityServer/ AuthServer. 




