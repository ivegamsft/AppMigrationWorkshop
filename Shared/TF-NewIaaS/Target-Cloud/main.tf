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












