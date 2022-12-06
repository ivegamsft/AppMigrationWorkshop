
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
