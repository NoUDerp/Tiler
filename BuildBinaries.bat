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

REM dotnet publish -c Release -r linux-x64 /p:PublishTrimmed=true -o binaries
REM del binaries\Tiler-Linux-amd64
REM ren binaries\Tiler Tiler-Linux-amd64

REM dotnet publish -c Release -r linux-musl-x64 /p:PublishTrimmed=true -o binaries
REM del binaries\Tiler-Linux-musl-amd64
REM ren binaries\Tiler Tiler-Linux-musl-amd64
REM dotnet publish -c Release -f netcoreapp3.1 -r linux-x86 --self-contained true -p:PublishAot=false -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true /p:PublishTrimmed=true -p:PublishReadyToRun=false -o binaries
REM del binaries\Tiler-Linux-x86
REM ren binaries\Tiler Tiler-Linux-x86
REM dotnet publish -c Release -r linux-arm /p:PublishTrimmed=true -o binaries
REM del binaries\Tiler-Linux-arm32
REM ren binaries\Tiler Tiler-Linux-arm32
REM dotnet publish -c Release -r linux-arm64 /p:PublishTrimmed=true -o binaries
REM del binaries\Tiler-Linux-arm64
REM ren binaries\Tiler Tiler-Linux-arm64
REM dotnet publish -c Release -r osx-x64 /p:PublishTrimmed=true -o binaries
REM del binaries\Tiler-OSX-x64
REM ren binaries\Tiler Tiler-OSX-x64
REM dotnet publish -c Release -r osx-arm64 /p:PublishTrimmed=true -o binaries
REM del binaries\Tiler-OSX-arm64
REM ren binaries\Tiler Tiler-OSX-arm64

