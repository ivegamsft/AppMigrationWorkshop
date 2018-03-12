<%@ Control Language="C#" CodeFile="MyResumes.ascx.cs" Inherits="MyResumes_ascx" %>
<asp:GridView ID="GridView1" Runat="server" DataSourceID="ObjectDataSource1" AllowPaging="True"
    AutoGenerateColumns="False" Width="100%" DataKeyNames="MyResumeID">
    <Columns>
        <asp:HyperLinkField Text="View" DataNavigateUrlFields="resumeid" DataNavigateUrlFormatString="~/employer/viewresume.aspx?id={0}"></asp:HyperLinkField>
        <asp:BoundField HeaderText="Title" DataField="JobTitle"></asp:BoundField>
        <asp:BoundField HeaderText="Education" DataField="EducationLevelName"></asp:BoundField>
        <asp:BoundField HeaderText="Experience" DataField="ExperienceLevelName"></asp:BoundField>
        <asp:BoundField HeaderText="Location" DataField="TargetCity"></asp:BoundField>
        <asp:CommandField ShowDeleteButton="True"></asp:CommandField>
    </Columns>
</asp:GridView><br />
<asp:ObjectDataSource ID="ObjectDataSource1" Runat="server" TypeName="JobSiteStarterKit.BOL.MyResume"
    DeleteMethod="Delete" SelectMethod="GetMyResumes" DataObjectTypeName="JobSiteStarterKit.BOL.MyResume">
    <SelectParameters>
        <asp:ProfileParameter Name="username" Type="String" PropertyName="UserName"></asp:ProfileParameter>
    </SelectParameters>
</asp:ObjectDataSource>
