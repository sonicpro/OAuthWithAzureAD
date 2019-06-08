Demo for using access tokens for the client_credentials flow issued by Azure AD
============================================================


Sources and the explanation are here: https://github.com/Azure-Samples/active-directory-dotnet-daemon


The solution consists of Web API 2 service (TodoListService.NETFramework) and a console daemon app (TodoListDaemon) that consumes the service.<sup>[1](#f1)</sup>

***
######<small id="f1">1</small> Instructions for configuring "TodoListDaemon" application.
Put the key from "ClientSecret.gpg" into AppKey setting in the App.config.

