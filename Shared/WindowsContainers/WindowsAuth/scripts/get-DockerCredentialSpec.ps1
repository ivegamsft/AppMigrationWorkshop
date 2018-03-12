
Invoke-WebRequest "https://raw.githubusercontent.com/Microsoft/Virtualization-Documentation/live/windows-server-container-tools/ServiceAccounts/CredentialSpec.psm1" -UseBasicParsing -OutFile $env:TEMP\cred.psm1
import-module $env:temp\cred.psm1
New-CredentialSpec -Name win -AccountName containerhost
#This will return location and name of JSON file
Get-CredentialSpec
