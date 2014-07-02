
Push-Location MayBee

nuget pack -Build -Prop Configuration=Release -Output ..\..\..\LocalNugetFeed

Pop-Location

Push-Location MayBee.Serialization.JsonNet

nuget pack -Build -Prop Configuration=Release -Output ..\..\..\LocalNugetFeed

Pop-Location
