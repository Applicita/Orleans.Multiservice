Param(
    [Parameter(Mandatory, HelpMessage="The name (without 'Service' suffix) of the logical service to add to the CoreTeam multiservice solution in the current directory; used in the name of the new service project and in new namespaces + classes in the Apis and Contracts projects")]
    [string]
    $Name
)

# Function to update the Program.cs file to add a new parameter to the RegisterEndpoints method
function Update-RegisterEndpoints {
    $newParameter = "`n    typeof(Applicita.eShop.Apis.${Name}Api.${Name}Endpoints)`n"
    $apisDirectory = Join-Path -Path $PWD -ChildPath "Apis"
    $programFile = Get-ChildItem -Path $apisDirectory -Recurse -Filter "Program.cs" -ErrorAction SilentlyContinue | Select-Object -First 1

    if ($programFile -ne $null) {
        $programContent = Get-Content -Path $programFile.FullName -Raw
        $pattern = "(?s)(app\s*\.RegisterEndpoints\s*\(.+?\))\s*\)"
        $modifiedContent = $programContent -replace $pattern, "`$1,$newParameter)"

        if ($modifiedContent -ne $programContent) {
            Set-Content -Path $programFile.FullName -Value $modifiedContent
            Write-Output "Successfully added new parameter to RegisterEndpoints call in $($programFile.FullName):$newParameter"
            return
        }
    }

    Write-Warning "Could not automatically add below parameter to the RegisterEndpoints(...) call; please add it manually:$newParameter"
}

dotnet new mcs-orleans-multiservice --RootNamespace Applicita.eShop -M . --Logicalservice $Name --allow-scripts Yes

Update-RegisterEndpoints
