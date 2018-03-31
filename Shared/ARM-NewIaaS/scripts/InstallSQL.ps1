function log
{
    param
    (
        [string]$logFile,
        [string]$message
    )

    Write-Output "$(get-date -format s)-$message" | Out-File $logFile -Append
}

$scriptPath = $script:MyInvocation.MyCommand.Path
$scriptFolder = [system.io.path]::GetDirectoryName($scriptPath)
$scriptName = [system.io.path]::GetFileName($scriptPath)
$logFile = Join-Path $scriptFolder "$scriptName.txt"

try
{
    log -message "Starting Install SQL Script" -logFile $logFile

    # $sqlDownloadPath = "c:\sqldownload"
    $sqlInstallerPath = "c:\sqlInstall"
    
    # mkdir $sqlDownloadPath
    mkdir $sqlInstallerPath
    
    # Write-Host "Downloading SQL"
    
    # $url="https://download.microsoft.com/download/0/4/B/04BE03CD-EAF3-4797-9D8D-2E08E316C998/SQLEXPRADV_x64_ENU.exe"
    # $localPath = Join-Path $sqlDownloadPath "SQLEXPRADV_x64_ENU.exe"
    # $client = new-object System.Net.WebClient 
    # $client.DownloadFile($url, $localPath) 
    
    log -message "Enabling .NET 3.5" -logFile $logFile
    dism /online /enable-feature /featurename:NetFx3
    
    log -message "Checking if SQL installer was downloaded" -logFile $logFile
    if (Test-Path (Join-Path "$scriptFolder" "SQLEXPRADV_x64_ENU") )
    {
        log -message "    SQL Package found" -logFile $logFile
    }
    else
    {
        log -message "    SQL Package NOT found!" -logFile $logFile
    }
    
    log -message "Extracting SQL package" -logFile $logFile
    & "$scriptFolder\..\SQLEXPRADV_x64_ENU" /Q /X:$sqlInstallerPath
    
    while ( (get-process -Name SQLEXPRADV_x64_ENU -ErrorAction SilentlyContinue) -ne $null) { log -message "   extracting Files..." -logFile $logFile ; Start-Sleep -seconds 2}
    
    log -message "Installing SQL" -logFile $logFile
    Set-Location $sqlInstallerPath
    ./setup.exe /q /ACTION=Install /FEATURES=SQL,Tools /INSTANCENAME=MSSQLSERVER /SQLSVCACCOUNT="NT AUTHORITY\Network Service" /SQLSYSADMINACCOUNTS=".\administrators" /AGTSVCACCOUNT="NT AUTHORITY\Network Service" /IACCEPTSQLSERVERLICENSETERMS /TCPENABLED=1

	log -message "Enabling firewall port for SQL" -logFile $logFile
	netsh advfirewall firewall add rule name="SQL Server" dir=in action=allow protocol=TCP localport=1433

    log -message "End of $scriptPath execution" -logFile $logFile
}
catch
{
    log -message "An error occured.`n$_" -logFile $logFile
}

