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

### Exercise 1: Opening Cloud Shell for the first time<a name="ex1"></a>

----

1. Open your browser and go to <a href="https://shell.azure.com" target="_new">https://shell.azure.com</a>

1. Proceed with authentication

1. If you have access to more than one subscription, Select the Azure directory that is associated with your Azure subscription

1. If this is the first time you accessed the Cloud Shell, `Select` "PowerShell (Windows)" when asked which shell to use.

    ![image](./media/pic1.jpg)

    > Note: If this is not the first time and it is the "Bash" shell that starts, please click in the dropdown box that shows "Bash" and select "PowerShell" instead.

1. If you have at least contributor rights at subscription level, please select which subscription you would like the initialization process create a storage account and click "Create storage" button.
    ![image](./media/pic2.jpg)
    

### Exercise 2: Downloading the materials to the Cloud Shell environment<a name="ex2"></a>

----

1. If not already open, open your browser and navigate to <a href="https://shell.azure.com" target="_new">https://shell.azure.com</a>. Proceed with authentication if needed.

1. The Azure Cloud Shell persists its data on a mapped folder to Azure Files service. Change directories to `C:\Users\ContainerAdministrator\CloudDrive` with

    ```powershell
    cd C:\Users\ContainerAdministrator\CloudDrive
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

1. If not already open, open your browser and navigate to <a href="https://shell.azure.com" target="_new">https://shell.azure.com</a>. Proceed with authentication if needed.

1. Change the current folder to the location of cloned files

    ```powershell
    cd C:\Users\ContainerAdministrator\CloudDrive\AppMigrationWorkshopRepo\AppMigrationWorkshop\HOL\ARM-NewIaaS\dsc
    ```
    
1. Copy the following folders to the Cloud Shell PowerShell modules folder 

    ```powershell
    copy-item cDisk -Destination C:\users\ContainerAdministrator\CloudDrive\.pscloudshell\WindowsPowerShell\Modules -Recurse -Force
    copy-item xActiveDirectory -Destination C:\users\ContainerAdministrator\CloudDrive\.pscloudshell\WindowsPowerShell\Modules -Recurse -Force
    copy-item xComputerManagement -Destination C:\users\ContainerAdministrator\CloudDrive\.pscloudshell\WindowsPowerShell\Modules -Recurse -Force
    copy-item xDisk -Destination C:\users\ContainerAdministrator\CloudDrive\.pscloudshell\WindowsPowerShell\Modules -Recurse -Force
    copy-item xNetworking -Destination C:\users\ContainerAdministrator\CloudDrive\.pscloudshell\WindowsPowerShell\Modules -Recurse -Force
    ```

1. This solution was created using Visual Studio 2017 and it provides automatically a deployment script, please execute it by replacing some of the values as follows:

    ```powershell
    .\Deploy-AzureResourceGroup.ps1 -ResourceGroupLocation <deployment location> `
                                        -ResourceGroupName <resource group name> `
                                        -UploadArtifacts `
                                        -TemplateFile .\azuredeploy.json `
                                        -TemplateParametersFile .\azuredeploy.parameters.json

    ``` 

    Where:

    ```xml
    <deployment location> - Azure Location the template will for the location property of all resources
    <resource group name> - Name of the resource group where all resources will be created
    ```

    Example:

    ```powershell
    .\Deploy-AzureResourceGroup.ps1 -ResourceGroupLocation westus `
                                    -ResourceGroupName AppModernization-RG `
                                    -UploadArtifacts `
                                    -TemplateFile .\azuredeploy.json `
                                    -TemplateParametersFile `
                                    .\azuredeploy.parameters.json
    ```

## Summary

In this hands-on lab, you learned how to:
* Use the Azure Cloud Shell
* Deploy Azure resources from an automated template

----

Copyright 2016 Microsoft Corporation. All rights reserved. Except where otherwise noted, these materials are licensed under the terms of the MIT License. You may use them according to the license as is most appropriate for your project. The terms of this license can be found at https://opensource.org/licenses/MIT.

