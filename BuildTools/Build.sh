#!/bin/bash

./UpdateVersion.sh

cd ../c_sharp_json_diff
dotnet build --configuration Release
dotnet pack c_sharp_json_diff.csproj --configuration Release
