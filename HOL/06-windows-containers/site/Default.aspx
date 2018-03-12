<%@ Page Language="C#" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="System.Security.Principal" %>
<%@ Import Namespace="System.Security.Claims" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Configuration" %>

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
			<h2>PrincipalContext</h2>
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
   	<div class="container">
		<div class="col-md">
			<h2>Database Connection String: "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=dotnet;Data Source=sql.contoso.com"</h2>
			<h2> <p class="lead">You Authenticated on <b><%= Server.MachineName %></b> using <b><%= Request.LogonUserIdentity.AuthenticationType %> <%= Request.LogonUserIdentity.ImpersonationLevel %></h2>
            <%
                var connString = ConfigurationManager.ConnectionStrings["RemoteSqlServer"];
                SqlConnection MyConnection = new SqlConnection(connString.ToString());
                SqlCommand MyTblCommand = new SqlCommand("SELECT TOP (10) [Id],[Data] FROM tbl_data", MyConnection);
                SqlCommand MyCommand = new SqlCommand("select suser_name() as username", MyConnection);
                        

                // Set the command type that you will run.
                MyCommand.CommandType = CommandType.Text;

                // Open the connection.
                try
                {
                    MyCommand.Connection.Open();
                    // Run the SQL statement, and then get the returned rows to the DataReader.
                    SqlDataReader MyDataReader = MyCommand.ExecuteReader();
                    while (MyDataReader.Read())
                    {
                        Response.Write("================================================</br>");
			            Response.Write("Database User ID: " + MyDataReader["username"] + "</br>");
                    }
		    
		     MyDataReader.Close();
		
		     SqlDataReader MyTblDataReader = MyTblCommand.ExecuteReader();
		     while(MyTblDataReader.Read())
		     {
			Response.Write("================================================</br>");
                        Response.Write("Id: " + MyTblDataReader["Id"] + "</br>");
                        Response.Write("Data: " + MyTblDataReader["Data"] + "</br>");
		     }
                }
                catch (Exception ex)
                {
                    Response.Write("Exception: " + ex.ToString());
                }
 			%>
		</div>
	</div>
    </form>
</body>
</html>
