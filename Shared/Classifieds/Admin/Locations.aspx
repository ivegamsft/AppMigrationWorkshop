<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeFile="Locations.aspx.cs"
    Inherits="Locations_aspx" Title="Manage Locations" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="Main" Runat="server">
    <div id="body">
        <div id="col_main_left">
            <div id="user_assistance">
                <a id="content_start"></a>
                <h3>
                    Actions</h3>
                <p>
                    <asp:HyperLink ID="BackToAdminLink" Runat="server" NavigateUrl="~/Admin/Default.aspx">back to Administration</asp:HyperLink></p>
            </div>
        </div>
        <div id="col_main_right">
            <h2 class="section">
                Locations</h2>
            <div class="content_right">
                <fieldset>
                    <p>
                        <asp:ListBox ID="LocationsListBox" Runat="server" DataSourceID="LocationsDataSource"
                            DataValueField="Id" DataTextField="Name" CssClass="fixed" Rows="6">
                        </asp:ListBox></p>
                    <p>
                        <asp:Button ID="RemoveLocationButton" Runat="server" Text="Remove selected" OnClick="RemoveLocationButton_Click" />
                    </p>
                    <legend>New:</legend><span>
                    <asp:TextBox ID="NewLocationTextBox" Runat="server" CssClass="fixed" ValidationGroup="AddLocation"></asp:TextBox>
                    </span>
                    <asp:RequiredFieldValidator ID="RequiredLocationValidator" Runat="server" ErrorMessage="*"
                        ControlToValidate="NewLocationTextBox" ValidationGroup="AddLocation">
                    </asp:RequiredFieldValidator>
                    <asp:Button ID="AddLocationButton" Runat="server" Text="Add Location" OnClick="AddLocationButton_Click"
                        ValidationGroup="AddLocation" />
                    <asp:ObjectDataSource ID="LocationsDataSource" Runat="server" TypeName="AspNet.StarterKits.Classifieds.Web.LocationCache"
                        SelectMethod="GetAllLocations">
                    </asp:ObjectDataSource>
                </fieldset>
            </div>
        </div>
    </div>
</asp:Content>