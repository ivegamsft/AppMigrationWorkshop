<%@ Page Language="C#" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="System.Security.Principal" %>
<%@ Import Namespace="System.Security.Claims" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0-beta.3/css/bootstrap.min.css" integrity="sha384-Zug+QiDoJOrZ5t4lssLdxGhVrurbmBWopoEl+M6BdEfwnCJZtKxi1KgxUyJq13dy" crossorigin="anonymous">
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="row">
                <div class="jumbotron">
                    <h1 class="display-4">Hello <b><%= Request.LogonUserIdentity.Name %></b><br></h1>
                    <p class="lead">You Authenticated on <b><%= Server.MachineName %></b> using <b><%= Request.LogonUserIdentity.AuthenticationType %> <%= Request.LogonUserIdentity.ImpersonationLevel %></b></p>
                </div>
            </div>
            <div class="row">
                <div class="col-md">
                    <h2>Claims</h2>
                    <ul>
                    <%
                        foreach(Claim c in Request.LogonUserIdentity.Claims)
                        {
                            Response.Write("<li><b>" + c.Type + "</b>=" + c.Value + "</li>");
                        }	
                    %>
                    </ul>
                </div>
            </div>
            <div class="row">
                <div class="col-md">
                    <h2>Roles/Groups</h2>
                    <ul>
                    <%
                        foreach(IdentityReference g in Request.LogonUserIdentity.Groups)
                        {
                            string groupName = new System.Security.Principal.SecurityIdentifier(g.Value).Translate(typeof(System.Security.Principal.NTAccount)).ToString();
                            Response.Write("<li><b>" + groupName + "</b>=" + g.Value + "</li>");	
                        }
                    %>
                    <ul>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
