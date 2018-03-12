Import-module ActiveDirectory
Add-KdsRootKey –EffectiveTime ((get-date).addhours(-10));
New-ADServiceAccount -Name host -DNSHostName host1.contoso.com `
    -PrincipalsAllowedToRetrieveManagedPassword "Domain Controllers", "domain admins", "CN=hosts,CN=Users,DC=contoso,DC=com" `
    -KerberosEncryptionType RC4, AES128, AES256 `
    -ServicePrincipalNames HTTP/host1, HTTP/host1.contoso.com

#Configure gMSA account for constrained delegation, for example if you need to delegate to a SQL Server, assuming your server is names sql in the contoso domain
Set-ADServiceAccount –Identity host -add @{"msDS-AllowedToDelegateTo"="MSSQLSvc/sql:1433","MSSQLSvc/sql.contoso.com:1433"}

#Configure the container host for contrained delegation
Set-ADComputer -Identity host1 -add @{"msDS-AllowedToDelegateTo"="MSSQLSvc/sql:1433","MSSQLSvc/sql.contoso.com:1433"}
