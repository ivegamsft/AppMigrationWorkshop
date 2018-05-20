# Setting up the source applications

## Overview

In this lab, you will install three sample legacy applications. The sample legacy applications will be used as the source for migrating to Azure.

Applications

* Timetracker
* Classifieds
* Jobs

## Prerequisites

* An active Azure Subscription
* You have the working source environment deployed on [Lab 01](../01-setup/README.md)

## Exercises

This hands-on-lab has the following exercises:

1. [Exercise 1: Configuration Steps on Jumpbox VM](#ex1)
1. [Exercise 2: Configuration Steps on SQL Server](#ex2)
1. [Exercise 3: Configuration Steps on Web Server](#ex3)
1. [Exercise 4: Test the web applications](#ex4)

---

### Exercise 1: Configuration Steps on Jumpbox VM<a name="ex1"></a>

These configuration steps will be performed from the Jumpbox.

#### Sign on to Jump Box VM from Azure Portal

1. Go to the Azure Portal (http://portal.azure.com)

1. Sign on with `Microsoft Account` or `Work or School Account` associated with your Azure subscription

    ![image](./media/02-01-a.png)

    ![image](./media/02-01-b.png)

1. In the portal, in the left navigation pane, click `Resource Groups`

    ![image](./media/02-01-c.png)

1. From the Resource Group list, select the one deployed in HOL 1 (e.g. AppModernization-RG)

    ![image](./media/02-01-d.png)

1. Locate the `Jumpbox` Virtual Machine by searching for `jmp` in the resource name field. The Jumpbox will be suffixed with **-jmp** (i.e. apmqy63-jmp)

    ![image](./media/02-01-e.png)

1. Click on the JumpBox VM and click `Connect`

    ![image](./media/02-01-f.png)

1. When prompted to Save or Open, choose `Open`

    ![image](./media/02-01-h.png)

1. When prompted to connect to remote session, choose `Connect`

    ![image](./media/2018-03-12_22-42-59.png)

1. When Prompted, choose `More Choices > Use a Different Account`

    ![image](./media/2018-03-12_22-44-04.png)

1. Enter the administrator credentials as follows:

    > User name: **appmig\appmigadmin**
    >
    > Password: **@pp_M!gr@ti0n-2018**

    ![image](./media/2018-03-12_22-46-56.png)

1. When prompted to accept the certificate, choose `Yes`

    ![image](./media/2018-03-12_22-47-12.png)

1. Open Server Manager and disable the Internet Explorer Enhanced Security Configuration

    ![image](./media/2018-03-12_23-08-18.png)

1. Choose `Off` for Administrators and click `Ok`

    ![image](./media/2018-03-12_23-09-24.png)

##### Install Required PowerShell Modules

1. Open a PowerShell session

    ![image](./media/2018-03-12_22-51-28.png)

1. Install Active Directory and DNS remote administration tools

    ```powershell
    Add-WindowsFeature -Name RSAT-AD-PowerShell
    Add-WindowsFeature -Name RSAT-DNS-Server

    ```

##### Creating Service Account (used in the application pools later)

1. Create the service account that will be assigned to the application pools later

    ```powershell
    New-ADUser -SamAccountName AppsSvcAcct -Name "AppsSvcAcct" -UserPrincipalName AppsSvcAcct@appmig.local -AccountPassword (ConvertTo-SecureString -AsPlainText "@pp_M!gr@ti0n-2018" -Force) -Enabled $true -PasswordNeverExpires $true

    ```

##### Add DNS records for each application

1. Create DNS entries (A record) for each web site

   ````powershell
    $dc = Get-ADDomainController

    Add-DnsServerPrimaryZone -NetworkID "10.0.0.0/16" -ReplicationScope "Forest" -ResponsiblePerson "appmigadmin@appmig.local" -DynamicUpdate Secure -ComputerName $dc.hostname -Verbose -ErrorAction SilentlyContinue

    $webSrv = Get-DnsServerResourceRecord -ComputerName $dc.hostname -ZoneName $dc.domain | Where-Object {$_.hostname -like "*-web"}

    Add-DnsServerResourceRecordA -Name timetracker -IPv4Address $webSrv.RecordData.IPv4Address.IPAddressToString -ZoneName $dc.Domain -CreatePtr -ComputerName $dc.HostName
    Add-DnsServerResourceRecordA -Name classifieds -IPv4Address $webSrv.RecordData.IPv4Address.IPAddressToString -ZoneName $dc.Domain -CreatePtr -ComputerName $dc.HostName
    Add-DnsServerResourceRecordA -Name jobs -IPv4Address $webSrv.RecordData.IPv4Address.IPAddressToString -ZoneName $dc.Domain -CreatePtr -ComputerName $dc.HostName

    ````

##### Download, extract and copy workshop materials locally

1. If not already in a session, open a `PowerShell window`

1. Clone the GitHub repository to the root

    ````powershell
    cd\
    git clone https://github.com/AzureCAT-GSI/AppMigrationWorkshop.git

    ````
1. Extract each application

    ````powershell
    [System.Reflection.Assembly]::LoadWithPartialName('System.IO.Compression.FileSystem')

    Get-ChildItem "C:\AppMigrationWorkshop\Shared\SourceApps\Apps\" -Exclude "petshop" | `
            % { $dest = Join-Path $_.directoryname ([system.io.path]::GetFileNameWithoutExtension($_.name)); `
                mkdir $dest -force; [System.IO.Compression.ZipFile]::ExtractToDirectory($_.fullname, $dest)}

    ````
1. Copying the database backup files to the SQL server

    ```powershell
    copy-item "C:\AppMigrationWorkshop\Shared\SourceApps\Databases\" -Destination \\10.0.1.100\c$ -Recurse

    ```

1. Copying the application source files to the IIS server

    ```powershell
    copy-item "C:\AppMigrationWorkshop\Shared\SourceApps\Apps\" -Destination \\10.0.0.4\c$ -Recurse

    ```
---

### Exercise 2: Configuration Steps on SQL Server<a name="ex2"></a>

These configuration steps will be performed from the SQL server. You can access this machine from the JumpBox as the servers are not publicly accessible.

1. In the Azure Portal, locate the machine name of the SQL server. The machine will suffixed with `-sql`. **Copy the machine name to the clipboard**.

    ![image](./media/2018-03-13_8-20-02.png)

1. From the JumpBox, start a remote desktop connection to the `SQL Server` machine

    ![image](./media/2018-03-13_8-15-41.png)

1. Enter the SQL server VM (**paste from the previous copy since the names will all be different in each deployment**) name and click `Connect`. Enter the Administrator credentials and click `Ok`

    > Note: **User name here MUST have the domain name**, otherwise these steps will fail. E.g. `APPMIG\appmigadmin`

    ![image](./media/2018-03-13_8-26-05.png)

1. Once on the SQL server, open a PowerShell session and Issue the following commands to restore the databases

    ````powershell
    SQLCMD -E -S $($ENV:COMPUTERNAME) -Q "RESTORE DATABASE [TimeTracker] FROM DISK='C:\Databases\timetracker.bak' WITH MOVE 'tempname' TO 'C:\Databases\timetracker.mdf', MOVE 'TimeTracker_Log' TO 'C:\Databases\timetracker_log.ldf'"
    SQLCMD -E -S $($ENV:COMPUTERNAME) -Q "RESTORE DATABASE [Classifieds] FROM DISK='C:\Databases\classifieds.bak' WITH MOVE 'Database' TO 'C:\Databases\classifieds.mdf', MOVE 'Database_log' TO 'C:\Databases\classifieds_log.ldf'"
    SQLCMD -E -S $($ENV:COMPUTERNAME) -Q "RESTORE DATABASE [Jobs] FROM DISK='C:\Databases\jobs.bak' WITH MOVE 'EmptyDatabase' TO 'C:\Databases\jobs.mdf', MOVE 'EmptyDatabase_log' TO 'C:\Databases\jobs_log.ldf'"

    ````

1. Creating the SQL logins

    ````powershell
    SQLCMD -E -S $($ENV:COMPUTERNAME) -Q "CREATE LOGIN [APPMIG\AppsSvcAcct] FROM WINDOWS"

    ````

1. Create users in the database

    ````powershell
    SQLCMD -E -S $($ENV:COMPUTERNAME) -Q "USE timetracker; CREATE USER [APPMIG\AppsSvcAcct];"
    SQLCMD -E -S $($ENV:COMPUTERNAME) -Q "USE classifieds; CREATE USER [APPMIG\AppsSvcAcct];"
    SQLCMD -E -S $($ENV:COMPUTERNAME) -Q "USE jobs; CREATE USER [APPMIG\AppsSvcAcct];"

    ````

1. Configure the user as db_owner

    ````powershell
    SQLCMD -E -S $($ENV:COMPUTERNAME) -Q "USE timetracker; EXEC sp_addrolemember 'db_owner', 'APPMIG\AppsSvcAcct'"
    SQLCMD -E -S $($ENV:COMPUTERNAME) -Q "USE classifieds; EXEC sp_addrolemember 'db_owner', 'APPMIG\AppsSvcAcct'"
    SQLCMD -E -S $($ENV:COMPUTERNAME) -Q "USE jobs; EXEC sp_addrolemember 'db_owner', 'APPMIG\AppsSvcAcct'"

    ````

---

### Exercise 3: Configuration Steps on Web Server<a name="ex3"></a>

These configuration steps will be performed from the Web server. You can access this machine from the JumpBox as the servers are not publicly accessible.

1. In the Azure Portal, locate the machine name of the Web server. The machine will suffixed with `-web`. Copy the machine name to the clipboard.

    ![image](./media/2018-03-18_19-33-48.png)

1. From the JumpBox, start a remote desktop connection to the `Web Server` machine

    ![image](./media/2018-03-13_8-15-41.png)

1. Enter the Web server VM name and click `Connect`. Enter the Administrator credentials and click `Ok`. Please notice that this is the same case of all other VMs, you must precede the username with the domain name APPMIG.

    ![image](./media/2018-03-18_19-35-59.png)

1. Once on the Web Server, open a PowerShell session

1. Issue the following commands to configure the IIS Application Pools

    ````powershell
    c:\windows\system32\inetsrv\appcmd.exe add apppool /name:"TimeTrackerAppPool" /managedPipelineMode:"Integrated"
    c:\windows\system32\inetsrv\appcmd.exe add apppool /name:"ClassifiedsAppPool" /managedPipelineMode:"Classic"
    c:\windows\system32\inetsrv\appcmd.exe add apppool /name:"JobsAppPool" /managedPipelineMode:"Integrated"

    ````

1. Grant the necessary permissions for the service account

    ````powershell
    c:\Windows\Microsoft.NET\Framework\v2.0.50727\aspnet_regiis.exe -ga appmig\AppsSvcAcct

    ````

1. Configure Application Pools to use the service account

    ````powershell
    c:\windows\system32\inetsrv\appcmd set config /section:applicationPools "/[name='TimeTrackerAppPool'].processModel.identityType:SpecificUser" "/[name='TimeTrackerAppPool'].processModel.userName:appmig\AppsSvcAcct" "/[name='TimeTrackerAppPool'].processModel.password:@pp_M!gr@ti0n-2018"
    c:\windows\system32\inetsrv\appcmd set config /section:applicationPools "/[name='ClassifiedsAppPool'].processModel.identityType:SpecificUser" "/[name='ClassifiedsAppPool'].processModel.userName:appmig\AppsSvcAcct" "/[name='ClassifiedsAppPool'].processModel.password:@pp_M!gr@ti0n-2018"
    c:\windows\system32\inetsrv\appcmd set config /section:applicationPools "/[name='JobsAppPool'].processModel.identityType:SpecificUser" "/[name='JobsAppPool'].processModel.userName:appmig\AppsSvcAcct" "/[name='JobsAppPool'].processModel.password:@pp_M!gr@ti0n-2018"

    ````

1. Delete the default web site to avoid conflicts

    ````powershell
    c:\windows\system32\inetsrv\appcmd delete site "Default Web Site"
    `````

1. Create the IIS web sites

    ````powershell
    c:\windows\system32\inetsrv\APPCMD add site /name:TimeTracker /id:1 /bindings:http://timetracker:80 /physicalPath:C:\Apps\TimeTracker
    c:\windows\system32\inetsrv\APPCMD set site TimeTracker "/[path='/'].applicationPool:TimeTrackerAppPool"
    c:\windows\system32\inetsrv\APPCMD add site /name:Classifieds /id:2 /bindings:http://classifieds:80 /physicalPath:C:\Apps\Classifieds
    c:\windows\system32\inetsrv\APPCMD set site Classifieds "/[path='/'].applicationPool:ClassifiedsAppPool"
    c:\windows\system32\inetsrv\APPCMD add site /name:Jobs /id:3 /bindings:http://jobs:80 /physicalPath:C:\Apps\Jobs
    c:\windows\system32\inetsrv\APPCMD set site Jobs "/[path='/'].applicationPool:JobsAppPool"

    ````

1. On the file system, update the web.config files for each source apps to connect to the database. The files are in the `C:\Apps` folder. Replace all occurrences of `<sqlServerName>` with your SQL Server VM name

    * C:\Apps\Jobs\Web.Config
    * C:\Apps\Classifieds\Web.Config
    * c:\Apps\TimeTracker\Web.Config

---

### Exercise 4: Test the web applications<a name="ex4"></a>

This configuration is performed from the Jump Box

1. Disable the IE Enhanced Security Configuration to allow local browsing from the web server. Click `Server Manager`

    ![image](./media/2018-03-18_1-43-14.png)

1. Locate the Security Information section and click `Configure IE ESC`

    ![image](./media/2018-03-18_1-43-44.png)

1. Under Administrators, select `Off` then `Ok`

    ![image](./media/2018-03-18_1-44-06.png)

1. Open the browser and navigate to the following URLs to ensure the sites are functioning:

    * http://timetracker
    * http://classifieds
    * http://jobs

## Summary

In this hands-on lab, you learned how to:

* Configure the legacy applications

---
Copyright 2016 Microsoft Corporation. All rights reserved. Except where otherwise noted, these materials are licensed under the terms of the MIT License. You may use them according to the license as is most appropriate for your project. The terms of this license can be found at https://opensource.org/licenses/MIT.

