<%@ Application Language="C#" %>
<%@ Import Namespace="System.Threading" %>
<%@ Import Namespace="System.Globalization" %>



<script runat="server">

    void Application_Start(Object sender, EventArgs e) {
        // Code that runs on application startup
        if (Roles.Enabled)
        {
            if (!Roles.RoleExists("ProjectAdministrator"))
            {
                Roles.CreateRole("ProjectAdministrator");
            }
            if (!Roles.RoleExists("ProjectManager"))
            {
                Roles.CreateRole("ProjectManager");
            }

            if (!Roles.RoleExists("Consultant"))
            {
                Roles.CreateRole("Consultant");
            }
        }


    }
    
    void Application_End(Object sender, EventArgs e) {
        //  Code that runs on application shutdown

    }
        
    void Application_Error(Object sender, EventArgs e) { 
        // Code that runs when an unhandled error occurs

    }

    void Application_BeginRequest(Object sender, EventArgs e)
    {
    }
    void Session_Start(Object sender, EventArgs e) {
        // Code that runs when a new session is started
        

    }

    void Session_End(Object sender, EventArgs e) {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
