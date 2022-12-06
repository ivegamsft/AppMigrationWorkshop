resource "azurerm_subnet" "extrasubnet" {
  name                 = var.subnet_name
  resource_group_name =  var.resource_group_name
  virtual_network_name = var.virtual_network_name
  address_prefixes     = [var.subnet_prefix]
  enforce_private_link_endpoint_network_policies = var.enforce_private_link_endpoint_network_policies 
  

}

resource "azurerm_network_security_group" "nsg" {
  name            = "${var.virtual_network_name}-${azurerm_subnet.extrasubnet.name}-nsg" 
  resource_group_name = var.resource_group_name
  location            = var.location

  # tags = var.tags
}

resource "azurerm_subnet_network_security_group_association" "subnet" {
  subnet_id                 = azurerm_subnet.extrasubnet.id
  network_security_group_id = azurerm_network_security_group.nsg.id
}

variable "subnet_name" {
   
}

variable "subnet_prefix" {

}

variable "virtual_network_name"  {

}

variable "resource_group_name" {
    
}

variable "location" {

}

variable "enforce_private_link_endpoint_network_policies" {
  
}

output "subnet_id" {
    value = azurerm_subnet.extrasubnet.id
}