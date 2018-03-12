<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeFile="viewjobposting.aspx.cs" Inherits="viewjobposting_aspx" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="server">
    <div style="text-align: center">
        <div align="center">
            <asp:Label ID="Label14" Runat="server" Text="View Job Posting" SkinID="FormHeading"></asp:Label>
        </div>
        <br />
        <table style="width: 100%"><tr>
                <td align="left" width="20%" colspan="2">
                    <asp:Label ID="Label16" Runat="server" Text="Contact Details" SkinID="FormGroupLabel"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right" width="20%" nowrap>
                    <asp:Label ID="Label1" Runat="server" Text="Company :"></asp:Label>
                </td>
                <td align="left">
                    <asp:Label ID="lblCompany" Runat="server" Text="Label"></asp:Label><br />
                    <asp:LinkButton ID="btnViewProfile" runat="server" OnClick="btnViewProfile_Click">View Company Profile</asp:LinkButton></td>
            </tr>
            <tr>
                <td align="right" width="20%" nowrap>
                    <asp:Label ID="Label2" Runat="server" Text="Contact Person :"></asp:Label>
                </td>
                <td align="left">
                    <asp:Label ID="lblContactPerson" Runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" width="20%" colspan="2">
                    <asp:Label ID="Label17" Runat="server" Text="Job Details" SkinID="FormGroupLabel"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right" width="20%" nowrap>
                    <asp:Label ID="Label3" Runat="server" Text="Job Title :"></asp:Label>
                </td>
                <td align="left">
                    <asp:Label ID="lblTitle" Runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right" width="20%" nowrap>
                    <asp:Label ID="Label13" Runat="server" Text="Description :"></asp:Label>
                </td>
                <td align="left">
                    <asp:Label ID="lblDesc" Runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right" width="20%" nowrap>
                    <asp:Label ID="Label9" Runat="server" Text="Job Type :"></asp:Label>
                </td>
                <td align="left">
                    <asp:Label ID="lblJobType" Runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right" width="20%" nowrap>
                    <asp:Label ID="Label4" Runat="server" Text="Department :"></asp:Label>
                </td>
                <td align="left">
                    <asp:Label ID="lblDept" Runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right" width="20%" nowrap>
                    <asp:Label ID="Label5" Runat="server" Text="Job Code :"></asp:Label>
                </td>
                <td align="left">
                    <asp:Label ID="lblJobCode" Runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right" width="20%" nowrap>
                    <asp:Label ID="Label10" Runat="server" Text="Education Level :"></asp:Label>
                </td>
                <td align="left">
                    <asp:Label ID="lblEduLevel" Runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" width="20%" colspan="2">
                    <asp:Label ID="Label18" Runat="server" Text="Location" SkinID="FormGroupLabel"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right" width="20%" nowrap>
                    <asp:Label ID="Label6" Runat="server" Text="City :"></asp:Label>
                </td>
                <td align="left">
                    <asp:Label ID="lblCity" Runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right" width="20%" nowrap>
                    <asp:Label ID="Label7" Runat="server" Text="State :"></asp:Label>
                </td>
                <td align="left">
                    <asp:Label ID="lblState" Runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right" width="20%" nowrap>
                    <asp:Label ID="Label8" Runat="server" Text="Country :"></asp:Label>
                </td>
                <td align="left">
                    <asp:Label ID="lblCountry" Runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" width="20%" colspan="2">
                    <asp:Label ID="Label19" Runat="server" Text="Salary Details" SkinID="FormGroupLabel"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right" width="20%" nowrap>
                    <asp:Label ID="Label11" Runat="server" Text="Min. Salary :"></asp:Label>
                </td>
                <td align="left">
                    <asp:Label ID="lblMinSal" Runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right" width="20%" nowrap>
                    <asp:Label ID="Label12" Runat="server" Text="Max. Salary :"></asp:Label>
                </td>
                <td align="left">
                    <asp:Label ID="lblMaxSal" Runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right" width="20%">
                </td>
                <td align="left">
                </td>
            </tr>
            <tr>
                <td align="right" width="20%" nowrap>
                    <asp:Label ID="Label15" Runat="server" Text="Posting Date :"></asp:Label>
                </td>
                <td align="left">
                    <asp:Label ID="lblPostDt" Runat="server" Text="Label"></asp:Label>
                </td>
            </tr></table>
        <br />
        <div align=center>
        <asp:Button ID="Button1" Runat="server" Text=" Back " CssClass="dataentryformbutton" OnClick="Button1_Click" />
            <asp:Button ID="Button2" Runat="server" Text="Add to My Jobs" OnClick="Button2_Click" /><br />
        </div>
   
    </div>


</asp:Content>
