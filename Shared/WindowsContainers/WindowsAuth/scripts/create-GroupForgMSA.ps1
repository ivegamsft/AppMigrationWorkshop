New-ADGroup -GroupCategory Security -DisplayName "Windows Container Hosts" -Name hosts -GroupScope Universal
Add-ADGroupMember -Members (Get-ADComputer -Identity host1) -Identity hosts
#Validate that they were indeed created
Get-ADGroup -Identity hosts
Get-ADGroupMember -Identity hosts
