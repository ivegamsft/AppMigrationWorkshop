<%@ Page Language="C#" CodeFile="postresume.aspx.cs" Inherits="postresume_aspx" MasterPageFile="~/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="server">

<div align=center>
<asp:Label ID="Label14" Runat="server" Text="Post Your Resume" SkinID="FormHeading"></asp:Label>
</div>

<br />

<table width="100%"><tr>
        <td align="left" colspan="2">
            <asp:Label ID="Label15" Runat="server" SkinID="FormGroupLabel" Text="Job Details"></asp:Label>
        </td>
    </tr>
    <tr>
        <td align="right" width="10%" nowrap>
            <asp:Label ID="Label1" Runat="server" Text="Job Title :"></asp:Label></td>
        <td align="left">
            <asp:TextBox ID="txtJobTitle" Runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Runat="server" ErrorMessage="Please enter job title"
                ControlToValidate="txtJobTitle" Display="Dynamic">
                *</asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td align="right" width="10%" nowrap>
            <asp:Label ID="Label6" Runat="server" Text="Desired Job Type :"></asp:Label>
        </td>
        <td align="left">
            <asp:DropDownList ID="ddlJobType" Runat="server">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td align="left" colspan="2">
            <asp:Label ID="Label16" Runat="server" SkinID="FormGroupLabel" Text="Location"></asp:Label>
        </td>
    </tr>
    <tr>
        <td align="right" width="10%" nowrap>
            <asp:Label ID="Label2" Runat="server" Text="Target City :"></asp:Label>
        </td>
        <td align="left">
            <asp:TextBox ID="txtCity" Runat="server"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align="right" width="10%" nowrap>
            <asp:Label ID="Label3" Runat="server" Text="Target Country :"></asp:Label>
        </td>
        <td align="left">
            <asp:DropDownList ID="ddlCountry" Runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td align="right" width="10%" nowrap>
            <asp:Label ID="Label4" Runat="server" Text="Target State :"></asp:Label>
        </td>
        <td  align="left">
            <asp:DropDownList ID="ddlState" Runat="server">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td align="right" width="10%" nowrap>
            <asp:Label ID="Label5" Runat="server" Text="Acceptable Relocation :"></asp:Label>
        </td>
        <td align="left">
            <asp:DropDownList ID="ddlRelocationCountry" Runat="server">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td align="left" colspan="2">
            <asp:Label ID="Label17" Runat="server" SkinID="FormGroupLabel" Text="Education and Experience"></asp:Label>&nbsp;&nbsp;</td>
    </tr>
    <tr>
        <td align="right" width="10%" nowrap>
            <asp:Label ID="Label7" Runat="server" Text="Education Level :"></asp:Label>
        </td>
        <td align="left">
            <asp:DropDownList ID="ddlEduLevel" Runat="server">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td align="right" width="10%" nowrap>
            <asp:Label ID="Label8" Runat="server" Text="Experience Level :"></asp:Label>
        </td>
        <td align="left">
            <asp:DropDownList ID="ddlExpLevel" Runat="server">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td align="left" colspan="2">
            <asp:Label ID="Label9" Runat="server" Text="Resume :" SkinID="FormGroupLabel"></asp:Label>
        </td>
    </tr>
    <tr>
        <td align="right" valign="top" colspan="2">
            &nbsp;<asp:TextBox ID="txtResume" Runat="server" Width="98%" TextMode="MultiLine" Rows="15"></asp:TextBox><br />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Runat="server" ErrorMessage="Please enter resume"
                ControlToValidate="txtResume" Display="Dynamic">
                *</asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td valign="top" align="left" colspan="2">
            <asp:Label ID="Label10" Runat="server" Text="Covering Letter Template :" SkinID="FormGroupLabel"></asp:Label>
        </td>
    </tr>
    <tr>
        <td align="right" valign="top" colspan="2">
            &nbsp;<asp:TextBox ID="txtCoveringLetter" Runat="server" Width="98%" TextMode="MultiLine" Rows="15"></asp:TextBox></td>
    </tr>
    <tr>
        <td align="center" colspan="2" width="10%">
            &nbsp;<asp:Button ID="btnSave" Runat="server" Text=" Save " OnClick="btnSave_Click" />
            <asp:Button ID="btnCancel" Runat="server" Text="Cancel" OnClick="btnCancel_Click"
                CausesValidation="False" />
            </td>
    </tr>
    <tr>
        <td align="center" colspan="2" width="10%">
            <asp:Label ID="lblMsg" runat="server" SkinID="FormLabel"></asp:Label></td>
    </tr>
    <tr>
        <td align="center" width="10%" colspan="2">
            <asp:ValidationSummary ID="ValidationSummary1" Runat="server" />
        </td>
    </tr></table>
    &nbsp;<br />
    &nbsp;</asp:Content>

