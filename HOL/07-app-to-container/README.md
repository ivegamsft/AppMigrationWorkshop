# Windows Containers

## Overview

In this lab, you will learn how to:

* Containerize and existing application using the Image2Docker community tools
* Add Docker support to an existing legacy application

## Prerequisites

* Ensure you have completed the previous labs
* Ensure you have installed the following:
  * Powershell 5.0 or later is required for Image2Docker
  * Docker

## Exercises

This hands-on-lab has the following exercises:

1. [Exercise 1: Gather information for source apps](#ex1)
1. [Exercise 2: Install Image2Docker Community Tool](#ex2)
1. [Exercise 3: Containerize Web Apps](#ex3)
1. [Exercise 4: Cut over/Finalize Migration](#ex4)
1. [Exercise 5: Add Docker support to a legacy application](#ex5)

### Exercise 1: Gather information for source apps<a name="ex1"></a>

1. In the Azure Portal, locate the machine name of the Web server. The machine will suffixed with `-web`. Copy the machine name to the clipboard.

    ![image](./media/2018-03-18_19-33-48.png)

1. From the JumpBox, start a remote desktop connection to the `Web Server` machine

    ![image](./media/2018-03-13_8-15-41.png)

1. Enter the Web Server VM name and click `Connect`. Enter the Administrator credentials and click `Ok`

    ![image](./media/2018-03-18_19-35-59.png)

1. open the IIS Manager, you should see all the source apps deployed.

    ![image](./media/07-a-1.PNG)

1. Navigate to each of the sites to make sure they are up and running

    * http://classifieds
    * http://jobs
    * http://timetracker

1. While on the web app host, identify the IP address of the current server, we will need that for image2docker.

    ```powershell
    ipconfig

    Windows IP Configuration
    Ethernet adapter Local Area Connection:

    Connection-specific DNS Suffix  . : reddog.microsoft.com
    Link-local IPv6 Address . . . . . : fe80::c0f9:22f0:a23c:1688%12
    IPv4 Address. . . . . . . . . . . : 10.0.1.11
    Subnet Mask . . . . . . . . . . . : 255.255.255.0
    Default Gateway . . . . . . . . . : 10.0.1.1
    ```

### Exercise 2: Install Image2Docker Community Tool<a name="ex2"></a>

1. RDP into the `Windows Container Azure VM`. It is suffixed with `-cnt`

    ![image](./media/2018-03-18_6-20-25.png)

1. The `Image2docker` tool for IIS and ASP.NET sites has 3 different Source Types that can be inspected. More details can be found at the [image2docker git hub repo](https://github.com/docker/communitytools-image2docker-win/blob/master/docs/IIS.md)

    * Disk Images

        ````powershell
        ConvertTo-Dockerfile `
        -ImagePath c:\iis.vhd `
        -OutputPath c:\i2d2\iis `
        -Artifact IIS
        ````

    * Local Machine

        ````powershell
        ConvertTo-Dockerfile `
        -Local `
        -OutputPath c:\i2d2\iis `
        -Artifact IIS
        ````

    * Remote Machine

        ````powershell
            ConvertTo-Dockerfile `
            -RemotePath \\192.168.1.11\c$ `
            -OutputPath c:\i2d2\iis `
            -Artifact IIS
        ````

1. Open `PowerShell`, and execute the following command

    ```powershell
    Install-Module Image2Docker
    Import-Module Image2Docker
    ```

1. Now that we have image2docker installed, and we have looked briefly at the usage of the tool, we will now use the image2docker tool from our Windows Container host to inspect our IIS/ASP.NET web sites on a Remote Machine.

### Exercise 2: Containerize Web Apps<a name="ex2"></a>

In this exercise we will containerize the applications that we deployed in HOL 2.

1. If not already connected, RDP into the `Windows Container Azure VM`. It is suffixed with `-cnt`

    ![image](./media/2018-03-18_6-20-25.png)

1. From powershell execute the following commands

    ```powershell
    cd c:\
    mkdir dockerimages
    cd dockerimages
    mkdir jobs, timetracker, classifieds
    ```

1. Containerize each of the apps individually. Replace the IP address with the IP Address of your web server

    ````powershell
    #Jobs
    ConvertTo-Dockerfile -RemotePath \\[YOUR WEB SERVER IP ADDRESS]\c$ -OutputPath c:\dockerimages\jobs -Artifact IIS -ArtifactParam Jobs
    #TimeTracker
    ConvertTo-Dockerfile -RemotePath \\[YOUR WEB SERVER IP ADDRESS]\c$ -OutputPath c:\dockerimages\timetracker -Artifact IIS -ArtifactParam TimeTracker
    #Classifieds
    ConvertTo-Dockerfile -RemotePath \\[YOUR WEB SERVER IP ADDRESS]\c$ -OutputPath c:\dockerimages\classifieds -Artifact IIS -ArtifactParam Classifieds
    ````

1. The timetracker app requires a classic app pool. Let's change the docker file. Change if from:

    ````powershell
    RUN New-Website -Name 'TimeTracker' -PhysicalPath 'C:\Apps\TimeTracker' -Port 80 -ApplicationPool '.NET v2.0' -Force;
    ````

    To:
    ````powershell
    RUN New-Website -Name 'TimeTracker' -PhysicalPath 'C:\Apps\TimeTracker' -Port 80 -ApplicationPool 'Classic .NET AppPool' -Force;
    ````

1. TODO: ADD CHANGE TO USE APP CREDENTIALS

1. Build the Docker Images

    ````powershell
    cd c:\dockerimages
    docker build -t jobs-site ./jobs
    docker build -t timetracker-site ./timetracker
    docker build -t classifieds-site ./classifieds
    ````

    >
    > This will take some time to build the images once the images are done, run the following:

    ````powershell
    docker images
    ````
    >

1. Once the builds are complete, you should see something like this:

    ```powershell
    REPOSITORY          TAG                                     IMAGE ID            CREATED             SIZE
    classifieds-site    latest                                  608d1205dc2d        7 minutes ago       14.3GB
    timetracker-site    latest                                  54e6a9eb4975        8 minutes ago       14.3GB
    jobs-site           latest                                  5f529280222e        13 minutes ago      14.3GB
    microsoft/iis       latest                                  6608c8e5b344        6 weeks ago         10.7GB
    microsoft/aspnet    3.5-windowsservercore-10.0.14393.1715   b2a36e946a02        5 months ago        14.1GB
    ```

1. Let's run a container for TimeTracker

    ```powershell
    docker run -d -p 80:80 timetracker-site
    docker ps
    # You should see something like this. Make note of the name
    CONTAINER ID        IMAGE               COMMAND                   CREATED              STATUS              PORTS                NAMES
    5ff3a058f09f        timetracker-site        "C:\\ServiceMonitor..."   About a minute ago   Up About a minute   0.0.0.0:80->80/tcp   happy_poincare

    docker inspect -f "{{ .NetworkSettings.Networks.nat.IPAddress }}" happy_poincare
    # You should see something like this
    172.19.249.182
    ```

1. On the SQL machine, add the gSMA account to the databases

    ````sql
    CREATE LOGIN [appmig\chost-gsma$] FROM WINDOWS
    sp_addsrvRolemember "appmig\chost-gsma$", "sysadmin"
    USE [Jobs]
    GO
    CREATE USER [appmig\chost-gsma$] FOR LOGIN [appmig\chost-gsma$] WITH DEFAULT_SCHEMA=[dbo]
    GO
    USE [Timetracker]
    GO
    CREATE USER [appmig\chost-gsma$] FOR LOGIN [appmig\chost-gsma$] WITH DEFAULT_SCHEMA=[dbo]
    GO
    USE [Classifieds]
    GO
    CREATE USER [appmig\chost-gsma$] FOR LOGIN [appmig\chost-gsma$] WITH DEFAULT_SCHEMA=[dbo]
    GO
    ````

1. Now open a browser and navigate to http://[YOUR CONTAINER IP ADDRESS]. You should now see the `timetracker` Web site running inside the container.

    ![image](./media/07-a-2.PNG)

### Exercise 3:  Cut over/Finalize Migration<a name="ex3"></a>

Now that we have an application up and running in a container on our Container host, the last step is you to finalize the migration. We will do that by simply cutting over the routing and DNS information

1. Update the DNS records by RDP-ing into the DNS/AD Server, in this case the server is 'dc1.contoso.com' and open DNS. Here we can see that the `timetracker` dns record is a cname pointing to 'XXXXXXXXXX'

    ![image](./media/07-a-3.PNG)

1. Update the record to point to the Windows Container host, in this case 'XXXXXXXXXX'

    ![image](./media/07-a-4.PNG)

1. Open a browser to verify that you can indeed get to the site.

    ![image](./media/07-a-5.PNG)

### Exercise 4: Add Docker support to an existing application<a name="ex4"></a>

In this HOL we will go through creating the solution into Visual Studio 2017 and adding support for Docker Containers. We need to do this becuase the applicaiton was written in an over version of Visual Studio and we need to update it.
The `Jobs` application is used as the source, but the treatment will be similar for each source application

#### Assumptions

* You have Visual Studio 2017 Installed or are accessing the application from the jump box
* You have the source solution downloaded from the Repo
* You have .NET 3.5 Installed

1. Open Visual Studio 2017

1. Create an Empty Web Site by File > New > Project > Visual C# > Web > Web Site > ASP.NET Empty Web Site. Name the project JobWebSite.

    ![image](./media/hol7-4-a.PNG)

1. Open Windows Explorer and copy the Jobs Web Site Files

    ![image](./media/hol7-4-b.PNG)

1. Paste the files into the JobWebSite project in Visual Studio 2017

    ![image](./media/hol7-4-c.PNG)

1. All the files should now be in the solution/project.

    ![image](./media/hol7-4-d.PNG)

1. You can remove the uneeded files "ProjectName.webproj" and "MyTemplate.vstemplate"

1. Add a blank text file to the Solution, Not the Web Site and rename it to `Dockerfile`, make sure to remove the ".txt" extension.

    ![image](./media/hol7-4-e.PNG)

    ![image](./media/hol7-4-f.PNG)

1. Add the following to the Dockerfile

    ````docker
    # escape=`
    FROM microsoft/aspnet:3.5-windowsservercore-10.0.14393.1715
    SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

    RUN Remove-Website 'Default Web Site';

    # Set up website: Jobs
    RUN New-Item -Path 'C:\inetpub\wwwroot\Jobs' -Type Directory -Force;

    RUN New-Website -Name 'JobsWebSite' -PhysicalPath 'C:\inetpub\wwwroot\Jobs' -Port 80 -ApplicationPool '.NET v2.0' -Force;

    EXPOSE 80

    COPY ["JobsWebSite", "/inetpub/wwwroot/Jobs"]

    RUN $path='C:\inetpub\wwwroot\Jobs'; `
        $acl = Get-Acl $path; `
        $newOwner = [System.Security.Principal.NTAccount]('BUILTIN\IIS_IUSRS'); `
        $acl.SetOwner($newOwner); `
        dir -r $path | Set-Acl -aclobject  $acl
    ````

1. Copy the solution from your Dev VM to the Windows Container Host. In this case it has been copied to 'C:\upgrades\JobsWebSite'. Open a command prompt or Powershell and run the following command:

    ````powershell
    docker build -t jobswebsite ./
    ````

1. Now run a container using the image
    ````powershell
    docker run -d -p 80:80 jobswebsite
    ````

## References

* [Image2Docker](https://github.com/docker/communitytools-image2docker-win)
* [Image2Docker IIS and ASP.NET](https://github.com/docker/communitytools-image2docker-win/blob/master/docs/IIS.md)

## Summary

In this hands-on lab, you learned how to:

* Containerize and existing application using the Image2Docker community tools
* Add Docker support to an existing legacy application

----
Copyright 2016 Microsoft Corporation. All rights reserved. Except where otherwise noted, these materials are licensed under the terms of the MIT License. You may use them according to the license as is most appropriate for your project. The terms of this license can be found at https://opensource.org/licenses/MIT.