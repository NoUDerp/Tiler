dotnet publish -c Release -r win-x64 /p:PublishTrimmed=true -o binaries
del binaries\Tiler-Windows-x64.exe
ren binaries\Tiler.exe Tiler-Windows-x64.exe

dotnet publish -c Release -r win-x86 --self-contained true -p:PublishAot=false -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true /p:PublishTrimmed=true -p:PublishReadyToRun=false -o binaries
del binaries\Tiler-Windows-x86.exe
ren binaries\Tiler.exe Tiler-Windows-x86.exe

dotnet publish -c Release -r win-arm64 /p:PublishTrimmed=true -o binaries
del binaries\Tiler-Windows-arm64.exe
ren binaries\Tiler.exe Tiler-Windows-arm64.exe

dotnet publish -c Windows7 -r win-x86 --self-contained true -p:PublishSingleFile=true -p:PublishAot=false -p:IncludeNativeLibrariesForSelfExtract=true /p:PublishTrimmed=true -p:PublishReadyToRun=false -o binaries
del binaries\Tiler-Windows7-x86.exe
ren binaries\Tiler.exe Tiler-Windows7-x86.exe

dotnet publish -c Release -r win-arm --self-contained true -p:PublishSingleFile=true -p:PublishAot=false -p:IncludeNativeLibrariesForSelfExtract=true /p:PublishTrimmed=true -p:PublishReadyToRun=false -o binaries
del binaries\Tiler-Windows-arm.exe
ren binaries\Tiler.exe Tiler-Windows-arm.exe

dotnet publish -c Windows7 -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishAot=false -p:IncludeNativeLibrariesForSelfExtract=true /p:PublishTrimmed=true -p:PublishReadyToRun=false -o binaries
del binaries\Tiler-Windows7-x64.exe
ren binaries\Tiler.exe Tiler-Windows7-x64.exe