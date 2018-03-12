<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeFile="PostAd.aspx.cs"
    Inherits="PostAd_aspx" Title="Post an Ad" %>
<%@ Register TagPrefix="uc1" TagName="LocationDropDown" Src="Controls/LocationDropDown.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CategoryPath" Src="Controls/CategoryPath.ascx" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="Main" Runat="server">
    <div id="body">
        <div id="col_main_left">
            <div id="user_assistance">
                <a id="content_start"></a>
                <h3>
                    Step 1: Select Category</h3>
                <p>
                    Lorem ipsum dolor sit amet, consectetuer adipiscing elit, 
                    sed diam nonummy nibh euismod tincidunt ut laoreet dolore 
                    magna aliquam erat volutpat.</p>
                <h3>
                    Step 2: Specify Ad</h3>
                <p>
                    Lorem ipsum dolor sit amet, consectetuer adipiscing elit, 
                    sed diam nonummy nibh euismod tincidunt ut laoreet dolore 
                    magna aliquam erat volutpat.</p>
                <h3>
                    Step 3: Add Photos</h3>
                <p>
                    Lorem ipsum dolor sit amet, consectetuer adipiscing elit, 
                    sed diam nonummy nibh euismod tincidunt ut laoreet dolore 
                    magna aliquam erat volutpat.</p>
            </div>
        </div>
        <div id="col_main_right">
            <asp:Wizard ID="PostAdWizard" Runat="server" OnFinishButtonClick="PostAdWizard_FinishButtonClick"
                DisplaySideBar="False" CssClass="wizard" StepStyle-CssClass="wizard-step" ActiveStepIndex="0"
                OnPreviousButtonClick="PostAdWizard_PreviousButtonClick" NavigationStyle-HorizontalAlign="Left" >
                <WizardSteps>
                    <asp:WizardStep ID="WizardStep1" Runat="server" Title="Category Selection">
                        <h2 class="section">
                            Post an Ad: Category Selection</h2>
                        <div class="content_right">
                            <fieldset>
                                <legend class="select_category">Please select a Category for the Ad:</legend>
                                <p>
                                    <uc1:CategoryPath ID="CategoryPath" Runat="server" OnCategorySelectionChanged="CategoryPath_CategorySelectionChanged" />
                                </p>
                                <div>
                                    <asp:DataList Runat="server" ID="SubcategoriesList" DataSourceID="SubcategoriesDS"
                                        OnItemCommand="SubcategoriesList_ItemCommand" CellSpacing="10" RepeatColumns="2">
                                        <ItemTemplate>
                                            <asp:LinkButton Runat="Server" ID="CategoryButton" CommandArgument='<%# Eval("Id") %>'
                                                Text='<%# Eval("Name") %>' Font-Size="12px" Font-Bold="True" />
                                        </ItemTemplate>
                                    </asp:DataList>
                                    <asp:ObjectDataSource ID="SubcategoriesDS" Runat="server" TypeName="AspNet.StarterKits.Classifieds.Web.CategoryCache"
                                        SelectMethod="GetCategoriesByParentId" OnSelected="SubcategoriesDS_Selected">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="CategoryPath" PropertyName="CurrentCategoryId"
                                                Type="Int32" DefaultValue="0" Name="parentCategoryId" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                </div>
                            </fieldset>
                        </div>
                    </asp:WizardStep>
                    <asp:WizardStep ID="AdDetailsStep" Runat="server" Title="Enter Ad Details">
                        <h2 class="section">
                            Post an Ad: Details</h2>
                        <div class="content_right">
                            <fieldset>
                                <legend>
                                    Your Selected Category:</legend>
                                    <asp:Label Runat="server" ID="CategoryPathLabel"></asp:Label>
                                    |
                                    <asp:LinkButton Runat="server" ID="ChangeCategoryButton" OnClick="ChangeCategoryButton_Click"
                                        ValidationGroup="ChangeCategory">Change</asp:LinkButton>
                                <p>
                                    <asp:ValidationSummary Runat="server" ID="ValidationSummary1" HeaderText="Please correct the following:" />
                                </p>
                                <p></p>
                                <legend>Type:</legend> 
                                   <asp:RadioButtonList Runat="server" ID="AdTypeSelection">
                                        <asp:ListItem Value="1" Selected="True">For Sale</asp:ListItem>
                                        <asp:ListItem Value="2">Wanted</asp:ListItem>
                                    </asp:RadioButtonList>
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
                                TextMode="MultiLine" Columns="80" Rows="8" CssClass="post_description"></asp:TextBox>
                            </span>
                            <asp:RequiredFieldValidator Runat="server" ErrorMessage="A Description is required."
                                ID="RequiredDescription" ControlToValidate="DescriptionTextBox">
                                *</asp:RequiredFieldValidator>
                            
                            <p>
                            </p>
                            <legend>URL: <span class="small_text">(optional)</span></legend>
                            <asp:TextBox Text='<%# Bind("URL") %>' Runat="server" ID="UrlTextBox" CssClass="post_url"></asp:TextBox>
                            <asp:CustomValidator runat="server" OnServerValidate="URLValidator_ServerValidate"
                             ErrorMessage="A valid URL starting starting with http:// or https:// is required."
                             ToolTip="A valid URL is required." ID="URLRequiredFormat" >*
                             </asp:CustomValidator>                                   
                            <p>
                            </p>
                            <legend>Price:</legend>$
                            <asp:TextBox Text='<%# Bind("Price", "{0:f2}") %>' Runat="server" ID="PriceTextBox"
                                CssClass="post_dollars"></asp:TextBox><asp:CustomValidator ID="PriceValidator" Runat="server"
                                    ErrorMessage="The Price is not valid." OnServerValidate="PriceValidator_ServerValidate">
                                    *</asp:CustomValidator>
                            <p>
                            </p>
                            <legend>Location:</legend>
                            <uc1:LocationDropDown Runat="server" ID="LocationDropDown" />
                            <asp:CustomValidator Runat="server" ErrorMessage="A valid Location is required."
                                ID="ValidLocationRequired" OnServerValidate="ValidLocationRequired_ServerValidate">
                                *</asp:CustomValidator>
                           <p>
                            </p>
                            <legend>For how long should this Ad be listed?</legend>
                                <asp:DropDownList ID="NumDaysList" Runat="server">
                                    <asp:ListItem Value="2">2 days</asp:ListItem>
                                    <asp:ListItem Value="7" Selected="True">7 days</asp:ListItem>
                                    <asp:ListItem Value="14">14 days</asp:ListItem>
                                    <asp:ListItem Value="21">21 days</asp:ListItem>
                                </asp:DropDownList>
                            </fieldset>
                        </div>
                    </asp:WizardStep>
                    <asp:WizardStep ID="WizardStep2" Runat="server" StepType="Complete" Title="Done">
                        <h2 class="section">
                            Done!</h2>
                        <div class="content_right">
                            <p>
                                Your Ad was successfully submitted.&nbsp;</p>
                            <p style="text-align: center">
                                <asp:HyperLink Runat="server" ID="UploadImagesLink" Font-Bold="True" NavigateUrl="ManagePhotos.aspx?id=">Upload Photos for your Ad now</asp:HyperLink>
                                <br />
                                    <asp:HyperLink Runat="server" ID="MyAdsLink" Font-Bold="True" NavigateUrl="~/MyAds.aspx">Go to My Ads page</asp:HyperLink>
                            </p>
                        </div>
                    </asp:WizardStep>
                </WizardSteps>
                <StepStyle CssClass="wizard-step" />
                <NavigationStyle HorizontalAlign="Left" />
            </asp:Wizard>
        </div>
    </div>
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
