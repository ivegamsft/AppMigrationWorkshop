<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeFile="Register.aspx.cs"
    Inherits="Register_aspx" Title="Register" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="Main" Runat="server">
    <div id="body">
        <div id="col_main_left">
            <div id="user_assistance">
                <a id="content_start"></a>
                <h3>
                    Help and Other Links</h3>
                <p>
                    User assistance goes here. Lorem ipsum dolor sit amet, consectetuer adipiscing elit,
                    sed diam nonummy nibh euismod.</p>
                <p>
                    Title Tips</p>
                <p>
                    Ut wisi enim ad minim veniam, quis nostrud exercitation ulliam corper suscipit lobortis.
                    Start Date &amp; Price Ut wisi enim ad minim veniam, quis nostrud.</p>
                <p>
                    Photos &amp; Attachments</p>
                <p>
                    Duis autem veleum iriure dolor in hendrerit in vulputate velit esse molestie consequat,
                    vel willum lunombro.</p>
                <p>
                    URL</p>
                <p>
                    Ut wisi enim ad minim veniam, quis nostrud exercitation ulliam corper suscipit lobortis
                    nisl ut.</p>
                <p>
                    Description</p>
                <p>
                    Wero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zzril
                    delenit augue duis dolore te feugait nulla facilisi.
                </p>
            </div>
        </div>
        <div id="col_main_right">
            <h2 class="section">
                Register</h2>
            <div class="content_right">
                <asp:Label id="InfoLabel" runat="server" ForeColor="Red" Font-Italic="true" visible="false"/>
                <fieldset>
                    <asp:CreateUserWizard ID="CreateUserWizardControl" Runat="server" OnCreatedUser="CreateUserWizardControl_CreatedUser"
                        CreateUserButtonText="Submit" TitleTextStyle-Height="50px" FinishDestinationPageUrl="~/Default.aspx"
                        ContinueDestinationPageUrl="~/Default.aspx" ContinueButtonText="Continue to Homepage"
                        ContinueButtonType="Link" ActiveStepIndex="0" CssClass="register_text" OnCreateUserError="CreateUserWizardControl_CreateUserError">
                        <WizardSteps>
                            <asp:CreateUserWizardStep ID="CreateUserWizardStep1" Runat="server" Title="Account Details:">
                                <ContentTemplate>
                                    <legend>First Name:</legend><span>
                                    <asp:TextBox Runat="server" ID="FirstName" CssClass="user_info"></asp:TextBox></span>
                                    <asp:RequiredFieldValidator Runat="server" ControlToValidate="FirstName" ValidationGroup="CreateUserWizardControl" ErrorMessage="First name is required."
                                        ToolTip="First name is required." ID="FirstNameRequired" Display="Dynamic">
                                        </asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="FirstNameRequiredFormat" runat="server" 
                                         ValidationGroup="CreateUserWizardControl" ControlToValidate="FirstName"
                                         ErrorMessage="First name is required and must be less than 40 characters long and contain apostrophes, spaces, or periods." 
                                         Display="Dynamic" OnServerValidate="FirstNameValidator_ServerValidate" ToolTip="A valid first name is required.">
                                    </asp:CustomValidator>
                                    <legend>Last Name:</legend>
                                    <asp:TextBox Runat="server" ID="LastName" CssClass="user_info"></asp:TextBox></span>
                                    <asp:RequiredFieldValidator Runat="server" ControlToValidate="LastName" ValidationGroup="CreateUserWizardControl" ErrorMessage="Last name is required."
                                        ToolTip="Last name is required." ID="LastNameRequired" Display="Dynamic">
                                        </asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="LastNameRequiredFormat" runat="server" 
                                         ValidationGroup="CreateUserWizardControl" ControlToValidate="LastName"
                                         ErrorMessage="Last name is required and must be less than 40 characters long and contain apostrophes, spaces, or periods."
                                         Display="Dynamic" OnServerValidate="LastNameValidator_ServerValidate" ToolTip="A valid last name is required.">
                                    </asp:CustomValidator>
                                     <legend>Email:</legend><span>
                                    <asp:TextBox Runat="server" ID="Email" CssClass="user_info"></asp:TextBox></span>
                                        <asp:RequiredFieldValidator Runat="server" ControlToValidate="Email" ValidationGroup="CreateUserWizardControl" ErrorMessage="Email is required."
                                        ToolTip="Email is required." ID="EmailRequired" Display="Dynamic">
                                        </asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator runat="server"
                                            ControlToValidate="Email" ValidationGroup="CreateUserWizardControl"
                                            ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$"
                                            ErrorMessage="A valid email is required."
                                        ToolTip="A valid email is required." ID="EmailRequiredFormat" display="dynamic">
                                        </asp:RegularExpressionValidator>                                   
                                    <legend>User Name:</legend><span>
                                    <asp:TextBox Runat="server" ID="UserName" CssClass="user_info"></asp:TextBox></span>
                                    <asp:RequiredFieldValidator Runat="server" ControlToValidate="UserName" ValidationGroup="CreateUserWizardControl"
                                        ErrorMessage="User name is required." ToolTip="User name is required." ID="UserNameRequired" Display="Dynamic">
                                        </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator runat="server" 
                                         ControlToValidate="UserName" ValidationGroup="CreateUserWizardControl"
                                         ValidationExpression="^[0-9a-zA-Z''-'\s]{1,40}$"
                                         ErrorMessage="User name is required and must be less than 40 characters long and contain only alphanumerics, apostrophes, spaces, or periods."
                                         ToolTip="A valid user name is required." ID="UserNameRequiredFormat" Display="Dynamic">
                                        </asp:RegularExpressionValidator>                                    
                                    <legend>Password:</legend><span>
                                    <asp:TextBox Runat="server" TextMode="Password" ID="Password" CssClass="register_password"></asp:TextBox></span>
                                    <asp:RequiredFieldValidator Runat="server" ControlToValidate="Password" ValidationGroup="CreateUserWizardControl"
                                        ErrorMessage="Password is required." ToolTip="Password is required." ID="PasswordRequired">
                                        </asp:RequiredFieldValidator>
                                    <legend>Confirm Password:</legend><span>
                                    <asp:TextBox Runat="server" TextMode="Password" ID="ConfirmPassword" CssClass="register_password"></asp:TextBox></span>
                                    <asp:RequiredFieldValidator Runat="server" ControlToValidate="ConfirmPassword" ValidationGroup="CreateUserWizardControl"
                                        ErrorMessage="Confirm password is required." ToolTip="Confirm password is required."
                                        ID="ConfirmPasswordRequired">
                                        </asp:RequiredFieldValidator>
                                   <legend>Security Question:</legend><span>
                                    <asp:TextBox Runat="server" ID="Question" CssClass="register_question"></asp:TextBox></span>
                                    <asp:RequiredFieldValidator Runat="server" ControlToValidate="Question" ValidationGroup="CreateUserWizardControl"
                                        ErrorMessage="Security question is required." ToolTip="Security question is required."
                                        ID="QuestionRequired">
                                        </asp:RequiredFieldValidator>
                                    <legend>Security Answer:</legend><span>
                                    <asp:TextBox Runat="server" ID="Answer" CssClass="register_question"></asp:TextBox></span>
                                    <asp:RequiredFieldValidator Runat="server" ControlToValidate="Answer" ValidationGroup="CreateUserWizardControl"
                                        ErrorMessage="Security answer is required." ToolTip="Security answer is required."
                                        ID="AnswerRequired">
                                        </asp:RequiredFieldValidator>
                                    <asp:CompareValidator Runat="server" ControlToValidate="ConfirmPassword" ValidationGroup="CreateUserWizardControl"
                                        ID="PasswordCompare" Display="Dynamic" ErrorMessage="The password and confirmation password must match."
                                        ControlToCompare="Password">
                                    </asp:CompareValidator>
                                    <asp:Literal Runat="server" ID="FailureText" EnableViewState="True"></asp:Literal>
                                </ContentTemplate>
                            </asp:CreateUserWizardStep>
                            <asp:CompleteWizardStep Runat="server">
                            </asp:CompleteWizardStep>
                        </WizardSteps>
                        <FinishNavigationTemplate>
                            <asp:Button Runat="server" CausesValidation="False" Text="Previous" CommandName="MovePrev"
                                ID="FinishPreviousButton" />
                            <asp:Button Runat="server" Text="Finish" CommandName="MoveToFinish" ID="FinishButton" />
                        </FinishNavigationTemplate>
                        <TitleTextStyle Height="50px"></TitleTextStyle>
                    </asp:CreateUserWizard>
                 </fieldset>
            </div>
        </div>
    </div>
</asp:Content>
