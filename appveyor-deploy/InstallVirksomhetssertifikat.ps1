Param( 
    [string]$virksomhetssertifikat
)

$virksomhetssertifikatExists = (Test-Path $virksomhetssertifikat)
if(!$virksomhetssertifikatExists)
{
    throw "Could not find virksomhetssertifikat at $virksomhetssertifikat. Please check that this is the decryption path for the virksomhetssertifikat."
}

certutil -f -p $env:virksomhetssertifikat_pass -importpfx $virksomhetssertifikat