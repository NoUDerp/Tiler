dotnet publish -r win-x64 -c Release -f netcoreapp3.0
copy bin\Release\netcoreapp3.0\win-x64\publish\Tiler.exe binaries\Tiler-Windows-x64.exe /y

dotnet publish -r win-x86 -c Release -f netcoreapp3.0
copy bin\Release\netcoreapp3.0\win-x86\publish\Tiler.exe binaries\Tiler-Windows-x86.exe /y

dotnet publish -r linux-x64 /p:PublishReadyToRun=false -c Release -f netcoreapp3.0
copy bin\Release\netcoreapp3.0\linux-x64\publish\Tiler binaries\Tiler-Linux-x64 /y

dotnet publish -r linux-arm64 -c Release /p:PublishReadyToRun=false  -f netcoreapp3.0
copy bin\Release\netcoreapp3.0\linux-arm64\publish\Tiler binaries\Tiler-Linux-arm64 /y

dotnet publish -r linux-musl-x64 /p:PublishReadyToRun=false -c Release -f netcoreapp3.0
copy bin\Release\netcoreapp3.0\linux-musl-x64\publish\Tiler binaries\Tiler-Linux-musl-x64 /y

dotnet publish -r linux-arm -c Release /p:PublishReadyToRun=false  -f netcoreapp3.0
copy bin\Release\netcoreapp3.0\linux-arm\publish\Tiler binaries\Tiler-Linux-arm32 /y

dotnet publish -r osx-x64 -c Release /p:PublishReadyToRun=false -f netcoreapp3.0
copy bin\Release\netcoreapp3.0\osx-x64\publish\Tiler binaries\Tiler-OSX-x64 /y