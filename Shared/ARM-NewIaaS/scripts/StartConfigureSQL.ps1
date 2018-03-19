param
(
	[string]$adminUsername,
	[string]$adminPassword
)

Start-Transcript -path "$PSScriptRoot\StartConfigureSQLLog.txt"

$password =  ConvertTo-SecureString $adminPassword -AsPlainText -Force
[System.Management.Automation.PSCredential]$credential = New-Object System.Management.Automation.PSCredential ("$env:COMPUTERNAME\$adminUserName", $password)

$command = "$PSScriptRoot\ConfigureSQL.ps1"
Enable-PSRemoting -force -ErrorAction SilentlyContinue
Invoke-Command -FilePath $command -Credential $credential -ComputerName $env:COMPUTERNAME
#Disable-PSRemoting -Force  -ErrorAction SilentlyContinue

Stop-Transcript
[string]::Join("`r`n",(Get-Content "$PSScriptRoot\StartConfigureSQLLog.txt")) | Out-File "$PSScriptRoot\StartConfigureSQLLog.txt"