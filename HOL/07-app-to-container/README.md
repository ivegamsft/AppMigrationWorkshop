# Windows Containers:

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

---

This hands-on-lab has the following exercises:

1. [Exercise 1: Install Image2Docker Community Tool](#ex1)
1. [Exercise 2: Containerize Web Apps](#ex2)
1. [Exercise 3: Cut over/Finalize Migration](#ex3)
1. [Exercise 4: Add Docker support to a legacy application](#ex4)

### Exercise 1: Install Image2Docker Community Tool<a name="ex1"></a>

---

1. RDP into your Windows Container Host. In this case, the server is named `host1.contoso.com`, open `PowerShell`, and execute the following command

    ```powershell
    Install-Module Image2Docker
    Import-Module Image2Docker
    ```

### Exercise 2: Containerize Web Apps<a name="ex2"></a>

---

In this exercise we will containerize the applications that we deployed in HOL 2.

The `Image2docker` tool for IIS and ASP.NET sites has 3 different Source Types that can be inspected. More details can be found at the [image2docker git hub repo](https://github.com/docker/communitytools-image2docker-win/blob/master/docs/IIS.md)

1. Disk Images

    ```powershell
    ConvertTo-Dockerfile `
    -ImagePath c:\iis.vhd `
    -OutputPath c:\i2d2\iis `
    -Artifact IIS
    ```

1. Local Machine

    ```powershell
    ConvertTo-Dockerfile `
    -Local `
    -OutputPath c:\i2d2\iis `
    -Artifact IIS
    ```

1. Remote Machine

    ```powershell
        ConvertTo-Dockerfile `
        -RemotePath \\192.168.1.11\c$ `
        -OutputPath c:\i2d2\iis `
        -Artifact IIS
    ```

    Now that we have image2docker installed, and we have looked briefly at the usage of the tool, we will now use the image2docker tool from our Windows Container host to inspect our IIS/ASP.NET web sites on a Remote Machine.

1. RDP into your IIS Source ASP.NET Web App Host. In this case, the server is named `aw-webapp.contoso.com` and open the IIS Manager, you should see all the source apps deployed.
    
    ![image](./media/07-a-1.png)

1. Navigate to each of the sites to make sure they are up and running

    * http://classifieds
    * http://jobs
    * http://ibuyspy
    * http://petshop
    * http://timetracker

1. While on the web app host, identify the IP address of the server, we will need that for image2docker.

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

1. RDP into your Windows Container Host. In this case, the server is named `host1.contoso.com`

1. From powershell execute the following commands
   
    ```powershell
    cd c:\
    mkdir dockerimages
    cd dockerimages
    mkdir jobs, petshop, ibuyspy, timetracker, classifieds
    ```

1. Containerize each of the apps individually

    ```powershell
    #Jobs
    ConvertTo-Dockerfile -RemotePath \\10.0.1.11\c$ -OutputPath c:\dockerimages\jobs -Artifact IIS -ArtifactParam Jobs
    #PetShop
    ConvertTo-Dockerfile -RemotePath \\10.0.1.11\c$ -OutputPath c:\dockerimages\petshop -Artifact IIS -ArtifactParam PetShop
    #IBuySpy
    ConvertTo-Dockerfile -RemotePath \\10.0.1.11\c$ -OutputPath c:\dockerimages\ibuyspy -Artifact IIS -ArtifactParam ibuyspy
    #TimeTracker
    ConvertTo-Dockerfile -RemotePath \\10.0.1.11\c$ -OutputPath c:\dockerimages\timetracker -Artifact IIS -ArtifactParam TimeTracker
    #Classified
    ConvertTo-Dockerfile -RemotePath \\10.0.1.11\c$ -OutputPath c:\dockerimages\classifieds -Artifact IIS -ArtifactParam Classifieds
    ```

1. Build the Docker Images
        
    ```powershell
    cd c:\dockerimages
    docker build -t jobs-site ./jobs
    docker build -t petshop-site ./petshop
    docker build -t ibuyspy-site ./ibuyspy
    docker build -t timetracker-site ./timetracker
    docker build -t classifieds-site ./classifieds
    ```

    >
    > This will take some time to build the images once the images are done Docker images
    >

1. Once the builds are complete, you should see something like this:

    ```powershell
    REPOSITORY          TAG                                     IMAGE ID            CREATED             SIZE
    classifieds-site    latest                                  608d1205dc2d        7 minutes ago       14.3GB
    timetracker-site    latest                                  54e6a9eb4975        8 minutes ago       14.3GB
    ibuyspy-site        latest                                  ce135e7ae63c        10 minutes ago      14.3GB
    petshop-site        latest                                  70f8d2016397        11 minutes ago      14.3GB
    jobs-site           latest                                  5f529280222e        13 minutes ago      14.3GB
    microsoft/iis       latest                                  6608c8e5b344        6 weeks ago         10.7GB
    microsoft/aspnet    3.5-windowsservercore-10.0.14393.1715   b2a36e946a02        5 months ago        14.1GB
    ```

1. Let's run a container for IBuySpy
    
    ```powershell
    docker run -d -p 80:80 ibuyspy-site
    docker ps
    # You should see something like this. Make note of the name
    CONTAINER ID        IMAGE               COMMAND                   CREATED              STATUS              PORTS                NAMES
    5ff3a058f09f        ibuyspy-site        "C:\\ServiceMonitor..."   About a minute ago   Up About a minute   0.0.0.0:80->80/tcp   happy_poincare

    docker inspect -f "{{ .NetworkSettings.Networks.nat.IPAddress }}" happy_poincare
    # You should see something like this
    172.19.249.182
    ```
  
1. Now open a browser and navigate to http://172.19.249.182. You should now see the `IBuySpy` Web site running inside the container.
    
    ![image](./media/08-a-2.png)
    
### Exercise 3:  Cut over/Finalize Migration<a name="ex3"></a>

---

Now that we have an application up and running in a container on our Container host, the last step is you to finalize the migration. We will do that by simply cutting over the routing and DNS information

1. Update the DNS records by RDP-ing into the DNS/AD Server, in this case the server is 'dc1.contoso.com' and open DNS. Here we can see that the ibusyspy dns record is a cname pointing to 'aw-webapp.contoso.com'

    ![image](./media/07-a-3.png)

1. Update the record to point to the Windows Container host, in this case 'host1.contoso.com'
    
    ![image](./media/07-a-4.png)

1. Open a browser to verify that you can indeed get to the site.
    
    ![image](./media/07-a-5.png)

### Exercise 4: Add Docker support to an existing application<a name="ex4"></a>

---

In this HOL we will go through pulling the solution into Visual Studio 2017 and add support for Docker Containers. In this lab we will focus on the Jobs source application, but the treatment will be similar for each source application

#### Assumptions

* You have Visual Studio 2017 Installed or are accessing the application from the jump box
* You have the source solution downloaded from the Repo
* You have .NET 3.5 Installed

1. Open Visual Studio 2017
1. Create an Empty Web Site by File > New > Project > Visual C# > Web > Web Site > ASP.NET Empty Web Site. Name the project JobWebSite.
![image](./media/hol7-4-a.png)
1. Open Windows Explorer and copy the Jobs Web Site Files
![image](./media/hol7-4-b.png)
1. Paste the files into the JobWebSite project in Visual Studio 2017
![image](./media/hol7-4-c.png)
1. All the files should now be in the solution/project. 
![image](./media/hol7-4-d.png)
1. You can remove the uneeded files "ProjectName.webproj" and "MyTemplate.vstemplate"
1. Add a blank text file to the Solition, Not the Web Site and rename it to Dockerfile, make sure to remove the ".txt" extension.
![image](./media/hol7-4-e.png)
![image](./media/hol7-4-f.png)

1. Add the following to the Dockerfile
    ```Docker
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

    ```
1. Copy the solution from your Dev VM to the Windows Container Host. In this case it has been copied to 'C:\upgrades\JobsWebSite'. Open a command prompt or Powershell and run the following command:
    ```powershell
    docker build -t jobswebsite ./
    ```
1. Now run a container using the image
    ```powershell
        docker run -d -p 80:80 jobswebsite
    ```
## References
* [Image2Docker](https://github.com/docker/communitytools-image2docker-win)
* [Image2Docker IIS and ASP.NET](https://github.com/docker/communitytools-image2docker-win/blob/master/docs/IIS.md)

## Summary

In this hands-on lab, you learned how to:

* Containerize and existing application using the Image2Docker community tools
* Add Docker support to an existing legacy application


---
Copyright 2016 Microsoft Corporation. All rights reserved. Except where otherwise noted, these materials are licensed under the terms of the MIT License. You may use them according to the license as is most appropriate for your project. The terms of this license can be found at https://opensource.org/licenses/MIT.


