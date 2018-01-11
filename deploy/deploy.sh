ApiKey=$1
Source=$2

nuget pack ./api-client-shared/api-client-shared.nuspec -Verbosity detailed

nuget push ./api-client-shared.4.0-netstandardbeta.nupkg -Verbosity detailed -ApiKey $ApiKey -Source $Source