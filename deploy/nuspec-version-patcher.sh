#Find current assembly version
# 1: Zero29 lists versions of all AssemblyInfo-files. There is two.
# 2: Take the first one
# 3: Grep with regex to retrieve the assembly version number (ex. '4.3.0.0') at end of line and excluding build number,
#    ending up with '4.3.0.'
assemblyVersionWithoutBuildNumber=$(mono ./Zero29.1.0.0/tools/Zero29.exe -l | head -n 1 | egrep -o '([0-9].){3}')

if [ ${#assemblyVersionWithoutBuildNumber} -eq "0" ];then
	echo "Did not find assembly version with version patcher. Please check that patcher is installed correctly and that it can find the assembly version files. Exiting!" >&2 #Echo and send to stderr
	exit 1 # terminate and indicate error
fi

echo "Assembly version found with version patcher is '${assemblyVersionWithoutBuildNumber}' (build number excluded)."

assemblyVersion="${assemblyVersionWithoutBuildNumber}${TRAVIS_BUILD_NUMBER}"

betaSuffix="beta"
if [ ${TRAVIS_BRANCH} == $betaSuffix ];then
	echo "Is on beta branch, setting '-${betaSuffix}' as package suffix."
	assemblyVersion+="-${betaSuffix}"
fi

echo "Patching version in .nuspec to '${assemblyVersion}'!"

#Patch assembly version number in .nuspec
sed -i.originalfilebackup "s/VERSION_PLACEHOLDER/${assemblyVersion}/g" api-client-shared.nuspec

