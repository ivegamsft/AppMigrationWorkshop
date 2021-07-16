resource "azurerm_storage_account" "sqldb" {
  name                     = "sqldbstorage${var.random}"
  location            = var.location
  resource_group_name = var.resource_group_name
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_mssql_server" "sqldb" {
  name                         = "dbserver-${var.random}"
  location            = var.location
  resource_group_name = var.resource_group_name
  version                      = "12.0"
  administrator_login          = var.sql_administrator_login
  administrator_login_password = var.sql_administrator_login_password
}

resource "azurerm_mssql_database" "defaultdb" {
  name           = "acctest-db-d"
  server_id      = azurerm_mssql_server.sqldb.id
  collation      = "SQL_Latin1_General_CP1_CI_AS"
  license_type   = "LicenseIncluded"
  max_size_gb    = 4
  read_scale     = true
  sku_name       = "BC_Gen5_2"
  zone_redundant = true

  tags = {
    foo = "bar"
  }

}

resource "azurerm_mssql_database_extended_auditing_policy" "example" {
  database_id                             = azurerm_mssql_database.defaultdb.id
  storage_endpoint                        = azurerm_storage_account.sqldb.primary_blob_endpoint
  storage_account_access_key              = azurerm_storage_account.sqldb.primary_access_key
  storage_account_access_key_is_secondary = false
  retention_in_days                       = 6
}

# Required Variables

variable "sql_administrator_login" {

}      

variable "sql_administrator_login_password" {

}

variable "random" {

}

variable "location" {
    
}

variable "resource_group_name" {

}
