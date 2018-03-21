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
                    <h1 class="display-4">Hello <b><%= Request.LogonUserIdentity.Name %></b></h1>
                    <p class="lead">
                        You Authenticated on: <b><%= Server.MachineName %></b> using AuthenticationType:<b><%= Request.LogonUserIdentity.AuthenticationType %></b> with impersonation level:<b><%= Request.LogonUserIdentity.ImpersonationLevel %></b>
                    </p>
                </div>
            </div>
            <div class="row">
                <div class="col-md">
                    <h2>Your claims</h2>
                    <ul>
                        <%
                            foreach (Claim c in Request.LogonUserIdentity.Claims)
                            {
                                Response.Write("<li><b>" + c.Type + "</b>=" + c.Value + "</li>");
                            }
                        %>
                    </ul>
                </div>
            </div>
            <div class="row">
                <div class="col-md">
                    <h2>Windows Principal Context</h2>
                </div>
            </div>
            <div class="row">
                <div class="col-md">
                    <h3>Testing Roles/Groups</h2>
                    <ul>
                        <%  try
                            {
                                foreach (IdentityReference g in Request.LogonUserIdentity.Groups)
                                {
                                    string groupName = new System.Security.Principal.SecurityIdentifier(g.Value).Translate(typeof(System.Security.Principal.NTAccount)).ToString();
                                    Response.Write("<li><b>" + groupName + "</b>=" + g.Value + "</li>");
                                }
                            }
                            catch (Exception gex)
                            {
                                Response.Write("<div class='alert alert-danger'><b>An error occurred translating group names::" + gex.ToString() + "</b></div>");
                            }
                        %>
                    </ul>
                </div>
            </div>
        </div>
        <div class="container">
            <div class="col-md">
                <h2>Testing Connection to database using connection String: <%= ConfigurationManager.ConnectionStrings["RemoteSqlServer"].ToString() %></h2>
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
                        while (MyTblDataReader.Read())
                        {
                            Response.Write("================================================</br>");
                            Response.Write("Id: " + MyTblDataReader["Id"] + "</br>");
                            Response.Write("Data: " + MyTblDataReader["Data"] + "</br>");
                        }
                    }
                    catch (Exception sqlex)
                    {
                        Response.Write("<div class='alert alert-danger'>An exception occurred connecting to the database: " + sqlex.ToString() + "</div>");
                    }
                %>
            </div>
        </div>
    </form>
</body>
</html>
