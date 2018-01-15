#Find current assembly version
# 1: Zero29 lists versions of all AssemblyInfo-files. There is two.
# 2: Take the first one
# 3: Grep with regex to retrieve the assembly version number (ex. '4.3.0.0') at end of line and excluding build number,
#    ending up with '4.3.0.'
assemblyVersionWithoutBuildNumber=$(mono ./Zero29.1.0.0/tools/Zero29.exe -l | head -n 1 | egrep -o '([0-9].){3}')

mono ./Zero29.1.0.0/tools/Zero29.exe -l

echo "Assembly version found with version_patcher is '${assemblyVersionWithoutBuildNumber}' (build number excluded)."

assemblyVersion="${assemblyVersionWithoutBuildNumber}${TRAVIS_BUILD_NUMBER}"

echo "Patching version in .nuspec to '${assemblyVersion}!'"

#Patch assembly version number in .nuspec
sed -i.originalfilebackup "s/VERSION_PLACEHOLDER/${assemblyVersion}/g" api-client-shared.nuspec

