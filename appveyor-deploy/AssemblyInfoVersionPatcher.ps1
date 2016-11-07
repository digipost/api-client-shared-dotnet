Param(
  [string]$assemblyInfoPath  
)

. $PSScriptRoot\Common.ps1

$assemblyInfoName = (Get-Item $assemblyInfoPath).Name

Print-Step-Header "before_build: Patching version in $assemblyInfoName"

$globalAssemblyFileContent = Get-Content $assemblyInfoPath
					
$versionMatchPattern = '[0-9]+(\.([0-9]+|\*)){1,3}'				
$assemblyVersionMatchPattern = "(?<major>[0-9]+)\.(?<minor>[0-9])+(\.(?<build>([0-9])))?(\.(?<revision>([0-9])))?"

$hasAssemblyVersionMatchPattern = "AssemblyVersion\(`"$versionMatchPattern`"\)"

$hasAssemblyVersion = "'"+$globalAssemblyFileContent+"'" -match $hasAssemblyVersionMatchPattern
if (!$hasAssemblyVersion)
{
	throw "Could not get AssemblyVersion element. Have you deleted element from AssemblyInfo or moved the file? Stopping before world explodes!"	
}
else
{
	Write-Host "AssemblyVersion found, lets get this 🎉 started!"
	$assemblyVersionFormattedCorrectly = $matches[0] -match $assemblyVersionMatchPattern	
	if (!$assemblyVersionFormattedCorrectly) 
	{
		throw "The Global Assembly Version is not formatted correctly! Should be in format [major.minor.build], and optionally revision ($'$assemblyVersionMatchPattern') 😭"	    
	}
	
	$major=$matches['major'] -as [int]
	$minor=$matches['minor'] -as [int]
	$build=$matches['build'] -as [int]
	$revision=$matches['revision'] -as [int]	
}

Write-Host " - Version of assembly before patching: $major.$minor.$build.$revision."

if($env:APPVEYOR_REPO_BRANCH -eq "master")
{
	$patchedVersion = "$major.$minor.$build"
}
else 
{
    $patchedVersion = "$major.$minor.$build.$env:APPVEYOR_BUILD_NUMBER"
}

$AssemblyInformationalVersion = "$patchedVersion-$env:APPVEYOR_REPO_SCM" + ($env:APPVEYOR_REPO_COMMIT).Substring(0, 7)

Update-AppveyorBuild -Version $AssemblyInformationalVersion

Write-Host "	Patched File Version: $patchedVersion"
Write-Host "	Patched Informational Version: $AssemblyInformationalVersion"

Write-Host "- Starting patching of global assembly file"

$assemblyFileVersionBlob = "AssemblyFileVersion(`"$patchedVersion`")";
$assemblyVersionBlob = "AssemblyVersion(`"$patchedVersion`")";
$assemblyInformationalVersionBlob = "AssemblyInformationalVersion(`"$AssemblyInformationalVersion`")";

$globalAssemblyFileContent -replace "AssemblyFileVersion\(`"$versionMatchPattern`"\)", $assemblyFileVersionBlob `
		 -replace "AssemblyVersion\(`"$versionMatchPattern`"\)", $assemblyVersionBlob `
		 -replace "AssemblyInformationalVersion\(`"$versionMatchPattern`"\)", $assemblyInformationalVersionBlob | Out-File "$assemblyInfoPath"

function Verify-Content-Patched
{	
	$patchedAssemblyInfoData = Get-Content $assemblyInfoPath

	(Find-Data-Or-Throw -description "AssemblyFileVersion" -source $patchedAssemblyInfoData -data "$assemblyFileVersionBlob")
	(Find-Data-Or-Throw -description "AssemblyVersion" -source $patchedAssemblyInfoData -data "$assemblyVersionBlob")
	(Find-Data-Or-Throw -description "AssemblyInformationalVersion" -source $patchedAssemblyInfoData -data "$assemblyInformationalVersionBlob")	
}
(Verify-Content-Patched)

Write-Host "AssemblyInfo.cs successfully patched to $patchedVersion 👏" -foreground "Green"