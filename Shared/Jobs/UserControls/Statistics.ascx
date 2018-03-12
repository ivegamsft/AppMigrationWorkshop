<%@ Control Language="C#" CodeFile="Statistics.ascx.cs" Inherits="Statistics_ascx" %>
<table cellspacing="0" cellpadding="5" border="1">
    <tr>
        <td nowrap="nowrap" align="center" colspan="2">
            <asp:Label ID="Label4" Text="Jobs/Resumes Stats" Runat="server" SkinID="FormLabel"></asp:Label>
        </td>
    </tr>
    <tr>
        <td nowrap="nowrap" align="right" width="50%">
            <asp:Label ID="Label1" Text="Total Jobs :" Runat="server"></asp:Label>
        </td>
        <td align="left" width="50%">
            <asp:Label ID="lblJobs" Text="Label" Runat="server" SkinID="FormLabel"></asp:Label>
        </td>
    </tr>
    <tr>
        <td nowrap="nowrap" align="right" width="50%">
            <asp:Label ID="Label2" Text="Total Resumes :" Runat="server"></asp:Label>
        </td>
        <td align="left" width="50%">
            <asp:Label ID="lblResumes" Text="Label" Runat="server" SkinID="FormLabel"></asp:Label>
        </td>
    </tr>
    <tr>
        <td nowrap="nowrap" align="right" width="50%">
            <asp:Label ID="Label3" Text="Total Companies :" Runat="server"></asp:Label>
        </td>
        <td align="left" width="50%">
            <asp:Label ID="lblCompanies" Text="Label" Runat="server" SkinID="FormLabel"></asp:Label>
        </td>
    </tr>
</table>
