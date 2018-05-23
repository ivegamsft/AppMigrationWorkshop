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

This hands-on-lab has the following exercises:

1. [Exercise 1: Setup for Deployments](#ex1)
1. [Exercise 2: Configure Windows Authentication and Delegation in the domain (Kerberos)](#ex2)
1. [Exercise 3: Configure the Windows Container Host gMSA](#ex3)
1. [Exercise 4: Deploy a containerized web app to validate Configuration](#ex4)
1. [Advanced troubleshooting](#ex5)

### Exercise 1: Setup for Deployments<a name="ex1"></a>

1. RDP into the `Windows Container Azure VM`. It is suffixed with `-cnt`

    ![image](./media/2018-03-18_6-20-25.png)

1. Validate that `Docker` is installed by opening `PowerShell` and run the following command:

    ````powershell
    docker info
    ````

    ![image](./media/2018-03-18_6-23-51.png)

1. Open `PowerShell` and run the following command to view the docker images downloaded on the machine:

    ````powershell
    docker images
    ````

    You should see something similar to below:

    ````powershell
    REPOSITORY                    TAG                 IMAGE ID            CREATED             SIZE
    microsoft/windowsservercore   ltsc2016            db8182d67b6c        6 weeks ago         10.4GB
    microsoft/nanoserver          sac2016             5a5dfd4deb23        6 weeks ago         1.1GB
    ````

1. Configure the Docker daemon to open a tcp port for remote access. Open or create the daemon.json file in `C:\ProgramData\docker\config` folder from PowerShell with the following :

	```powershell
	notepad c:\ProgramData\docker\config\daemon.json
	```
    > [!WARNING]
    > This is not a secure port. For a reference how how to use a secure port go to the [eShop Modernizing Repo](https://github.com/dotnet-architecture/eShopModernizing/wiki/03.-How-to-deploy-your-Windows-Containers-based-app-into-Azure-VMs-(Including-CI-CD))
	Copy the following code in and Save the file
    ```json
    { "hosts": ["tcp://0.0.0.0:2375","npipe://"] }
    ```

    ![image](./media/2018-03-18_6-27-42.png)

   

1. Restart the Docker service

    ````powershell
    Restart-Service Docker
    ````

1. Run the following command to open the required firewall ports on the Windows Container Host

    ````powershell
    netsh advfirewall firewall add rule name="Docker Rule" dir=in action=allow protocol=TCP localport=2375
    ````

1. Disable the IE Enhanced Security Configuration setting to allow browsing on the server. Open Server Manager and locate the setting. Disable it for Administrators.

    ![image](./media/2018-03-18_6-37-37.png)

1. Install Azure PowerShell tools on the Docker host

    ````powershell
    Install-Module PowerShellGet -Force
    Install-Module -Name AzureRM -AllowClobber
    ````

1. You will see the following output. Enter `Y` or `Yes to All`

    ````powershell
    Untrusted repository

    You are installing the modules from an untrusted repository. If you trust this repository, change
    its InstallationPolicy value by running the Set-PSRepository cmdlet.

    Are you sure you want to install the modules from 'PSGallery'?
    [Y] Yes  [A] Yes to All  [N] No  [L] No to All  [S] Suspend  [?] Help (default is "N"): Y
    ````

1. Import the Azure RM Module

    ````powershell
    Import-Module -Name AzureRM
    ````

1. Once the Azure PowerShell tools are installed, login to Azure

    ````powershell
    Login-AzureRMAccount
    ````
	`NOTE: You may need to run Select-AzureRMSubscription to select the appropriate subscription if you have access to multiple with the account you have used to login`

1. You will need the private IP address of the Windows Container Host.
1. Install docker on the development/jump environment. By downloading the installfrom from https://download.docker.com/win/stable/Docker%20for%20Windows%20Installer.exe and executing it following the default installation prompts.
1. Validate from your development/jump environment that you can connect to the host sever, assuming your public IP address is `[YOUR PRIVATE IP ADDRESS]`

    ````powershell
    docker --host tcp://[YOUR PRIVATE IP ADDRESS] images
    REPOSITORY                    TAG                 IMAGE ID            CREATED             SIZE
    microsoft/windowsservercore   ltsc2016            db8182d67b6c        6 weeks ago         10.4GB
    microsoft/nanoserver          sac2016             5a5dfd4deb23        6 weeks ago         1.1GB
    ````

### Exercise 2: Setup for Windows Authentication and Delegation (Kerberos)<a name="ex2"></a>

#### Assumptions

* AD Domain at a minimum of 2008 Functional Level
* AD Server is a minimum of Windows Server 2012

This configuration will be done from on the domain controller.

1. From the jump box, connect to the domain controller (DC). The DC can be located by searching for the VM name suffixed with `-dc`

    ![image](./media/2018-03-18_15-28-10.png)

1. For docker, we need to create an AD group for the gSMA accounts. execute the following. Replace with values specific to your environment:

    ```powershell
    $groupName = "<YOUR GROUP NAME E.G. 'CONTAINER-HOSTS'>"
    $containerHostName = "<YOUR WINDOWS CONTAINER HOST NAME E.G. HOST1-CNT>"
    New-ADGroup -GroupCategory Security -DisplayName "Windows Container Hosts" -Name $groupName -GroupScope Universal
    Add-ADGroupMember -Members (Get-ADComputer -Identity $containerHostName) -Identity $groupName
    #Validate that they were indeed created
    Get-ADGroup -Identity $groupName
    Get-ADGroupMember -Identity $groupName
    ```

    The output should look something like the following:

    ![image](./media/2018-03-18_15-32-48.png)

1. If you have not already set the Kds Root Key, execute the following:

    ```powershell
    Import-module ActiveDirectory
    Add-KdsRootKey -EffectiveTime ((get-date).addhours(-10));
    ```
1. Create a gMSA Account and set it for constrained delegation. Replace the values with your particular VM names.

    > Note: The name of the AD Service account is hard-coded in the examples to `chost-gmsa`. This can be any valid sAMAccountName name.

    ````powershell
    New-ADServiceAccount -Name "chost-gmsa" -DNSHostName "[YOUR CONTAINER HOST NAME].appmig.local" `
    -PrincipalsAllowedToRetrieveManagedPassword  "Domain Controllers", "domain admins", "CN=container-hosts,CN=Users,DC=appmig,DC=local" `
    -KerberosEncryptionType RC4, AES128, AES256 `
    -ServicePrincipalNames HTTP/[YOUR CONTAINER HOST NAME], HTTP/[YOUR CONTAINER HOST NAME].appmig.local
    ````

1. Configure the gMSA account for constrained delegation. Replace the values for your SQL server.

    ````powershell
    Set-ADServiceAccount -Identity "chost-gmsa" -add @{"msDS-AllowedToDelegateTo"="MSSQLSvc/[YOUR SQL SERVER NAME]:1433","MSSQLSvc/[YOUR SQL SERVER NAME].appmig.local:1433"}
    ````

1. Configure the Windows Container host for constrained delegation

    ````powershell
    Set-ADComputer -Identity [YOUR CONTAINER HOST NAME] -add @{"msDS-AllowedToDelegateTo"="MSSQLSvc/[YOUR SQL SERVER NAME]:1433","MSSQLSvc/[YOUR SQL SERVER NAME].appmig.local:1433"}
    ````

#### Exercise 3: Configure the Windows Container Host gSMA<a name="ex3"></a>

The following commands are run from the Windows Container host machine

1. Reboot the Windows Container Host Server to update the AD group membership

1. Once the Windows Container host is restarted, add the gMSA account by executing the following command

    > Note: The name of the AD Service account is hard-coded here to `chost-gmsa`

    ````powershell
    Enable-WindowsOptionalFeature -FeatureName ActiveDirectory-Powershell -online -all
    Get-ADServiceAccount -Identity "chost-gmsa"
    Install-ADServiceAccount -Identity "chost-gmsa"
    Test-AdServiceAccount -Identity "chost-gmsa"
    ````

    You should see output something like this:

    ````powershell
    Get-ADServiceAccount -Identity
    DistinguishedName : CN=chost-gmsa,CN=Managed Service Accounts,DC=appmig,DC=local
    Enabled           : True
    Name              : chost-gmsa
    ObjectClass       : msDS-GroupManagedServiceAccount
    ObjectGUID        : fff6c1c6-c3f0-4b0c-a34c-682708270f80
    SamAccountName    : chost-gmsa$
    SID               : S-1-5-21-5555555-222222222-945891031-1107
    UserPrincipalName :

    Install-ADServiceAccount -Identity chost-gmsa
    Test-AdServiceAccount -Identity chost-gmsa
    True
    ````
    > Note: If you receive the message `Install-ADServiceAccount : Cannot Install service account.  Error Message: ‘{Access Denied}`, reboot the Windows Container Host to update the AD group membership
    >
    > In a multi-domain environment, You may see an error like this:

    ````powershell
    Install-ADServiceAccount : Cannot install service account. Error Message: '{Access Denied}
    A process has requested access to an object, but has not been granted those access rights.'.
    At line:1 char:1
    + Install-ADServiceAccount -Identity euhost
    + ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        + CategoryInfo          : WriteError: (euhost:String) [Install-ADServiceAccount], ADException
        + FullyQualifiedErrorId : InstallADServiceAccount:PerformOperation:InstallServiceAcccountFailure,Microsoft.ActiveDirectory.Management.Commands.InstallADServiceAccount
    ````

    > In this case, you can also add the gMSA account to the server like this

    ````powershell
    Get-ADServiceAccount -Identity chost-gmsa | Set-ADServiceAccount -PrincipalsAllowedToRetrieveManagedPassword 'chost-gmsa$'
    ````

### Exercise 4: Deploy a containerized web app to validate configuration<a name="ex4"></a>

1. On the container host, we need to create a credentials spec file for Docker

    > > Note: The name of the AD Service account is hard-coded in the examples to `chost-gmsa`

     ````powershell
    Invoke-WebRequest "https://raw.githubusercontent.com/Microsoft/Virtualization-Documentation/live/windows-server-container-tools/ServiceAccounts/CredentialSpec.psm1" -UseBasicParsing -OutFile $env:TEMP\cred.psm1
    Import-Module $env:temp\cred.psm1
    New-CredentialSpec -Name win -AccountName chost-gmsa
    ````

1. Run the following command to return location and name of credential spec JSON file

     ````powershell
    Get-CredentialSpec
    ````

    You should see something like this

     ````powershell
    Name Path
    ---- ----
    win  C:\ProgramData\docker\CredentialSpecs\win.json
    ````

1. Now that we have a credential spec file, let's create a docker image with the correct version of Windows and IIS and the sample application to validate that authentication is working.

1. Copy the sample site located in the `site` folder in the repo to a folder onto the Container host (in this example, `C:\site`).

    ![image](./media/2018-03-18_16-21-43.png)
	
	`NOTE: All Sources files were located on the jump box (name is unique per deployment) on the C Drive under AppMigrationWorkshop. You can access them from the containers box via a UNC path 
	\\<containerhost>\c$\AppMigrationWorkShop. You will be required to disable the firewall on the jump box or open the relevant file sharing ports`

1. Validate the docker file in the `site` folder

    ````docker
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
    ````
1. Update the connection string in the web.config file. Replace the [YOUR SQL SERVER NAME] with the your SQL Server Host Name.
    ```xml
        <?xml version="1.0"?>
        <!--
        For more information on how to configure your ASP.NET application, please visit
        https://go.microsoft.com/fwlink/?LinkId=169433
        -->
        <configuration>
            <system.web>
            <compilation debug="true" targetFramework="4.6.1"/>
            <httpRuntime targetFramework="4.6.1"/>
            </system.web>
            <connectionStrings>
                <remove name="RemoteSqlSever" />
                <clear />
                <add name="RemoteSqlServer" connectionString="Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=Classifieds;Data Source=[YOUR SQL SERVER NAME].appmig.local" providerName="System.Data.SqlClient" />
            </connectionStrings>
        </configuration>
    ```
1. From `PowerShell` execute the following:

    > Note: this may take up to 20 mins to download the images for the first time

    ````powershell
    cd c:\site
    docker build -t iis-site .
    ````

1. Run a container in the background

    > Note: In the example below, the host name is hard-coded to `chost`.

    ````powershell
    docker run -d -h chost-gmsa --security-opt "credentialspec=file://win.json" iis-site --name aspnet
    ````

1. Validate that the container is running:

    ````powershell
    docker ps -a

    ##if you see the that the container has exited, like below
    CONTAINER ID        IMAGE       COMMAND                     CREATED         STATUS      PORTS                   NAMES
    ab2e4be9167a        iis-site    "C:\\ServiceMonitor..."     13 seconds ago  Exited (2147943452) 3 seconds ago   determined_leavitt

    ##You can restart the container by grabbing the name, in this example 'determined_leavitt
    docker run determined_leavitt

    ##You can inspect the container for the internal IP address
    docker inspect -f "{{ .NetworkSettings.Networks.nat.IPAddress }}" determined_leavitt

    172.19.244.78
    ````

1. In the host server open a browser and navigate to `http://[YOUR IP ADDRESS]/`. You will be prompted for domain credentials.

    ![image](./media/07a-1.PNG)

1. Then you should see the web site

    ![image](./media/07a-2.PNG)

### Exercise 4: Advanced Troubleshooting<a name="ex4"></a>

For this lab, follow the steps from [advanced-troubleshooting.md](./advanced-troubleshooting.md)

----

## Summary

In this hands-on lab, you learned how to:

* Setup for Deployments and Windows Authentication and Delegation(Kerberos)
* Configure and test a Windows Container gSMA Account
* Test Kerberos configuration with delegation

----

Copyright 2016 Microsoft Corporation. All rights reserved. Except where otherwise noted, these materials are licensed under the terms of the MIT License. You may use them according to the license as is most appropriate for your project. The terms of this license can be found at https://opensource.org/licenses/MIT.