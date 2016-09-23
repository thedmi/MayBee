
Push-Location MayBee

nuget pack MayBee.csproj -Build -Prop Configuration=Release -Output ..\..\..\LocalNugetFeed

Pop-Location

Push-Location MayBee.Serialization.JsonNet

nuget pack MayBee.Serialization.JsonNet.csproj -Build -Prop Configuration=Release -Output ..\..\..\LocalNugetFeed

Pop-Location
