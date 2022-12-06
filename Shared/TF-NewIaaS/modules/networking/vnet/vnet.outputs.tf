
output "vnet_name" {
    value = azurerm_virtual_network.vnet.name
}

output "vnet_id" {
    value = azurerm_virtual_network.vnet.id
}

output "vnet_rg" {
    value = azurerm_virtual_network.vnet.resource_group_name
}

output "vnet_location" {
    value = azurerm_virtual_network.vnet.location
}

output "default_subnet_id" {
  value = azurerm_subnet.vnet.id

}

output "default_subnet_name" {
  value = azurerm_subnet.vnet.name
}

output "defaultsub_nsg_name" {
    value = azurerm_network_security_group.nsg.name
}





