
Push-Location Maybe

nuget pack -Build -Prop Configuration=Release -Output ..\..\..\LocalNugetFeed

Pop-Location

Push-Location Maybe.Serialization.JsonNet

nuget pack -Build -Prop Configuration=Release -Output ..\..\..\LocalNugetFeed

Pop-Location
