function Find-Data-Or-Throw
{
    param(  [string]$description, 
            [string]$source,
            [string]$data            
    )

    if( -not($source -like "*$data*") )
    {
        throw "Did not find $data in $description. Please check spelling and/or source"
    }
}

function Print-Step-Header
{
    param(  [string]$description )

    Write-Host "----------------------------------------------------------------        "   -foreground "Yellow"
    Write-Host "$description                                                            "   -foreground "Yellow"
    Write-Host "----------------------------------------------------------------        "   -foreground "Yellow"
}