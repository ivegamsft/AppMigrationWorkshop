<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeFile="MyFavorites.aspx.cs" Inherits="MyFavorites_aspx" Title="Untitled Page" %>
<%@ Register Src="../UserControls/DisplayModeController.ascx" TagName="DisplayModeController" TagPrefix="uc4" %>
<%@ Register Src="../UserControls/MyJobs.ascx" TagName="MyJobs" TagPrefix="uc3" %>
<%@ Register Src="../UserControls/MySearches.ascx" TagName="MySearches" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="MyResumes" Src="../UserControls/MyResumes.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="server">
    <div align="center">
        <asp:Label ID="Label14" Runat="server" SkinID="FormHeading" Text="My Favorites"></asp:Label>
        <br />
    <asp:WebPartManager ID="WebPartManager1" runat="server">
    </asp:WebPartManager>
        <br />
        <uc4:DisplayModeController ID="DisplayModeController1" runat="server" />
        <br />
        <asp:CatalogZone ID="CatalogZone1" runat="server" HeaderText="" VerbButtonType="Link">
            <ZoneTemplate>
                <asp:PageCatalogPart ID="PageCatalogPart1" runat="server" Title="Available Web Parts" />
            </ZoneTemplate>
        </asp:CatalogZone>
    </div>
    <br />
    <table style="width: 100%">
        <tr>
            <td style="width: 100%">
                <asp:WebPartZone HeaderText="My Resumes" ID="WebPartZone1" runat="server" WebPartVerbRenderMode="TitleBar" ShowTitleIcons="False">
                    <ZoneTemplate>
                        <uc1:MyResumes ID="MyResumes1" Runat="server" title="My Resumes" />
                    </ZoneTemplate>
                </asp:WebPartZone>
            </td>
        </tr>
    </table>
    <br />
    <br />
    </asp:Content>
