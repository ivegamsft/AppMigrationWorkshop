# Going Live

## Overview

In this lab you will learn how to transition a source environment to Azure.

## Prerequisites

Ensure you have completed the previous labs where the following is already configured:

1. Create a new azure internal DNS server which will host the DNS records and also do recursion to the on prem version ( this will be DNS server which you already have up and running)

1. Setup the monitoring solution for Azure DNS as well

## Excercies

1. [Exercise 1: DNS](#ex1)
1. [Exercise 2: Containers](#ex2)
1. [Exercise 3: Application Gateway](#ex3)
1. [Exercise 4: Traffic manager](#ex4)
1. [Exercise 5: Decomission the old environment(optional)](#ex5)

### Exercise 1: DNS<a name="ex1"></a>

Monitor Azure DNS
https://docs.microsoft.com/en-us/azure/log-analytics/log-analytics-dns

1. Resolve names from the container to check that it's working

1. Now that we have an application up and running in a container on our Container host, the last step is you to finalize the migration. We will do that by simply cutting over the routing and DNS information

1. Update the DNS records by RDPing into the DNS/AD Server, in this case the server is 'dc1.contoso.com' and open DNS. Here we can see that the ibusyspy dns record is a cname pointing to 'aw-webapp.contoso.com'

    ![image](./media/11-a-3.PNG)

1. Update the record to point to the Windows Container host, in this case 'host1.contoso.com'

    ![image](./media/11-a-4.PNG)

1. Open a browser to verify that you can indeed get to the site.

    ![image](./media/11-a-5.PNG)

### Exercise 2: Containers<a name="ex2"></a>

#### Assumptions

1. internal DNS server configuration of the containers will be targetting the on prem DNS server

1. setup configuration to hit the azure dns server

Azure DNS delegation
https://docs.microsoft.com/en-us/azure/dns/dns-delegate-domain-azure-dns

### Exercise 3: Application Gateway<a name="ex3"></a>

---

#### Assumptions

1. Deploy a new application gateway

1. Setup WAF and deploy a new self signed certificate to have SSL termination and enable OWASP

1. Enable monitoring for the application gateway

### Exercise 4: Traffic manager<a name="ex4"></a>

---

#### Assumptions

External endpoints are used for services hosted outside Azure, either on-premises or with a different hosting provider.

1. Setup a new traffic manager and target the WAF you've just created

1. The balancing method whilst everything is not configured will be having one of the endpoints disabled

1. Enable monitoring for traffic manager as well

1. Open the browser and validate that which you're reaching the new server

### Exercise 5: Verify the whole setup<a name="ex5"></a>

Let's verify the changes

#### Assumptions

1. Setup NSG which will block all traffic:  DC/DNS, Web server, IaaS SQL server
1. Test the website
1. Monitor the new resources through the dashboard

## Summary

In this hands-on lab, you learned how to:

* Configure internal DNS
* Configure Azure DNS
* Cutover live traffic using Azure services

---

Copyright 2016 Microsoft Corporation. All rights reserved. Except where otherwise noted, these materials are licensed under the terms of the MIT License. You may use them according to the license as is most appropriate for your project. The terms of this license can be found at https://opensource.org/licenses/MIT.