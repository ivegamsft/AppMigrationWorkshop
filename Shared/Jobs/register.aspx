<%@ Page Language="C#" CodeFile="register.aspx.cs" Inherits="register_aspx" EnableTheming="true" MasterPageFile="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="server">

<div align=center>
    <asp:Label ID="Label2" Runat="server" Text="Employees/ Employers! Register here!!"
        SkinID="FormHeading"></asp:Label>
</div>        
    <br />
    Registration is a two step process:<br />
    <ol>
        <li>Choose a user name and password</li><li>Choose whether you would like to register as an "Employee" or "Employer".</li></ol>

<p>
    Once you register with us you can access respective secured areas for posting resumes
    or posting jobs.</p>

    <div align=center>
    <asp:CreateUserWizard ID="CreateUserWizard1" Runat="server" 
        CreateUserButtonText="Register Me" 
        OnContinueButtonClick="CreateUserWizard1_ContinueButtonClick" OnNextButtonClick="CreateUserWizard1_NextButtonClick" OnCreatedUser="CreateUserWizard1_CreatedUser" OnActiveStepChanged="CreateUserWizard1_ActiveStepChanged">
        <WizardSteps>
<asp:WizardStep Runat="server" ID="WizardStep2" Title="Please tell us about yourself"><table style="width: 100%"><tr><td contenteditable="true" align="right"><asp:Label Runat="server" Text="First Name :" SkinID="FormLabel" ID="Label3"></asp:Label>
 </td><td align="left"><asp:TextBox Runat="server" ID="TextBox1"></asp:TextBox>
     <asp:RequiredFieldValidator Runat="server" ControlToValidate="TextBox1" Display="Dynamic"
         ErrorMessage="Please enter first name" ID="RequiredFieldValidator1">
     </asp:RequiredFieldValidator>
 </td></tr><tr><td contenteditable="true" align="right"><asp:Label Runat="server" Text="Last Name :" SkinID="FormLabel" ID="Label4"></asp:Label>
 </td><td align="left"><asp:TextBox Runat="server" ID="TextBox2"></asp:TextBox>
     <asp:RequiredFieldValidator Runat="server" ControlToValidate="TextBox2" Display="Dynamic"
         ErrorMessage="Please enter last name" ID="RequiredFieldValidator2">
     </asp:RequiredFieldValidator>
 </td></tr></table></asp:WizardStep>
<asp:CreateUserWizardStep ID="CreateUserWizardStep1" Runat="server" Title="Sign Up for Your New Account"></asp:CreateUserWizardStep>
<asp:WizardStep ID="WizardStep1" Runat="server" Title="Choose your role" StepType=Step>
                <table width="100%">
                    <tr>
                        <td style="width: 100px">
                            <asp:Label Runat="server" Text="Register As :" ID="Label1"></asp:Label>









                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100px">
                            <asp:DropDownList Runat="server" Height="22px" Width="135px" ID="DropDownList1">
                            </asp:DropDownList>









                        </td>
                    </tr>
                </table>
            </asp:WizardStep>
<asp:CompleteWizardStep ID="CompleteWizardStep1" Runat="server" Title="Complete"></asp:CompleteWizardStep>
</WizardSteps><StartNavigationTemplate>
            <asp:Button Runat="server" Text="Next" CommandName="MoveNext" ID="StartNextButton" />
        
</StartNavigationTemplate>
    </asp:CreateUserWizard>
    </div>
    </asp:Content>

