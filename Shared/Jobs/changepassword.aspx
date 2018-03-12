<%@ Page Language="C#" CodeFile="changepassword.aspx.cs" Inherits="changeregistrationinfo_aspx" MasterPageFile="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="server">
    <div align=center>
    <asp:Label ID="Label1" Runat="server" Text="Change your password!" SkinID="FormHeading"></asp:Label>
    <br /><br />
<asp:ChangePassword ID="ChangePassword1" Runat="server" OnContinueClick="ChangePassword1_ContinueClick" ContinueDestinationPageUrl="~/default.aspx">
</asp:ChangePassword>
</div>
</asp:Content>

