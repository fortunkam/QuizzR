Need to install main.bicep first, then use the generated principalId (output parameter) in rbac.bicep.
By default the workflow does not have permissions (currently contributor, needs owner) to run rbac.bicep so you will need to do the assignment manually 

This is because the role assignment name is built from the principalId, definitionId and resourceGroupId and cannot be determined in the same file (since the principal id doesn't exist until the wep app is created)

Deployment needs the Azure Credentials stored as a Github Secret
https://docs.microsoft.com/en-us/dotnet/architecture/devops-for-aspnet-developers/actions-deploy

az ad sp create-for-rbac --name "github_actions_quiz_role" --sdk-auth --role contributor