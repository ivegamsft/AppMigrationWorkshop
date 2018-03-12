param
(
	$random=(get-date).ticks
)
Write-Host $random

$SQLServerName = $ENV:COMPUTERNAME

Write-Host "$SQLServerName"

# Enabling firewall port for SQL
netsh advfirewall firewall add rule name="SQL Server" dir=in action=allow protocol=TCP localport=1433

# Adding BUILTIN\Administrators to sysadmins (not recommended for production)
sqlcmd -S $SQLServerName -Q "CREATE LOGIN [BUILTIN\ADMINISTRATORS] FROM WINDOWS; EXEC sp_addsrvrolemember 'BUILTIN\Administrators','sysadmin'"

# Enabling TCP protocol
$TypeName = "Microsoft.SqlServer.SqlWmiManagement"
$AssemblyPath = [System.Reflection.Assembly]::LoadWithPartialName($TypeName).Location;
Add-Type -Path $AssemblyPath

$smo = 'Microsoft.SqlServer.Management.Smo.'  
$wmi = new-object ($smo + 'Wmi.ManagedComputer').  

$uri = "ManagedComputer[@Name='$SQLServerName']/ ServerInstance[@Name='MSSQLSERVER']/ServerProtocol[@Name='Tcp']"  
$Tcp = $wmi.GetSmoObject($uri)  
$Tcp.IsEnabled = $true  
$Tcp.Alter()  

# Restarting SQL Service
Stop-Service 'MSSQLSERVER'
Start-Service 'MSSQLSERVER'

Write-Host "Completed execution $(get-date)"
