Enable-WindowsOptionalFeature -FeatureName ActiveDirectory-Powershell -online -all
Get-ADServiceAccount -Identity host 
Install-ADServiceAccount -Identity host
Test-AdServiceAccount -Identity host