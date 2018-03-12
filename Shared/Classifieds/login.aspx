<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeFile="Login.aspx.cs" 
    Inherits="Login_aspx" Title="Login" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="Main" Runat="server">
    <div id="body">
        <div id="col_main_left">
            <div id="user_assistance">
                <a id="content_start"></a>
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
                Login</h2>
            <div class="content_right">
                <asp:Panel ID="AccessNoticePanel" Runat="server" EnableViewState="False" Visible="False">
                    <b>You have accessed a page or feature that requires login information.</b><br />
                    Use the form below to log in using your account information, or <b>
                    <asp:HyperLink ID="ProtectedPageRegisterLink" Runat="server" NavigateUrl="~/Register.aspx">click here to register</asp:HyperLink></b>.<br />
                    <br />
                </asp:Panel>
                <asp:Login ID="LoginConrol" Runat="server" TitleText="" CssClass="login_box">
                <TextBoxStyle CssClass="text"></TextBoxStyle>                
                </asp:Login>
                <p>
                    <asp:HyperLink ID="RegisterLink" Runat="server" NavigateUrl="~/Register.aspx">Create an Account</asp:HyperLink>
                </p>
                <p>
                    <asp:LinkButton ID="ForgotPasswordButton" Runat="server" OnClick="ForgotPasswordButton_Click">Forgot Password?</asp:LinkButton>
                </p>
                <asp:PasswordRecovery ID="PasswordRecovery" Runat="server" Visible="False"
                     UserNameTitleText="" 
                     QuestionTitleText="Step 2: Identity Confirmation." 
                     UserNameInstructionText="Step 1: Enter your User Name." Width="280px" OnInit="PasswordRecovery_Init" OnSendMailError="PasswordRecovery_SendMailError">
                    <TitleTextStyle Font-Bold="True"></TitleTextStyle>
                    <InstructionTextStyle Font-Bold="True"></InstructionTextStyle>
                    <LabelStyle Wrap="False" />
                </asp:PasswordRecovery>
                <p>&nbsp;</p>
                <p>&nbsp;</p>
                <p>&nbsp;</p>
                <p>&nbsp;</p>
                <p>&nbsp;</p>
            </div>
        </div>
    </div>
</asp:Content>
