<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeFile="MyAds.aspx.cs"
    Inherits="MyAds_aspx" Title="My Ads" %>
<%@ Register TagPrefix="uc1" TagName="CategoryDropDown" Src="Controls/CategoryDropDown.ascx" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="Main" Runat="server">
    <div id="body">
        <div id="search_n_no_browse">
            <div id="search">
                <fieldset>
                    <legend>Search</legend>
                    <asp:TextBox ID="CommonSearchTextBox" Runat="server" CssClass="search_box"
                        AccessKey="s" />
                    <uc1:CategoryDropDown ID="CommonCategoryDropDown" Runat="server" SelectOptionVisible="false" />
                    <asp:Button ID="SearchButton" Runat="server" Text="Search" PostBackUrl="~/Search.aspx" />
                    <asp:HyperLink ID="AdvancedSearchLink" Runat="server" NavigateUrl="~/AdvancedSearch.aspx">[go to advanced search]</asp:HyperLink>
                </fieldset>
            </div>
        </div>
        <div id="content_header">
            <a id="content_start"></a>
            <h2>
                Go to...</h2>
            <ul>
                <li>
                <asp:LinkButton ID="CurrentAdsButton" Runat="server" OnClick="CurrentAdsButton_Click">Current Ads</asp:LinkButton></li><li>
                <asp:LinkButton ID="InactiveAdsButton" Runat="server" OnClick="InactiveAdsButton_Click">Inactive Ads</asp:LinkButton></li><li>
                <asp:LinkButton ID="SavedAdsButton" Runat="server" OnClick="SavedAdsButton_Click">Saved Bookmarks</asp:LinkButton></li>
                <asp:PlaceHolder ID="ActivationAdsButtonPanel" Runat="server"><li>
                <asp:LinkButton ID="ActivationAdsButton" Runat="server" OnClick="ActivationAdsButton_Click">Ads waiting for activation</asp:LinkButton></li></asp:PlaceHolder>
                <li>
                <asp:HyperLink ID="ProfileLink" Runat="server" NavigateUrl="~/MyProfile.aspx">My Profile</asp:HyperLink></li>
            </ul>
        </div>
        <div id="content">
            <asp:MultiView ID="MyAdsMultiView" Runat="server" ActiveViewIndex="0">
                <asp:View ID="UserAdsView" Runat="server">
                    <h2 class="section">
                        My Current Ads</h2>
                    <asp:GridView ID="CurrentAdsGrid" Runat="server" DataSourceID="CurrentAds" AutoGenerateColumns="False"
                        OnRowCommand="CurrentAdsGrid_RowCommand" EnableViewState="False" DataKeyNames="Id"
                        OnRowDataBound="CurrentAdsGrid_RowDataBound" BorderWidth="0" CssClass="item_list"
                        ShowFooter="True">
                        <EmptyDataTemplate>
                            You have no currently active ads.
                            <asp:HyperLink ID="PostAdLink" Runat="server" NavigateUrl="~/PostAd.aspx">Click here to Post a new Ad.</asp:HyperLink>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:ImageField DataImageUrlFormatString="~/Images/preview-photo.gif" DataImageUrlField="PreviewImageId"
                                SortExpression="PreviewImageId" ItemStyle-CssClass="col_photo_nopreview" HeaderStyle-CssClass="col_photo_nopreview" 
                                AlternateText="Icon indicating that there are photos for this ad.">
                            </asp:ImageField>
                            <asp:HyperLinkField HeaderText="Title" DataNavigateUrlFields="Id" DataNavigateUrlFormatString="ShowAd.aspx?id={0}"
                                DataTextField="Title" SortExpression="Title" HeaderStyle-CssClass="col_title"
                                ItemStyle-CssClass="col_title"></asp:HyperLinkField>
                            <asp:BoundField HeaderText="Posted" DataField="DateCreated" SortExpression="DateCreated"
                                DataFormatString="{0:dd/mm/yy}" HeaderStyle-CssClass="col_general" ItemStyle-CssClass="col_general"></asp:BoundField>
                            <asp:BoundField HeaderText="Expires" DataField="ExpirationDate" SortExpression="ExpirationDate"
                                NullDisplayText="N/A" DataFormatString="{0:M}" HeaderStyle-CssClass="col_general"
                                ItemStyle-CssClass="col_general"></asp:BoundField>
                            <asp:BoundField HeaderText="# Views" DataField="NumViews" SortExpression="NumViews"
                                HeaderStyle-CssClass="col_general" ItemStyle-CssClass="col_general"></asp:BoundField>
                            <asp:BoundField HeaderText="# Resp." DataField="NumResponses" SortExpression="NumResponses"
                                HeaderStyle-CssClass="col_general" ItemStyle-CssClass="col_general"></asp:BoundField>
                            <asp:BoundField HeaderText="Category" DataField="CategoryName" SortExpression="CategoryName"
                                HeaderStyle-CssClass="col_category" ItemStyle-CssClass="col_category"></asp:BoundField>
                            <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="EditAd.aspx?id={0}"
                                Text="Edit" HeaderStyle-CssClass="col_general" ItemStyle-CssClass="col_general"></asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="ManagePhotos.aspx?id={0}"
                                Text="Photos" HeaderStyle-CssClass="col_general" ItemStyle-CssClass="col_general"></asp:HyperLinkField>
                            <asp:TemplateField HeaderStyle-CssClass="col_general" ItemStyle-CssClass="col_general"><ItemTemplate>
                                <asp:LinkButton ID="UnlistButton" Runat="server" CommandArgument='<%# Eval("Id") %>'
                                    CommandName="Unlist">Unlist</asp:LinkButton>
                            </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <RowStyle CssClass="row1"></RowStyle>
                        <AlternatingRowStyle CssClass="row2"></AlternatingRowStyle>
                        <FooterStyle CssClass="item_list_footer"></FooterStyle>
                    </asp:GridView>
                  </asp:View>
                  <asp:View ID="UserInactiveAdsView" Runat="server">
                    <h2 class="section">
                        My Inactive Ads</h2>
                    <asp:GridView ID="InactiveAdsGrid" Runat="server" DataSourceID="InactiveAds" AutoGenerateColumns="False"
                        DataKeyNames="Id" BorderWidth="0" CssClass="item_list" ShowFooter="True">
                        <EmptyDataTemplate>
                            You do not have any inactive ads.
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:ImageField DataImageUrlFormatString="~/Images/preview-photo.gif" DataImageUrlField="PreviewImageId"
                                SortExpression="PreviewImageId" ItemStyle-CssClass="col_photo_nopreview" HeaderStyle-CssClass="col_photo_nopreview" 
                                AlternateText="Icon indicating that there are photos for this ad.">
                            </asp:ImageField>
                            <asp:HyperLinkField HeaderText="Title" DataNavigateUrlFields="Id" DataNavigateUrlFormatString="ShowAd.aspx?id={0}"
                                DataTextField="Title" AccessibleHeaderText="Title" HeaderStyle-CssClass="col_title"
                                ItemStyle-CssClass="col_title"></asp:HyperLinkField>
                            <asp:BoundField HeaderText="Expired on" DataField="ExpirationDate" SortExpression="ExpirationDate"
                                NullDisplayText="N/A" DataFormatString="{0:d}" HeaderStyle-CssClass="col_general"
                                ItemStyle-CssClass="col_general"></asp:BoundField>
                                <asp:BoundField HeaderText="# Views" DataField="NumViews" SortExpression="NumViews"
                                HeaderStyle-CssClass="col_general" ItemStyle-CssClass="col_general"></asp:BoundField>
                            <asp:BoundField HeaderText="# Resp." DataField="NumResponses" SortExpression="NumResponses"
                                HeaderStyle-CssClass="col_general" ItemStyle-CssClass="col_general"></asp:BoundField>
                            
                            <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="PostAd.aspx?relist={0}"
                                Text="Relist" HeaderStyle-CssClass="col_general" ItemStyle-CssClass="col_general"></asp:HyperLinkField>
                            <asp:CommandField ShowDeleteButton="True" DeleteText="Remove" HeaderStyle-CssClass="col_general"
                                ItemStyle-CssClass="col_general"></asp:CommandField>
                        </Columns>
                        <RowStyle CssClass="row1"></RowStyle>
                        <AlternatingRowStyle CssClass="row2"></AlternatingRowStyle>
                        <FooterStyle CssClass="item_list_footer"></FooterStyle>
                    </asp:GridView>
                    <asp:PlaceHolder ID="ActivationAdsPanel" Runat="server"><h2 class="section">
                        My Ads waiting for Activation</h2>
                        <asp:GridView ID="ActivationAdsGrid" Runat="server" DataSourceID="PendingAds" AutoGenerateColumns="False"
                            BorderWidth="0" CssClass="item_list" ShowFooter="True">
                            <Columns>
                                <asp:HyperLinkField HeaderText="Title" DataNavigateUrlFields="Id" DataNavigateUrlFormatString="EditAd.aspx?id={0}"
                                    DataTextField="Title" AccessibleHeaderText="Title" HeaderStyle-CssClass="col_general"
                                    ItemStyle-CssClass="col_general"></asp:HyperLinkField>
                                <asp:HyperLinkField HeaderText="Title" DataNavigateUrlFields="Id" DataNavigateUrlFormatString="ShowAd.aspx?id={0}"
                                    DataTextField="Title" AccessibleHeaderText="Title" HeaderStyle-CssClass="col_general"
                                    ItemStyle-CssClass="col_general"></asp:HyperLinkField>
                                <asp:BoundField HeaderText="Date Added" DataField="DateCreated" SortExpression="DateAdded"
                                    HeaderStyle-CssClass="col_general" ItemStyle-CssClass="col_general"></asp:BoundField>
                                <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="ManagePhotos.aspx?id={0}"
                                    Text="Photos" HeaderStyle-CssClass="col_general" ItemStyle-CssClass="col_general"></asp:HyperLinkField>
                            </Columns>
                            <RowStyle CssClass="row1"></RowStyle>
                            <AlternatingRowStyle CssClass="row2"></AlternatingRowStyle>
                            <FooterStyle CssClass="item_list_footer"></FooterStyle>
                        </asp:GridView>
                    </asp:PlaceHolder>
                </asp:View>
                <asp:View ID="SavedAdsView" Runat="server">
                    <h2 class="section">
                        My Bookmarks</h2>
                    <asp:GridView ID="SavedAdsGrid" Runat="server" AutoGenerateColumns="False" DataSourceID="SavedAds"
                        DataKeyNames="Id" BorderWidth="0" CssClass="item_list" ShowFooter="True">
                        <EmptyDataTemplate>
                            You do not have any saved bookmarks.
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:ImageField DataImageUrlFormatString="~/PhotoDisplay.ashx?photoid={0}&size=small"
                                DataImageUrlField="PreviewImageId" SortExpression="PreviewImageId" HeaderStyle-CssClass="col_photo"
                                ItemStyle-CssClass="col_photo" AlternateText="Photo representing ad."></asp:ImageField>
                            <asp:HyperLinkField HeaderText="Title" DataNavigateUrlFields="Id" DataNavigateUrlFormatString="ShowAd.aspx?id={0}"
                                DataTextField="Title" AccessibleHeaderText="Title" HeaderStyle-CssClass="col_title"
                                ItemStyle-CssClass="col_title"></asp:HyperLinkField>
                            <asp:BoundField HeaderText="Category" DataField="CategoryName" SortExpression="CategoryName"
                                HeaderStyle-CssClass="col_general" ItemStyle-CssClass="col_general"></asp:BoundField>
                            <asp:BoundField HeaderText="Date Saved" DataField="DateCreated" SortExpression="DateAdded"
                                HeaderStyle-CssClass="col_general" ItemStyle-CssClass="col_general"></asp:BoundField>
                            <asp:CommandField ShowDeleteButton="True" DeleteText="Remove" HeaderStyle-CssClass="col_general"
                                ItemStyle-CssClass="col_general"></asp:CommandField>
                        </Columns><RowStyle CssClass="row1"></RowStyle>
                        <AlternatingRowStyle CssClass="row2"></AlternatingRowStyle>
                        <FooterStyle CssClass="item_list_footer"></FooterStyle>
                    </asp:GridView></asp:View></asp:MultiView>
        </div>
    </div>
    <asp:ObjectDataSource ID="InactiveAds" Runat="server" SelectMethod="GetInactiveAds"
        TypeName="AspNet.StarterKits.Classifieds.BusinessLogicLayer.AdsDB" OldValuesParameterFormatString="{0}"
        DeleteMethod="RemoveFromUserList">
        <DeleteParameters>
            <asp:Parameter Type="Int32" Name="id"></asp:Parameter>
        </DeleteParameters>
        <SelectParameters>
            <asp:ProfileParameter Name="memberId" DefaultValue="0" Type="Int32" PropertyName="MemberId"></asp:ProfileParameter>
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="CurrentAds" Runat="server" TypeName="AspNet.StarterKits.Classifieds.BusinessLogicLayer.AdsDB"
        SelectMethod="GetActiveAds">
        <SelectParameters>
            <asp:ProfileParameter Name="memberId" Type="Int32" PropertyName="MemberId"></asp:ProfileParameter>
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="PendingAds" Runat="server" TypeName="AspNet.StarterKits.Classifieds.BusinessLogicLayer.AdsDB"
        SelectMethod="GetPendingAds" OnSelected="PendingAds_Selected">
        <SelectParameters>
            <asp:ProfileParameter Name="memberId" DefaultValue="0" Type="Int32" PropertyName="MemberId"></asp:ProfileParameter>
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="SavedAds" Runat="server" TypeName="AspNet.StarterKits.Classifieds.BusinessLogicLayer.AdsDB"
        SelectMethod="GetSavedAds" DeleteMethod="RemoveSavedAd" OldValuesParameterFormatString="{0}">
        <DeleteParameters>
            <asp:Parameter Type="Int32" Name="id"></asp:Parameter>
            <asp:ProfileParameter Name="memberId" DefaultValue="0" Type="Int32" PropertyName="MemberId"></asp:ProfileParameter>
        </DeleteParameters>
        <SelectParameters>
            <asp:ProfileParameter Name="memberId" DefaultValue="0" Type="Int32" PropertyName="MemberId"></asp:ProfileParameter>
        </SelectParameters>
    </asp:ObjectDataSource></asp:Content>