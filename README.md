# QuizExperiment

The following repo is an online quiz platform.  It is written using dotnet with Blazor Webassembly on the front end and SignalR on the backend.

The project consists of...

* /QuizExperiment.Admin - The quiz application (split into 2 parts client and server)
* /Env - Bicep scripts for deploying the resources to Azure to host your own instance
* /github/workflows - Github Actions from deploying the application (and infrastructure)

## Running in your own AD tenant

1. Fork this repo
2. Create and add Azure credentials to a github secret called AZURE_CREDENTIALS. (see [this](https://docs.microsoft.com/en-us/azure/developer/github/connect-from-azure?tabs=azure-portal%2Cwindows#use-the-azure-login-action-with-a-service-principal-secret) for more info) The user will need to have permisions to create a new resource group, deploy resources to it and push an application to app service.
3. Set up and configure the application for AD authentication (see [this](https://docs.microsoft.com/en-us/aspnet/core/blazor/security/webassembly/hosted-with-azure-active-directory?view=aspnetcore-6.0#client-app-configuration) for more info, you will only need to configure the client settings in the [appsettings.json](./QuizExperiment.Admin/Client/wwwroot/appsettings.json) file)
4. Change the prefix assigned to the resource in [main.bicep](./Env/main.bicep).
5. Update the QRCodeImageUrl and ClientJoinLink in [appsettings.json](./QuizExperiment.Admin/Client/wwwroot/appsettings.json), this is the url to the client application, to generate the QR code I used https://www.the-qrcode-generator.com/
6. Change the LOCATION, RESOURCEGROUP, APPNAME environment variables in [main.bicep](./Env/main.bicep).
7. Commit the changes, this should cause the action to run and deploy everything
8. Create and present new quizzes using <APPNAME>.azurewebsites.net/manage, players can join using <APPNAME>.azurewebsites.net

## How to run a quiz

1. Create a quiz using the `/manage` url.
2. Once created present it using the the link on the `/manage` page or the "Save and Present" option in the editor.
3. Share the presenter screen in the online meeting.
4. Players join using the root (`/`)` url.
5. Once the required number of players have joined, press "Start" on the presenter screen, each question will be presented.  The question will end either when the timer expires or all the registered players have answered.
6. Once all questions have been asked the leaderboard will be shown.