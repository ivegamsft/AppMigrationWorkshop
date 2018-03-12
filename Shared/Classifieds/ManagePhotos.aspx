<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeFile="ManagePhotos.aspx.cs"
    Inherits="ManagePhotos_aspx" Title="Manage Photos" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="Main" runat="server">
    <div id="body">
        <div id="col_main_left">
            <div id="user_assistance">
                <a id="content_start"></a>
                <h3>
                    Actions</h3>
                <p>
                    <asp:HyperLink ID="BackToEditAdLink" runat="server">Back to Edit Ad</asp:HyperLink></p>
                <p>
                    <asp:HyperLink ID="MyAdsLink" runat="server" NavigateUrl="~/MyAds.aspx">Back to My Ads</asp:HyperLink></p>
                <p>
                    <asp:HyperLink ID="ShowAdLink" runat="server">Show Ad</asp:HyperLink></p>
                <h3>
                    Help</h3>
                <p>
                    Lorem ipsum dolor sit amet, consectetuer adipiscing elit, 
                    sed diam nonummy nibh euismod tincidunt ut laoreet dolore 
                    magna aliquam erat volutpat.</p>
            </div>
        </div>
        <div id="col_main_right">
            <h4 class="section">
                Manage Photos</h4>
            <div class="content_right">
                Ad:
                <asp:Label ID="AdTitleLabel" runat="server" Font-Bold="True"></asp:Label>
                <asp:Panel ID="NoUploadsPanel" runat="server" Visible="False">
                    <p>
                        The site does not offer uploading ad photos at this point.</p>
                </asp:Panel>
                <asp:Panel ID="MainUploadsPanel" runat="server">
                    <p>
                        You can upload up to
                        <asp:Label ID="NumTotalUploadLabel" runat="server" Text="x Photos"></asp:Label>
                        in total.</p>
                    <asp:Panel ID="PreviewTipPanel" runat="server">
                        <p>
                            Use the
                            <img src="Images/non-preview-photo.gif" alt="Non Preview Photo" />
                            icon to select a different thumbnail photo for your ad.</p>
                    </asp:Panel>
                    <asp:Label ID="UploadErrorMessage" runat="server" Visible="False" ForeColor="Red">
                    The selected file was not a recognizable image type.
                    </asp:Label>
                    <asp:GridView ID="PhotoGridView" runat="server" DataSourceID="PhotoDataSource" AutoGenerateColumns="False"
                        DataKeyNames="Id" OnRowCommand="PhotoGridView_RowCommand" BorderWidth="1" ShowFooter="false"
                        ShowHeader="false" CellPadding="10">
                        <EmptyDataTemplate>
                            No photos have been uploaded for this Ad.
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Image runat="Server" ID="IsCurrentPreview" Visible='<%# ((bool)Eval("IsMainPreview")) %>'
                                        ImageUrl="~/Images/preview-photo.gif" AlternateText="Icon indicating that there are photos for this ad."></asp:Image>
                                    <asp:ImageButton ID="SelectAsPreviewLink" runat="Server" CommandName="SelectAsPreview"
                                        AlternateText="Select as Preview" CommandArgument='<%# Eval("Id") %>' Visible='<%# !((bool)Eval("IsMainPreview")) %>'
                                        ImageUrl="~/Images/non-preview-photo.gif" ></asp:ImageButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <img src='<%# Eval("Id", "PhotoDisplay.ashx?photoid={0}&size=medium") %>' style="border:0;"
                                        alt="Photo of Ad" /></ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowDeleteButton="True" DeleteText="Remove"></asp:CommandField>
                        </Columns>
                        <RowStyle CssClass="row1"></RowStyle>
                        <AlternatingRowStyle CssClass="row2"></AlternatingRowStyle>
                        <FooterStyle CssClass="item_list_footer"></FooterStyle>
                        <HeaderStyle CssClass="item_list_footer"></HeaderStyle>
                    </asp:GridView>
                    <asp:ObjectDataSource ID="PhotoDataSource" runat="server" TypeName="AspNet.StarterKits.Classifieds.BusinessLogicLayer.PhotosDB"
                        SelectMethod="GetPhotosByAdId" DeleteMethod="RemovePhotoById" InsertMethod="InsertPhoto"
                        OldValuesParameterFormatString="{0}" OnSelected="PhotoDataSource_Selected">
                        <SelectParameters>
                            <asp:QueryStringParameter Name="adId" DefaultValue="0" QueryStringField="id" Type="Int32">
                            </asp:QueryStringParameter>
                        </SelectParameters>
                        <InsertParameters>
                            <asp:QueryStringParameter Name="adId" DefaultValue="0" QueryStringField="id" Type="Int32">
                            </asp:QueryStringParameter>
                            <asp:Parameter Name="bytesFull"></asp:Parameter>
                            <asp:Parameter Name="bytesMedium"></asp:Parameter>
                            <asp:Parameter Name="bytesSmall"></asp:Parameter>
                            <asp:Parameter Name="useAsPreview" Type="boolean"></asp:Parameter>
                        </InsertParameters>
                    </asp:ObjectDataSource>
                    <asp:DetailsView ID="UploadPhotoDetailsView" runat="server" DataSourceID="PhotoDataSource"
                        DefaultMode="Insert" DataKeyNames="Id" AutoGenerateRows="False" OnItemInserting="UploadPhotoDetailsView_ItemInserting"
                        CellPadding="5" GridLines="None" CellSpacing="5">
                        <Fields>
                            <asp:TemplateField>
                                <InsertItemTemplate>
                                    <asp:FileUpload ID="PhotoFile" Runat="server" FileBytes='<%# Bind("UploadBytes") %>' />
                                </InsertItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowInsertButton="True" InsertText="Upload New" ButtonType="Button"
                                ShowCancelButton="False"></asp:CommandField>
                        </Fields>
                        <InsertRowStyle Width="50%"></InsertRowStyle>
                    </asp:DetailsView>
                </asp:Panel>
            </div>
        </div>
    </div>
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="two-columns">
        <tr>
            <td align="left" valign="top" class="left" style="width:210px">
            </td>
            <td align="left" valign="top">
                <h2>
                </h2>
            </td>
        </tr>
    </table>
</asp:Content>
