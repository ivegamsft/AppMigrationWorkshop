# DevOps with Containers

## Overview

In this lab, you will create a VSTS CI/CD pipeline that will deploy one of the apps in a container to the container host.

## Prerequisites

* You have created the environment described in HOL 1 Setup
* You have runthrough the configuration for the Container Host VM as described in HOL 7
* You have a Dockerfile image like the one created in HOL 7

## Excercies

This hands-on-lab has the following exercises:

1. [Exercise 1: Create an Azure Container Registry](#ex1)
1. [Exercise 2: Create a Build definitions](#ex2)
1. [Exercise 3: Create a Release definition](#ex3)

### Exercise 1 - Create an Azure Container Registry<a name="ex1"></a>

----

Azure Container Registry will serve as a place to save your container images, which you can later pull and deploy to different environment. For more information on Azure container registry go to (link).

1. Log in to your Azure subscription

1. Click on create a new resource and type in Azure Container Registry. Click Create.

	![HOL9-2](./media/hol9-1.PNG =250x)

1. Create a unique registry name and deploy to the existing `AppModernization` Resource Group

1. Enable admin user

1. Click Create

    ![HOL9-2](./media/hol9-2.PNG =250x)

### Exercise 2 - Create a Build definition<a name="ex2"></a>

----

1. Go to settings and click on Create New Service Endpoint. Choose Azure Resource Manager.

    ![HOL9-3](./media/hol9-3.PNG =160x)

1. Select the right subscription and the App Migration Resource Group. Click `OK`.

    ![HOL9-4](./media/hol9-4.PNG =320x)

1. Click on `Builds and Releases`, `Build` and then click on `Create a new build definition`.

    ![HOL9-5](./media/hol9-5.PNG =320x)

1. Pick the current VSTS repo as a source.

    ![HOL9-6](./media/hol9-6.PNG =320x)

1. Click on `Start with an Empty Process`.

    ![HOL9-7](./media/hol9-7.PNG =320x)

1. Click on `Add a task to the phase`.

    ![HOL9-8](./media/hol9-8.PNG =320x)

1. Search for `Docker`. Select the Docker result.

    ![HOL9-9](./media/hol9-9.PNG =800x)

1. Select Azure Container Registry as the Container Registry Type. Pick the ACR created in Exercise 1. Leave the action as Build and Image and use the Dockerfile from Hands on Lab 7 as your Docker File. Make sure to include source tags and latest tag.

    ![HOL9-10](./media/hol9-10.PNG =640x)

1. Add a second Docker task. This time configure action to Push an image. Again, make sure to include the Source tags and Latest tag.

    ![HOL9-11](./media/hol9-11.PNG =800x)

1. Click on `Save and Queue`.

### Exercise 3 - Create a Release definition<a name="ex3"></a>

----

1. Once the build is complete click on `Release`.

    ![HOL9-12](./media/hol9-12.PNG =640x)

1. Click on `Start with an empty process`.

    ![HOL9-13](./media/hol9-13.PNG =320x)

1. Click on `View environment tasks`

    ![HOL9-14](./media/hol9-14.PNG =320x)

1. Click `Add task` and search for Docker. Select the Docker result.

    ![HOL9-15](./media/hol9-15.PNG =640x)

1. Configure the action to Run Docker command and add the following command. Replace the values with your Docker `Host VM name` and your `ACR name`.

    ```powershell
    docker -H tcp://[YOUR DOCKER HOST VM NAME].cloudapp.azure.com:2375 run -d --restart no [YOUR ACR NAME].azurecr.io/appmodernizationsamples:latest -p 8080
    ```

## Summary

In this hands-on lab, you learned how to:

* Create an Azure Container Registry
* Create a Build definition to create your container and push to an ACR
* Create a Release definition to push your container to a host

----
Copyright 2016 Microsoft Corporation. All rights reserved. Except where otherwise noted, these materials are licensed under the terms of the MIT License. You may use them according to the license as is most appropriate for your project. The terms of this license can be found at https://opensource.org/licenses/MIT.
