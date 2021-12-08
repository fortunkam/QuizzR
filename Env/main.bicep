param prefix string = 'mfquiz'

var appInsightsName = '${prefix}-appinsights'
var appPlanName = '${prefix}-appplan'
var appName = '${prefix}-webapp'
var logAnalyticsName = '${prefix}loganalytics'
var signalRName = '${prefix}loganalytics'
var app_to_signalr_role_assignment = '${guid(webApplication.identity.principalId, signalRAppServerRoleDefinitionId, resourceGroup().id)}'

var signalRAppServerRoleDefinitionId = '/subscriptions/44e1dac0-0651-4835-8781-ee7b6e6f238f/providers/Microsoft.Authorization/roleDefinitions/420fcaa2-552c-430f-98ca-3264be4806c7'




// Log Analytics workspace

resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2020-10-01' = {
  name: logAnalyticsName
  location: resourceGroup().location
  properties: {
    sku: {
      name: 'PerGB2018'
    }
  }
}


// App Insights

resource appInsights 'Microsoft.Insights/components@2020-02-02-preview' = {
  name: appInsightsName
  location: resourceGroup().location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: logAnalyticsWorkspace.id
  }
}

// App Service Plan
resource appServicePlan 'Microsoft.Web/serverfarms@2020-12-01' = {
  name: appPlanName
  location: resourceGroup().location
  sku: {
    name: 'S1'
    tier: 'Standard'
    capacity: 1
  }
  kind: 'app'
}


//App Service
resource webApplication 'Microsoft.Web/sites@2018-11-01' = {
  name: appName
  location: resourceGroup().location
  tags: {
    'hidden-related:${resourceGroup().id}/providers/Microsoft.Web/serverfarms/appServicePlan': 'Resource'
  }
  properties: {
    serverFarmId: appServicePlan.id
  }
  identity: {
    type: 'SystemAssigned'
  }
}



resource appSettings 'Microsoft.Web/sites/config@2021-02-01' = {
  name: 'appsettings'
  parent: webApplication
  properties: {
    'APPINSIGHTS_INSTRUMENTATIONKEY': appInsights.properties.InstrumentationKey
    'APPLICATIONINSIGHTS_CONNECTION_STRING' : 'InstrumentationKey=${appInsights.properties.InstrumentationKey};IngestionEndpoint=https://uksouth-1.in.applicationinsights.azure.com/'
    'ApplicationInsightsAgent_EXTENSION_VERSION': '~3'
    'XDT_MicrosoftApplicationInsights_Mode':'Recommended'
    'Azure:SignalR:ConnectionString':'Endpoint=https://${signalR.properties.hostName};AuthType=aad;Version=1.0;'
  }
}


// Add SignalR Service
resource signalR 'Microsoft.SignalRService/signalR@2021-09-01-preview' = {
  name: signalRName
  location: resourceGroup().location
  sku: {
    name: 'Standard_S1'
    tier: 'Standard'
    capacity: 1
  }
  kind: 'SignalR'
  properties: {
    tls: {
      clientCertEnabled: false
    }
    features: [
      {
        flag: 'ServiceMode'
        value: 'Default'
      }
      {
        flag: 'EnableConnectivityLogs'
        value: 'True'
      }
    ]
    cors: {
      allowedOrigins: [
        '*'
      ]
    }
    publicNetworkAccess: 'Enabled'
  }
}

output webApplicationPrincipalId string = webApplication.identity.principalId

