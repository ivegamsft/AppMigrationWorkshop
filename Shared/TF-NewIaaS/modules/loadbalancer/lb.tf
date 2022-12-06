resource "azurerm_public_ip" "ext-lb" {
  name                = var.lb_pip
  location            = var.location
  resource_group_name  = var.resource_group_name
  allocation_method   = "Static"
  sku = "Standard"
}

resource "azurerm_lb" "ext-lb" {
  name                = var.lb_name
  location            = var.location
  resource_group_name  = var.resource_group_name
  sku = "Standard"

  frontend_ip_configuration {
    name                 = "PublicIPAddress"
    public_ip_address_id = azurerm_public_ip.ext-lb.id
  }
}

resource "azurerm_lb_backend_address_pool" "be_pool" {
  loadbalancer_id = azurerm_lb.ext-lb.id
  name            = var.pool_name
}

output "backendpool_id" {
  value = azurerm_lb_backend_address_pool.be_pool.id
  
}

variable "location" {

}

variable "resource_group_name" {

}

variable "lb_pip" {

}

variable "lb_name" {
    
}

variable "pool_name" {

}