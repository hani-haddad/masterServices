#!/bin/bash

# Continuously synchronize folders (excluding bin and obj directories)
while :
do
    rsync -a /app/AuthProject/ /build/AuthProject/AuthProject --exclude bin --exclude obj --delete

    # Sync the SharedModelNamespace folder
    rsync -a /app/SharedModelNamespace/ /build/SharedModelNamespace/SharedModelNamespace --exclude bin --exclude obj --delete

	sleep 0.3
done
