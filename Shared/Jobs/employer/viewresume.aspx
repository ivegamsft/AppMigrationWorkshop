<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeFile="viewresume.aspx.cs" Inherits="viewresume_aspx" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="server">
<div align="center">
        <asp:Label ID="Label14" Runat="server" Text="View Resume" SkinID="FormHeading"></asp:Label><br />
    &nbsp;</div>

    <table width="100%"><tr>
            <td>
                <asp:Label ID="lblName" Runat="server" SkinID="FormGroupLabel"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblEducation" Runat="server" SkinID="FormGroupLabel"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblExperience" Runat="server" SkinID="FormGroupLabel"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblCoveringLetter" Runat="server" Width="100%"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblResume" Runat="server" Width="100%"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnBack" Runat="server" Text=" Back " OnClick="btnBack_Click" />&nbsp;<asp:Button
                    ID="Button1" Runat="server" Text="Add to My Resumes" OnClick="Button1_Click" />
            </td>
        </tr></table>
</asp:Content>
