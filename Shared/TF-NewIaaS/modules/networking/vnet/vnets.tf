# Creates a VNET with one default Subnet


resource "azurerm_virtual_network" "vnet" {
  name                = var.vnet_name
  resource_group_name = var.resource_group_name
  location            = var.location
  address_space       = [var.address_space] 
  dns_servers         = var.dns_servers
  tags                = var.tags

  # ddos_protection_plan {
  #   id     = var.ddos_plan_id
  #   enable = true
  # }

}

resource "azurerm_subnet" "vnet" {
  name                 = "default"
  resource_group_name  = var.resource_group_name
  virtual_network_name = var.vnet_name
  address_prefixes     = [var.default_subnet_prefix]
  # enforce_private_link_service_network_policies = true

  depends_on = [azurerm_virtual_network.vnet]
}

resource "azurerm_network_security_group" "nsg" {
  name            = "${var.vnet_name}-${azurerm_subnet.vnet.name}-nsg" 
  resource_group_name = var.resource_group_name
  location            = var.location

  tags = var.tags
}

resource "azurerm_subnet_network_security_group_association" "subnet" {
  subnet_id                 = azurerm_subnet.vnet.id
  network_security_group_id = azurerm_network_security_group.nsg.id
}

# This output is here (not in the output file) because
# it is needed to pass the zone info through to the VM deployment module. 
output "zones" {
    value = var.region_zones
}




