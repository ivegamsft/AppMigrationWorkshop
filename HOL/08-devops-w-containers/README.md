# DevOps with Containers

## Overview

In this lab, you will create a VSTS CI/CD pipeline that will deploy one of the apps in a container to the container host.

## Prerequisites

* You have created the environment described in HOL 1 Setup
* You have run through the configuration for the Container Host VM as described in HOL 7
* You have a Dockerfile image like the one created in HOL 7

## Exercises

This hands-on-lab has the following exercises:
1. [Exercise 1: Setup Network Security Groups, Firewall Ports, and Puplic IP for Container Host](#ex1)
1. [Exercise 2: Create an Azure Container Registry](#ex2)
1. [Exercise 3: Create a Build definitions](#ex3)
1. [Exercise 4: Create a Release definition](#ex4)

### Exercise 1 - Setup Network Security Groups, Firewall Ports, and Puplic IP for Container Host<a name="ex1"></a>
1. Login to https://shell.azure.com
    ```powershell
    Select-AzureRmSubscription -Subscription "<your subscription name>"
    ```
1. Create a public IP in Azure and add it to your Windows Container Host
    ```powershell
    #set your vairable nems for resource group, location, the Windows container host VM's NIC, and new PIP name
    $rgName = "<your resource group name>"
    $location = "<your deployment location>"
    $nicName = "<your container host vm name>-cnt-NIC-001"
    $dnsLabel = "<your container host vm name>"
    $pipName = "<your container host vm name>-cnt-NIC-001-PIP"

    #Deploy the new PIP
    $pip = New-AzureRmPublicIpAddress -ResourceGroupName $rgName -Name $pipName -Location $location -AllocationMethod Dynamic -DomainNameLabel $dnsLabel

    #Bind the new PIP to the Windows Container Host VM's NIC
    $nic = Get-AzureRmNetworkInterface -Name $nicName -ResourceGroupName $rgName
    $nic.IpConfigurations[0].PublicIpAddress = $pip 
    Set-AzureRmNetworkInterface -NetworkInterface $nic

    #Validate that a Public IP Address has been assigned
    (Get-AzureRmPublicIpAddress -Name $pipName -ResourceGroupName $rgName).IpAddress
    ```
1. RDP/Login to the Windows Container Host and open the needed firewall ports. Open a cmd or powershell prompt.
    ```powershell
    netsh advfirewall firewall add rule name="Docker Rule" dir=in action=allow protocol=TCP localport=2375
1. Create a Rule on your NSG to allow remote management of docker on your host vm.
    ```powershell
    $rgName = "<your resource group name>"
    $nsgName = "<your NSG name>"
    
    #Add a rule for docker deployments to your existing NSG
    $nsg = Get-AzureRmNetworkSecurityGroup -Name  $nsgName -ResourceGroupName $rgName `
        | Add-AzureRmNetworkSecurityRuleConfig -Name docker-rule -Description "Docker Rule" -Access Allow `
        -Protocol Tcp -Direction Inbound -Priority 200 -SourceAddressPrefix Internet -SourcePortRange * `
        -DestinationAddressPrefix 10.0.0.5  -DestinationPortRange 2375 |  Set-AzureRmNetworkSecurityGroup
    ```

1. Validate from your development environment that you can connect to the host sever.
    ```powershell
    #Get your Publick IP Address and the DNS Lable
    (Get-AzureRmPublicIpAddress -Name $pipName -ResourceGroupName $rgName).IpAddress
    (Get-AzureRmPublicIpAddress -Name $pipName -ResourceGroupName $rgName).DnsSettingsText

    docker --host tcp://<your Windows Contianer Host Public IP address> info
    PS C:\WINDOWS\system32> docker --host tcp://13.66.48.150 info
        Containers: 0
        Running: 0
        Paused: 0
        Stopped: 0
        Images: 2
        Server Version: 17.06.2-ee-6
        Storage Driver: windowsfilter
        Windows: 
        Logging Driver: json-file
        Plugins:
        Volume: local
        Network: l2bridge l2tunnel nat null overlay transparent
        Log: awslogs etwlogs fluentd json-file logentries splunk syslog
        Swarm: inactive
        Default Isolation: process
        Kernel Version: 10.0 14393 (14393.2068.amd64fre.rs1_release.180209-1727)
        Operating System: Windows Server 2016 Datacenter
        OSType: windows
        Architecture: x86_64
        CPUs: 2
        Total Memory: 7GiB
        Name: apm3pjq-cnt
        ID: KUHW:KYVP:OMYO:XW5A:DKIR:QDAA:2X7Z:NRZV:JYNQ:5ZF3:FXQI:NDYW
        Docker Root Dir: C:\ProgramData\docker
        Debug Mode (client): false
        Debug Mode (server): false
        Registry: https://index.docker.io/v1/
        Experimental: false
        Insecure Registries:
        127.0.0.0/8
        Live Restore Enabled: false
    ```

### Exercise 2 - Create an Azure Container Registry<a name="ex2"></a>

Azure Container Registry will serve as a place to save your container images, which you can later pull and deploy to different environment. For more information on Azure container registry go to (link).

1. Open a browser and Log in to your Azure subscription

1. Click on `Create a new resource` and type `Azure Container Registry` 

    ![image](./media/2018-03-21_0-44-12.png)

1. Click `Create`

    ![image](./media/hol9-1.png)

1. Create a unique registry name and deploy to the existing resource group you have set up for this lab (in this example `AppMigrationACR`)

1. Enable the `Admin user` and select the `Standard SKU`

    ![image](./media/2018-03-21_0-48-02.png)

1. Click `Create`

    ![image](./media/2018-03-21_0-48-02.png)

### Exercise 3 - Create a Build definition<a name="ex3"></a>

This lab is completed from the JumpBox. 

1. Open a browser and create a new Visual Studio project in your Visual Studio online account.

1. Create a new project (in this example AppMigrationVSO)

    ![image](./media/2018-03-21_0-52-22.png)

1. On the jumpbox, open a powershell window and navigate to the source directory where the jobswebsite is stored (from HOL 7, they are in c:\apps\jobswebsite)

1. Connect the source to the new repo and push your current changes to the solution to the new VSO repo.

    ````powershell
    cd \apps\jobswebsite
    git init
    git remote add origin <Your repo git url here>
    git add *
    git commit -m "initial commit"
    git push -u origin --all
    ````
1. In the browser, navigate to the project that contains the JobWebSite (in this example `AppMigrationVSO`)

1. Go to `settings > Services`

    ![image](./media/2018-03-21_0-55-07.png)

1. Click on Create New Service Endpoint. Choose Azure Resource Manager.

    ![image](./media/hol9-3.png)

1. Select the right subscription and the App Migration Resource Group. Click `OK`.

    ![image](./media/hol9-4.png)

1. From the main menu, click on `Builds and Releases > Build`

    ![image](./media/2018-03-21_0-57-40.png)

1. Click on `New definition`

    ![image](./media/hol9-5.png)

1. Pick the current VSTS repo as a source.

    ![image](./media/hol9-6.png)

1. Click on `Start with an Empty Process`.

    ![image](./media/hol9-7.png)

1. Click `Add a task to the phase`.

    ![image](./media/hol9-8.png)

1. Search for `Docker`. Select the Docker result and click `Add`.

    ![image](./media/hol9-9.png)

1. Select the `Build an image` task

1. Select Azure Container Registry as the Container Registry Type. Pick the ACR created in Exercise 1. Leave the action as `Build an image`

1. For the docker file, browse and select the source we uploaded in the previous step

    ![images](./media/2018-03-21_3-06-37.png)

    ![images](./media/hol9-10.png)

1. Add a second Docker task. This time configure action to `Push an image`. Again, make sure to include the Source tags and Latest tag.

    ![images](./media/hol9-11.png)

1. Click on `Save and queue > Save & Queue` to create a build.

    ![image](./media/2018-03-21_1-14-48.png)

1. View the logs for the build and ensure the container is built and pushed to ACR (this may take > 10 mins or more)

### Exercise 4 - Create a Release definition<a name="ex4"></a>

1. Once the build is complete click on `Releases`.

    ![image](./media/hol9-12.png)

1. Click on `Start with an empty process`.

    ![image](./media/hol9-13.png)

1. Click on `View environment tasks`

    ![image](./media/hol9-14.png)

1. Click `Add task` and search for Docker. Select the Docker result.

    ![image](./media/hol9-15.png)

1. Configure the action to Run Docker command and add the following command. Replace the values with your Docker `Host VM name` and your `ACR name`.

    ```powershell
    docker -H tcp://[YOUR DOCKER HOST VM NAME].cloudapp.azure.com:2375 run -d --restart no [YOUR ACR NAME].azurecr.io/appmodernizationsamples:latest -p 8080
    ```

## Summary

In this hands-on lab, you learned how to:

* Upload source to VSO
* Create an Azure Container Registry
* Create a Build definition to create your container and push to an ACR
* Create a Release definition to push your container to a host

----
Copyright 2016 Microsoft Corporation. All rights reserved. Except where otherwise noted, these materials are licensed under the terms of the MIT License. You may use them according to the license as is most appropriate for your project. The terms of this license can be found at https://opensource.org/licenses/MIT.
