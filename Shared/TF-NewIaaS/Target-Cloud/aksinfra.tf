# Deploy AKS

module "aks" {
  source = "../modules/aks"

  resource_group_name = azurerm_resource_group.aks.name
  location            = azurerm_resource_group.aks.location
  prefix              = "aks-${random_integer.deployment.result}"
  subnet_id = module.subnet_aks.subnet_id
  private = false


}

resource "azurerm_kubernetes_cluster_node_pool" "windows" {
  lifecycle {
    ignore_changes = [
      availability_zones, enable_auto_scaling, enable_host_encryption, enable_node_public_ip, fips_enabled, id, kubelet_disk_type, max_count, max_pods, min_count, node_labels, node_taints, orchestrator_version, os_disk_size_gb, vnet_subnet_id
    ]
  }

  name                  = "win"
  kubernetes_cluster_id = module.aks.aks_id
  vm_size               = "Standard_DS2_v2"
  node_count            = 1
  os_type = "Windows"

  tags = {
    Environment = "jcroth"
  }
}