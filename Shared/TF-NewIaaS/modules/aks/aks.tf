
resource "azurerm_kubernetes_cluster" "aks" {
  lifecycle {
   ignore_changes = [
     default_node_pool[0].node_count
   ]
  }

  name                = "${var.prefix}-k8s"
  location            = var.location
  resource_group_name = var.resource_group_name
  dns_prefix          = "${var.prefix}-k8s"

  default_node_pool {
    name       = "default"
    node_count = 2
    vm_size    = "Standard_DS2_v2"
    enable_auto_scaling = true
    min_count = 1
    max_count = 3    
    type = "VirtualMachineScaleSets"
    vnet_subnet_id = var.subnet_id
  }

  network_profile {
    network_plugin = "azure"
    network_policy = "azure"
    load_balancer_sku = "standard"
    service_cidr       = "192.168.100.0/24"
    dns_service_ip     = "192.168.100.10"
    docker_bridge_cidr = "172.17.0.1/16"
    # pod_cidr = "10.244.0.0/16" (needed with Kubenet)
    # load_balancer_profile {
    #     outbound_ip_address_ids = []
    # }

  }


  identity {
    type = "SystemAssigned"
  }

  # linux_profile {
  #   admin_username = "sysadmin"

  #   ssh_key {
  #     key_data = "~/.ssh/id_rsa.pub"
  #   }
  # }

  windows_profile {
    admin_username = "sysadmin"
    admin_password = "P@ssw0rd12345!!"
  }

  private_cluster_enabled = var.private

}

output "client_certificate" {
  value = azurerm_kubernetes_cluster.aks.addon_profile
}

output "kube_config" {
  value = azurerm_kubernetes_cluster.aks.kube_config_raw
}

output "aks_name" {
  value = azurerm_kubernetes_cluster.aks.name
}

output "aks_id" {
  value = azurerm_kubernetes_cluster.aks.id
}

output "aks_node_resource_group" {
  value = azurerm_kubernetes_cluster.aks.node_resource_group
}

# output "kubelet_id" {
#   value = azurerm_kubernetes_cluster.private.kubelet_identity.user_assigned_identity_id
# }
