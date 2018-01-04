Param(
  [string]$signingKeyPath,
  [string]$signingKeyDestination  
)

$signingKeyExists = (Test-Path $signingKeyPath)
if(!$signingKeyExists)
{
    throw "Could not find signing key at $signingKeyPath. Please check that this is the decryption path for the signing key."
}

Write-Host "Moving signing key from $signingKeyPath to $signingKeyDestination."

New-Item -ItemType directory -Path (Split-Path -Path $signingKeyDestination)
Move-Item -path $signingKeyPath -destination $signingKeyDestination

$signingKeyMoved = (Test-Path $signingKeyDestination)
if(!$signingKeyMoved){
    throw "Did find signing key at $signingKeyPath, but unable to move to $signingKeyDestination. This may be a permissions problem."
}

Write-Host "Signing key was successfully moved from $signingKeyPath to $signingKeyDestination" -foreground "green"

