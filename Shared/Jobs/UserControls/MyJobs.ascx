<%@ Control Language="C#" CodeFile="MyJobs.ascx.cs" Inherits="MyJobs_ascx" %>
<asp:GridView ID="GridView1" Runat="server" DataSourceID="ObjectDataSource1" AllowPaging="True"
    AutoGenerateColumns="False" Width="100%" DataKeyNames="MyJobID">
    <Columns>
        <asp:HyperLinkField Text="View" DataNavigateUrlFields="postingid" DataNavigateUrlFormatString="~/jobseeker/viewjobposting.aspx?id={0}"></asp:HyperLinkField>
        <asp:BoundField HeaderText="Date" DataField="PostingDate" DataFormatString="{0:MM/dd/yyyy}"></asp:BoundField>
        <asp:BoundField HeaderText="Title" DataField="Title"></asp:BoundField>
        <asp:BoundField HeaderText="Location" DataField="City"></asp:BoundField>
        <asp:BoundField HeaderText="Company" DataField="CompanyName"></asp:BoundField>
        <asp:CommandField ShowDeleteButton="True"></asp:CommandField>
    </Columns>
</asp:GridView><br />
<asp:ObjectDataSource ID="ObjectDataSource1" Runat="server" TypeName="JobSiteStarterKit.BOL.MyJob"
    DeleteMethod="Delete" SelectMethod="GetMyJobs" DataObjectTypeName="JobSiteStarterKit.BOL.MyJob">
    <SelectParameters>
        <asp:ProfileParameter Name="username" Type="String" PropertyName="UserName"></asp:ProfileParameter>
    </SelectParameters>
</asp:ObjectDataSource>
