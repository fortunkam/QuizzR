param prefix string = 'mfquiz'

var appInsightsName = '${prefix}-appinsights'
var appPlanName = '${prefix}-linux-appplan'
var appName = '${prefix}-webapp'
var betaAppName = '${prefix}-webapp-beta'
var logAnalyticsName = '${prefix}loganalytics'
var signalRName = '${prefix}signalr'
var questionSetTableName = 'questionsets'
param storageAccountName string = '${prefix}store${uniqueString(resourceGroup().id)}'

param workbookDisplayName string = 'Quiz Analysis Workbook'
var workbookType = 'workbook'
param workbookName string = '5ebb021f-9af7-4d88-8a01-4e1e1ab81402'
param location string = resourceGroup().location

@description('The Runtime stack of current web app')
param linuxFxVersion string = 'DOTNETCORE|9.0'
param linuxFxBetaVersion string = 'DOTNETCORE|9.0'

@secure()
param giphyApiKey string

@secure()
param openAIDeploymentName string

@secure()
param openAIEndpoint string

@secure()
param openAIKey string

var workbookContent = loadTextContent('workbookcontent.json')

@allowed([
  'Premium_LRS'
  'Premium_ZRS'
  'Standard_GRS'
  'Standard_GZRS'
  'Standard_LRS'
  'Standard_RAGRS'
  'Standard_RAGZRS'
  'Standard_ZRS'
])
param storageAccountType string = 'Standard_LRS'

// Log Analytics workspace

resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2020-10-01' = {
  name: logAnalyticsName
  location: location
  properties: {
    sku: {
      name: 'PerGB2018'
    }
  }
}


// App Insights

resource appInsights 'Microsoft.Insights/components@2020-02-02-preview' = {
  name: appInsightsName
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: logAnalyticsWorkspace.id
  }
}

// App Service Plan
resource appServicePlan 'Microsoft.Web/serverfarms@2021-03-01' = {
  name: appPlanName
  location: location
  sku: {
    name: 'B1'
    tier: 'Basic'
    size: 'B1'
    family: 'B'
    capacity: 1
  }
  kind: 'linux'
  properties: {
    reserved: true
  }
}


//App Service
resource webApplication 'Microsoft.Web/sites@2021-03-01' = {
  name: appName
  location: location
  tags: {
    'hidden-related:${resourceGroup().id}/providers/Microsoft.Web/serverfarms/appServicePlan': 'Resource'
  }
  properties: {
    httpsOnly: true
    serverFarmId: appServicePlan.id
    siteConfig: {
      linuxFxVersion: linuxFxVersion
    }
  }
  identity: {
    type: 'SystemAssigned'
  }
}

//App Service (Beta)
resource webApplicationBeta 'Microsoft.Web/sites@2021-03-01' = {
  name: betaAppName
  location: location
  tags: {
    'hidden-related:${resourceGroup().id}/providers/Microsoft.Web/serverfarms/appServicePlan': 'Resource'
  }
  properties: {
    httpsOnly: true
    serverFarmId: appServicePlan.id
    siteConfig: {
      linuxFxVersion: linuxFxBetaVersion
    }
  }
  identity: {
    type: 'SystemAssigned'
  }
}



resource appSettings 'Microsoft.Web/sites/config@2021-03-01' = {
  name: 'appsettings'
  parent: webApplication
  properties: {
    'APPINSIGHTS_INSTRUMENTATIONKEY': appInsights.properties.InstrumentationKey
    'APPLICATIONINSIGHTS_CONNECTION_STRING' : 'InstrumentationKey=${appInsights.properties.InstrumentationKey};IngestionEndpoint=https://uksouth-1.in.applicationinsights.azure.com/'
    'ApplicationInsightsAgent_EXTENSION_VERSION': '~3'
    'XDT_MicrosoftApplicationInsights_Mode':'Recommended'
    'Azure__SignalR__ConnectionString':'Endpoint=https://${signalR.properties.hostName};AuthType=aad;Version=1.0;'
    'Azure__Storage__ConnectionString':'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};AccountKey=${listKeys(storageAccount.id,storageAccount.apiVersion).keys[0].value}'
    'QuizAssetsContainerName':'quizassets'
    'Giphy__ApiKey': giphyApiKey
    'OpenAI__Endpoint': openAIEndpoint
    'OpenAI__Key': openAIKey
    'OpenAI__DeploymentName': openAIDeploymentName
  }
}

resource appSettingsBeta 'Microsoft.Web/sites/config@2021-03-01' = {
  name: 'appsettings'
  parent: webApplicationBeta
  properties: {
    'APPINSIGHTS_INSTRUMENTATIONKEY': appInsights.properties.InstrumentationKey
    'APPLICATIONINSIGHTS_CONNECTION_STRING' : 'InstrumentationKey=${appInsights.properties.InstrumentationKey};IngestionEndpoint=https://uksouth-1.in.applicationinsights.azure.com/'
    'ApplicationInsightsAgent_EXTENSION_VERSION': '~3'
    'XDT_MicrosoftApplicationInsights_Mode':'Recommended'
    'Azure__SignalR__ConnectionString':'Endpoint=https://${signalR.properties.hostName};AuthType=aad;Version=1.0;'
    'Azure__Storage__ConnectionString':'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};AccountKey=${listKeys(storageAccount.id,storageAccount.apiVersion).keys[0].value}'
    'QuizAssetsContainerName':'quizassets'
    'Giphy__ApiKey': giphyApiKey
    'OpenAI__Endpoint': openAIEndpoint
    'OpenAI__Key': openAIKey
    'OpenAI__DeploymentName': openAIDeploymentName
  }
}


// Add SignalR Service
resource signalR 'Microsoft.SignalRService/signalR@2021-09-01-preview' = {
  name: signalRName
  location: location
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

resource storageAccount 'Microsoft.Storage/storageAccounts@2021-06-01' = {
  name: storageAccountName
  location: location
  sku: {
    name: storageAccountType
  }
  kind: 'StorageV2'
  properties: {}
}

resource blobservices 'Microsoft.Storage/storageAccounts/blobServices@2021-08-01' = {
  name: 'default'
  parent: storageAccount
  properties: {
    cors: {
      corsRules: [
      ]
    }
  }
}

resource tableservices 'Microsoft.Storage/storageAccounts/tableServices@2021-08-01' = {
  name: 'default'
  parent: storageAccount
  properties: {
    cors: {
      corsRules: [
      ]
    }
  }
}

resource storageAssetsContainer 'Microsoft.Storage/storageAccounts/tableServices/tables@2021-08-01' = {
  name: questionSetTableName
  parent: tableservices
}

resource funcworkbook 'Microsoft.Insights/workbooks@2021-08-01' = {
  name: workbookName
  location: location
  kind: 'shared'
  properties: {
    category: workbookType
    displayName: workbookDisplayName
    serializedData: workbookContent
    sourceId: appInsights.id
  }
}

output webApplicationPrincipalId string = webApplication.identity.principalId

