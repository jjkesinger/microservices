param ($server, [SecureString]$sqluser, [SecureString]$sqlpassword, $resourceGroup, $location, [SecureString]$servicebusnamespace, [SecureString]$databasename)
# $ = "P@ssword"
# $ = "adminuser"
# $ = "nsp-esc-dev"
# $ = "microservice"
# $ = "centralus"
# $ = "nsp-esc-dev"
# $ = "payroll"

$sqlservername = ConvertTo-SecureString $server -Force

Write-Output "Test"
Write-Output $sqlservername

# Write-Output "Logging In"
# #az login --service-principal $env:servicePrincipalId -tenent $env:tenentId

# Write-Output "Creating Resource group"
# az group create --name $resourceGroup --location $location

# Write-Output "Creating Azure Service Bus, Queues and Topics"
# az servicebus namespace create --name $servicebusnamespace --resource-group $resourceGroup --location $location --sku standard
# az servicebus queue create --name payroll_queue --namespace-name $servicebusnamespace --resource-group $resourceGroup

# az servicebus topic create --name paycheck --namespace-name $servicebusnamespace --resource-group $resourceGroup
# az servicebus topic subscription create --name benefit_subscriber --namespace-name $servicebusnamespace -g $resourceGroup --topic-name paycheck

# az servicebus topic create --name hsa --namespace-name $servicebusnamespace --resource-group $resourceGroup
# az servicebus topic subscription create --name payroll_subscriber --namespace-name $servicebusnamespace -g $resourceGroup --topic-name hsa

# Write-Output "Creating SQL Server and Database"
# az sql server create -l $location -g $resourceGroup -n $sqlservername -u $sqluser -p $sqlpassword
# az sql db create -g $resourceGroup -s $sqlservername -n $databasename -e 'Free'

# $sql = az sql db show-connection-string -s $sqlservername -n $databasename -c odbc
# $keys = az servicebus namespace authorization-rule keys list -g microservice --namespace-name $servicebusnamespace -n RootManageSharedAccessKey | ConvertFrom-Json

# $env:DB_CONNECTIONSTRING = $sql -replace "<username>", $sqluser -replace "<password>", $sqlpassword
# $env:SB_CONNECTIONSTRING=$keys[0].primaryConnectionString
# $env:SQL_PW=$sqlpassword

# Write-Output "Set environmental variables" $env:DB_CONNECTIONSTRING

# docker-compose build