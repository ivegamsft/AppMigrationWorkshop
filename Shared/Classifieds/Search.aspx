<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeFile="Search.aspx.cs"
    Inherits="Search_aspx" Title="Search" %>
<%@ Register TagPrefix="uc1" TagName="AdvancedSearch" Src="Controls/AdvancedSearch.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CategoryDropDown" Src="Controls/CategoryDropDown.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CategoryPath" Src="Controls/CategoryPath.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CategoryBrowse" Src="Controls/CategoryBrowse.ascx" %>
<%@ Register TagPrefix="uc1" TagName="FeaturedAd" Src="Controls/FeaturedAd.ascx" %>
<%@ Import Namespace="AspNet.StarterKits.Classifieds.Web"  %>
<asp:Content ID="SecondBarContent" ContentPlaceHolderID="SecondBar" Runat="server">
    <div id="crumbs_text">
        <uc1:CategoryPath ID="CategoryPath" Runat="server" OnCategorySelectionChanged="CategoryPath_CategorySelectionChanged" /></div>
</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="Main" Runat="server">
    <div id="body">
        <div id="search_n_browse">
            <div id="search">
                <asp:Panel ID="SearchPanel" Runat="server" DefaultButton="SearchButton">
                    <fieldset>
                        <legend>Search</legend>
                        <asp:TextBox ID="SearchTermTextBox" Runat="server" CssClass="search_box" />
                        <uc1:CategoryDropDown ID="CategoryDropDown" Runat="server" SelectOptionVisible="false"
                            OnCategorySelectionChanged="CategoryDropDown_CategorySelectionChanged" />
                        <asp:Button ID="SearchButton" Runat="server" Text="Search" OnClick="SearchButton_Click" />
                        <asp:HyperLink ID="AdvancedSearchLink" Runat="server" NavigateUrl="~/AdvancedSearch.aspx">[go to advanced search]</asp:HyperLink>
                    </fieldset>
                </asp:Panel>
            </div>
            <div id="browse">
                <fieldset>
                    <legend>Continue Browsing Subcategories</legend>
                    <asp:ListBox ID="SubcategoriesList" Runat="server" Rows="6" DataSourceID="CategoryDataSource"
                        DataTextField="NameWithActiveCount" DataValueField="IdString" AutoPostBack="True"
                        OnDataBound="SubcategoriesList_DataBound" OnSelectedIndexChanged="SubcategoriesList_SelectedIndexChanged">
                    </asp:ListBox>
                </fieldset>
            </div>
        </div>
        <div id="content_header">
            <a id="content_start"></a>
            <asp:PlaceHolder ID="SearchingByAdvancedPanel" Runat="server" Visible="false">
                    <h2>Advanced Search Criteria</h2> <br /><br />
                    <asp:Label ID="AdvancedSearchCriteria" runat="server"></asp:Label>
                    <br />
                    <asp:LinkButton ID="RemoveAdvancedSearch" runat="server" 
                    Text="[remove advanced criteria]" OnClick="RemoveAdvancedSearch_Click"></asp:LinkButton>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="NormalSearch" Runat="server" Visible="false">
                    <asp:Label ID="NormalSearchLabel" runat="server" CssClass="normal_weight"></asp:Label>
                    <br />
                    <asp:Label ID="NormalSearchCriteria" runat="server"></asp:Label> <br />
                    <asp:LinkButton ID="ClearSearch" runat="server" 
                    Text="[clear search]" OnClick="ClearSearch_Click"></asp:LinkButton>
            </asp:PlaceHolder>
        </div>
        <div id="content">
            <asp:Panel ID="ResultsPanel" Runat="server">
                <asp:GridView ID="AdsGrid" Runat="server" DataSourceID="AdSearchDataSource" AutoGenerateColumns="False"
                    DataKeyNames="Id" AllowPaging="True" PageSize="15" AllowSorting="True"
                    BorderWidth="0px" CssClass="item_list">
                    <EmptyDataTemplate>
                        No Ads were found matching your query.
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:ImageField DataImageUrlFormatString="~/PhotoDisplay.ashx?photoid={0}&amp;size=small"
                            DataImageUrlField="PreviewImageId" SortExpression="PreviewImageId" AlternateText="Ad preview photo.">
                            <ItemStyle CssClass="col_photo" />
                            <HeaderStyle CssClass="col_photo" />
                        </asp:ImageField>
                        <asp:BoundField HeaderText="Start Date" DataField="DateCreated" SortExpression="DateCreated"
                            DataFormatString="{0:MM/dd/yy}" HtmlEncode="False">
                            <ItemStyle CssClass="col_startdate" />
                            <HeaderStyle CssClass="col_startdate" />
                        </asp:BoundField>
                        <asp:HyperLinkField HeaderText="Title" DataNavigateUrlFields="Id" DataNavigateUrlFormatString="~/ShowAd.aspx?id={0}"
                            DataTextField="Title" SortExpression="Title">
                            <ItemStyle CssClass="col_title" />
                            <HeaderStyle CssClass="col_title" />
                        </asp:HyperLinkField>
                        <asp:BoundField HeaderText="Price" DataField="Price" SortExpression="Price" DataFormatString="{0:c2}" HtmlEncode="False">
                            <ItemStyle CssClass="col_price" />
                            <HeaderStyle CssClass="col_price" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Location" DataField="Location" SortExpression="Location">
                            <ItemStyle CssClass="col_location" />
                            <HeaderStyle CssClass="col_location" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Category" DataField="CategoryName" SortExpression="CategoryName">
                            <ItemStyle CssClass="col_category" />
                            <HeaderStyle CssClass="col_category" />
                        </asp:BoundField>
                        <asp:TemplateField SortExpression="Title" HeaderText="Type">
                            <ItemTemplate><%# OutputFormatting.AdTypeToString(Eval("AdType")) %></ItemTemplate>
                            <ItemStyle CssClass="col_general" />
                            <HeaderStyle CssClass="col_general" />
                        </asp:TemplateField>
                    </Columns>
                    <RowStyle CssClass="row1"></RowStyle>
                    <AlternatingRowStyle CssClass="row2"></AlternatingRowStyle>
                    <PagerStyle CssClass="item_list_footer" />
                </asp:GridView></asp:Panel>
            <asp:Panel ID="BrowsePanel" Runat="server" Visible="False">
                <asp:PlaceHolder ID="Hide" Runat="Server" Visible="True">
                    <div class="display_left">
                        <uc1:FeaturedAd ID="FeaturedAd" Runat="server" />
                    </div>
                    <div class="display_right">
                        <uc1:CategoryBrowse ID="CategoryBrowser" Runat="server"  OnCategorySelectionChanged="CategoryBrowse_CategorySelectionChanged">
                        </uc1:CategoryBrowse></div>
                </asp:PlaceHolder></asp:Panel>
        </div>
    </div>
    <!-- uc1:AdvancedSearch (invisible) is used to handle parameters for AdSearchDataSource -->
    <uc1:AdvancedSearch ID="AdvancedSearch" Runat="server" Visible="False"></uc1:AdvancedSearch>
    <asp:ObjectDataSource ID="AdSearchDataSource" Runat="server" TypeName="AspNet.StarterKits.Classifieds.BusinessLogicLayer.AdsDB"
        SelectMethod="GetActiveAdsByQuery">
        <SelectParameters>
            <asp:Parameter Type="Int32" DefaultValue="50" Name="recordLimit"></asp:Parameter>
            <asp:ControlParameter Name="categoryId" DefaultValue="0" Type="Int32" ControlID="CategoryDropDown"
                PropertyName="CurrentCategoryId"></asp:ControlParameter>
            <asp:ControlParameter Name="memberId" ControlID="AdvancedSearch" PropertyName="MemberId"
                Type="Int32" DefaultValue="0" />
            <asp:ControlParameter Name="maxPrice" ControlID="AdvancedSearch" PropertyName="MaximumPrice"
                Type="Decimal" DefaultValue="-1" />
            <asp:ControlParameter Name="searchTerm" DefaultValue="" Type="String" ControlID="SearchTermTextBox"
                PropertyName="Text"></asp:ControlParameter>
            <asp:ControlParameter Name="location" ControlID="AdvancedSearch" PropertyName="Location"
                Type="String" DefaultValue="" />
            <asp:ControlParameter Name="adType" ControlID="AdvancedSearch" PropertyName="AdType"
                Type="Int32" DefaultValue="0" />
            <asp:Parameter Type="Int32" DefaultValue="0" Name="adLevel"></asp:Parameter>
            <asp:ControlParameter Name="dayRange" ControlID="AdvancedSearch" PropertyName="DayRange"
                Type="Int32" DefaultValue="-1" />
            <asp:ControlParameter Name="mustHaveImage" ControlID="AdvancedSearch" PropertyName="MustHavePhotos"
                Type="Boolean" DefaultValue="False" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="CategoryDataSource" TypeName="AspNet.StarterKits.Classifieds.Web.CategoryCache"
        SelectMethod="GetBrowseCategoriesByParentId" Runat="server">
        <SelectParameters>
            <asp:ControlParameter Name="parentCategoryId" DefaultValue="0" Type="Int32" ControlID="CategoryDropDown"
                PropertyName="CurrentCategoryId"></asp:ControlParameter>
        </SelectParameters>
    </asp:ObjectDataSource></asp:Content>