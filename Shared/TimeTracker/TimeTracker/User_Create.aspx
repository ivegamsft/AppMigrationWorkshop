<%@ Page Language="C#" MasterPageFile="~/TimeTracker/MasterPage.master" CodeFile="User_Create.aspx.cs"
    Inherits="User_Create_aspx" Title="My Company - Time Tracker - Administration - User Creation" Culture="auto" UICulture="auto"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <div id="adminedit">
        <a name="content_start" id="content_start"></a>
        <fieldset>
            <!-- add H2 here and hide it with css since you can not put h2 inside a legend tag -->
            <h2 class="none">
                User Detail</h2>
            <legend>User Detail</legend>
            <asp:CreateUserWizard ID="CreateUserWizard1" runat="server" OnFinishButtonClick="Wizard_FinishButton_Click"
                ContinueDestinationPageUrl="TimeEntry.aspx" OnCreatedUser="CreateUserWizard1_CreatedUser">
                <WizardSteps>
                    <asp:CreateUserWizardStep runat="server" Title="Sign Up for Your New Account">
                    </asp:CreateUserWizardStep>
                    <asp:WizardStep runat="server">
                        <center>
                            Add the user to a gropup:
                            <br />
                            <asp:DropDownList ID="GroupName" runat="server" Width="150px">
                                <asp:ListItem Value="0" Text="Project Administrator"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Project Manager"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Project Consultant"></asp:ListItem>
                            </asp:DropDownList>
                        </center>
                    </asp:WizardStep>
                    <asp:CompleteWizardStep runat="server" Title="Complete">
                    </asp:CompleteWizardStep>
                </WizardSteps>
                <TitleTextStyle CssClass="header-lightgray" />
            </asp:CreateUserWizard>
            <asp:Label runat="server" ID="noAccessMsg" Visible="false" EnableViewState="false"
                Text="User can not be created at this time. Please contact your admistratior"></asp:Label>
        </fieldset>
    </div>
</asp:Content>
