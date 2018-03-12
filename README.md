# Application Migration Workshop

Contains content for conducting an Application Migration Workshop

| Current Status   |
| ---------------- |
| _In Development_ |

The workshop can be conducted in two different forms. Open Hack and Hand on Labs (HOLs).

In the OpenHack format, a set of challenges are issued and the participants can choose how to complete the effort. 

The HOLs on the other hand have specific step-by-step instructions that detail how to complete the challenge. Both can be used in combination if desired.

## Open Hack challenges

1. [Open Hack 1: Setting up the Azure environment](OpenHack/openHack01.md). In this challenge you will create the environment that is needed for the workshop.
1. [Open Hack 2: Create a Cloud DevOps Ready pipeline](OpenHack/openHack02.md). Now that you have the source applications configured, create a cloud DevOps ready pipleline with CI/CD.
1. [Open Hack 3: Enable legacy applications with CI/CD](OpenHack/openHack03.md). Now that we have DevOps working on a new app, get it working with the legacy applications
1. [Open Hack 4: Windows Containers and authentication](OpenHack/openHack04.md). Some legacy applications require native Windows functionality.
1. [Open Hack 5: CI/CD with Windows Containers](OpenHack/openHack05.md). Now that you have the application working in a container, use your DevOps pipeline to deploy it to Azure.
1. [Open Hack 6: Monitoring](OpenHack/openHack06.md).With the applications deployed into production, we need to monitor them both at the infrastructure and application levels.
1. [Open Hack 7: Going live](OpenHack/openHack07.md). Now that you have the application in the cloud and it has been tested in the cloud. Make the original version of the application accessible and then cut over to the migrated version.

----

## Module 0 - Introduction and course overview

In this session, we will provide a brief overview of the workshop and provide an overview of the app migration factory.

[View PowerPoint](Presentation/Module00-DigitalTransformation.pptx?raw=true)

### HOL 1: [Setting up the Azure environment](HOL/01-setup/README.MD)

In this lab you will create the environment that is needed for the workshop.

* Run a custom ARM Template to scaffold the resources used during the training

----

## Module 1 - Application Inventory 

In this session, we will understand how to inventory applications, assess dependencies and understand the configuration requirements.

[View PowerPoint](Presentation/Module01-Inventory.pptx?raw=true)

### HOL 2 - [Configure legacy Applications](HOL/02-configure-source-apps/README.MD): 

In this lab you will inventory the sample applications.

* Explore the sample applications
* Gather information about the source applications

----

## Module 2 - Migration patterns and approaches

In this session, we will understand the different patterns and identify the proper treatment for your legacy applications.

[View PowerPoint](Presentation/Module02-Migration-patterns-and-approaches.pptx?raw=true)

### HOL 3 - [Choose Migration Path](HOL/03-choose-migration-path/README.MD): 

In this lab you will review the legacy application, align the requirements to the different Azure offerings and provide a path for migration.

* Provide possible treatments for the legacy applications

----

## Module 4 - Continuous Integration, Continous Deployment and DevOps Pipelines

In this session, we will understand how Ci and CD can be used to help with the application migration factory.

[View PowerPoint](Presentation/Module04-Devops.pptx?raw=true)

### HOL 4 -[DevOps with App Service](HOL/04-devops-w-app-service/README.MD)

In this lab you will setup CI and CD with a simple application

### HOL 5 - [Migrate Source Apps to PaaS](HOL/05-deploy-to-paas/README.MD)

In this lab you will use CI/CD with the sample legacy applications

----

## Module 5 - Windows Containers

In this session, we will understand how Windows Containers can help with legacy workloads that require .NET and Windows integrated authentication.

[View PowerPoint](Presentation/Module05-Authentication.pptx?raw=true)
[View PowerPoint](Presentation/Module06-Windows-Containers.pptx?raw=true)

### HOL 6 - [Windows Containers](HOL/06-windows-containers/README.MD)

In this lab you will learn how to set up Windows containers

### HOL 6 (Supplemental) - [Advanced troubleshooting with Windows Containers](HOL/06-windows-containers/advanced-troubleshooting.md)

In this lab you will learn advanced techniques for working with Windows Containers

----

## Module 7 - Migrate Source Applications

In this session, we will learn techniques for migrating applications and databases

[View PowerPoint](Presentation/Module07-Database-Migrations.pptx?raw=true)

### HOL 7 - [Migrate Source Apps to Windows Containers](HOL/07-app-to-container/README.MD): 

In this lab you will learn how to containerize legacy applications

----

## Module 8 - DevOps with Windows Containers

In this session, we will learn how to create a CI/CD pipeline with Windows Containers.

### HOL 8 - [DevOps with Containers](HOL/08-devops-w-containers/README.MD): 

In this lab you will learn how to use CI/CD piplelines with Windows Containers

----

## Module 9 - Monitoring

In this session, we will learn about monitoring and mangement of Azure resources and applications

[View PowerPoint](Presentation/Module09-Monitoring-and-Alerting.pptx?raw=true)

### HOL 9 - [Monitoring](HOL/09-monitoring-alerting/README.MD): 

In this lab you will enable monitoring for the legacy applications

----

## Module 10 - Going Live 

In this session, we will learn how to plan for transistioning the legacy applications to running in production on Azure.

[View PowerPoint](Presentation/Module10-Going-Live.pptx?raw=true)

### HOL 10 - [Going Live](HOL/10-going-live/README.MD): 

In this lab, you will use some Azure services to help with going live


----

## Stretch Goals

This lab represents optional stretch goal exercises where you can add an additional scenarios on your own.

### Windows Container Scenarios

* Windows Integrated Authentation (Host Domain Joined Only)
* Container to Container Trusted Sub System
* Trusted Subsystem to SQL
* Impersonation to FileShare
* Containers with DNS
* Containers with SSL
* Load Balance across containers