#!/bin/sh

dotnet publish -c Release -r linux-x64 /p:PublishTrimmed=true -o ./binaries
rm binaries/Tiler-Linux-amd64
mv ./binaries/Tiler ./binaries/Tiler-Linux-amd64

dotnet publish -c Release -r linux-musl-x64 --self-contained true -p:PublishAot=false -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true /p:PublishTrimmed=true -p:PublishReadyToRun=false -o ./binaries
rm ./binaries/Tiler-Linux-musl-amd64
mv ./binaries/Tiler ./binaries/Tiler-Linux-musl-amd64

dotnet publish -c Release -r linux-arm --self-contained true -p:PublishAot=false -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true /p:PublishTrimmed=true -p:PublishReadyToRun=false -o ./binaries
rm ./binaries/Tiler-Linux-arm32
mv ./binaries/Tiler ./binaries/Tiler-Linux-arm32

dotnet publish -c Release -r linux-arm64 --self-contained true -p:PublishAot=false -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true /p:PublishTrimmed=true -p:PublishReadyToRun=false -o ./binaries
rm ./binaries/Tiler-Linux-arm64
mv ./binaries/Tiler ./binaries/Tiler-Linux-arm64