# Setting up the source environment using Resource Manager Templates

## Overview

In this lab, you will deploy a pre-built environment that you will use for the labs. The automated template will create 3 environments. Here is what is deployed:

Source environment

* 1 Windows Server 2016 VM with Visual Studio 2017 that will act as the jump box to manage the environment
* 1 Windows Server 2016 VM that will act as the domain controller for the environment
* 1 Windows Server 2008 machine that will act as the web server for the source applications
* 1 Windows Server 2008 machine with SQL 2008 installed and configured

Target environment

* 1 Azure App Service plan with 3 web applications
* 3 Azure SQL databases
* 1 Windows Server 2016 that will act as the Docker container host
* 1 Azure application gateway
* 1 Azure traffic manager

## Prerequisites

* An active Azure Subscription
* You are contributor at the subscription level

## Exercises

This hands-on-lab has the following exercises:

1. [Exercise 1: Opening Cloud Shell for the first time](#ex1)
1. [Exercise 2: Downloading the materials to the Cloud Shell environment](#ex2)
1. [Exercise 3: Deployment of Azure resources](#ex3)
1. [Exercise 4: Monitoring your deployment](#ex4)

### Exercise 1: Opening Cloud Shell for the first time<a name="ex1"></a>

----

1. Open your browser and go to <a href="https://shell.azure.com" target="_new">https://shell.azure.com</a>

1. Sign on with `Microsoft Account` or `Work or School Account` associated with your Azure subscription

    ![image](./media/02-01-a.png)

    ![image](./media/02-01-b.png)

1. If you have access to more than one subscription, Select the Azure directory that is associated with your Azure subscription

1. If this is the first time you accessed the Cloud Shell, `Select` "PowerShell (Windows)" when asked which shell to use.

    ![image](./media/pic1.jpg)

    > Note: If this is not the first time and it is the "Bash" shell that starts, please click in the dropdown box that shows "Bash" and select "PowerShell" instead.

1. If you have at least contributor rights at subscription level, please select which subscription you would like the initialization process to create a storage account and click "Create storage" button.
    ![image](./media/pic2.jpg)

1. You should see a command prompt like this one:
    ![image](./media/pic3.jpg)

### Exercise 2: Downloading the materials to the Cloud Shell environment<a name="ex2"></a>

----

1. If not already open, open your browser and navigate to <a href="https://shell.azure.com" target="_new">https://shell.azure.com</a>. Proceed with authentication if needed.

1. The Azure Cloud Shell persists its data on a mapped folder to Azure Files service. Change directories to `C:\Users\ContainerAdministrator\CloudDrive` with

    ```powershell
    cd C:\Users\ContainerAdministrator\CloudDrive
    ```
    > If you need to delete the directory and start over run the following:
    ```powershell
    Remove-Item -Recurse -Force .\AppMigrationWorkshopRepo\
    ```

1. Create a folder called `AppMigrationWorkshopRepo`

    ```powershell
    md AppMigrationWorkshopRepo
    ```
1. Change folder to the newly create one

    ```powershell
    cd AppMigrationWorkshopRepo
    ```
1. Clone the repository from its source

    ```powershell
    git clone https://github.com/AzureCAT-GSI/AppMigrationWorkshop
    ```

### Exercise 3: Deployment of Azure resources<a name="ex3"></a>

----

In the automated deployment, we are using PowerShell Desired State Configuration (DSC) modules to help configure the virtual machines. You need to download them to the environment, so they can be deployed. The deployment script uses these modules to build the zip file that is used by the PowerShell DSC VM Extension and uploads it to the staging storage account.

1. If not already open, open your browser and navigate to <a href="https://shell.azure.com" target="_new">https://shell.azure.com</a>. Proceed with Authentication if needed.

1. Change the current folder to the location of cloned files

    ```powershell
    cd C:\Users\ContainerAdministrator\CloudDrive\AppMigrationWorkshopRepo\AppMigrationWorkshop\Shared\ARM-NewIaaS\dsc
    ```

1. Copy the following folders to the Cloud Shell PowerShell modules folder

    ```powershell
    copy-item cDisk -Destination C:\users\ContainerAdministrator\CloudDrive\.pscloudshell\WindowsPowerShell\Modules -Recurse -Force
    copy-item xActiveDirectory -Destination C:\users\ContainerAdministrator\CloudDrive\.pscloudshell\WindowsPowerShell\Modules -Recurse -Force
    copy-item xComputerManagement -Destination C:\users\ContainerAdministrator\CloudDrive\.pscloudshell\WindowsPowerShell\Modules -Recurse -Force
    copy-item xDisk -Destination C:\users\ContainerAdministrator\CloudDrive\.pscloudshell\WindowsPowerShell\Modules -Recurse -Force
    copy-item xNetworking -Destination C:\users\ContainerAdministrator\CloudDrive\.pscloudshell\WindowsPowerShell\Modules -Recurse -Force
    ```

1. Change directories to the location of the ARM deployment script

    ````powershell
    cd ..
    ````

1. This solution was created using Visual Studio 2017 and it provides automatically a deployment script, please execute it by replacing some of the values as follows:

    ````powershell
    .\Deploy-AzureResourceGroup.ps1 -ResourceGroupLocation <deployment location> `
                                        -ResourceGroupName <resource group name> `
                                        -UploadArtifacts `
                                        -TemplateFile .\azuredeploy.json `
                                        -TemplateParametersFile .\azuredeploy.parameters.json

    ````

    Where:

    ````xml
    <deployment location> - Azure Location the template will for the location property of all resources
    <resource group name> - Name of the resource group where all resources will be created
    ````

    Example:

    ````powershell
    .\Deploy-AzureResourceGroup.ps1 -ResourceGroupLocation westus `
                                    -ResourceGroupName AppModernization-RG `
                                    -UploadArtifacts `
                                    -TemplateFile .\azuredeploy.json `
                                    -TemplateParametersFile `
                                    .\azuredeploy.parameters.json
    ````

### Exercise 4: Monitoring your deployment<a name="ex4"></a>

----
Although you can monitor your deployment from a PowerShell command prompt without any issues, CloudShell has a fixed timeout of 20 minutes, if your deployment takes more than it to complete (our case, this deployment takes around 35) you will see the following message:

  ![image](./media/pic8.png)

Since CloudShell is based on containers, when you reconnect, a new session will be presented to you and the deployment will be lost.

As mentioned before, if your deployment was executed from a PowerShell command prompt in a Virtual Machine or your own physical computer, it will not timeout and you will see the result of the deployment like this one:

  ![image](./media/pic7.png)

The idea of this exercise is to show the a to monitor your deployment that is independent from your deployment method (PowerShell command prompt, Azure CLI, Visual Studio, CloudShell, SDK, etc.), which is through the Resource Group's blade's Deployment property.

1. Go to the Azure Portal (http://portal.azure.com)

1. In the portal, in the left navigation pane, click `Resource Groups`

    ![image](./media/02-01-c.png)

1. From the Resource Group list, select the one deployed in HOL 1 (e.g. AppModernization-RG)

    ![image](./media/02-01-d.png)

1. From the Resource Group blade, there is a left menu item list, click on "Deployments"

    ![image](./media/pic4.png)

1. This will list all deployments executed and being executed, there is a column with the status of the deployment.

    ![image](./media/pic5.png)

1. Your master deployment item is called azuredeploy-<MMDD>-<HHMM>, this is the main item to monitor, if you want more details about it (all other deployments being shown here are created by the main deployment). Click azuredeploy-<MMDD>-<HHMM>.

    ![image](./media/pic6.png)

1. This will show all deployments chained to the master deployment. If there is any issue or if you want to check more details you can click on "Operation Details" or "Related Events" link.

    ![image](./media/pic10.png)

1. You will notice that your deployment is completed after status of azuredeploy-<MMDD>-<HHMM> deployment is Succeeded and it jumps to the top of the deployment list.

    ![image](./media/pic11.png)

## Summary

In this hands-on lab, you learned how to:

* Use the Azure Cloud Shell
* Deploy Azure resources from an automated template
* Log on to the Azure Portal
* Use Deployment blade item of the Resource Group to monitor a deployment

----

Copyright 2016 Microsoft Corporation. All rights reserved. Except where otherwise noted, these materials are licensed under the terms of the MIT License. You may use them according to the license as is most appropriate for your project. The terms of this license can be found at https://opensource.org/licenses/MIT.

