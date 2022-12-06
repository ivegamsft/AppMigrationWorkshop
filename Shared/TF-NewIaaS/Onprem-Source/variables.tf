variable "tenant_id" {

}

variable "subscription_id" {

}

variable "location" {
}


variable "tags" {
  description = "ARM resource tags to any resource types which accept tags"
  type        = map(string)

  default = {
    project = "appmigration"
  }
}

variable "admin_username" {
  default = "sysadmin"
}

variable "admin_password" {
}


