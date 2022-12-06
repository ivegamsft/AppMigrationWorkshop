# Resource Group
resource "random_integer" "deployment" {
  min = 10000
  max = 99999
}

resource "azurerm_resource_group" "rg" {
  name     = "${random_integer.deployment.result}-rg"
  location = var.location
}

# Virtual Network
module "create_vnet" {
  source = "../modules/networking/vnet"

  resource_group_name = azurerm_resource_group.rg.name
  location            = var.location
  vnet_name           = "vnet-${random_integer.deployment.result}"

  address_space = "192.168.0.0/16"
  default_subnet_prefix = "192.168.1.0/24"
  dns_servers = null
  region_zones = 1
 
}
 
 # Bastion

 module "create_bastion" {
  source = "../modules/bastion"

  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  virtual_network_name = module.create_vnet.vnet_name
  subnet_cidr = "192.168.200.0/24"

}

# VM - Domain Controller

module "create_windowsserver_dc" {
  source = "../modules/compute"

  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  vnet_subnet_id      = module.create_vnet.default_subnet_id

  server_name       = "dc-${random_integer.deployment.result}"
  admin_username                 = var.admin_username
  admin_password                 = var.admin_password

}

# VM - SQL Database

module "create_windowsserver_sql" {
  source = "../modules/compute"

  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  vnet_subnet_id      = module.create_vnet.default_subnet_id

  server_name       = "sql-${random_integer.deployment.result}"
  admin_username                 = var.admin_username
  admin_password                 = var.admin_password

}

# VM - Web FrontEnd

module "create_windowsserver_iis" {
  source = "../modules/compute"

  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  vnet_subnet_id      = module.create_vnet.default_subnet_id

  server_name       = "iis-${random_integer.deployment.result}"
  admin_username                 = var.admin_username
  admin_password                 = var.admin_password

}
# VM - Dev Box

module "create_windowsserver_dev" {
  source = "../modules/compute"

  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  vnet_subnet_id      = module.create_vnet.default_subnet_id

  server_name       = "dev-${random_integer.deployment.result}"
  admin_username                 = var.admin_username
  admin_password                 = var.admin_password

}

