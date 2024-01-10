. "./ps1/set-location.ps1"
. "./ps1/variable.ps1"

$connectionString = Read-Host "更新対象DBの接続文字列を入力してください"
if([string]::IsNullOrWhiteSpace($connectionString)) {
    exit
}

SetLocation
dotnet ef database update --no-build --verbose --project:$project --startup-project:$startupProject --context:$context --connection:$connectionString
