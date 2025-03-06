#!/bin/bash

# Ensure script runs in bash or zsh
if [[ -z "$BASH_VERSION" && -z "$ZSH_VERSION" ]]; then
    echo "Please run this script using bash or zsh."
    exit 1
fi

# Check if required commands are available
check_command() {
    command -v "$1" >/dev/null 2>&1 || { echo "Error: $1 is not installed."; exit 1; }
}

check_command node
check_command npm
check_command dotnet

# Navigate to backend directory (update if needed)
echo "Setting up backend..."
cd backend || { echo "Backend directory not found!"; exit 1; }

dotnet restore || { echo "Failed to restore .NET dependencies."; exit 1; }

# Ensure dotnet-ef is installed locally
dotnet tool install --local dotnet-ef || echo "dotnet-ef already installed locally."
dotnet tool restore || { echo "Failed to restore .NET tools."; exit 1; }

# Apply migrations using the locally installed tool
dotnet tool run dotnet-ef database update --project BackendNotes/BackendNotes.csproj || { echo "Failed to apply database migrations."; exit 1; }

cd BackendNotes

dotnet run --urls "https://localhost:7031" &
BACKEND_PID=$!

cd ..
cd ..

# Navigate to frontend directory (update if needed)
echo "Setting up frontend..."

cd frontend || { echo "Frontend directory not found!"; exit 1; }

npm install || { echo "Failed to install frontend dependencies."; exit 1; }
ng serve &
FRONTEND_PID=$!

# Wait for processes to keep the script running
echo "App is running. Press Ctrl+C to stop."
wait $BACKEND_PID $FRONTEND_PID
