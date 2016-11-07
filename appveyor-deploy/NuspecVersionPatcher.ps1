Param(
  [string]$assembly,
  [string]$nuspec
)

. $PSScriptRoot\Common.ps1

$nuspecName = (Get-Item $nuspec).Name

Print-Step-Header "after_build: Patching $nuspecName"

$root = (Get-Item (Resolve-Path -Path $PSScriptRoot)).parent.FullName

$dllPath = "$root\$assembly"
$dllExists = (Test-Path $dllPath)

Write-Host " - Fetching version from .dll with path $dllPath"

if(!$dllExists){
    throw "The dll was not found at $dllPath. Please provide a path to one of the built dlls in solution."
}

$version = [System.Reflection.Assembly]::LoadFile($dllPath).GetName().Version

$versionStr = "{0}.{1}.{2}.{3}" -f ($version.Major, $version.Minor, $version.Build, $version.Revision)

$prereleaseSuffix = "beta"
$foundVersionMessage = "- Found version in .dll: $versionStr"
if($env:APPVEYOR_REPO_BRANCH -eq $prereleaseSuffix)
{
    $versionStr = "$versionStr-$prereleaseSuffix"
    $foundVersionMessage = "$foundVersionMessage, patching to $versionStr to define prerelase"
}

$content = (Get-Content ".\$nuspec") 

(Find-Data-Or-Throw -description "unpatched nuspec file. The <version> tag must be specified as '<version>`$version`$</version>' to be able to patch version in $nuspecName" -data "`$version`$" -source $content)

$content = $content -replace '\$version\$',$versionStr

$content | Out-File "$root\$nuspec"

function Verify-Content-Patched
{   
    $patchedNuspecData = Get-Content ".\$nuspec";
    (Find-Data-Or-Throw -description ".nuspec version" -data $versionStr -source $patchedNuspecData)
}
(Verify-Content-Patched)

Write-Host "Successfully patched .nuspec version tag to $versionStr" -foreground "Green"
