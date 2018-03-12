<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeFile="MyProfile.aspx.cs"
    Inherits="MyProfile_aspx" Title="My Profile" %>
<%@ Register TagPrefix="uc1" TagName="CategoryDropDown" Src="Controls/CategoryDropDown.ascx" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="Main" Runat="server">
    <div id="body">
        <div id="col_main_left">
            <div id="user_assistance">
                <a id="content_start"></a>
                <h3>
                    Actions</h3>
                <p>
                    <a href="MyAds.aspx">back to My Ads</a></p>
                <h3>
                    Help and Other Links</h3>
                <p>
                    Lorem ipsum dolor sit amet, consectetuer adipiscing elit, 
                    sed diam nonummy nibh euismod tincidunt ut laoreet dolore 
                    magna aliquam erat volutpat.</p>
            </div>
        </div>
        <div id="col_main_right">
            <h2 class="section">
                My Profile</h2>
            <div class="content_right">
                <fieldset>
                    <legend>First Name:</legend><span>
                    <asp:TextBox ID="FirstNameTextBox" Runat="server" CssClass="user_info"></asp:TextBox></span>
                    <asp:RequiredFieldValidator Runat="server" ControlToValidate="FirstNameTextBox" ValidationGroup="ChangeProfile" ErrorMessage="First name is required."
                        ToolTip="First name is required." ID="FirstNameRequired" Display="Dynamic">
                    </asp:RequiredFieldValidator>
                     <asp:CustomValidator ID="FirstNameRequiredFormat" runat="server" 
                        ValidationGroup="CreateUserWizardControl" ControlToValidate="FirstNameTextBox"
                        ErrorMessage="First name is required and must be less than 40 characters long and contain apostrophes, spaces, or periods." 
                        Display="Dynamic" OnServerValidate="FirstNameValidator_ServerValidate" ToolTip="A valid first name is required.">
                     </asp:CustomValidator>
                    <legend>Last Name:</legend><span>
                    <asp:TextBox ID="LastNameTextBox" Runat="server" CssClass="user_info"></asp:TextBox></span>
                    <asp:RequiredFieldValidator Runat="server" ControlToValidate="LastNameTextBox" ValidationGroup="ChangeProfile" ErrorMessage="Last name is required."
                         ToolTip="Last name is required." ID="LastNameRequired" Display="Dynamic">
                    </asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="LastNameRequiredFormat" runat="server" 
                         ValidationGroup="CreateUserWizardControl" ControlToValidate="LastNameTextBox"
                         ErrorMessage="Last name is required and must be less than 40 characters long and contain apostrophes, spaces, or periods."
                         Display="Dynamic" OnServerValidate="LastNameValidator_ServerValidate" ToolTip="A valid last name is required.">
                    </asp:CustomValidator>
                    <legend>Email:</legend><span>
                    <asp:TextBox ID="EmailTextBox" Runat="server" CssClass="user_info"></asp:TextBox></span>
                    <asp:RequiredFieldValidator Runat="server" ControlToValidate="EmailTextBox" ValidationGroup="ChangeProfile" ErrorMessage="Email is required."
                         ToolTip="Email is required." ID="EmailRequired" Display="Dynamic">
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator runat="server" ID="EmailRequiredFormat"
                         ControlToValidate="EmailTextBox" ValidationGroup="ChangeProfile"
                         ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$"
                         ErrorMessage="A valid email is required."
                         ToolTip="A valid email is required." display="dynamic">
                    </asp:RegularExpressionValidator>
                    <asp:Label ID="EmailNotValid" runat="server" Visible="False" ForeColor="red"></asp:Label>
                    <p>
                        <asp:Button ID="SaveButton" Runat="server" Text="Save" Width="88px" OnClick="SaveButton_Click" />
                        <asp:Button ID="CancelButton" Runat="server" Text="Cancel" Width="88px" OnClick="CancelButton_Click"
                            CausesValidation="False" />
                    </p>
                    <h3 class="section">Login Information</h3>
                    <asp:ChangePassword ID="ChangePasswordControl" Runat="server" 
                        ContinueButtonText="Return to My Ads" ContinueButtonType="Link" ContinueDestinationPageUrl="~/MyAds.aspx"
                        OnChangedPassword="ChangePasswordControl_ChangedPassword" LabelStyle-HorizontalAlign="Left">
                    </asp:ChangePassword>
                </fieldset>
            </div>
        </div>
    </div>
</asp:Content>