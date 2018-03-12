<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MyFavorites.aspx.cs" Inherits="MyFavorites_aspx" Title="Untitled Page" %>
<%@ Register Src="../UserControls/DisplayModeController.ascx" TagName="DisplayModeController" TagPrefix="uc3" %>
<%@ Register Src="../UserControls/MyJobs.ascx" TagName="MyJobs" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/MySearches.ascx" TagName="MySearches" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div align=center>
    <asp:Label ID="Label14" Runat="server" Text="My Favorites" SkinID="FormHeading"></asp:Label>
    <br /><br />
    <asp:WebPartManager ID="WebPartManager1" runat="server">
    </asp:WebPartManager>
    <uc3:DisplayModeController ID="DisplayModeController1" runat="server" />
    <br />
    <asp:CatalogZone ID="CatalogZone1" runat="server" HeaderText="" VerbButtonType="Link">
        <ZoneTemplate>
            <asp:PageCatalogPart ID="PageCatalogPart1" runat="server" Title="Available Web Parts" />
        </ZoneTemplate>
    </asp:CatalogZone>
</div>
    <br />
    <table style="width: 100%" align="center">
        <tr>
            <td style="width: 100%">
                <asp:WebPartZone ID="WebPartZone1" runat="server" BorderStyle="NotSet" WebPartVerbRenderMode="TitleBar">
                    <ZoneTemplate>
                        <uc1:MyJobs ID="MyJobs1" runat="server" Title="My Jobs"/>
                    </ZoneTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:WebPartZone>
            </td>
        </tr>
        <tr>
            <td style="width: 100%">
                <asp:WebPartZone ID="WebPartZone2" runat="server" BorderStyle="NotSet" WebPartVerbRenderMode="TitleBar" Width="100%">
                    <ZoneTemplate>
                        <uc2:MySearches ID="MySearches1" runat="server" Title="My Searches" width="100%"/>
                    </ZoneTemplate>
                </asp:WebPartZone>
            </td>
        </tr>
    </table>
</asp:Content>

