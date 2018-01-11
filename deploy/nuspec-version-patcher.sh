#Find current assembly version
assemblyVersion=$(mono ./Zero29.1.0.0/tools/Zero29.exe -l | head -n 1 | egrep -o '(\d+.){3}\d+')

#Patch assembly version in .nuspec
sed -i '' "s/VERSION_PLACEHOLDER/${assemblyVersion}/g" api-client-shared.nuspec

