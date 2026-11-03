$ErrorActionPreference = 'Stop'
$env:DOTNET_CLI_TELEMETRY_OPTOUT = '1'
dotnet restore "$PSScriptRoot/Track1.sln" --configfile "$PSScriptRoot/NuGet.Config"
if ($LASTEXITCODE -ne 0) { throw 'Restore failed' }
dotnet build "$PSScriptRoot/Track1.sln" --configuration Release --no-restore --nologo
if ($LASTEXITCODE -ne 0) { throw 'Build failed' }
Get-ChildItem -LiteralPath $PSScriptRoot -Filter *.csproj -Recurse | Sort-Object FullName | ForEach-Object {
    Write-Host $_.Directory.FullName
    dotnet run --project $_.FullName --configuration Release --no-build
    if ($LASTEXITCODE -ne 0) { throw "Checks failed: $($_.FullName)" }
}
