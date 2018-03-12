<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeFile="resumesearch.aspx.cs" Inherits="resumesearch_aspx" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="server">

<div align="center">
        <asp:Label ID="Label14" Runat="server" Text="Search Resume Database" SkinID="FormHeading"></asp:Label>
    <br />
    <asp:Label ID="lblResumeCount" Runat="server" SkinID="Slogan"></asp:Label><br />
</div>
    <table style="width: 100%"><tr>
            <td style="width: 100px" valign="top" align="right">
                <asp:Label ID="Label1" Runat="server" Text="Skills :"></asp:Label></td>
            <td style="width: 100px" align="left">
                <asp:TextBox ID="txtSkills" Runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Runat="server" ErrorMessage="Please enter skills to search for"
                    ControlToValidate="txtSkills" Display="Dynamic">
                    *</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td style="width: 100px" valign="top" align="right">
                <asp:Label ID="Label3" Runat="server" Text="Country :"></asp:Label></td>
            <td style="width: 100px" align="left">
                <asp:DropDownList ID="ddlCountry" Runat="server" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged"
                    AutoPostBack="True">
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td style="width: 100px" valign="top" align="right">
                <asp:Label ID="Label4" Runat="server" Text="State :"></asp:Label></td>
            <td style="width: 100px" align="left">
                <asp:DropDownList ID="ddlState" Runat="server">
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td style="width: 100px" valign="top" align="right">
                <asp:Label ID="Label2" Runat="server" Text="City :"></asp:Label></td>
            <td style="width: 100px" align="left">
                <asp:TextBox ID="txtCity" Runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td valign="top" align="center" colspan="2">
                <asp:Button ID="btnSearch" Runat="server" Text="Search" OnClick="btnSearch_Click" />
                <asp:Button ID="btnCancel" Runat="server" Text="Cancel" CausesValidation="False" /></td>
        </tr>
        <tr>
            <td align="center" colspan="2" valign="top">
                <asp:Label ID="lblMsg" runat="server" SkinID="FormLabel"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top" align="left" colspan="2">
                <asp:ValidationSummary ID="ValidationSummary1" Runat="server" />
            </td>
        </tr></table>
    <br />
    <asp:GridView ID="GridView1" Runat="server" Width="100%" AutoGenerateColumns="False"
        OnRowDataBound="GridView1_RowDataBound" DataKeyNames="ResumeID" OnRowCommand="GridView1_RowCommand" AllowPaging="True" OnPageIndexChanging="GridView1_PageIndexChanging">
        <Columns>
            <asp:BoundField HeaderText="Title" DataField="JobTitle"></asp:BoundField>
            <asp:BoundField ShowHeader="False" HeaderText="Education" DataField="educationlevelid"></asp:BoundField>
            <asp:BoundField HeaderText="Experience" DataField="experiencelevelid"></asp:BoundField>
            <asp:BoundField HeaderText="Location" DataField="TargetCity"></asp:BoundField>
            <asp:ButtonField Text="View Details" CommandName="viewdetails"></asp:ButtonField>
        </Columns>
    </asp:GridView>
    <br />
</asp:Content>
