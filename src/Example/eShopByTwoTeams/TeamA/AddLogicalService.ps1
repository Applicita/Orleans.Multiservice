Param(
    [Parameter(Mandatory, HelpMessage="The name (without 'Service' suffix) of the logical service to add to the CoreTeam multiservice solution in the current directory; used in the name of the new service project and in new namespaces + classes in the Apis and Contracts projects")]
    [string]
    $Name
)
dotnet new mcs-orleans-multiservice --RootNamespace Applicita.eShop -M . --Logicalservice $Name --allow-scripts Yes