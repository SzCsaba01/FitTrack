
#!/bin/bash

# Set project names and .NET version
SOLUTION_NAME="FitTrack"
DOTNET_VERSION="net8.0"
SRC_DIR="src"
TEST_DIR="test"

# Create directories
mkdir -p $SRC_DIR
mkdir -p $TEST_DIR

# Create the solution
dotnet new sln -n $SOLUTION_NAME

# Create projects targeting chosen .NET version
dotnet new webapi -n ${SOLUTION_NAME}.API -o $SRC_DIR/${SOLUTION_NAME}.API --framework $DOTNET_VERSION
dotnet new classlib -n ${SOLUTION_NAME}.Application -o $SRC_DIR/${SOLUTION_NAME}.Application --framework $DOTNET_VERSION
dotnet new classlib -n ${SOLUTION_NAME}.Common -o $SRC_DIR/${SOLUTION_NAME}.Common --framework $DOTNET_VERSION
dotnet new classlib -n ${SOLUTION_NAME}.Domain -o $SRC_DIR/${SOLUTION_NAME}.Domain --framework $DOTNET_VERSION
dotnet new classlib -n ${SOLUTION_NAME}.Infrastructure -o $SRC_DIR/${SOLUTION_NAME}.Infrastructure --framework $DOTNET_VERSION

# Create test project targeting chosen .NET version
dotnet new xunit -n ${SOLUTION_NAME}.Tests -o $TEST_DIR/${SOLUTION_NAME}.Tests --framework $DOTNET_VERSION

# Add projects to solution
dotnet sln add $SRC_DIR/${SOLUTION_NAME}.API/${SOLUTION_NAME}.API.csproj
dotnet sln add $SRC_DIR/${SOLUTION_NAME}.Application/${SOLUTION_NAME}.Application.csproj
dotnet sln add $SRC_DIR/${SOLUTION_NAME}.Common/${SOLUTION_NAME}.Common.csproj
dotnet sln add $SRC_DIR/${SOLUTION_NAME}.Domain/${SOLUTION_NAME}.Domain.csproj
dotnet sln add $SRC_DIR/${SOLUTION_NAME}.Infrastructure/${SOLUTION_NAME}.Infrastructure.csproj
dotnet sln add $TEST_DIR/${SOLUTION_NAME}.Tests/${SOLUTION_NAME}.Tests.csproj

# Add project references
dotnet add $SRC_DIR/${SOLUTION_NAME}.API/${SOLUTION_NAME}.API.csproj reference $SRC_DIR/${SOLUTION_NAME}.Application/${SOLUTION_NAME}.Application.csproj
dotnet add $SRC_DIR/${SOLUTION_NAME}.API/${SOLUTION_NAME}.API.csproj reference $SRC_DIR/${SOLUTION_NAME}.Infrastructure/${SOLUTION_NAME}.Infrastructure.csproj
dotnet add $SRC_DIR/${SOLUTION_NAME}.Application/${SOLUTION_NAME}.Application.csproj reference $SRC_DIR/${SOLUTION_NAME}.Domain/${SOLUTION_NAME}.Domain.csproj
dotnet add $SRC_DIR/${SOLUTION_NAME}.Infrastructure/${SOLUTION_NAME}.Infrastructure.csproj reference $SRC_DIR/${SOLUTION_NAME}.Domain/${SOLUTION_NAME}.Domain.csproj
dotnet add $TEST_DIR/${SOLUTION_NAME}.Tests/${SOLUTION_NAME}.Tests.csproj reference $SRC_DIR/${SOLUTION_NAME}.API/${SOLUTION_NAME}.API.csproj
dotnet add $TEST_DIR/${SOLUTION_NAME}.Tests/${SOLUTION_NAME}.Tests.csproj reference $SRC_DIR/${SOLUTION_NAME}.Application/${SOLUTION_NAME}.Application.csproj
dotnet add $TEST_DIR/${SOLUTION_NAME}.Tests/${SOLUTION_NAME}.Tests.csproj reference $SRC_DIR/${SOLUTION_NAME}.Domain/${SOLUTION_NAME}.Domain.csproj
dotnet add $TEST_DIR/${SOLUTION_NAME}.Tests/${SOLUTION_NAME}.Tests.csproj reference $SRC_DIR/${SOLUTION_NAME}.Infrastructure/${SOLUTION_NAME}.Infrastructure.csproj

echo "Solution $SOLUTION_NAME with $DOTNET_VERSION projects created successfully!"
