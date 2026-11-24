# QuizzR deployment

The following repo is an online quiz platform.  It is written using dotnet with Blazor Webassembly on the front end and SignalR on the backend.  It uses Azure Entra B2C to authenticate admin users (to allow the creation and running of quizzes).

The project consists of...

* /QuizExperiment.Admin - The quiz application (split into 2 parts client and server)
* /Env - Bicep scripts for deploying the resources to Azure to host your own instance
* /github/workflows - Github Actions from deploying the application (and infrastructure)

## Running in your own AD B2C Instance

1. Fork this repo
2. Create an Azure AD B2C instance (see [this](https://docs.microsoft.com/en-us/azure/active-directory-b2c/tutorial-create-tenant) for more info)
3. Following the instructions for configuring your instance to talk to your B2C tenant (see [this for more info](https://learn.microsoft.com/en-us/aspnet/core/blazor/security/webassembly/hosted-with-azure-active-directory-b2c?view=aspnetcore-7.0)), you will only need to configure the client settings in the [appsettings.json](./QuizExperiment.Admin/Client/wwwroot/appsettings.json) file)
4. Create and add Azure credentials to a github secret called AZURE_CREDENTIALS. (see [this](https://docs.microsoft.com/en-us/azure/developer/github/connect-from-azure?tabs=azure-portal%2Cwindows#use-the-azure-login-action-with-a-service-principal-secret) for more info) The user will need to have permisions to create a new resource group, deploy resources to it and push an application to app service.
5. Change the prefix assigned to the resource in [main.bicep](./Env/main.bicep).
6. Update the QRCodeImageUrl and ClientJoinLink in [appsettings.json](./QuizExperiment.Admin/Client/wwwroot/appsettings.json), this is the url to the client application, to generate the QR code I used https://www.the-qrcode-generator.com/
7. Change the LOCATION, RESOURCEGROUP, APPNAME environment variables in [main.bicep](./Env/main.bicep).
8. Update the `ConnectionString` property in the [Server/appsettings.json](./QuizExperiment.Admin/Server/appsettings.json#L11) with your prefix to reference your SignalR resource that will be deployed.
9. Commit the changes, this should cause the action to run and deploy everything
10. Create and present new quizzes using <APPNAME>.azurewebsites.net/manage, players can join using <APPNAME>.azurewebsites.net

## Troubleshooting the Azure welcome page

If App Service shows the default Azure .NET welcome page after a successful deployment, check the startup command under **Configuration > General settings** (or **Settings > Stack settings** in the newer portal). It must be:

```text
dotnet QuizExperiment.Admin.Server.dll
```

The hosted Blazor publish output contains both client and server runtime configurations, so do not rely on App Service to infer the server entry point. The infrastructure and application workflows explicitly set this command. Save the setting and restart the app to apply it to an existing deployment.

The beta workflow deploys to `quizzr-webapp-beta`; the main workflow deploys to `quizzr-webapp`. To redeploy manually, run the corresponding application workflow from **Actions > Run workflow**, selecting the desired branch for beta or `main` for production. Pull request builds do not deploy to production. Both workflows verify that the deployed URL returns the QuizzR page rather than treating ZIP deployment success as proof that the application started.

If the page still does not load, inspect **Log stream** for startup failures and confirm that `QuizExperiment.Admin.Server.dll`, its runtime configuration, and `wwwroot/index.html` are present under `/home/site/wwwroot`.

## How to run a quiz

1. Create a quiz using the `/manage` url.
2. Once created present it using the the link on the `/manage` page or the "Save and Present" option in the editor.
3. Share the presenter screen in the online meeting.
4. Players join using the root (`/`)` url.
5. Once the required number of players have joined, press "Start" on the presenter screen, each question will be presented.  The question will end either when the timer expires or all the registered players have answered.
6. Once all questions have been asked the leaderboard will be shown.
