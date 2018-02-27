<#
 	.DISCLAIMER
    This Sample Code is provided for the purpose of illustration only and is not intended to be used in a production environment.
    THIS SAMPLE CODE AND ANY RELATED INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED,
    INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.  
    We grant You a nonexclusive, royalty-free right to use and modify the Sample Code and to reproduce and distribute the object
    code form of the Sample Code, provided that You agree: (i) to not use Our name, logo, or trademarks to market Your software
    product in which the Sample Code is embedded; (ii) to include a valid copyright notice on Your software product in which the
    Sample Code is embedded; and (iii) to indemnify, hold harmless, and defend Us and Our suppliers from and against any claims
    or lawsuits, including attorneys’ fees, that arise or result from the use or distribution of the Sample Code.
    Please note: None of the conditions outlined in the disclaimer above will supersede the terms and conditions contained
    within the Premier Customer Services Description.
#>
Configuration IISInstall 
{ 
	WindowsFeature WebServer
	{
		Ensure = "Present"
		Name = "Web-WebServer"
	}

	WindowsFeature WebStaticContent
	{
		Ensure = "Present"
		Name = "Web-Static-Content"
	}

	WindowsFeature WebDefaultDoc
	{
		Ensure = "Present"
		Name = "Web-Default-Doc"
	}
	
	WindowsFeature WebDirBrowsing
	{
		Ensure = "Present"
		Name = "Web-Dir-Browsing"
	}

	WindowsFeature WebHttpErrors
	{
		Ensure = "Present"
		Name = "Web-Http-Errors"
	}

	WindowsFeature WebAppDev
	{
		Ensure = "Present"
		Name = "Web-App-Dev"
	}

	WindowsFeature WebAspNet
	{
		Ensure = "Present"
		Name = "Web-Asp-Net"
	}

	WindowsFeature WebNetExt
	{
		Ensure = "Present"
		Name = "Web-Net-Ext"
	}

	WindowsFeature WebISAPIExt
	{
		Ensure = "Present"
		Name = "Web-ISAPI-Ext"
	}

	WindowsFeature WebISAPIFilter
	{
		Ensure = "Present"
		Name = "Web-ISAPI-Filter"
	}

	WindowsFeature WebHealth
	{
		Ensure = "Present"
		Name = "Web-Health"
	}

	WindowsFeature WebHttpLogging
	{
		Ensure = "Present"
		Name = "Web-Http-Logging"
	}

	WindowsFeature WebRequestMonitor
	{
		Ensure = "Present"
		Name = "Web-Request-Monitor"
	}

	WindowsFeature WebSecurity
	{
		Ensure = "Present"
		Name = "Web-Security"
	}

	WindowsFeature WebWindowsAuth
	{
		Ensure = "Present"
		Name = "Web-Windows-Auth"
	}

	WindowsFeature WebFiltering
	{
		Ensure = "Present"
		Name = "Web-Filtering"
	}

	WindowsFeature WebMgmtTools
	{
		Ensure = "Present"
		Name = "Web-Mgmt-Tools"
	}

	WindowsFeature WebMgmtConsole
	{
		Ensure = "Present"
		Name = "Web-Mgmt-Console"
	}

	WindowsFeature WebScriptingTools
	{
		Ensure = "Present"
		Name = "Web-Scripting-Tools"
	}

	WindowsFeature WebMgmtService
	{
		Ensure = "Present"
		Name = "Web-Mgmt-Service"
	}

	WindowsFeature WebMgmtCompat
	{
		Ensure = "Present"
		Name = "Web-Mgmt-Compat"
	}

	WindowsFeature WebMetabase
	{
		Ensure = "Present"
		Name = "Web-Metabase"
	}

	WindowsFeature WebWMI
	{
		Ensure = "Present"
		Name = "Web-WMI"
	}

	WindowsFeature WebLgcyMgmtConsole
	{
		Ensure = "Present"
		Name = "Web-Lgcy-Mgmt-Console"
	}

	WindowsFeature NETFrameworkCore
	{
		Ensure = "Present"
		Name = "NET-Framework-Core"
	}

	WindowsFeature RSATWebServer
	{
		Ensure = "Present"
		Name = "RSAT-Web-Server"
	}

	WindowsFeature WASProcessModel
	{
		Ensure = "Present"
		Name = "WAS-Process-Model"
	}
		
	WindowsFeature WASNETEnvironment
	{
		Ensure = "Present"
		Name = "WAS-NET-Environment"
	}
	
	WindowsFeature WASConfigAPIs
	{
		Ensure = "Present"
		Name = "WAS-Config-APIs"
	}

} 