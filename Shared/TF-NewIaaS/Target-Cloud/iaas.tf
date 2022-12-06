 
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

# Load Balancer 

module "create_external_lb" {
  source = "../modules/loadbalancer"

  location            = azurerm_resource_group.iaas.location
  resource_group_name = azurerm_resource_group.iaas.name

  lb_name       = "lb-${random_integer.deployment.result}"
  lb_pip = "pip-${random_integer.deployment.result}"
  pool_name = "iispool-${random_integer.deployment.result}"

}

resource "azurerm_network_interface_backend_address_pool_association" "iis1" {
  network_interface_id    = module.create_windowsserver_iis01.nic_id
  ip_configuration_name   = "internal"
  backend_address_pool_id = module.create_external_lb.backendpool_id
}

resource "azurerm_network_interface_backend_address_pool_association" "iis2" {
  network_interface_id    = module.create_windowsserver_iis02.nic_id
  ip_configuration_name   = "internal"
  backend_address_pool_id = module.create_external_lb.backendpool_id
}



