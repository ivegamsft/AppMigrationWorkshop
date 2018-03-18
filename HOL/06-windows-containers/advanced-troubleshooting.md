# Windows Containers - Advanced Troubleshooting

## Overview

In this lab, you will learn how to:

* Configure your containers for troubleshooting

## Prerequisites

* Ensure you have completed the previous labs

## Exercises

1. [Configure IIS Management inside container](#ex1)
1. [Configure Kerberos Logging inside container](#ex2)
1. [Export event logs from container](#ex3)
1. [Remote Debugging in the container](#ex4)

### Exercise 1: Configure IIS Management inside the container<a name="ex1"></a>

These are only needed for accessing the IIS admin UI

1. RDP into the Windows Container Host and install IIS with the management tools if it is not already installed.

    ```powershell
    Install-WindowsFeature -name Web-Server -IncludeManagementTools
    ```

1. Connect interactively to the container

    ```powershell
    docker ps
    CONTAINER ID        IMAGE               COMMAND                   CREATED             STATUS              PORTS               NAMES
    5d1e3bf011e1        iis-site            "C:\\ServiceMonitor..."   About an hour ago   Up About an hour    80/tcp              adoring_rosalind

    docker exec -it adoring_rosalind powershell
    ```
    You should now be on a powershell prompt inside the container. Validate by running `whoami` from the command line

    ````powershell
    whoami

    #You should see the following
    user manager\containeradministrator
    ````

1. Install and Configure IIS and enable it for Remote Management. Replace the values for UserName and Password

    ```powershell
    #These are only needed for accessing the IIS admin UI
    net user <USERNAME> <PASSWORD> /add

    net localgroup Administrators <USERNAME> /add

    Install-WindowsFeature Web-Mgmt-Service

    New-ItemProperty -Path HKLM:\software\microsoft\WebManagement\Server -Name EnableRemoteManagement -Value 1 -Force

    Start-Service WMSVC
    ```
1. On the container host, open the IIS Manager by clicking `Start` and searching for `inetmgr`

    ![image](./media/2018-03-18_16-50-37.png)

1. Click on File > Connect to a Server.

    ![image](./media/07b-1.PNG)

1. Enter the IP Address of your Container

    ![image](./media/07b-2.PNG)

1. Enter the local credentials of the container

    ![image](./media/07b-3.PNG)

1. You'll get a certificate alert, ignore and click on Connect

    ![image](./media/07b-4.PNG)

1. You should now have access to IIS running inside of your container.
    
    ![image](./media/07b-5.PNG)
        

### Exercise 2: Configure Kerberos Logging inside container<a name="ex2"></a>

---

1. Connect interactively to the container

    ```powershell
    docker ps
    CONTAINER ID        IMAGE               COMMAND                   CREATED             STATUS              PORTS               NAMES
    5d1e3bf011e1        iis-site            "C:\\ServiceMonitor..."   About an hour ago   Up About an hour    80/tcp              adoring_rosalind

    docker exec -it adoring_rosalind powershell

    #You should now be at a powershell prompt inside the container
    ```

1. Validate connectivity to the AD Domain

    ```powershell
    nltest /parentdomain
    nltest /query

    #You should see something like this
    PS C:\> nltest /parentdomain
    appmig.local. (1)
    The command completed successfully
    PS C:\> nltest /query
    Flags: 0
    Connection Status = 0 0x0 NERR_Success
    The command completed successfully
    PS C:\>

    ```

1. Enable Kerberos debugging in the container

    ```powershell
    New-ItemProperty -Path HKLM:\SYSTEM\CurrentControlSet\Control\Lsa\Kerberos\Parameters -Name LogLevel -PropertyType DWord -Value 1 -Force
    ````

    You should see the following

    ```powershell
    LogLevel     : 1
    PSPath       : Microsoft.PowerShell.Core\Registry:
    PSParentPath : Microsoft.PowerShell.Core\Registry:
    PSChildName  : Parameters
    PSDrive      : HKLM
    PSProvider   : Microsoft.PowerShell.Core\Registry
    ```

### Exercise 3: Export event logs from container<a name="ex3"></a>

---

1. If not already open, launch a PowerShell prompt in the container

1. Connect interactively to the container

    ```powershell
    docker ps
    CONTAINER ID        IMAGE               COMMAND                   CREATED             STATUS              PORTS               NAMES
    5d1e3bf011e1        iis-site            "C:\\ServiceMonitor..."   About an hour ago   Up About an hour    80/tcp              adoring_rosalind

    docker exec -it adoring_rosalind powershell

    #You should now be at a powershell prompt inside the container
    ```

1. You can export log files from the container using `(wevtutil epl <LogName> <FileName.evtx>)` and opening the logs on the host machine. This is helpful when viewing the errors that occurred in the running container. In this example, we will be exporting the `Security` Event log.

1. Run the following commands:

    ```powershell
    wevtutil epl Security c:\SecurityBackup.evtx
    ```

1. Run the following to ensure that the log file was exported:

    ```powershell
    cd\    
    dir
    ```

    You should see something like this with your `SecurityBackup.evtx` file

    ```powershell
    Directory: C:\


    Mode                LastWriteTime         Length Name
    ----                -------------         ------ ----
    d-----        2/22/2018  12:46 AM                inetpub
    d-----        7/16/2016   1:18 PM                PerfLogs
    d-r---        2/22/2018  12:33 AM                Program Files
    d-----        2/22/2018  12:29 AM                Program Files (x86)
    d-----        2/22/2018  12:47 AM                site
    d-r---        2/22/2018  12:37 AM                Users
    d-----        2/22/2018  12:39 AM                Windows
    -a----        2/22/2018  12:51 AM          69632 SecurityBackup.evtx
    -a----       11/22/2016  10:45 PM           1894 License.txt
    -a----        12/8/2017   7:00 PM         126632 ServiceMonitor.exe
    ```

1. From the Windows Container Host we can copy the exported application log from inside the container to the host so that we can view it.

    ```powershell
    docker cp adoring_rosalind:/SecurityBackup.evtx c:\mylogs\SecurityBackup.evtx
    ```

1. Open `Event Viewer` on the host machine and right click on `Even Viewer(local) > Open Saved Log`

1. Navigate to the location you saved the exported log (in this example, `c:\mylogs\SecurityBackup.evtx`), and `Open`.

### Exercise 4: Remote Debugging in the container<a name="ex4"></a>

#### Reference Links for Remote Debugging Tools and Information

* [Remote Debugging Tools](https://docs.microsoft.com/en-us/visualstudio/debugger/remote-debugging) : https://docs.microsoft.com/en-us/visualstudio/debugger/remote-debugging
* [Remote Debugger Port Assignments](https://docs.microsoft.com/en-us/visualstudio/debugger/remote-debugger-port-assignments) : https://docs.microsoft.com/en-us/visualstudio/debugger/remote-debugger-port-assignments

1. For Visual Studio 2017 we want ports 4022 (32-bit debugger) and 4023 (64-bit debugger).

1. For this scenario we will create a simple ASP.NET Web Application

1. From the Development VM Open Visual Studio 2017 and Create a new ASP.NET Web Application (.NET Framework) called WebApplication1

    ![image](./media/07b-6.PNG)

1. Choose MVC and Enable Docker Support

    ![image](./media/07b-7.PNG)

1. Add the remote debugger to your docker file and open the appropriate ports

    ```docker
    #### Remote Debugger Configuration for VS 2017
    #### Add this to the bottom of your Docker file in the solution
    EXPOSE 4022 4023
    RUN mkdir c:\tools
    RUN INVOKE-WebRequest -OutFile c:\tools\RemoteTools.amd64ret.enu.exe -Uri https://aka.ms/vs/15/release/RemoteTools.amd64ret.enu.exe;
    RUN powershell.exe -Command Start-Process c:\tools\RemoteTools.amd64ret.enu.exe -ArgumentList '/quiet' -Wait;

    ```

1. Build the solution

1. Open powershell and cd to your solution directory and build the image on the Windows Container host, in this example the IP address of the Windows Container Host is 10.0.1.5

    ```powershell
    cd c:\Users\azadmin.Contoso\Documents\Visual Studio 2017\Projects\WebApplication1\WebApplication1

    docker --host tcp://10.0.1.5 build -t webapplication1 .
    ```

1. From the Windows Container Host start the container with the appropriate ports mapped

    ```powershell
    # docker run -d -p 80:80 -p 4022:4022 -p 4023:4023 <imagename>
    # e.g.
    docker run -d -p 80:80 -p 4022:4022 -p 4023:4023 webapplication1
    ```
1. From the Windows Container host start the remote debugger

    ```powershell
    #docker exec -it <container name> "C:\Program Files\Microsoft Visual Studio 15.0\Common7\IDE\Remote Debugger\x64\msvsmon.exe" /nostatus /silent /noauth /anyuser /nosecuritywarn

    #e.g.
    docker exec -it thirsty_clarke "C:\Program Files\Microsoft Visual Studio 15.0\Common7\IDE\Remote Debugger\x64\msvsmon.exe" /nostatus /silent /noauth /anyuser /nosecuritywarn
    ```

1. Since the container is running remotely on the Windows Container Host, you will need to load the symbols in Visual Studio 2017. Click on `Tools > Options > Debugging > Symbols` and navigate to your project folder to the `obj/Debug` folder.

    ![image](./media/07b-11.PNG)

1. From the Development VM in `Visual Studio 2017` will now connect to the remote debugger, load the symbols and enjoy debugging at its finest by Click on `Debug > Attach to Process`:

    ![image](./media/07b-8.PNG)

1. In the 'Attach to Process' dialog choose `Connection type: 'Remote (no authentication)'` and Connection target: `10.0.1.5:4022` and hit `Enter`

    ![image](./media/07b-9.PNG)

1. Set the Attach to: `Managed (v4.6, v4.5, v4.0) code`

1. Click the checkbox for `Show processes from all users`

1. Click `Refresh` and click on the `w3wp.exe` process

1. Click `Attach`.

    ![image](./media/07b-10.PNG)

1. Now validate that you are able to set break points in your application for debugging.

## Summary

In this hands-on lab, you learned how to:

* Configure Windows containers for advanced debugging and troubleshooting

----

Copyright 2016 Microsoft Corporation. All rights reserved. Except where otherwise noted, these materials are licensed under the terms of the MIT License. You may use them according to the license as is most appropriate for your project. The terms of this license can be found at https://opensource.org/licenses/MIT.



