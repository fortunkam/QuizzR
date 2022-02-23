$storageAccount="mfquizstorezcwne2u72u3cw"
$container="quizassets"
$pathToFiles="../QuizExperiment.Admin/Server/Games"

foreach($file in Get-ChildItem $pathToFiles -Filter *.json)
{
    $contents = Get-Content $file | ConvertFrom-Json
    $id=$contents.id
    $userid=$contents.userid
    $title=$contents.title

    az storage blob upload -c $container -f $file -n "$userid/$id/quiz.json" --account-name $storageAccount
    
    az storage blob metadata update -c $container -n "$userid/$id/quiz.json" --account-name $storageAccount --metadata title=$title userid=$userid id=$id

    # Upload to local development storage (azurite)

    az storage blob upload -c $container -f $file -n "$userid/$id/quiz.json" --connection-string "UseDevelopmentStorage=true"

    az storage blob metadata update -c $container -n "$userid/$id/quiz.json" --connection-string "UseDevelopmentStorage=true" --metadata title=$title userid=$userid id=$id
}