#!/bin/bash

/bin/bash /ci/sync-folders.sh &
sleep 5

cd /build

service ssh start
dotnet restore
dotnet $@
