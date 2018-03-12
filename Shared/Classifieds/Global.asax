<%@ Application Language="C#" %> 
<%@ Import Namespace="AspNet.StarterKits.Classifieds.Web" %>

<script runat="server">
    
	void Application_Start(Object sender, EventArgs e) 
    {
        
        if (!Roles.RoleExists("Administrators")) Roles.CreateRole("Administrators");
        if (!Roles.RoleExists("Guests")) Roles.CreateRole("Guests");

        ClassifiedsHttpApplication cha = new ClassifiedsHttpApplication();
        cha.Application_Start(sender, e);
    }
	
</script>