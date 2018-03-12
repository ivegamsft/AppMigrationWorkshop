# Migrate Source Apps to PAAS

## Overview
In this lab, you will learn how to migrate legacy applications to PaaS. You will learn how to:

* Migrate a database to Azure SQL PAAS using Azure SQL Migrate
* Create VS 2017 empty project
* Copy files from source apps
* Add to VSTS
* Create pipeline and deploy to the PAAS app service
* Update the connection string in the APP Settings of the app service

## Prerequisites

* VS 2017 Installed
* Source Apps available
* You have a VSTS Account
* You have an Azure Subscription
* You have an Azure SQL DB Deployed

## Exercises

---

This hands-on-lab has the following exercises:

1. [Exercise 1: Migrate Databases to Azure SQL](#ex1)
1. [Exercise 2: Create Visual Studio Solution and Import Source Apps](#ex2)
1. [Exercise 3: Create CI/CD Pipeline in VSTS](#ex3)

### Exercise 1: Migrate Databases to Azure SQL<a name="ex1"></a>

---

This lab is done from the jump box

1. RDP into the jump box

1. Open a browser and download the Database Migration Assistant [here](https://www.microsoft.com/en-us/download/details.aspx?id=53595)

1. When prompted, click `Run > Next > Click the 'I Accept ...'` checkbox. 

1.  Click `Next > Install > Finish` then Launch the Data Migration Assistant

1. Click on the + Migration and name the Project `JobsDB`. Click `Create`
    
    ![image](./media/06-01-a.PNG)

1. Select the Source by entering your SQL server name `Server Name > Windows Authentication > Uncheck Encrypt connection` 

1. Select the `JSSKDB`

    ![image](./media/06-01-b.PNG)

1. Select `Target > Enter Authentication Credentials` and select the `jobsappsql[YOUR UNIQUE SUFFIX]` DB in the Azure subscription and resource group you have deployed the template from HOL 1.
    
    ![image](./media/06-01-c.PNG)

1. Select `objects`.  Make note of any blocking issues and non-blocking issues that will need to be addressed 

1. Click `Generate SQL script`

   ![image](./media/06-01-d.PNG)

1. To script & deploy the Schema, click `Deploy schema`. 

   ![image](./media/06-01-e.PNG)

1. Once the schema has been deployed click `Migrate Data > Start data migration`

   ![image](./media/06-01-f.PNG)

1. Wait for the migration to complete

   ![image](./media/06-01-g.PNG)


### Exercise 2: Create a Visual Studio Solution and Import the Source Apps<a name="ex2"></a>

---

1. Make sure you have the Jobs Source Apps downloaded on your Dev VM. In this example they are located in the `c:\SourceApps\JSSKCS` folder

    ![image](./media/06-02-a.PNG)

1. Open Visual Studio 2017. Select `File > New Project > Visual C# > Web > Web Site` and choose `ASP.NET Empty Web Site` as the template. 
 
1. Name the project `JobsSite`.

    ![image](./media/06-02-b.PNG)

1. You should now have an empty web site solution as a target to copy the Jobs source files. 

1. Open Windows Explorer and navigate to the folder where you have stored the Jobs Source Files

1.  Select all and copy all the files to the clipboard.

    ![image](./media/06-02-c.PNG)

1. Paste them into the Empty Visual Studio 2017 Solution

    ![image](./media/06-02-d.PNG)

1. Delete the `MyTemplate.vstemplate` and `ProjectName.webproj` files

    ![image](./media/06-02-f.PNG)

1. You now have a Visual Studio 2017 Web Site project that we can publish to Azure

    ![image](./media/06-02-e.PNG)


### Exercise 3: Create CI/CD Pipeline in VSTS<a name="ex2"></a>



1. Create an Azure Web Application

    ```powershell
    $webapp = "[A UNIQUE NAME]" # this should be a unique name
    $location = "[YOUR REGION]"
    $rgName = "[YOUR RESOURCE GROUP NAME]"
    $subscription = "[YOUR SUBSCRIPTION ID]"

    Login-AzureRmAccount
    Select-AzureRmSubscription -Subscription $subscription
    #Create the resource group
    New-AzureRmResourceGroup -Name $rgName -Location $location
    #Create the app service plan
    New-AzureRmAppServicePlan -Name $webapp -Location $location -ResourceGroupName $rgName -Tier Free
    #Create an empty web app to deploy to
    New-AzureRmWebApp -Name $webapp -Location $location -AppServicePlan $webapp -ResourceGroupName $rgName
    ```

1. Open a browser and navigate to you VSTS Portal (https://<your tenant>.visualstudio.com/_projects)

1. Click `Create New Project`

    ![image](./media/06-03-a.PNG)

1. Name the project `MyJobsApp123`. For the version control type, choose `Git`. For the `Work item process` choose `Agile`

    ![image](./media/06-03-b.PNG)

1. We now need to push the web site code to remote repo. On your DEV VM open `PowerShell` and navigate to your folder that has the Web Site Project

    ```powershell
    cd c:\sourceapps\jobswebsite\jobssite
    git init
    git remote add origin https://<your tenant>.visualstudio.com/_git/MyJobsApp123
    git push --set-upstream origin master
    ```

1. Browse to the VSTS Portal and click `Code` in the navigation, you should see the files in the repo
    
    ![image](./media/06-03-c.PNG)

1. Now that we have our source files in VSTS, we can create a Build Definition. Click on `Build and Release` > `New definition`

    ![image](./media/06-03-d.PNG)

1. Click `Empty Process`

    ![image](./media/06-03-e.PNG)

1. In the Tasks menu click the `+` symbol. Enter `archive` in the search > select the `Archive Files` task

    ![image](./media/06-03-f.PNG)

1. In the Tasks click the `+` symbol > enter `publish artifact` > select the `Publish Build Artifacts` task

    ![image](./media/06-03-h.PNG)

1. Select the `Publish Artifact` task

1. In the `Path to publish`, enter `$(build.artifactsstagingdirectory)` in the ' and Artifact name `drop` > the Artifact publish location `Visual Studio Team Service/TFS`

    ![image](./media/06-03-i.PNG)

1. Click on `Triggers` and enable continuous integration

    ![image](./media/06-03-j.PNG)

1. Click `Save and Queue` and you should see a new Build

    ![image](./media/06-03-k.PNG)

1. Click on the Build # and you can verify in the Log that the Archive of the job site did indeed happen

    ![image](./media/06-03-l.PNG)

1. Now that we have our source files, we can create a Release Definition to deploy our site

1. In the VSTS Portal now click `Releases > + New definition`

    ![image](./media/06-04-a.PNG)

1. Choose `Azure App Service` Deployment

    ![image](./media/06-04-b.PNG)

1. Choose and authorize VSTS to access your subscription

1.  Set the App type to `Web App` > Choose the 'myjobsapp123' > Validate that the Package or folder is set to $(System.DefaultWorkingDirectory/**/*.zip) and click 'Save'

    ![image](./media/06-04-c.PNG)

1. On the Release Pipeline click on `+ Add` next Artifacts

    ![image](./media/06-04-d.PNG)

1. Select the Build definition from the previous steps

    ![image](./media/06-04-e.PNG)

1. Click on Create Release > Create

    ![image](./media/06-04-f.PNG)

1. You should now see the release 'IN PROGRESS'

    ![image](./media/06-04-g.PNG)

1. Click on Log to verify the release was successful

    ![image](./media/06-04-h.PNG)


---
## Summary

In this hands-on lab, you learned how to:

* Migrate a database to Azure SQL PAAS using Azure SQL Migrate
* Create VS 2017 empty project
* Copy files from source apps
* Add to VSTS
* Create pipeline and deploy to the PAAS app service
* Update the connection string in the APP Settings of the app service

---
Copyright 2016 Microsoft Corporation. All rights reserved. Except where otherwise noted, these materials are licensed under the terms of the MIT License. You may use them according to the license as is most appropriate for your project. The terms of this license can be found at https://opensource.org/licenses/MIT.


