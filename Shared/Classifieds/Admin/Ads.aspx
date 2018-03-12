<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeFile="Ads.aspx.cs"
    Inherits="Ads_aspx" Title="Ads and Features - Site Administration" %>
<%@ Register TagPrefix="uc1" TagName="CategoryPath" Src="../Controls/CategoryPath.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CategoryDropDown" Src="~/Controls/CategoryDropDown.ascx" %>
<%@ PreviousPageType VirtualPath="~/Admin/Default.aspx" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="Main" Runat="server">
    <div id="body">
        <div id="admin_panel">
            <div id="query">
                <fieldset>
                    <legend>Query</legend>
                    <div>
                        <span class="input_label">Full-Text:</span> <span class="input_control">
                        <asp:TextBox ID="QueryTextBox" Runat="server" /></span>
                    </div>
                    <div>
                        <span class="input_label">Location:</span> <span class="input_control">
                        <asp:TextBox ID="LocationTextBox" Runat="server"></asp:TextBox></span>
                    </div>
                    <div>
                        <span class="input_label">Username:</span> <span class="input_control">
                        <asp:TextBox ID="UserNameTextBox" Runat="server" OnTextChanged="UserNameTextBox_TextChanged">
                        </asp:TextBox></span></div>
                    <div>
                        <span class="input_label">Date Range:</span> <span class="input_control">
                        <asp:DropDownList ID="DateRangeDropDown" Runat="server">
                            <asp:ListItem Value="1">within the last 24 hours</asp:ListItem>
                            <asp:ListItem Value="2">within the last 48 hours</asp:ListItem>
                            <asp:ListItem Value="7">within the last week</asp:ListItem>
                            <asp:ListItem Value="30">within the last month</asp:ListItem>
                            <asp:ListItem Value="-1" Selected="True">Show all</asp:ListItem>
                        </asp:DropDownList></span></div>
                    <div>
                        <span class="input_label">Category:</span> <span class="input_control">
                        <uc1:CategoryDropDown ID="CategoryDropDown" Runat="server" SelectOptionVisible="false"
                            AllCategoriesOptionText="[All Categories]" /></span>
                    </div>
                </fieldset>
            </div>
            <div id="status">
                <fieldset>
                    <legend>Properties</legend>
                    <div>
                        <span class="input_label">Status:</span> <span class="input_control">
                        <asp:DropDownList ID="AdStatusList" Runat="server">
                            <asp:ListItem Value="100" Selected="True">Active</asp:ListItem>
                            <asp:ListItem Value="-200">Activation Pending</asp:ListItem>
                            <asp:ListItem Value="-50">Inactive</asp:ListItem>
                            <asp:ListItem Value="-100">Marked as Deleted</asp:ListItem>
                            <asp:ListItem Value="0">Show all</asp:ListItem>
                        </asp:DropDownList></span>
                    </div>
                    <div>
                        <span class="input_label">Normal/Featured:</span> <span class="input_control">
                        <asp:DropDownList ID="AdLevelList" Runat="server">
                            <asp:ListItem Value="10">Normal</asp:ListItem>
                            <asp:ListItem Value="50">Featured</asp:ListItem>
                            <asp:ListItem Value="0" Selected="True">Show all</asp:ListItem>
                        </asp:DropDownList></span>
                    </div>
                    <div>
                        <span class="input_label">For Sale/Wanted:</span> <span class="input_control">
                        <asp:DropDownList ID="AdTypeDropDown" Runat="server" CssClass="fixed-main">
                            <asp:ListItem Value="1">For Sale</asp:ListItem>
                            <asp:ListItem Value="2">Wanted</asp:ListItem>
                            <asp:ListItem Value="0" Selected="True">Show all</asp:ListItem>
                        </asp:DropDownList></span>
                    </div>
                    <div>
                        <span class="input_label">Limit Results to:</span> <span class="input_control">
                        <asp:DropDownList ID="ResultLimitDropDown" Runat="server">
                            <asp:ListItem Selected="True" Value="50">50 records</asp:ListItem>
                            <asp:ListItem Value="100">100 records</asp:ListItem>
                            <asp:ListItem Value="0">Show all</asp:ListItem>
                        </asp:DropDownList></span>
                    </div>
                    <div>
                        <span class="input_label"></span><span class="input_control">
                        <asp:Button ID="QueryButton" Runat="server" Text="Query" OnClick="QueryButton_Click"
                            Font-Size="small" /></span></div>
                </fieldset>
            </div>
        </div>
        <div id="content_header">
            <a id="content_start"></a>
            <h2>
                <span class="normal_weight">
                <asp:HyperLink ID="BackToAdminLink" Runat="server" NavigateUrl="~/Admin/Default.aspx">back to Administration</asp:HyperLink></span>
            </h2>
        </div>
        <div id="content">
            <asp:Panel ID="WelcomePanel" Runat="server" Visible="True" HorizontalAlign="center">
                Use the above Query Panel to specify a search criteria.
            </asp:Panel>
            <asp:Panel ID="ResultsPanel" Runat="server" Visible="False">
                <asp:GridView ID="AdsGrid" Runat="server" DataSourceID="AdsDataSource" AutoGenerateColumns="False"
                    DataKeyNames="Id" AllowPaging="True" PageSize="20" AllowSorting="True" BorderWidth="0px"
                    CssClass="item_list" OnDataBound="AdsGrid_DataBound">
                    <PagerStyle CssClass="item_list_footer" />
                    <EmptyDataTemplate>
                        No ads matched your query.
                    </EmptyDataTemplate>
                    <AlternatingRowStyle CssClass="row2"></AlternatingRowStyle>
                    <Columns>
                        <asp:TemplateField ItemStyle-CssClass="col_checkbox" HeaderStyle-CssClass="col_checkbox">
                            <ItemTemplate>
                                <asp:CheckBox ID="AdCheckBox" Runat="server" CssClass="AdminAdsGrid" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:ImageField DataImageUrlFormatString="~/Images/preview-photo.gif" DataImageURLField="PreviewImageId"
                            HeaderText="Photo" SortExpression="PreviewImageId" HeaderStyle-CssClass="col_photo_nopreview"
                            ItemStyle-CssClass="col_photo_nopreview" FooterStyle-Font-Italic="true" AccessibleHeaderText="Ad Photo" AlternateText="Icon indicating that there are photos for this ad.">
                        </asp:ImageField>
                        <asp:BoundField HeaderText="Entry Date" DataField="DateCreated" SortExpression="DateCreated"
                            DataFormatString="{0:dd/mm/yy}" HeaderStyle-CssClass="col_startdate" ItemStyle-CssClass="col_startdate"></asp:BoundField>
                        <asp:HyperLinkField HeaderText="Title" DataNavigateUrlFields="Id" DataNavigateUrlFormatString="~/ShowAd.aspx?id={0}"
                            DataTextField="Title" SortExpression="Title" HeaderStyle-CssClass="col_title"
                            ItemStyle-CssClass="col_title"></asp:HyperLinkField>
                        <asp:BoundField HeaderText="Price" DataField="Price" SortExpression="Price" DataFormatString="{0:c2}"
                            HeaderStyle-CssClass="col_price" ItemStyle-CssClass="col_price"></asp:BoundField>
                        <asp:BoundField HeaderText="Category" DataField="CategoryName" SortExpression="CategoryName"
                            HeaderStyle-CssClass="col_category" ItemStyle-CssClass="col_category"></asp:BoundField>
                        <asp:BoundField HeaderText="# Views" DataField="NumViews" SortExpression="NumViews"
                            HeaderStyle-CssClass="col_general" ItemStyle-CssClass="col_general"></asp:BoundField>
                        <asp:BoundField HeaderText="# Resp." DataField="NumResponses" SortExpression="NumResponses"
                            HeaderStyle-CssClass="col_general" ItemStyle-CssClass="col_general"></asp:BoundField>
                        <asp:HyperLinkField HeaderText="Actions" DataNavigateUrlFields="Id" DataNavigateUrlFormatString="~/EditAd.aspx?id={0}"
                            Text="Edit" HeaderStyle-CssClass="col_general" ItemStyle-CssClass="col_general"></asp:HyperLinkField>
                    </Columns>
                    <RowStyle CssClass="row1"></RowStyle>
                </asp:GridView>
                <asp:Panel ID="AdActionsPanel" Runat="server">
                    <br />
                    For Selected:&nbsp;
                    <asp:DropDownList ID="SelectedActionList" Runat="server">
                        <asp:ListItem Value="FeaturedLevel">Set Level: Featured Ads</asp:ListItem>
                        <asp:ListItem Value="NormalLevel">Set Level: Normal Ads</asp:ListItem>
                        <asp:ListItem Value="Activate">Mark as: Active</asp:ListItem>
                        <asp:ListItem Value="Unlist">Mark as: Inactive</asp:ListItem>
                        <asp:ListItem Value="Pending">Mark as: Activation Pending</asp:ListItem>
                        <asp:ListItem Value="MarkDeleted">Mark as: Deleted</asp:ListItem>
                        <asp:ListItem Value="Remove">Remove from Database</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Button ID="SelectedActionButton" Runat="server" Text="  Ok  " OnClick="SelectedActionButton_Click" />&nbsp;<br />
                </asp:Panel></asp:Panel>
        </div>
    </div>
    <table width="742" border="0" cellspacing="0" cellpadding="0" class="triple-header">
        <tr align="left" valign="top">
            <td style="width:216px">
            </td>
            <td style="width:269px">
            </td>
            <td style="width:257px">
            </td>
        </tr>
    </table>
    <asp:ObjectDataSource ID="AdsDataSource" Runat="server" TypeName="AspNet.StarterKits.Classifieds.BusinessLogicLayer.AdsDB"
        SelectMethod="GetAdsByQuery">
        <SelectParameters>
            <asp:ControlParameter Name="recordLimit" DefaultValue="50" Type="Int32" ControlID="ResultLimitDropDown"
                PropertyName="SelectedValue"></asp:ControlParameter>
            <asp:ControlParameter Name="categoryId" DefaultValue="0" Type="Int32" ControlID="CategoryDropDown"
                PropertyName="CurrentCategoryId"></asp:ControlParameter>
            <asp:Parameter Name="memberId" DefaultValue="0" Type="Int32" />
            <asp:Parameter Type="Decimal" DefaultValue="-1" Name="maxPrice"></asp:Parameter>
            <asp:ControlParameter Name="searchTerm" DefaultValue="" Type="String" ControlID="QueryTextBox"
                PropertyName="Text"></asp:ControlParameter>
            <asp:ControlParameter Name="location" DefaultValue="" Type="String" ControlID="LocationTextBox"
                PropertyName="Text"></asp:ControlParameter>
            <asp:ControlParameter Name="adType" DefaultValue="0" Type="Int32" ControlID="AdTypeDropDown"
                PropertyName="SelectedValue"></asp:ControlParameter>
            <asp:ControlParameter Name="adStatus" Type="Int32" ControlID="AdStatusList" PropertyName="SelectedValue"
                DefaultValue=""></asp:ControlParameter>
            <asp:ControlParameter Name="adLevel" DefaultValue="" Type="Int32" ControlID="AdLevelList"
                PropertyName="SelectedValue"></asp:ControlParameter>
            <asp:ControlParameter Name="dayRange" DefaultValue="-1" Type="Int32" ControlID="DateRangeDropDown"
                PropertyName="SelectedValue"></asp:ControlParameter>
            <asp:Parameter Type="Boolean" DefaultValue="False" Name="mustHaveImage"></asp:Parameter>
        </SelectParameters>
    </asp:ObjectDataSource></asp:Content>
