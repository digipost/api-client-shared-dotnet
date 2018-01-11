#Find current assembly version
# 1: Zero29 lists versions of all AssemblyInfo-files. There is two.
# 2: Take the first one
# 3: Grep with regex to retrieve the assembly version number (ex. 4.3.0.0) at end of line
assemblyVersion=$(mono ./Zero29.1.0.0/tools/Zero29.exe -l | head -n 1 | egrep -o '(\d+.){3}\d+')

#Patch assembly version number in .nuspec
sed -i '' "s/VERSION_PLACEHOLDER/${assemblyVersion}/g" api-client-shared.nuspec

