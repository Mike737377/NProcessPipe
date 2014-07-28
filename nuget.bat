mkdir .\.builds\nuget
.\tools\NuGet.exe pack src\NProcessPipe\NProcessPipe.csproj -Prop Configuration=Release -OutputDirectory .\.builds\nuget