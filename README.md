# App - "Star News" (MAUI + MVVM + OData + OpenAI)
---
Application for publishing and reading news. 

The application features a comprehensive role-based system. It includes three primary roles: Common User, Writer, and Administrator. Each role is equipped with a unique set of rights and capabilities. Given the presence of a role system, the application also incorporates an authorization system on the server-side. Upon signing in, the application preserves the user's context (API Json Web Token), which refreshes every three hours upon reentry. 

You can find screenshots of the application below along with instructions on how to start up the project.

> [!NOTE]
> App wasn't localized into English, screens with localized English UI - only for preview

## Application architecture
![image](https://github.com/MVasili34/maui-news-app/assets/117523384/910b26fb-d332-409a-8b0c-dda4bd19d2f6)

## Some screenshots
![image](https://github.com/MVasili34/maui-star-news/assets/117523384/e6598700-511a-41d2-aead-d35c37ffba54)

## Local deployment
> [!IMPORTANT]
> Before start up App be sured that this list of software is installed and running

### Preparations
- Latest version of [Visual Studio 2022](https://visualstudio.microsoft.com/);
- Be shured that [.NET 7](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) is installed (there were plans to update project to .NET 8 but canceled);
- Download and and keep running [Docker Desktop](https://www.docker.com/products/docker-desktop/);
- Clone GitHub repo: https://github.com/MVasili34/maui-star-news/

### Run backend services
- Firstly, build API docker image from the solution folder:

```cli
 docker build -f API/ArticleODataAPI/Dockerfile . -t odataapi
```

- Secondly, create docker-compose from the folder containing <a href="API/ArticleODataAPI">API project</a>:

```cli
 docker-compose up
```
API will be running on http://localhost:8080.

> [!WARNING]
> The application has been initialy configured to work in the local network with opened ports and that's why in Android manifest file IP address is not 10.0.0.2. Please, changed its value to 10.0.0.2 if you want to run this app on emulated device and also don't forget to change > this settings in <a href="MobileApp/NewsMobileApp/MauiProgram.cs">configuration file</a> of .NET MAUI App.

> [!NOTE]
> Please note, that some app functions like Imgur API or OpenAI may required your personal service key.
> You can change Imgur API Key in <a href="MobileApp/NewsMobileApp/MauiProgram.cs">configuration file</a> of .NET MAUI App.

## Extra Content
You can read full article about this project and understand why some controversial decisions were made.
