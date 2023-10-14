#!/bin/bash
while :
do
	rsync -a /app/ /build/. --exclude bin --exclude obj --delete
	sleep 0.3
done