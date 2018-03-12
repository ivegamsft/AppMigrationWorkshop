## (WORK IN PROGRESS - VERY DRAFT, PLACEHOLDER ONLY FOR THE POWERSHELL/OTHER COMMANDS AT THIS MOMENT)

Outline for lab

* Create IIS app pool account
* Download source apps
* Install IIS web apps
* Restore SQL dbs
* Add DNS
* Run and test apps

# Setting up the source applications

## Overview
---
In this lab, you will install three source applications, acting as they are the intended applications to migrate to Azure using one the presented options in the next labs.

Applications
- Timetracker
- Classifieds
- Jobs

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

1. In the portal, in the left navigation pane, click on "Resource Groups" 

    ![image](./media/02-01-c.png)
 
1. From the Resource Group list that will show, please click the one you just deployed for the workshop (e.g. AppModernization)

    ![image](./media/02-01-d.png)

#### Install Required PowerShell Modules

1. Open a PowerShell command prompt
1. Install Active Directory remote administration tools
    ```
    Add-WindowsFeature -Name RSAT-AD-PowerShell
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

1. Download Workshop materials

    ```powershell
    (New-Object System.Net.WebClient).DownloadFile("https://github.com/AzureCAT-GSI/AppMigrationWorkshop/archive/master.zip",".\master.zip")
    ```

1. Extract materials to the root of c: drive
    ```powershell
    [System.Reflection.Assembly]::LoadWithPartialName('System.IO.Compression.FileSystem')
    [System.IO.Compression.ZipFile]::ExtractToDirectory(".\master.zip", "c:\")
    ```
1. Extracting each app
    ```powershell
    Get-ChildItem "C:\AppMigrationWorkshop-master\Shared\SourceApps\Apps\" -Exclude "*.msi" `
                             | % {  $dest = Join-Path $_.directoryname ([system.io.path]::GetFileNameWithoutExtension($_.name)); `
									    mkdir $dest -force; `
									    [System.IO.Compression.ZipFile]::ExtractToDirectory($_.fullname, $dest); `
									    del $_.fullname -force }
    ```
1. Copying databases to the SQL server
    ```powershell
    copy-item "C:\AppMigrationWorkshop-master\Shared\SourceApps\Databases\" \\10.0.1.100\c$ -Recurse
    ```

1. Copying applications to the IIS server
    ```powershell
    copy-item "C:\AppMigrationWorkshop-master\Shared\SourceApps\Apps\" \\10.0.0.4\c$ -Recurse
    ```


### Exercise 2: Configuration Steps on SQL Server<a name="ex2"></a>

    ```
    # Restoring DBs

    SQLCMD -E -S $($ENV:COMPUTERNAME) -Q "RESTORE DATABASE [TimeTracker] FROM DISK='C:\Databases\timetracker.bak' WITH MOVE 'tempname' TO 'C:\Databases\timetracker.mdf', MOVE 'TimeTracker_Log' TO 'C:\Databases\timetracker_log.ldf'"
    SQLCMD -E -S $($ENV:COMPUTERNAME) -Q "RESTORE DATABASE [Classifieds] FROM DISK='C:\Databases\classifieds.bak' WITH MOVE 'Database' TO 'C:\Databases\classifieds.mdf', MOVE 'Database_log' TO 'C:\Databases\classifieds_log.ldf'"
    SQLCMD -E -S $($ENV:COMPUTERNAME) -Q "RESTORE DATABASE [Jobs] FROM DISK='C:\Databases\jobs.bak' WITH MOVE 'EmptyDatabase' TO 'C:\Databases\jobs.mdf', MOVE 'EmptyDatabase_log' TO 'C:\Databases\jobs_log.ldf'"

    # Creating login
    SQLCMD -E -S $($ENV:COMPUTERNAME) -Q "CREATE LOGIN [APPMIG\AppsSvcAcct] FROM WINDOWS"

    # Creating users in the database

    SQLCMD -E -S $($ENV:COMPUTERNAME) -Q "USE timetracker; CREATE USER [APPMIG\AppsSvcAcct];"
    SQLCMD -E -S $($ENV:COMPUTERNAME) -Q "USE classifieds; CREATE USER [APPMIG\AppsSvcAcct];"
    SQLCMD -E -S $($ENV:COMPUTERNAME) -Q "USE jobs; CREATE USER [APPMIG\AppsSvcAcct];"

    # Adding user to db_owner
    SQLCMD -E -S $($ENV:COMPUTERNAME) -Q "USE timetracker; EXEC sp_addrolemember 'db_owner', 'APPMIG\AppsSvcAcct'"
    SQLCMD -E -S $($ENV:COMPUTERNAME) -Q "USE classifieds; EXEC sp_addrolemember 'db_owner', 'APPMIG\AppsSvcAcct'"
    SQLCMD -E -S $($ENV:COMPUTERNAME) -Q "USE jobs; EXEC sp_addrolemember 'db_owner', 'APPMIG\AppsSvcAcct'"
    ```

### Exercise 3: Configuration Steps on Web Server<a name="ex3"></a>
On IIS Server
    
    ```

    # Application Pool
    c:\windows\system32\inetsrv\appcmd.exe add apppool /name:"TimeTrackerAppPool" /managedPipelineMode:"Integrated"
    c:\windows\system32\inetsrv\appcmd.exe add apppool /name:"ClassifiedsAppPool" /managedPipelineMode:"Classic"
    c:\windows\system32\inetsrv\appcmd.exe add apppool /name:"JobsAppPool" /managedPipelineMode:"Integrated"

    # Give necessary permissions for the service account
    c:\Windows\Microsoft.NET\Framework\v2.0.50727\aspnet_regiis.exe -ga appmig\AppsSvcAcct

    # Configure Application Pools to use the service account

    c:\windows\system32\inetsrv\appcmd set config /section:applicationPools /[name='TimeTrackerAppPool'].processModel.identityType:SpecificUser /[name='TimeTrackerAppPool'].processModel.userName:AppsSvcAcct /[name='TimeTrackerAppPool'].processModel.password:@pp_M!gr@ti0n-2018
    c:\windows\system32\inetsrv\appcmd set config /section:applicationPools /[name='ClassifiedsAppPool'].processModel.identityType:SpecificUser /[name='ClassifiedsAppPool'].processModel.userName:AppsSvcAcct /[name='ClassifiedsAppPool'].processModel.password:@pp_M!gr@ti0n-2018
    c:\windows\system32\inetsrv\appcmd set config /section:applicationPools /[name='JobsAppPool'].processModel.identityType:SpecificUser /[name='JobsAppPool'].processModel.userName:AppsSvcAcct /[name='JobsAppPool'].processModel.password:@pp_M!gr@ti0n-2018

    # Deleting default web site

    c:\windows\system32\inetsrv\appcmd delete site "Default Web Site"

    # Creating web sites

    c:\windows\system32\inetsrv\APPCMD add site /name:TimeTracker /id:1 /bindings:http://timetracker:80: /physicalPath:C:\Apps\TimeTracker
    c:\windows\system32\inetsrv\APPCMD set site TimeTracker /[path='/'].applicationPool:"TimeTrackerAppPool"

    c:\windows\system32\inetsrv\APPCMD add site /name:Classifieds /id:2 /bindings:http://classifieds:80: /physicalPath:C:\Apps\Classifieds
    c:\windows\system32\inetsrv\APPCMD set site Classifieds /[path='/'].applicationPool:"ClassifiedsAppPool"

    c:\windows\system32\inetsrv\APPCMD add site /name:Jobs /id:3 /bindings:http://jobs:80: /physicalPath:C:\Apps\Jobs
    c:\windows\system32\inetsrv\APPCMD set site Jobs /[path='/'].applicationPool:"JobsAppPool"
    ```
