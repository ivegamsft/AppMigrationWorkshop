<%@ Page Language="C#" CodeFile="companyprofile.aspx.cs" Inherits="companyprofile_aspx" MasterPageFile="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="server">
<div align=center>
           <asp:Label ID="Label13" Runat="server" Text="Modify Your Company Profile" SkinID="formheading"></asp:Label>
</div>           
           <br />
    <br />
        <table style="width: 100%">
            <tr>
                <td valign="top" align="left" colspan="2">
                    <asp:Label ID="Label14" Runat="server" Text="Introduce Your Company" SkinID="FormGroupLabel"></asp:Label>
                </td>
            </tr>
            <tr>
                <td valign="top" align="right" width="40%">
                    <asp:Label ID="Label1" Runat="server" Text="Company Name :"></asp:Label>
                </td>
                <td align="left" width="60%">
                    <asp:TextBox ID="txtCompanyName" Runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Runat="server" ErrorMessage="Please enter company name"
                        ControlToValidate="txtCompanyName" Display="Dynamic">
                        *</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td valign="top" align="right" width="40%">
                    <asp:Label ID="Label12" Runat="server" Text="Brief Profile :"></asp:Label>
                </td>
                <td align="left" width="60%">
                    <asp:TextBox ID="txtProfile" Runat="server" Width="100%" Rows="5" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" colspan="2">
                    <asp:Label ID="Label15" Runat="server" Text="Location" SkinID="FormGroupLabel"></asp:Label>
                </td>
            </tr>
            <tr>
                <td valign="top" align="right" width="40%">
                    <asp:Label ID="Label2" Runat="server" Text="Address 1 :"></asp:Label></td>
                <td align="left" width="60%">
                    <asp:TextBox ID="txtAddress1" Runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Runat="server" ErrorMessage="Please enter address"
                        ControlToValidate="txtAddress1" Display="Dynamic">
                        *</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td valign="top" align="right" width="40%">
                    <asp:Label ID="Label3" Runat="server" Text="Address 2 :"></asp:Label></td>
                <td align="left" width="60%">
                    <asp:TextBox ID="txtAddress2" Runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top" align="right" width="40%">
                    <asp:Label ID="Label4" Runat="server" Text="City :"></asp:Label></td>
                <td align="left" width="60%"><asp:TextBox ID="txtCity" Runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" Runat="server" ErrorMessage="Please enter city"
                        ControlToValidate="txtCity" Display="Dynamic">
                        *</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td valign="top" align="right" width="40%">
                    <asp:Label ID="Label6" Runat="server" Text="Country :"></asp:Label>
                </td>
                <td align="left" width="60%">
                    &nbsp;<asp:DropDownList ID="ddlCountry" Runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td valign="top" align="right" width="40%">
                    <asp:Label ID="Label5" Runat="server" Text="State :"></asp:Label>
                </td>
                <td align="left" width="60%">
                    &nbsp;<asp:DropDownList ID="ddlState" Runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td valign="top" align="right" width="40%">
                    <asp:Label ID="Label7" Runat="server" Text="ZIP :"></asp:Label></td>
                <td align="left" width="60%">
                    <asp:TextBox ID="txtZIP" Runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" Runat="server" ErrorMessage="Please enter ZIP code"
                        ControlToValidate="txtZIP" Display="Dynamic">
                        *</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Runat="server" ErrorMessage="Please enter valid ZIP code"
                        ValidationExpression="\d{5}(-\d{4})?" ControlToValidate="txtZIP" Display="Dynamic">
                        *</asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" colspan="2">
                    <asp:Label ID="Label16" Runat="server" Text="Contact Details" SkinID="FormGroupLabel"></asp:Label>
                </td>
            </tr>
            <tr>
                <td valign="top" align="right" width="40%">
                    <asp:Label ID="Label8" Runat="server" Text="Phone :"></asp:Label></td>
                <td align="left" width="60%">
                    <asp:TextBox ID="txtPhone" Runat="server"></asp:TextBox><br />
                    <asp:Label ID="Label17" runat="server" Text="(e.g. 111-111-1111)"></asp:Label>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" Runat="server" ErrorMessage="Please enter valid phone number"
                        ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}" ControlToValidate="txtPhone" Display="Dynamic">
                        *</asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" Runat="server" ErrorMessage="Please enter phone number"
                        ControlToValidate="txtProfile" Display="Dynamic">
                        *</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td valign="top" align="right" width="40%">
                    <asp:Label ID="Label9" Runat="server" Text="Fax :"></asp:Label></td>
                <td align="left" width="60%">
                    <asp:TextBox ID="txtFax" Runat="server"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" Runat="server" ErrorMessage="Please enter valid Fax number"
                        ValidationExpression="((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}" ControlToValidate="txtFax" Display="Dynamic">
                        *</asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td valign="top" align="right" width="40%">
                    <asp:Label ID="Label10" Runat="server" Text="Email :"></asp:Label></td>
                <td align="left" width="60%">
                    <asp:TextBox ID="txtEmail" Runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" Runat="server" ErrorMessage="Please enter email address"
                        ControlToValidate="txtEmail" Display="Dynamic">
                        *</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" Runat="server" ErrorMessage="Please enter valid email address"
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtEmail" Display="Dynamic">
                        *</asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td style=" height: 26px;" valign="top" align="right" width="40%">
                    <asp:Label ID="Label11" Runat="server" Text="Web Site :"></asp:Label></td>
                <td style=" height: 26px;" align="left" width="60%">
                    <asp:TextBox ID="txtWebSiteUrl" Runat="server"></asp:TextBox><br />
                    <asp:Label ID="Label18" runat="server" Text="(e.g. http://www.somedomain.com)"></asp:Label>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator5" Runat="server" ErrorMessage="Please enter valid web site URL"
                        ValidationExpression="http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&amp;=]*)?" ControlToValidate="txtWebSiteUrl" Display="Dynamic">
                        *</asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td valign="top" align="right" width="40%">
                    </td>
                <td align="left" width="60%">
                    &nbsp;</td>
            </tr>
            <tr>
                <td valign="top" align="center" colspan="2" width="40%">
                    <asp:Button ID="btnSave" Runat="server" Text="  Save  " OnClick="btnSave_Click" />
                    <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Cancel" /></td>
            </tr>
            <tr>
                <td align="center" colspan="2" valign="top" width="40%">
                    <asp:Label ID="lblMsg" runat="server" SkinID="FormLabel"></asp:Label></td>
            </tr>
            <tr>
                <td valign="top" align="center" width="40%" colspan="2">
                    <asp:ValidationSummary ID="ValidationSummary1" Runat="server" />
                </td>
            </tr></table>

</asp:Content>

