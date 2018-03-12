<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeFile="Settings.aspx.cs"
    Inherits="Settings_aspx" Title="Site Settings" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="Main" Runat="server">
    <div id="body">
        <div id="col_main_left">
            <div id="user_assistance">
                <a id="content_start"></a>
                <h3>
                    Actions</h3>
                <p>
                    <asp:HyperLink ID="BackToAdminLink" Runat="server" NavigateUrl="~/Admin/Default.aspx">back to Administration</asp:HyperLink>&nbsp;</p>
                <asp:Panel ID="UpdateErrorPanel" Runat="server" Visible="False" EnableViewState="False">
                    <p>
                        <strong>There was an error writing to the Settings file<br />
                        App_Data\site-config.xml.</strong><br />Please make sure that the ASP.NET process has write-access to the file.</p>
                        <p>The textbox below contains the XML equivalent of the Site Settings you saved. You can copy it and update the file manually.</p>
                        <asp:TextBox ID="SettingsXmlTextBox" Runat="server" TextMode="multiLine" Rows="20" Width="100%"></asp:TextBox>
                </asp:Panel>
                
            </div>
        </div>
        <div id="col_main_right">
            <h2 class="section">
                Site Settings</h2>
            <div class="content_right">
                <fieldset>
                    <asp:FormView ID="SettingsFormView" Runat="server" DataSourceID="SettingsDataSource"
                        DefaultMode="Edit">
                        <EditItemTemplate>
                            <legend>Ads require activation before appearing on the site:</legend>
                            <asp:CheckBox Checked='<%# Bind("AdActivationRequired") %>' Runat="server" ID="AdApprovalRequiredCheckBox"
                                Text="Yes" />
                            <p class="new_section">
                            </p>
                            <legend>Maximum Number of Days for which an Ad is active:</legend><span>
                            <asp:TextBox Text='<%# Bind("MaxAdRunningDays") %>' Runat="server" ID="MaxAdRunningDaysTextBox"
                                Width="30px"></asp:TextBox>
                            <p class="new_section">
                            </p>
                            <legend>Allow Users to edit their Ads:</legend>
                            <asp:CheckBox Checked='<%# Bind("AllowUsersToEditAds") %>' Runat="server" ID="AllowUsersToEditAdsCheckBox"
                                Text="Yes" />
                            <p class="new_section">
                            </p>
                            <legend>When Users remove inactive Ads:</legend>
                            <asp:RadioButtonList ID="AllowUsersToRemoveAdsChoice" Runat="server" SelectedValue='<%# Bind("AllowUsersToDeleteAdsInDB") %>'>
                                <asp:ListItem Value="True">Remove them from the Database</asp:ListItem>
                                <asp:ListItem Value="False">Keep in Database, but mark as Deleted</asp:ListItem>
                            </asp:RadioButtonList>
                             <p class="new_section">
                            </p>
                            <legend>Maximum Number of Photos to Upload:</legend><span>
                            <asp:TextBox Text='<%# Bind("MaxPhotosPerAd") %>' Runat="server" ID="MaxPhotosPerAdTextBox"
                                Width="30px"></asp:TextBox>
                            (0 to disable uploads)
                             <p class="new_section">
                            </p>
                            <legend>Uploaded Photo:</legend>
                            <asp:RadioButtonList ID="StoringPhotosChoice" Runat="server" SelectedValue='<%# Bind("StorePhotosInDatabase") %>'>
                                <asp:ListItem Value="True">Store Photos in Database.</asp:ListItem>
                                <asp:ListItem Value="False">Store Photos in this Directory:</asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:TextBox Text='<%# Bind("ServerPhotoUploadDirectory") %>' Runat="server" ID="ServerPhotoUploadDirectory"
                                CssClass="post_title" /></span> <span class="small_text">
                                <br />
                                (must be within your web application directory and writable for the ASP.NET process)</span>
                             <p class="new_section">
                            </p>
                            <legend>For new postings, send notifications to Administrators:</legend>
                            <asp:DropDownList ID="AdminNotificationDropDown" Runat="server" SelectedIndex='<%# Bind("AdminNotification") %>'
                                CssClass="post_title">
                                <asp:ListItem Value="0">None</asp:ListItem>
                                <asp:ListItem Value="1">for each Ad</asp:ListItem>
                                <asp:ListItem Value="2">Daily</asp:ListItem>
                                <asp:ListItem Value="3">Hourly</asp:ListItem>
                            </asp:DropDownList>
                             <p class="new_section">
                            </p>
                            <legend>Name of this Site:</legend>
                            <asp:TextBox Text='<%# Bind("SiteName") %>' Runat="server" ID="SiteNameTextBox" CssClass="post_title"></asp:TextBox><br />
                            <p>
                            </p>
                            <legend>Contact Email Address:</legend><span>
                            <asp:TextBox Text='<%# Bind("SiteEmailAddress") %>' Runat="server" ID="SiteEmailAddressTextBox"
                                CssClass="post_title"></asp:TextBox></span>
                             <p class="new_section">
                            </p>
                            <p>
                                <asp:Button ID="UpdateButton" Runat="server" Text="Update" CommandName="Update" />
                                <asp:Button ID="Cancel" Runat="server" Text="Cancel" OnClick="Cancel_Click" />
                            </p>
                        </EditItemTemplate>
                    </asp:FormView>
                    <asp:ObjectDataSource ID="SettingsDataSource" Runat="server" TypeName="AspNet.StarterKits.Classifieds.BusinessLogicLayer.SiteSettings"
                        SelectMethod="GetSharedSettings" UpdateMethod="UpdateSettings" DataObjectTypeName="AspNet.StarterKits.Classifieds.BusinessLogicLayer.SiteSettings" OnUpdated="SettingsDataSource_Updated">
                        <UpdateParameters>
                            <asp:Parameter Type="Boolean" Name="AdActivationRequired"></asp:Parameter>
                            <asp:Parameter Type="Int32" Name="MaxAdRunningDays"></asp:Parameter>
                            <asp:Parameter Type="Int32" Name="MaxPhotosPerAd"></asp:Parameter>
                            <asp:Parameter Type="Boolean" Name="StorePhotosInDatabase"></asp:Parameter>
                            <asp:Parameter Type="Boolean" Name="AllowUsersToEditAds"></asp:Parameter>
                            <asp:Parameter Type="Boolean" Name="AllowUsersToDeleteAdsInDB"></asp:Parameter>
                            <asp:Parameter Type="String" Name="ServerPhotoUploadDirectory"></asp:Parameter>
                            <asp:Parameter Name="AdminNotification"></asp:Parameter>
                            <asp:Parameter Type="String" Name="SiteName"></asp:Parameter>
                            <asp:Parameter Type="String" Name="SiteEmailAddress"></asp:Parameter>
                        </UpdateParameters>
                    </asp:ObjectDataSource>
                </fieldset>
            </div>
        </div>
    </div>
</asp:Content>
