# QuizzR

The following repo is an online quiz platform.  It is written using dotnet with Blazor Webassembly on the front end and SignalR on the backend.

For instructions on how to deploy this to your own environment see the [deploy.md](./deploy.md) file.

The project consists of...

* /QuizExperiment.Admin - The quiz application (split into 2 parts client and server)
* /Env - Bicep scripts for deploying the resources to Azure to host your own instance
* /github/workflows - Github Actions from deploying the application (and infrastructure)

## How to run a quiz

1. Create a quiz using the `/manage` url (you will be prompted to sign in).
2. Once created present it using the the link on the `/manage` page or the "Save and Present" option in the editor.
3. Share the presenter screen in the online meeting.
4. Players join using the root (`/`)` url.
5. Once the required number of players have joined, press "Start" on the presenter screen, each question will be presented.  The question will end either when the timer expires or all the registered players have answered.
6. Once all questions have been asked the leaderboard will be shown.

## What is this project using and why?

### Blazor Web Assembly
The core web app is written in [Blazor Web Assembly](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor).  This means the client side interactions are written in C# instead of Javascript.  For Charting and Animations I am making use of the Blazorise library.

### SignalR
The interactions between the presenter and clients are made possible using [ASP.NET Core SignalR](https://docs.microsoft.com/en-us/aspnet/signalr/overview/getting-started/introduction-to-signalr).  SignalR takes care of setting up either a websocket connection or a long polling connection between the clients and the hub.

### Bicep
The Infrastructure deployment is done using [Bicep](https://docs.microsoft.com/en-us/azure/azure-resource-manager/bicep/overview?tabs=bicep) files.  Bicep is a native technology to Microsoft Azure and generates more readable code than ARM.

### Github Actions
The project contains 2 [github actions](https://github.com/features/actions), one to deploy the infrastructure, the other deploys the app.  

### Azure App Service
By default, the project is hosted on a Linux [App Service](https://docs.microsoft.com/en-us/azure/app-service/) Plan (B1).  Thanks to .NET Core I can take advantage of the cheaper compute sku.

### Azure SignalR Service
The [SignalR service](https://docs.microsoft.com/en-us/azure/azure-signalr/signalr-overview) handles connections between the clients and the hub and is essential in a potentially autoscaled environment as it provides a backplane for the hubs.

### Azure Application Insights
Custom Metrics are logged to Workspace backed [Application Insights](https://docs.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview) component.  This provides useful diagnostic features.

### Azure Storage Account
Quizzes are stored in Azure Blob Storage.  This is a cheap effective way of storing json files.

