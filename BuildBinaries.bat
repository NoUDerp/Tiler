dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true /p:PublishTrimmed=true -p:PublishReadyToRun=false -o binaries
del binaries\Tiler-Windows-x64.exe
ren binaries\Tiler.exe Tiler-Windows-x64.exe
dotnet publish -c Release -r win-x86 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true /p:PublishTrimmed=true -p:PublishReadyToRun=false -o binaries
del binaries\Tiler-Windows-x86.exe
ren binaries\Tiler.exe Tiler-Windows-x86.exe
dotnet publish -c Release -r win-arm --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true /p:PublishTrimmed=true -p:PublishReadyToRun=false -o binaries
del binaries\Tiler-Windows-arm.exe
ren binaries\Tiler.exe Tiler-Windows-arm.exe
dotnet publish -c Release -r win-arm64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true /p:PublishTrimmed=true -p:PublishReadyToRun=false -o binaries
del binaries\Tiler-Windows-arm64.exe
ren binaries\Tiler.exe Tiler-Windows-arm64.exe
dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true /p:PublishTrimmed=true -p:PublishReadyToRun=false -o binaries
del binaries\Tiler-Linux-x64
ren binaries\Tiler Tiler-Linux-x64
dotnet publish -c Release -r linux-musl-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true /p:PublishTrimmed=true -p:PublishReadyToRun=false -o binaries
del binaries\Tiler-Linux-musl-x64
ren binaries\Tiler Tiler-Linux-musl-x64
dotnet publish -c Release -r linux-arm --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true /p:PublishTrimmed=true -p:PublishReadyToRun=false -o binaries
del binaries\Tiler-Linux-arm32
ren binaries\Tiler Tiler-Linux-arm32
dotnet publish -c Release -r linux-arm64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true /p:PublishTrimmed=true -p:PublishReadyToRun=false -o binaries
del binaries\Tiler-Linux-arm64
ren binaries\Tiler Tiler-Linux-arm64
dotnet publish -c Release -r osx-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true /p:PublishTrimmed=true -p:PublishReadyToRun=false -o binaries
del binaries\Tiler-OSX-x64
ren binaries\Tiler Tiler-OSX-x64
