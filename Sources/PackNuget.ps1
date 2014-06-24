
Push-Location Maybe

nuget pack -Build -Prop Configuration=Release 

Pop-Location

Push-Location Maybe.Serialization.JsonNet

nuget pack -Build -Prop Configuration=Release 

Pop-Location
