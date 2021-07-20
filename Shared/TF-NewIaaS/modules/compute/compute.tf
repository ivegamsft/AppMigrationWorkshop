resource "azurerm_windows_virtual_machine" "compute" {
  name                = var.server_name
  location            = var.location
  resource_group_name = var.resource_group_name
  size                = "Standard_D2_v2"
  admin_username      = var.admin_username
  admin_password      = var.admin_password
  network_interface_ids = [
    azurerm_network_interface.compute.id,
  ]

  os_disk {
    caching              = "ReadWrite"
    storage_account_type = "Standard_LRS"
  }

  source_image_reference {
    publisher = "MicrosoftWindowsServer"
    offer     = "WindowsServer"
    sku       = "2016-Datacenter"
    version   = "latest"
  }

  boot_diagnostics {
    storage_account_uri = null
  }
}


resource "azurerm_network_interface" "compute" {
  name                = "${var.server_name}-nic"
  location            = var.location
  resource_group_name = var.resource_group_name

  ip_configuration {
    name                          = "internal"
    subnet_id                     = var.vnet_subnet_id
    private_ip_address_allocation = "Dynamic"
  }
}

output "nic_id" {
  value = azurerm_network_interface.compute.id
}



variable "admin_username" {
  default = "sysadmin"
}

variable "admin_password" {
  
}

variable "resource_group_name" {

}

variable "location" {

}

variable "vnet_subnet_id" {

}

variable "server_name" {
  
}