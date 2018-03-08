# Open Hack - Challenge 01

---

This document outlines the format for an Open Hack version of the App modernization workshop.

## Deploy Azure Infra

* [ARM Template](../../Shared/ARM-NewIaaS)

## Configure Source Applications

* [ ] Get source code [zip files](../../Shared/)
* [ ] Get [database backups](../../Shared)
* [ ] Configure web applications

### Applications

* Time Tracker
* Classifieds
* Jobs
* Optional
  * iBuySpy
  * Pet Shop

## Inventory Source Applications

Information you are looking for includes but may not be limited to:

* SSL Certificate
  * SPN/Kerberos
  * App Pool ID
  * Delegation Info
  * Ports and Protocols
  * DNS Name(s)
  * Hard Coded IP Addresses
  * Identity Federation
  * Security Information
    * Groups
  * Windows Integrated
* Source Operating System
* IIS Settings
* DB Versions
* DB Settings

To perform this inventory you can use the following tools:

## Select migration path for applications

Options include

* Full PaaS (Front End and Back End)
* Hybrid (Combination of VMs and PaaS)
* Containers

## Identify pre-reqs

This might include but is not limited to:

* Azure vNETs
* Resource Groups
* PaaS Services

## For Discussion

* Do you think you have enough information to migrate the app?
* Can you identify any potential blockers?
* Were the tools helpful?

## Helpful Resources

* [Migration checklist when moving to Azure App Service](https://azure.microsoft.com/en-us/blog/migration-checklist-when-moving-to-azure-app-service/)
* [Azure Websites Migration Assistant](https://azure.microsoft.com/en-us/downloads/migration-assistant/)
* [Microsoft Data Migration Assistant v3.4](https://www.microsoft.com/en-us/download/details.aspx?id=53595)
* [MAP Toolkit](https://technet.microsoft.com/en-us/library/bb977556.aspx?f=255&MSPPError=-2147217396)
* [Azure Migrate](https://azure.microsoft.com/en-us/services/azure-migrate/)

### 3rd Party Tools

This listing is not complete or meant to endores any product over another

* [CloudAtlas](https://www.cloudatlasinc.com/)
* [Movere](https://www.movere.io/)

## Helpful Links
