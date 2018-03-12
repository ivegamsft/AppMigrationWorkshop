# Windows Containers

## Overview
In this lab, you will learn how to:

* Setup for Deployments and Windows Authentication and Delegation(Kerberos)
* Configure and test a Windows Container gSMA Account
* Test Kerberos configuration with delegation

## Prerequisites
Ensure you have the following:

  * You have an Azure Subscription
  * You have an Azure Virtual Network deployed
  * You have an NSG deployed on the default subnet
  * You have completed HOL 1-setup
  * You are in a domain environment
  * You have the Windows Container Host Deployed 

## Exercises
---

This hands-on-lab has the following exercises:

1. [Exercise 1: Setup for Deployments](#ex1)
1. [Exercise 2: Configure Windows Authentication and Delegation (Kerberos)](#ex2)
1. [Exercise 3: Deploy a containerized web app to validate Configuration](#ex3)
1. [Advanced troubleshooting](#ex4)

### Exercise 1: Setup for Deployments<a name="ex1"></a>

---

1. RDP into the `Windows Container Azure VM`

1. Validate that `Docker` is installed by opening `PowerShell` and run the following command:

    ```powershell
    docker info
    ```

1. Open `PowerShell` and run the following command:
    
    ```powershell
    docker images
    #You should see the following results
    REPOSITORY                    TAG                 IMAGE ID            CREATED             SIZE
    microsoft/windowsservercore   ltsc2016            db8182d67b6c        6 weeks ago         10.4GB
    microsoft/nanoserver          sac2016             5a5dfd4deb23        6 weeks ago         1.1GB
    ```

1. Configure the Docker daemon to open a tcp port. Open or create the daemon.json file in `C:\ProgramData\docker\config` folder.

1. With the following :

    > [!WARNING]
    > This is not a secure port. For a reference how how to use a secure port go to the [eShop Modernizing Repo](https://github.com/dotnet-architecture/eShopModernizing/wiki/03.-How-to-deploy-your-Windows-Containers-based-app-into-Azure-VMs-(Including-CI-CD))

    ```json
    { "hosts": ["tcp://0.0.0.0:2375","npipe://"] }
    ```

1. Restart the Docker service

    ```powershell
    Restart-Service Docker
    ```

1. Open the needed firewall ports on the Windows Container Host
    
    ```powershell
    netsh advfirewall firewall add rule name="Docker Rule" dir=in action=allow protocol=TCP localport=2375
    ```
1. Create a Rule on your NSG to allow remote management of docker on your host vm.

    ```powershell
    $rgName = "<resource group name>"
    $nsgName = "<nsg name>"
    Get-AzureRmNetworkSecurityGroup -Name  $nsgName -ResourceGroupName $rgName | Add-AzureRmNetworkSecurityRuleConfig -Name docker-rule -Description "Docker Rule" -Access Allow -Protocol Tcp -Direction Inbound -Priority 200 -SourceAddressPrefix Internet -SourcePortRange * -DestinationAddressPrefix * -DestinationPortRange 2375 |  Set-AzureRmNetworkSecurityGroup
    ```

1. Obtain the public IP address from your Azure VM. This can be done from the portal. Or you can run the following powershell

    ```powershell
    $rgName = "<resource group name>"
    $pipName = "<pip name>"
    $pip = Get-AzureRmPublicIpAddress -Name $pipName -ResourceGroupName $rgName
    Write-Host "IP Address is: " $pip.IpAddress
    ```

1. Validate from your development environment that you can connect to the host sever, assuming your public IP address is `13.65.212.88`
    ```powershell
    docker --host tcp://13.65.212.88 images
    REPOSITORY                    TAG                 IMAGE ID            CREATED             SIZE
    microsoft/windowsservercore   ltsc2016            db8182d67b6c        6 weeks ago         10.4GB
    microsoft/nanoserver          sac2016             5a5dfd4deb23        6 weeks ago         1.1GB
    ```

### Exercise 2: Setup for Windows Authentication and Delegation (Kerberos)<a name="ex2"></a>

---

#### Assumptions

* AD Domain at a minimum of 2008 Functional Level
* AD Server is a minimum of Windows Server 2012

1. Create the AD Group. On the Domain Controller execute the following. Replace with values specific to your enviroment:

    ```powershell
    $groupName = "<group name e.g. 'hosts'>"
    $containerHostName = "<Windows Container Host Name e.g. host1>"
    New-ADGroup -GroupCategory Security -DisplayName "Windows Container Hosts" -Name $groupName -GroupScope Universal
    Add-ADGroupMember -Members (Get-ADComputer -Identity $containerHostName) -Identity $groupName
    #Validate that they were indeed created
    Get-ADGroup -Identity $groupName
    Get-ADGroupMember -Identity $groupName
    ```
    
1. Reboot the Windows Container Host Server

1. On the DC, if you have not already set the Kds Root Key, execute the following:

    ```powershell
    Import-module ActiveDirectory
    Add-KdsRootKey –EffectiveTime ((get-date).addhours(-10));
    ```

1. On the DC, create a gMSA Account, make sure to set it for constrained delegation.

    ```powershell
    #Assuming your Windows Container Host anem is host1.contoso.com

    New-ADServiceAccount -Name host -DNSHostName "host1.contoso.com" `
    -PrincipalsAllowedToRetrieveManagedPassword  "Domain Controllers", "domain admins", "CN=hosts,CN=Users,DC=contoso,DC=com" `
    -KerberosEncryptionType RC4, AES128, AES256 `
    -ServicePrincipalNames HTTP/host1, HTTP/host1.contoso.com
    ```

1. Configure the gMSA accout for constrained delegation, for example if you need to delegate to a SQL server named `sql.contoso.com`
    
    ```powershell
    Set-ADServiceAccount –Identity host -add @{"msDS-AllowedToDelegateTo"="MSSQLSvc/sql:1433","MSSQLSvc/sql.contoso.com:1433"}
    ```

1. Configure the Windows Container host for constrained delegation, again, assuming your Windows container host is name `host1.contoso.com`
    
    ```powershell
    Set-ADComputer -Identity host1 -add @{"msDS-AllowedToDelegateTo"="MSSQLSvc/sql:1433","MSSQLSvc/sql.contoso.com:1433"}
    ```

1. On the Windows Container host (e.g. host1) add the gMSA account by executing the following

    ```powershell
    Enable-WindowsOptionalFeature -FeatureName ActiveDirectory-Powershell -online -all
    Get-ADServiceAccount -Identity host 
    Install-ADServiceAccount -Identity host
    Test-AdServiceAccount -Identity host

    # You should see output something like this:

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
    
    >
    > In a multi-domain environment, You may see an error like this:

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

    > In this case, you can also add the gMSA account to the server like this

    ```powershell
    Get-ADServiceAccount -Identity host | Set-ADServiceAccount -PrincipalsAllowedToRetrieveManagedPassword 'host$'
    ```

### Exercise 3: Deploy a containerized web app to validate configuration<a name="ex3"></a>

---

1. On the host we need to create a credentials spec file for Docker
   
     ```powershell
    #Assuming the gMSA Account name is 'host'
    Invoke-WebRequest "https://raw.githubusercontent.com/Microsoft/Virtualization-Documentation/live/windows-server-container-tools/ServiceAccounts/CredentialSpec.psm1" -UseBasicParsing -OutFile $env:TEMP\cred.psm1
    Import-Module $env:temp\cred.psm1
    New-CredentialSpec -Name win -AccountName host

    #This will return location and name of JSON file
    Get-CredentialSpec

    #You should see something like this
    Name Path
    ---- ----
    win  C:\ProgramData\docker\CredentialSpecs\win.json
    ```

1. On the Windows Container host, create a docker image with the correct version of Windows and IIS and the sample application to validate that authentication is working. 

1. Copy the sample site located in the `site` folder in this repo to the `C:\site` folder.

1. Validate the docker file in the `site` folder

    ```docker
    FROM microsoft/iis:latest
    SHELL ["powershell"]
    RUN mkdir C:\site
    ADD . /site
    RUN Install-WindowsFeature NET-Framework-45-ASPNET ; \  
        Install-WindowsFeature Web-Asp-Net45
    RUN Remove-WebSite -Name 'Default Web Site' 
    RUN Install-WindowsFeature Web-Windows-Auth
    #These are only needed for accessing the IIS admin UI

    #Enable Windows Authentication on the site
    RUN Import-module IISAdministration; \
    New-IISSite -Name "Site" -PhysicalPath C:\site -BindingInformation "*:80:"

    RUN C:\windows\system32\inetsrv\appcmd.exe set config "Site" -section:system.webServer/security/authentication/anonymousAuthentication /enabled:"False" /commit:apphost

    RUN  C:\windows\system32\inetsrv\appcmd.exe set config "Site" -section:system.webServer/security/authentication/windowsAuthentication /enabled:"True" /commit:apphost
    EXPOSE 80
    ```

1. From `PowerShell` execute the following:

    ```powershell
    cd c:\site
    docker build -t iis-site .
    ```

1. Run a container in the background
    
    ```powershell
    docker run -d -h host --security-opt "credentialspec=file://win.json" iis-site --name aspnet
    ```
    
1. Validate that the container is running:

    ```powershell
    docker ps -a

    ##if you see the that the container has exited, like below
    CONTAINER ID        IMAGE       COMMAND                     CREATED         STATUS      PORTS                   NAMES
    ab2e4be9167a        iis-site    "C:\\ServiceMonitor..."     13 seconds ago  Exited (2147943452) 3 seconds ago   determined_leavitt

    ##You can restart the container by grabbing the name, in this example 'determined_leavitt
    docker run determined_leavitt

    ##You can inspect the container for the internal IP address
    docker inspect -f "{{ .NetworkSettings.Networks.nat.IPAddress }}" determined_leavitt

    172.19.244.78
    ```

1. In the host server open a browser and navigate to `http://172.19.244.78`, make sure to input your own IP address. You will then be prompted, enter valid credentials.

    ![image](./media/07a-1.PNG)
    
1. Then you should see the web site

    ![image](./media/07a-2.PNG)

### Exercise 4: Advanced Troubleshooting<a name="ex4"></a>

---

For this lab, follow the steps from [advanced-troubleshooting.md](./advanced-troubleshooting.md)

----

## Summary

In this hands-on lab, you learned how to:

* Setup for Deployments and Windows Authentication and Delegation(Kerberos)
* Configure and test a Windows Container gSMA Account
* Test Kerberos configuration with delegation

---
Copyright 2016 Microsoft Corporation. All rights reserved. Except where otherwise noted, these materials are licensed under the terms of the MIT License. You may use them according to the license as is most appropriate for your project. The terms of this license can be found at https://opensource.org/licenses/MIT.

