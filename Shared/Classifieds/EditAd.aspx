<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeFile="EditAd.aspx.cs"
    Inherits="EditAd_aspx" Title="Edit Ad Details" %>
<%@ Register TagPrefix="uc1" TagName="CategoryDropDown" Src="~/Controls/CategoryDropDown.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LocationDropDown" Src="Controls/LocationDropDown.ascx" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="Main" Runat="server">
    <div id="body">
        <div id="col_main_left">
            <div id="user_assistance">
                <a id="content_start"></a>
                <h3>
                    Actions</h3>
                <p>
                    <asp:HyperLink ID="BackLink" Runat="server">Go Back</asp:HyperLink></p>
                <asp:PlaceHolder ID="ManagePhotosLinkPanel" Runat="server"><p>
                    <asp:HyperLink ID="ManagePhotosLink" Runat="server">Manage Photos</asp:HyperLink></p>
                </asp:PlaceHolder>
                <p>
                    <asp:HyperLink ID="ShowAdLink" Runat="server">Show Ad Page</asp:HyperLink></p>
                <p>
                    &nbsp;</p>
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
                Edit Ad Details</h4>
            <div class="content_right">
                <fieldset>
                    <legend class="none">Title and Listing Information</legend><legend>Category: </legend>
                    <asp:PlaceHolder ID="ChangeCategoryPanel" Runat="server" Visible="False">
                        <uc1:CategoryDropDown ID="CategoryDropDown" Runat="server" AllCategoriesOptionVisible="false"></uc1:CategoryDropDown>
                        <p><asp:Button ID="ChangeCategoryOkButton" Runat="server" Text="  Ok  " OnClick="ChangeCategoryOkButton_Click" />
                        <asp:Button ID="ChangeCategoryCancelButton" Runat="server" Text="Cancel" OnClick="ChangeCategoryCancelButton_Click" />
                        </p></asp:PlaceHolder>
                    <asp:FormView ID="AdFormView" Runat="server" DataSourceID="AdDataSource" DefaultMode="Edit"
                        DataKeyNames="Id" OnItemUpdating="AdFormView_ItemUpdating">
                        <EditItemTemplate>
                            <asp:Label Text='<%# Eval("CategoryName") %>' Runat="server" ID="CategoryNameLabel" />
                            |
                            <asp:LinkButton ID="ChangeCategoryButton" Runat="Server" OnClick="ChangeCategoryButton_Click">Change Category</asp:LinkButton>
                            
                            <p>
                                <asp:ValidationSummary Runat="server" ID="ValidationSummary" HeaderText="Please correct the following:" /></p>
                            <p>
                            </p>
                            <legend>Title: <span class="small_text">(50 characters max)</span></legend><span>
                            <asp:TextBox Text='<%# Bind("Title") %>' Runat="server" ID="TitleTextBox" CssClass="post_title" MaxLength="50"></asp:TextBox></span>
                            <asp:RequiredFieldValidator Runat="server" ErrorMessage="A Title for the ad is required."
                                ID="RequiredTitle" ControlToValidate="TitleTextBox">
                                *</asp:RequiredFieldValidator>
                            <p>
                            </p>
                            <legend>Description: <span class="small_text">(500 characters max)</span></legend><span>
                            <asp:TextBox Text='<%# Bind("Description") %>' Runat="server" ID="DescriptionTextBox"
                                TextMode="MultiLine" Columns="80" Rows="8" CssClass="post_description" MaxLength="10"></asp:TextBox>
                            </span>
                            <asp:RequiredFieldValidator Runat="server" ErrorMessage="A Description is required."
                                ID="RequiredDescription" ControlToValidate="DescriptionTextBox">
                                *</asp:RequiredFieldValidator>
                            <p>
                            </p>
                            <legend>URL: <span class="small_text">(optional)</span></legend><span>
                            <asp:TextBox Text='<%# Bind("URL") %>' Runat="server" ID="URLTextBox" CssClass="post_url"></asp:TextBox>
                            </span>
                            <asp:CustomValidator runat="server" OnServerValidate="URLValidator_ServerValidate"
                             ErrorMessage="A valid URL starting starting with http:// or https:// is required."
                             ToolTip="A valid URL is required." ID="URLRequiredFormat" >*
                             </asp:CustomValidator>                                   
                            <p>
                            </p>
                            <legend>Price:</legend>$
                            <asp:TextBox Text='<%# Bind("Price", "{0:f2}") %>' Runat="server" ID="PriceTextBox"
                                CssClass="post_dollars"></asp:TextBox>
                             <asp:CustomValidator ID="PriceValidator" Runat="server" ControlToValidate="PriceTextBox" 
                                    ErrorMessage="The Price is not valid." OnServerValidate="PriceValidator_ServerValidate">
                                    *</asp:CustomValidator>
                            <p>
                            </p>
                            <legend>Location:</legend>
                            <uc1:LocationDropDown Runat="Server" ID="LocationDropDown" CurrentLocation='<%# Bind("Location") %>' />
                            <asp:CustomValidator Runat="server" ErrorMessage="A valid Location is required."
                                ID="ValidLocationRequired" OnServerValidate="ValidLocationRequired_ServerValidate">
                                *</asp:CustomValidator>
                           
                            <p>
                                <asp:Button ID="UpdateButton" Runat="server" Text="Save" CommandName="Update" />
                                <asp:Button ID="CancelButton" Runat="server" Text="Cancel" OnClick="CancelButton_Click" />
                            </p>
                        </EditItemTemplate>
                    </asp:FormView>
                </fieldset>
            </div>
        </div>
    </div>
                <asp:ObjectDataSource ID="AdDataSource" Runat="server" TypeName="AspNet.StarterKits.Classifieds.BusinessLogicLayer.AdsDB"
                    SelectMethod="GetAdById" UpdateMethod="UpdateAd" OnUpdated="AdDataSource_Updated"
                    OnSelected="AdDataSource_Selected" OldValuesParameterFormatString="original_{0}">
                    <UpdateParameters>
                    <asp:Parameter Name="original_Id" Type="Int32" />
                        <asp:ProfileParameter Type="Int32" Name="memberId" PropertyName="MemberId" />
                        <asp:Parameter Type="String" Name="title"></asp:Parameter>
                        <asp:Parameter Type="String" Name="description"></asp:Parameter>
                        <asp:Parameter Type="String" Name="URL"></asp:Parameter>
                        <asp:Parameter Type="Decimal" Name="price"></asp:Parameter>
                        <asp:Parameter Type="String" Name="location"></asp:Parameter>
                        <asp:Parameter Type="Boolean" Name="isRelisting" DefaultValue="False"></asp:Parameter>
                    </UpdateParameters>
                    <SelectParameters>
                        <asp:QueryStringParameter Name="adId" DefaultValue="0" Type="Int32" QueryStringField="id"></asp:QueryStringParameter>
                    </SelectParameters>
                </asp:ObjectDataSource>
<script type="text/javascript">
function textCounter(elem, maxLimit) 
{
    if (elem.value.length > maxLimit)
    {
       elem.value = elem.value.substring(0, maxLimit);
    }
} 

</script>    
</asp:Content>
