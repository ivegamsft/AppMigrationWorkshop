resource "azurerm_app_service_plan" "appservice" {
  name                = "${var.random}-appserviceplan"
  location            = var.location
  resource_group_name = var.resource_group_name

  sku {
    tier = "Standard"
    size = "S1"
  }
}

resource "azurerm_app_service" "appservice" {
  name                = "${var.random}-app-service"
  location            = var.location
  resource_group_name = var.resource_group_name
  app_service_plan_id = azurerm_app_service_plan.appservice.id

  site_config {
    dotnet_framework_version = "v4.0"
    scm_type                 = "LocalGit"
  }

  app_settings = {
    "SOME_KEY" = "some-value"
  }

  connection_string {
    name  = "Database"
    type  = "SQLServer"
    value = "Server=some-server.mydomain.com;Integrated Security=SSPI"
  }
}


# Required Variables


variable "random" {

}

variable "location" {
    
}

variable "resource_group_name" {

}
