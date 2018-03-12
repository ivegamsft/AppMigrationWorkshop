<%@ Page Language="C#" CodeFile="login.aspx.cs" Inherits="login_aspx" MasterPageFile="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="server">
<div align=center>
    <asp:Label ID="Label1" Runat="server" Text="Already Registered? Login here!" SkinID="FormHeading"></asp:Label>
    <br />
    <br />
<asp:Login ID="Login1" Runat="server" OnAuthenticate="Login1_Authenticate">
        </asp:Login><br />
        <asp:HyperLink ID="HyperLink1" Runat="server" NavigateUrl="~/register.aspx">New user? Register here!</asp:HyperLink>
    <br />
    <br />
        <asp:PasswordRecovery ID="PasswordRecovery1" Runat="server">
        </asp:PasswordRecovery>&nbsp;</div>        
</asp:Content>
        
