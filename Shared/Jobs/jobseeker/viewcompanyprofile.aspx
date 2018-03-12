<%@ Page Language="C#" CodeFile="viewcompanyprofile.aspx.cs" Inherits="viewcompanyprofile_aspx" MasterPageFile="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="server">
<div align=center>
           <asp:Label ID="Label13" Runat="server" Text="Company Profile" SkinID="formheading"></asp:Label>
</div>           
    <br />
        <table style="width: 100%">
            <tr>
                <td valign="top" align="left" colspan="2">
                    <asp:Label ID="Label14" Runat="server" Text="Company Details" SkinID="FormGroupLabel"></asp:Label>
                </td>
            </tr>
            <tr>
                <td valign="top" align="right" width="40%">
                    <asp:Label ID="Label1" Runat="server" Text="Company Name :"></asp:Label>
                </td>
                <td align="left" width="60%">
                    <asp:Label ID="lblName" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top" align="right" width="40%">
                    <asp:Label ID="Label12" Runat="server" Text="Brief Profile :"></asp:Label>
                </td>
                <td align="left" width="60%">
                    <asp:Label ID="lblProfile" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top" align="left" colspan="2">
                    <asp:Label ID="Label15" Runat="server" Text="Location" SkinID="FormGroupLabel"></asp:Label>
                </td>
            </tr>
            <tr>
                <td valign="top" align="right" width="40%">
                    <asp:Label ID="Label2" Runat="server" Text="Address 1 :"></asp:Label></td>
                <td align="left" width="60%">
                    <asp:Label ID="lblAddress1" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top" align="right" width="40%">
                    <asp:Label ID="Label3" Runat="server" Text="Address 2 :"></asp:Label></td>
                <td align="left" width="60%">
                    <asp:Label ID="lblAddress2" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top" align="right" width="40%">
                    <asp:Label ID="Label4" Runat="server" Text="City :"></asp:Label></td>
                <td align="left" width="60%">
                    <asp:Label ID="lblCity" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top" align="right" width="40%">
                    <asp:Label ID="Label6" Runat="server" Text="Country :"></asp:Label>
                </td>
                <td align="left" width="60%">
                    <asp:Label ID="lblCountry" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top" align="right" width="40%">
                    <asp:Label ID="Label5" Runat="server" Text="State :"></asp:Label>
                </td>
                <td align="left" width="60%">
                    <asp:Label ID="lblState" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top" align="right" width="40%">
                    <asp:Label ID="Label7" Runat="server" Text="ZIP :"></asp:Label></td>
                <td align="left" width="60%">
                    <asp:Label ID="lblZIP" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top" align="left" colspan="2">
                    <asp:Label ID="Label16" Runat="server" Text="Contact Details" SkinID="FormGroupLabel"></asp:Label>
                </td>
            </tr>
            <tr>
                <td valign="top" align="right" width="40%">
                    <asp:Label ID="Label8" Runat="server" Text="Phone :"></asp:Label></td>
                <td align="left" width="60%">
                    <asp:Label ID="lblPhone" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top" align="right" width="40%">
                    <asp:Label ID="Label9" Runat="server" Text="Fax :"></asp:Label></td>
                <td align="left" width="60%">
                    <asp:Label ID="lblFax" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top" align="right" width="40%">
                    <asp:Label ID="Label10" Runat="server" Text="Email :"></asp:Label></td>
                <td align="left" width="60%">
                    <asp:Label ID="lblEmail" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td style=" height: 26px;" valign="top" align="right" width="40%">
                    <asp:Label ID="Label11" Runat="server" Text="Web Site :"></asp:Label></td>
                <td style=" height: 26px;" align="left" width="60%">
                    <asp:Label ID="lblWebSite" runat="server" Text="Label"></asp:Label></td>
            </tr>
            </table>

</asp:Content>

