# Resource Group
resource "random_integer" "deployment" {
  min = 10000
  max = 99999
}

resource "azurerm_resource_group" "rg" {
  name     = "${random_integer.deployment.result}-rg-core"
  location = var.location
}

# Virtual Network
module "create_vnet" {
  source = "../modules/networking/vnet"

  resource_group_name = azurerm_resource_group.rg.name
  location            = var.location
  vnet_name           = "vnet-${random_integer.deployment.result}"

  address_space = "10.0.0.0/16"
  default_subnet_prefix = "10.0.0.0/24"
  dns_servers = null
  region_zones = 1
 
}

 # Bastion

 module "create_bastion" {
  source = "../modules/bastion"

  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  virtual_network_name = module.create_vnet.vnet_name
  subnet_cidr = "10.0.1.0/24"

}
 
### IAAS INFRA ###

resource "azurerm_resource_group" "iaas" {
  name     = "${random_integer.deployment.result}-rg-iaas"
  location = var.location
}

# Subnet
module "subnet_iaas" {
  source = "../modules/networking/subnet"

  subnet_name = "iaas_subnet"
  subnet_prefix = "10.0.2.0/24"
  virtual_network_name = module.create_vnet.vnet_name
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  enforce_private_link_endpoint_network_policies = true

}

# SQL VM
module "create_windowsserver_sql" {
  source = "../modules/compute"

  location            = azurerm_resource_group.iaas.location
  resource_group_name = azurerm_resource_group.iaas.name
  vnet_subnet_id      = module.subnet_iaas.subnet_id

  server_name       = "sql-${random_integer.deployment.result}"
  admin_username                 = var.admin_username
  admin_password                 = var.admin_password

}

# IIS VMs

module "create_windowsserver_iis01" {
  source = "../modules/compute"

  location            = azurerm_resource_group.iaas.location
  resource_group_name = azurerm_resource_group.iaas.name
  vnet_subnet_id      = module.subnet_iaas.subnet_id

  server_name       = "iis01-${random_integer.deployment.result}"
  admin_username                 = var.admin_username
  admin_password                 = var.admin_password

}

module "create_windowsserver_iis02" {
  source = "../modules/compute"

  location            = azurerm_resource_group.iaas.location
  resource_group_name = azurerm_resource_group.iaas.name
  vnet_subnet_id      = module.subnet_iaas.subnet_id

  server_name       = "iis02-${random_integer.deployment.result}"
  admin_username                 = var.admin_username
  admin_password                 = var.admin_password

}

# Load Balancer or App Gateway or ??


### PAAS INFRA ###

resource "azurerm_resource_group" "paas" {
  name     = "${random_integer.deployment.result}-rg-paas"
  location = var.location
}

module "subnet_paas" {
  source = "../modules/networking/subnet"

  subnet_name = "paas_subnet"
  subnet_prefix = "10.0.3.0/24"
  virtual_network_name = module.create_vnet.vnet_name
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  enforce_private_link_endpoint_network_policies = true

}

# AzureSQL

module "azure_sql" {
  source = "../modules/sql-db"

  resource_group_name = azurerm_resource_group.paas.name
  location            = azurerm_resource_group.paas.location
  random = random_integer.deployment.result
  sql_administrator_login          = var.sql_administrator_login
  sql_administrator_login_password = var.sql_administrator_password
  
}

module "azure_appservice" {
  source = "../modules/app_service"

  resource_group_name = azurerm_resource_group.paas.name
  location            = azurerm_resource_group.paas.location
  random = random_integer.deployment.result
  
}


# App Service


### AKS INFRA ###

resource "azurerm_resource_group" "aks" {
  name     = "${random_integer.deployment.result}-rg-aks"
  location = var.location
}

# Subnet
module "subnet_aks" {
  source = "../modules/networking/subnet"

  subnet_name = "aks_subnet"
  subnet_prefix = "10.0.16.0/20"
  virtual_network_name = module.create_vnet.vnet_name
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  enforce_private_link_endpoint_network_policies = true

}

# Deploy AKS

module "aks" {
  source = "../modules/aks"

  resource_group_name = azurerm_resource_group.aks.name
  location            = azurerm_resource_group.aks.location
  prefix              = "aks-${random_integer.deployment.result}"
  subnet_id = module.subnet_aks.subnet_id
  private = false


}










