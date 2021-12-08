// Assign RBAC role for web app to use signalR service

param principalId string
var signalRAppServerRoleDefinitionId = '/subscriptions/44e1dac0-0651-4835-8781-ee7b6e6f238f/providers/Microsoft.Authorization/roleDefinitions/420fcaa2-552c-430f-98ca-3264be4806c7'
var app_to_signalr_role_assignment = '${guid(principalId, signalRAppServerRoleDefinitionId, resourceGroup().id)}'

resource appToSignalRRoleAssignment 'Microsoft.Authorization/roleAssignments@2021-04-01-preview' = {
  name: app_to_signalr_role_assignment
  scope: resourceGroup()
  properties: {
    principalId: principalId
    roleDefinitionId: signalRAppServerRoleDefinitionId
  }
}
