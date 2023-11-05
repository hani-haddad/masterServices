#!/bin/bash
echo MYPATH
pwd

# # Start synchronization script in the background
/bin/bash /CRUDAPI/ci/sync-folders.sh &

# Wait briefly for synchronization to start
sleep 5

# Start SSH (if needed)
service ssh start


# Change to the build directory
cd /build/CRUDAPI

# Restore and run your application
dotnet restore
dotnet run
