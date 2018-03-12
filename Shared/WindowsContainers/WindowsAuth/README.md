# How to configure a Windows Container with an ASP.NET Web Application on IIS on a single AD Domain that requires Kerberos

## Assumptions
1. You have an AD Domain at a minimum 2008 functional level on a minumum of Windows Server 2012
1. You have a Windows Server 2016 Datacenter - with Containers
1. You have a SQL Server 2017 Enterprise Windows Server 2016 VM
1. Domain Name: contoso.com
1. DC Name: dc1.contoso.com
1. SQL Server Name: sql.contoso.com
1. Windows Container Host Name: host.contoso.com

## Steps

1. Create a global security group in AD
1. Add you server that is the Windows container host to that group
1. Create a gMSA account in AD with appropriate SPNs and Constrained Delegation
1. Add the gMSA account to the server that is the Windows container host
1. Install docker on host
1. Create the credentials spec file for Docker
1. Create a docker image with the correct verison of Windows and IIS and a sample app to validate that authentication is working
1. Run the docker image


### Create the AD Group
On the DC Execute the following*:

```powershell
New-ADGroup -GroupCategory Security -DisplayName "Windows Container Hosts" -Name hosts -GroupScope Universal
Add-ADGroupMember -Members (Get-ADComputer -Identity host1) -Identity hosts
#Validate that they were indeed created
Get-ADGroup -Identity hosts
Get-ADGroupMember -Identity hosts
```


You will need to reboot the host server after adding it to the group

### Create the gMSA Account, make sure to set it for constrained delegation. 
On the DC Execute the following:

```powershell
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
```
*If this a multi-domain scenario you will need to run as the root domain admin. If the root domain in the forest is contoso.net and this is the eu.contos.net domain in the same forest. Use the contos\<admin> run the Add-KdsRootKey
 
### Add the gMSA account to the host
On the host Execute the following:

```powershell
Enable-WindowsOptionalFeature -FeatureName ActiveDirectory-Powershell -online -all
Get-ADServiceAccount -Identity host 
Install-ADServiceAccount -Identity host
Test-AdServiceAccount -Identity host
```


You should see output like this:

```powershell

Get-ADServiceAccount -Identity host 
DistinguishedName : CN=host,CN=Managed Service Accounts,DC=contoso,DC=com
Enabled           : True
Name              : host
ObjectClass       : msDS-GroupManagedServiceAccount
ObjectGUID        : fff6c1c6-c3f0-4b0c-a34c-682708270f80
SamAccountName    : host$
SID               : S-1-5-21-5555555-222222222-945891031-1107
UserPrincipalName : 


Install-ADServiceAccount -Identity host
Test-AdServiceAccount -Identity host
True
```
In a multi-domain environment, I have seen an error like this:

```powershell
Install-ADServiceAccount : Cannot install service account. Error Message: '{Access Denied}
A process has requested access to an object, but has not been granted those access rights.'.
At line:1 char:1
+ Install-ADServiceAccount -Identity euhost
+ ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    + CategoryInfo          : WriteError: (euhost:String) [Install-ADServiceAccount], ADException
    + FullyQualifiedErrorId : InstallADServiceAccount:PerformOperation:InstallServiceAcccountFailure,Microsoft.ActiveDirectory.Man 
   agement.Commands.InstallADServiceAccount
```
You can also add the gMSA account to the server like this

```powershell
Get-ADServiceAccount -Identity euhost | Set-ADServiceAccount -PrincipalsAllowedToRetrieveManagedPassword 'host$'
```

### Install Docker on the host
Execute the following on the host:

```powershell
Install-Module -Name DockerMsftProvider -Repository PSGallery -Force
Install-Package -Name docker -ProviderName DockerMsftProvider
Restart-Computer -Force
```
Ensure windows is up to date by running:

```powershell
sconfig
```
This shows a text-based configuration menu, where you can choose option 6 to Download and Install Updates:

```powershell
===============================================================================
                         Server Configuration
===============================================================================

1) Domain/Workgroup:                    Workgroup:  WORKGROUP
2) Computer Name:                       WIN-HEFDK4V68M5
3) Add Local Administrator
4) Configure Remote Management          Enabled

5) Windows Update Settings:             DownloadOnly
6) Download and Install Updates
7) Remote Desktop:                      Disabled
...
```
### Create the credentials spec file for Docker

On the host we need to create the credentials spec file for Docker

```powershell
Invoke-WebRequest "https://raw.githubusercontent.com/Microsoft/Virtualization-Documentation/live/windows-server-container-tools/ServiceAccounts/CredentialSpec.psm1" -UseBasicParsing -OutFile $env:TEMP\cred.psm1
import-module $env:temp\cred.psm1
New-CredentialSpec -Name win -AccountName host
#This will return location and name of JSON file
Get-CredentialSpec

```

### Create a docker image with the correct verison of Windows and IIS and a sample app to validate that authentication is working

The docker file in the sample site looks something like this.

```docker
FROM microsoft/iis:latest 
  
SHELL ["powershell"] 
  
RUN mkdir C:\site 
ADD . /site 
  
RUN Install-WindowsFeature NET-Framework-45-ASPNET ; \   
    Install-WindowsFeature Web-Asp-Net45 
  
RUN Remove-WebSite -Name 'Default Web Site'  
  
RUN Install-WindowsFeature Web-Windows-Auth 
  
RUN Import-module IISAdministration; \ 
    New-IISSite -Name "Site" -PhysicalPath C:\site -BindingInformation "*:80:" 
  
RUN C:\windows\system32\inetsrv\appcmd.exe set config "Site" -section:system.webServer/security/authentication/anonymousAuthentication /enabled:"False" /commit:apphost 
RUN  C:\windows\system32\inetsrv\appcmd.exe set config "Site" -section:system.webServer/security/authentication/windowsAuthentication /enabled:"True" /commit:apphost 
  
EXPOSE 80 
```
Navigate to the site directory, making sure you dockerfile is there and run the following command:

```powershell
docker build -t iis-site .
```

Now you can run the image. There are several option you can run with

* Run interactively:
```powershell
docker run -it -h host --security-opt "credentialspec=file://win.json" iis-site cmd
```
* Run detached in the background, with no port mapping. In this case you will need to find the docker assigned IP address to you can test locally
```powershell
docker run -d -h host --security-opt "credentialspec=file://win.json" iis-site --name aspnet
docker inspect -f "{{ .NetworkSettings.Networks.nat.IPAddress }}" <container_name>
```

* Run detached in the backgroun, this will also map port 80 for access to the web site from domain members through the host.
```powershell
docker run -d -p 80:80 -h host --security-opt "credentialspec=file://win.json" iis-site --name aspnet
```


#### To validate that you can indeed communicate with the domain

* Connect interactively to the container

```powershell
docker exec -it <container_name> powershell
nltest /parentdomain
nltest /query
```
you should see resulst like this:

```powershell
PS C:\> nltest /parentdomain
contoso.com. (1)
The command completed successfully
PS C:\> nltest /query
Flags: 0
Connection Status = 0 0x0 NERR_Success
The command completed successfully
PS C:\>
```

If you would like to do remote management of IIS within the container you can run the following commands:

```powershell

#These are only needed for accessing the IIS admin UI
RUN net user <username> <password> /add
RUN net localgroup Administrators <username> /add 
RUN Install-WindowsFeature Web-Mgmt-Service
RUN New-ItemProperty -Path HKLM:\software\microsoft\WebManagement\Server -Name EnableRemoteManagement -Value 1 -Force
RUN Start-Service WMSVC
#These are only needed for accessing the IIS admin UI

```
Debugging Notes for Kerberos in the container:


```powershell
#Enable Kerberos logging in the container

New-ItemProperty -Path HKLM:\system\currentcontrolset\control\lsa\kerberos -Name LogLevel -Value 1 -PropertyType DWord

#Export the log file (wevtutil epl <LogName> <FileName.evtx>) and open it on the host machine to view the errors
```

# Reference
* [Getting Started with Group Managed Service Accounts](https://docs.microsoft.com/en-us/previous-versions/windows/it-pro/windows-server-2012-R2-and-2012/jj128431(v%3dws.11))
* [Create the Key Distribution Services KDS Root Key](https://docs.microsoft.com/en-us/previous-versions/windows/it-pro/windows-server-2012-R2-and-2012/jj128430(v=ws.11))
* [Windows Containers on Windows Server](https://docs.microsoft.com/en-us/virtualization/windowscontainers/quick-start/quick-start-windows-server)
* [IIS installed in a Windows Server Core based container](https://hub.docker.com/r/microsoft/iis/)
* [Nltest](https://docs.microsoft.com/en-us/previous-versions/windows/it-pro/windows-server-2012-R2-and-2012/cc731935(v=ws.11)#examples)
* [iis_auth_allsteps](https://gist.github.com/PatrickLang/27c743782fca17b19bf94490cbb6f960)
* Some errors can also be seen on the DC if you enable audit logging (https://docs.microsoft.com/en-us/previous-versions/windows/it-pro/windows-server-2008-R2-and-2008/dd772712(v=ws.10)) 
* Enable IIS admin console (https://blogs.msdn.microsoft.com/containerstuff/2017/02/14/manage-iis-on-a-container-with-the-iis-admin-console/)
* Enable VS debugging (https://www.richard-banks.org/2017/02/debug-net-in-windows-container.html)