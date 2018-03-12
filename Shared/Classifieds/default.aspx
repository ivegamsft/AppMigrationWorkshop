<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeFile="Default.aspx.cs"
    Inherits="Default_aspx" Title="Welcome" %>
<%@ Register TagPrefix="uc1" TagName="FeaturedAd" Src="Controls/FeaturedAd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CategoryBrowse" Src="Controls/CategoryBrowse.ascx" %>
<asp:Content ID="SecondBarContent" ContentPlaceHolderID="SecondBar" Runat="server">
    <div id="crumbs_search">
        <p>
            Search:
        </p>
        <p>
            <asp:TextBox ID="CommonSearchTextBox" Runat="server" CssClass="search_box" AccessKey="s"
                TabIndex="1"></asp:TextBox>
            <asp:Button ID="CommonSearchButton" Runat="server" Text="Search" CssClass="submit"
                PostBackUrl="~/Search.aspx" />
        </p>
    </div>
    <div id="whats_new">
        <p>
            What's new:
        </p>
        <p>
            <asp:DropDownList ID="CommonWhatsNewRangeList" Runat="server">
                        <asp:ListItem Value="1" Selected="True">in the last 24 hours</asp:ListItem>
                        <asp:ListItem Value="2">in the last 48 hours</asp:ListItem>
                        <asp:ListItem Value="7">in the last week</asp:ListItem>
                        <asp:ListItem Value="30">in the last month</asp:ListItem>
                    </asp:DropDownList>
            <asp:Button ID="CommonWhatsNewButton" Runat="server" Text="Go" CssClass="submit"
                PostBackUrl="~/Search.aspx" OnClick="CommonWhatsNewButton_Click"  />
        </p>
    </div>
</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="Main" Runat="server">
    <div id="body">
        <div id="col_main_left">
            <uc1:FeaturedAd ID="FeaturedAd" Runat="server" />
            <div id="announcements">
                <ul>
                    <li>The latest activity relating to your ads and offers will appear on the My Ads &amp;
                    Profile page. <a href="MyAds.aspx">Go to My Ads & Profile</a> </li>
                    <li>Placing a new ad is fast and easy. <a href="PostAd.aspx">Place a New Ad</a></li></ul>
            </div>
        </div>
        <div id="col_main_right">
            <h3 class="section">
                Browse Categories</h3>
            <div class="content_right">
                <uc1:CategoryBrowse ID="CategoryBrowser" Runat="server" AutoNavigate="True">
                </uc1:CategoryBrowse>
                <p>&nbsp;</p>
                <p>&nbsp;</p>
                <p>&nbsp;</p>
                <p>&nbsp;</p>
                <p>&nbsp;</p>
                <p>&nbsp;</p>
            </div>
        </div>
    </div>
</asp:Content>
