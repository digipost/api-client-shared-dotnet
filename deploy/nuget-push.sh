betaSuffix="beta"
masterSuffix="master"
if [[ ${TRAVIS_BRANCH} == $betaSuffix ]] || [[ ${TRAVIS_BRANCH} == $masterSuffix ]];then
	echo "Is on beta or master branch, deploying Nuget package ..."
	mono nuget.exe push *.nupkg -Verbosity detailed -ApiKey $NUGET_API_KEY -Source https://api.nuget.org/v3/index.json
else
	echo "Is not on beta or master branch, skipping deploy."
fi