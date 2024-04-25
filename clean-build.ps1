Clear-Host

$folderPath = ".\obj"

# Check if the folder exists before attempting to delete it
if (Test-Path $folderPath -PathType Container) {
    Write-Host "Removing temporary build objects within $folderPath"
    Remove-Item -Path $folderPath -Recurse -Force
}
else {
    Write-Host "$folderPath Does Not Exists"
}

Write-Host ""
Write-Host ""

dotnet clean repapi-clienthttp.sln
dotnet restore repapi-clienthttp.sln
dotnet build repapi-clienthttp.sln
