dotnet publish -c Release -r osx-x64 --self-contained true -p:PublishAot=false -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true /p:PublishTrimmed=true -p:PublishReadyToRun=false -o binaries
del binaries\Tiler-OSX-x64
ren binaries\Tiler Tiler-OSX-x64

dotnet publish -c Release -r osx-arm64 --self-contained true -p:PublishAot=false -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true /p:PublishTrimmed=true -p:PublishReadyToRun=false -o binaries
del binaries\Tiler-OSX-arm64
ren binaries\Tiler Tiler-OSX-arm64