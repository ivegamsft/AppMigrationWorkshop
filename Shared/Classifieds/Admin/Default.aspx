<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeFile="~/Admin/Default.aspx.cs"
    Inherits="AdminDefault_aspx" Title="Site Administration" %>
<asp:Content ID="SecondBarContent" ContentPlaceHolderID="SecondBar" Runat="server">
    <div id="crumbs_search">
        <p>
            Search:
        </p>
        <p>
            <asp:TextBox ID="CommonSearchTextBox" Runat="server" CssClass="search_box" AccessKey="s"
                TabIndex="1"></asp:TextBox>
            <asp:Button ID="CommonSearchButton" Runat="server" Text="Search" CssClass="submit"
                PostBackUrl="~/Admin/Ads.aspx" />
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
                PostBackUrl="~/Admin/Ads.aspx" />
        </p>
    </div>
</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="Main" Runat="server">
    <div id="body">
        <div id="col_main_left">
            <div id="user_assistance">
                <a id="content_start"></a>
                <h3>
                    Site Statistics</h3>
                <p>
                    Last generated on:<br />
                    <strong><asp:Label Runat="server" ID="LastGeneratedLabel" CssClass="stat" /></strong><br />
                    <asp:LinkButton ID="RefreshStatsButton" Runat="server" OnClick="RefreshStatsButton_Click">Refresh</asp:LinkButton></p>
            </div>
        </div>
        <div id="col_main_right">
            <h2 class="section">
                Site Administration</h2>
            <div class="content_right">
                <asp:FormView ID="StatsFormView" Runat="server" DataSourceID="StatsDataSource">
                    <ItemTemplate>
                        <p>
                            <span style="text-decoration:underline">Ads</span><br />
                            # Active Ads:
                            <asp:Label Text='<%# Eval("ActiveAds") %>' Runat="server" ID="ActiveAdsLabel" CssClass="stat">
                            </asp:Label><br />
                            # Total Ads:
                            <asp:Label Text='<%# Eval("TotalAds") %>' Runat="server" ID="TotalAdsLabel" CssClass="stat">
                            </asp:Label>
                            <br />
                            <strong><a href="Ads.aspx">Manage Ads &amp; Features</a></strong><br />
                            <br />
                            # Responses for currently active Ads:
                            <asp:Label Text='<%# Eval("ActiveAdResponses") %>' Runat="server" ID="ActiveAdResponsesLabel"
                                CssClass="stat">
                            </asp:Label><br />
                            # Total Responses:
                            <asp:Label Text='<%# Eval("TotalResponses") %>' Runat="server" ID="TotalResponsesLabel"
                                CssClass="stat">
                            </asp:Label><br />
                            <br />
                            <span style="text-decoration:underline">Activations & Deletions</span><br />
                            # Ads pending activation:
                            <asp:Label Text='<%# Eval("PendingAds") %>' Runat="server" ID="PendingAdsLabel" CssClass="stat" />
                            <br />
                            # Ads marked as deleted:
                            <asp:Label Text='<%# Eval("DeletedAds") %>' Runat="server" ID="Label1" CssClass="stat" />
                            <br />
                            <strong><a href="Activations.aspx">Manage Activations & Deletions</a></strong><br />
                            <br />
                            <br />
                            <span style="text-decoration:underline">Members</span><br />
                            # Total Members:
                            <asp:Label Text='<%# Eval("TotalUsers") %>' Runat="server" ID="TotalUsersLabel" CssClass="stat">
                            </asp:Label><br />
                            # New Registrations in last 7 days:
                            <asp:Label Text='<%# Eval("NewUsers") %>' Runat="server" ID="NewUsersLabel" CssClass="stat">
                            </asp:Label><br />
                            <br />
                            <br />
                            <span style="text-decoration:underline">Categories:</span><br />
                            </span> # Top-Level Categories:
                            <asp:Label Text='<%# Eval("TopCategories") %>' Runat="server" ID="TopCategoriesLabel"
                                CssClass="stat">
                            </asp:Label><br />
                            <strong><a href="Categories.aspx">Manage Categories</a></strong><br />
                            <br />
                            <span style="text-decoration: underline">Locations:</span><br />
                            # Pre-Defined Locations:
                            <asp:Label Text='<%# Eval("Locations") %>' Runat="server" ID="LocationsLabel" CssClass="stat">
                            </asp:Label><br />
                            <strong><a href="Locations.aspx">Manage Locations</a></strong><br />
                            <br />
                            <br />
                            Site Settings<br />
                            <strong><a href="Settings.aspx">Manage Settings</a></strong>
                            <p>
                    </ItemTemplate>
                </asp:FormView>
                <asp:ObjectDataSource ID="StatsDataSource" Runat="server" TypeName="AspNet.StarterKits.Classifieds.Web.StatsCache"
                    SelectMethod="GetStatistics" OnSelected="StatsDataSource_Selected">
                </asp:ObjectDataSource>
            </div>
        </div>
    </div>
</asp:Content>
