#!/bin/bash

clear

echo "Using .NET Version:-"
dotnet --version
echo ""
echo ""

folderPath="./obj"

# Check if the folder exists before attempting to delete it
if [ -d "$folderPath" ]; then
    echo "Removing temporary build objects within $folderPath"
    rm -r "$folderPath"
else
    echo "$folderPath Does Not Exist"
fi

echo ""
echo ""

dotnet clean repapi-clienthttp.sln
dotnet restore repapi-clienthttp.sln
dotnet build repapi-clienthttp.sln
