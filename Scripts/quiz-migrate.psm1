# Move quizzes from from user to another
function Copy-Quiz {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory = $true)]
        [string]$StorageAccountName,

        [Parameter(Mandatory = $true)]
        [string]$SourceUserId,

        [Parameter(Mandatory = $true)]
        [string]$DestinationUserId,

        [Parameter(Mandatory = $true)]
        [string]$SourceQuizId
    )

    $blobContainer = "quizassets"
    $destinationQuizId = (New-Guid).Guid
    $sourceBlob = "$SourceUserId/$SourceQuizId/quiz.json"
    $destinationBlob = "$DestinationUserId/$destinationQuizId/quiz.json"

    Write-Host "Copy $sourceBlob to $destinationBlob"

    az storage blob download `
        --account-name $StorageAccountName `
        --name $sourceBlob `
        --container-name $blobContainer `
        --file "./quiz.json" `
        --auth-mode login

    $quizFile = Get-Content "./quiz.json" | ConvertFrom-Json -Depth 5
    $quizFile.id = $destinationQuizId
    $quizFile.userid = $DestinationUserId

    $quizFile | ConvertTo-Json -Depth 5 | Out-File -FilePath "./quiz.json"

    az storage blob upload `
        --account-name $StorageAccountName `
        --name $destinationBlob `
        --container-name $blobContainer `
        --file "./quiz.json" `
        --auth-mode login

    $existingMetadata=$(az storage blob metadata show `
        --account-name $StorageAccountName `
        --container-name $blobContainer `
        --name $sourceBlob `
        --auth-mode login) | ConvertFrom-Json

    $questionCount = $existingMetadata.Questioncount
    $title = $existingMetadata.Title
    
    az storage blob metadata update `
        --account-name $StorageAccountName `
        --container-name $blobContainer `
        --name $destinationBlob `
        --metadata id=$destinationQuizId userid=$DestinationUserId title=$title questioncount=$questionCount `
        --auth-mode login

    Remove-Item -Path "./quiz.json" -Force


}

# Export the function as a module
Export-ModuleMember -Function Copy-Quiz

function Copy-Quizzes {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory = $true)]
        [string]$StorageAccountName,

        [Parameter(Mandatory = $true)]
        [string]$SourceUserId,

        [Parameter(Mandatory = $true)]
        [string]$DestinationUserId
    )

    $blobContainer = "quizassets"

    $quizzes = $(az storage blob list `
        --account-name $StorageAccountName `
        --container-name $blobContainer `
        --prefix $SourceUserId `
        --query "[].name" `
        --out tsv `
        --auth-mode login)

    foreach ($quiz in $quizzes) {       
        $parts = $quiz -split "/"
        $sourceQuizId = $parts[1]

        Write-Host "Source User: $SourceUserId, SourceQuizId: $sourceQuizId"

        Copy-Quiz -StorageAccountName $StorageAccountName `
            -SourceUserId $SourceUserId `
            -DestinationUserId $DestinationUserId `
            -SourceQuizId $sourceQuizId

    }


}

# Export the function as a module
Export-ModuleMember -Function Copy-Quizzes

# Define the function to list users from Azure AD B2C
function Get-AzureADB2CUsers {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory = $true)]
        [string]$TenantId
    )

    az login -t "$TenantId" --allow-no-subscriptions 

    # Get the list of users
    az ad user list --query "[].{objectid: objectId, displayName:displayName, issuer: userIdentities[0].issuer }" -o json 
}

# Export the function as a module
Export-ModuleMember -Function Get-AzureADB2CUsers


###########################
# Sample Usage...

# Needs logged in user
# Copy-Quiz -StorageAccountName "<STORAGE_ACCOUNT_NAME>" -SourceUserId "<SOURCE_USER_OBJECT_ID>" -DestinationUserId "<DESTINATION_USER_OBJECT_ID>" -SourceQuizId "<SOURCE_QUIZ_ID>"

# Needs logged in user
# Copy-Quizzes -StorageAccountName "<STORAGE_ACCOUNT_NAME>" -SourceUserId "<SOURCE_USER_OBJECT_ID>" -DestinationUserId "<DESTINATION_USER_OBJECT_ID>"  

# Will log into B2C tenant
# Get-AzureADB2CUsers -TenantId "<B2C_TENANT_ID>"