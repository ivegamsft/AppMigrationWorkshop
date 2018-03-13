## (WORK IN PROGRESS - VERY DRAFT, PLACEHOLDER ONLY FOR THE POWERSHELL/OTHER COMMANDS AT THIS MOMENT)

# Setting up the source applications

## Overview
---
In this lab, you will install three source applications. The sample legacy applications will be used as the source for migrating to Azure.

Applications
* Timetracker
* Classifieds
* Jobs

## Prerequisites
---
* An active Azure Subscription
* You have the working source environment deployed on [Lab 01](../01-setup/README.md)

## Excercises
---
This hands-on-lab has the following excercises:
1. [Exercise 1: Configuration Steps on Jumpbox VM](#ex1)
1. [Exercise 2: Configuration Steps on SQL Server](#ex2)
1. [Exercise 3: Configuration Steps on Web Server](#ex3)

### Exercise 1: Configuration Steps on Jumpbox VM<a name="ex1"></a>
---

#### Sign on to Jump Box VM from Azure Portal

1. Go to the Azure Portal (http://portal.azure.com)
1. Sign on with "Microsoft Account" or "Work or School Account"

    ![image](./media/02-01-a.png)

    ![image](./media/02-01-b.png)

1. In the portal, in the left navigation pane, click `Resource Groups` 

    ![image](./media/02-01-c.png)
 
1. From the Resource Group list, select the one deployed in HOL 1 (e.g. AppModernization-RG)

    ![image](./media/02-01-d.png)

1. Locate the `Jumpbox` Virtual Machine by searching for `jmp` in the resource name field. The Jumpbox will be suffixed with **-jmp** (i.e apmqy63-jmp)

    ![image](./media/02-01-e.png)

1. Click on the JumpBox VM and click `Connect`

    ![image](./media/02-01-f.png)

1. When prompted, choose `Open`

    ![image](./media/02-01-h.png)

1. When prompted, choose `Connect`

    ![image](./media/2018-03-12_22-42-59.png)

1. When Prompted, choose `More Choices > Use a Different Account`

    ![image](./media/2018-03-12_22-44-04.png)

1. Enter the adminstrator credentials as follows:

    > User name: appmig\appmigadmin
    >
    > Password: @pp_M!gr@ti0n-2018

    ![image](./media/2018-03-12_22-46-56.png)

1. When prompted, choose `Yes`

    ![image](./media/2018-03-12_22-47-12.png)

1. Open Server Manager and disable the Internet Explorer Enhanced Security Configuration

    ![image](./media/2018-03-12_23-08-18.png)

1. Choose `Off` for Administrators and click `Ok`

    ![image](./media/2018-03-12_23-09-24.png)

#### Install Required PowerShell Modules

1. Open a PowerShell command prompt

    ![image](./media/2018-03-12_22-51-28.png)

1. Install Active Directory and DNS remote administration tools
    ```
    Add-WindowsFeature -Name RSAT-AD-PowerShell
    Add-WindowsFeature -Name RSAT-DNS-Server
    ```

#### Creating Service Account (used in the application pools later)

1. Create the service account that will be assigned to the application pools later

    ```powershell
    New-ADUser -SamAccountName AppsSvcAcct -Name "AppsSvcAcct" -UserPrincipalName AppsSvcAcct@appmig.local -AccountPassword (ConvertTo-SecureString -AsPlainText "@pp_M!gr@ti0n-2018" -Force) -Enabled $true -PasswordNeverExpires $true
    ```

#### Adding DNS records for each application

1. Create DNS entries (A record) for each web site
   ```powershell
    $dc = Get-ADDomainController
    
    Add-DnsServerPrimaryZone -NetworkID "10.0.0.0/16" -ReplicationScope "Forest" -ResponsiblePerson "appmigadmin@appmig.local" -DynamicUpdate Secure -ComputerName $dc.hostname -Verbose -ErrorAction SilentlyContinue

    $webSrv = Get-DnsServerResourceRecord -ComputerName $dc.hostname -ZoneName $dc.domain | Where-Object {$_.hostname -like "*-web"}

    Add-DnsServerResourceRecordA -Name timetracker -IPv4Address $webSrv.RecordData.IPv4Address.IPAddressToString -ZoneName $dc.Domain -CreatePtr -ComputerName $dc.HostName
    Add-DnsServerResourceRecordA -Name classifieds -IPv4Address $webSrv.RecordData.IPv4Address.IPAddressToString -ZoneName $dc.Domain -CreatePtr -ComputerName $dc.HostName
    Add-DnsServerResourceRecordA -Name jobs -IPv4Address $webSrv.RecordData.IPv4Address.IPAddressToString -ZoneName $dc.Domain -CreatePtr -ComputerName $dc.HostName
    
    ```

#### Downloading, extracting and copying locally workshop materials

1. Download the Workshop materials to the JumpBox. Open a `PowerShell window`

1. Clone the GitHub repository to the root

    ````powershell
    cd\
    git clone https://github.com/AzureCAT-GSI/AppMigrationWorkshop.git
    ````

1. Extract each application

    ````powershell
    [System.Reflection.Assembly]::LoadWithPartialName('System.IO.Compression.FileSystem')
    Get-ChildItem "C:\AppMigrationWorkshop\Shared\SourceApps\Apps\" -Exclude "*.msi" `
        | % {  $dest = Join-Path $_.directoryname ([system.io.path]::GetFileNameWithoutExtension($_.name)); `
		mkdir $dest -force; `[System.IO.Compression.ZipFile::ExtractToDirectory($_.fullname, $dest); `
		del $_.fullname -force }
    ````

1. Copying the database backup files to the SQL server

    ```powershell
    copy-item "C:\AppMigrationWorkshop-master\Shared\SourceApps\Databases\" \\10.0.1.100\c$ -Recurse
    ```

1. Copying the application source files to the IIS server
    ```powershell
    copy-item "C:\AppMigrationWorkshop-master\Shared\SourceApps\Apps\" \\10.0.0.4\c$ -Recurse
    ```

### Exercise 2: Configuration Steps on SQL Server<a name="ex2"></a>

1. Begin a remote sesion to the `SQL Server` machine

1. Open a command line

1. Issue the following commands to restore the databases
    
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

### Exercise 3: Configuration Steps on Web Server<a name="ex3"></a>

1. Begin a remote session to the IIS Server

1. Open a command prompt

1. Issue the following commands to configure the IIS Application Pools


    ````powershell
    c:\windows\system32\inetsrv\appcmd.exe add apppool /name:"TimeTrackerAppPool" /managedPipelineMode:"Integrated"
    c:\windows\system32\inetsrv\appcmd.exe add apppool /name:"ClassifiedsAppPool" /managedPipelineMode:"Classic"
    c:\windows\system32\inetsrv\appcmd.exe add apppool /name:"JobsAppPool" /managedPipelineMode:"Integrated"
    ````


1. Grane the necessary permissions for the service account

    ````powershell
    c:\Windows\Microsoft.NET\Framework\v2.0.50727\aspnet_regiis.exe -ga appmig\AppsSvcAcct
    ````

1. Configure Application Pools to use the service account

    ````powershell
    c:\windows\system32\inetsrv\appcmd set config /section:applicationPools /[name='TimeTrackerAppPool'].processModel.identityType:SpecificUser /[name='TimeTrackerAppPool'].processModel.userName:AppsSvcAcct /[name='TimeTrackerAppPool'].processModel.password:@pp_M!gr@ti0n-2018
    c:\windows\system32\inetsrv\appcmd set config /section:applicationPools /[name='ClassifiedsAppPool'].processModel.identityType:SpecificUser /[name='ClassifiedsAppPool'].processModel.userName:AppsSvcAcct /[name='ClassifiedsAppPool'].processModel.password:@pp_M!gr@ti0n-2018
    c:\windows\system32\inetsrv\appcmd set config /section:applicationPools /[name='JobsAppPool'].processModel.identityType:SpecificUser /[name='JobsAppPool'].processModel.userName:AppsSvcAcct /[name='JobsAppPool'].processModel.password:@pp_M!gr@ti0n-2018
    ````

1. Delete the default web site to avoid conflicts

    ````powershell
    c:\windows\system32\inetsrv\appcmd delete site "Default Web Site"
    `````

1. Create the IIS web sites

    ````powershell
    c:\windows\system32\inetsrv\APPCMD add site /name:TimeTracker /id:1 /bindings:http://timetracker:80: /physicalPath:C:\Apps\TimeTracker
    c:\windows\system32\inetsrv\APPCMD set site TimeTracker /[path='/'].applicationPool:"TimeTrackerAppPool"

    c:\windows\system32\inetsrv\APPCMD add site /name:Classifieds /id:2 /bindings:http://classifieds:80: /physicalPath:C:\Apps\Classifieds
    c:\windows\system32\inetsrv\APPCMD set site Classifieds /[path='/'].applicationPool:"ClassifiedsAppPool"

    c:\windows\system32\inetsrv\APPCMD add site /name:Jobs /id:3 /bindings:http://jobs:80: /physicalPath:C:\Apps\Jobs
    c:\windows\system32\inetsrv\APPCMD set site Jobs /[path='/'].applicationPool:"JobsAppPool"
    ````
    
### Exercise 4: Test the web applications<a name="ex4"></a>

1. From the Jumpbox, open a browser

1. Navigate to the following URLs:

    * http://timetracker
    * http://classifieds
    * http://jobs

## Summary

In this hands-on lab, you learned how to:

* Configure the legacy applications

---
Copyright 2016 Microsoft Corporation. All rights reserved. Except where otherwise noted, these materials are licensed under the terms of the MIT License. You may use them according to the license as is most appropriate for your project. The terms of this license can be found at https://opensource.org/licenses/MIT.