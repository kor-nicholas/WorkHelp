#!/bin/bash
dotnet publish /p:PublishProfile=site1.PublishSettings
rm -rf bin/Debug/net6.0/publish/host.zip
cd bin/Debug/net6.0/publish
7z a host.zip .
cd ..
cd ..
cd ..
cd ..
