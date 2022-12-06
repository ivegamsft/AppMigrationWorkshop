
variable "resource_group_name" {
}

variable "location" {

}

variable "tags" {
  description = "ARM resource tags to any resource types which accept tags"
  type = map(string)

  default = {
    application = "networking"
  }
}

variable "vnet_name" {
}

variable "address_space" {
}

variable "default_subnet_prefix"  {

}

variable "dns_servers" {
  
}

# variable "ddos_plan_id" {

# }

variable "region_zones" {
  
}
