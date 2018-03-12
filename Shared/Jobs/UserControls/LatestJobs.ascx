<%@ Control Language="C#" CodeFile="LatestJobs.ascx.cs" Inherits="LatestJobs_ascx" %>
<asp:GridView ID="GridView1" Runat="server" DataSourceID="ObjectDataSource1" AllowPaging="True"
    AutoGenerateColumns="False" Width="100%" >
    <Columns>
        <asp:HyperLinkField HeaderText="Latest Jobs!!" NavigateUrl="~/jobseeker/viewjobposting.aspx?id="
            DataNavigateUrlFields="postingid" DataNavigateUrlFormatString="~/jobseeker/viewjobposting.aspx?id={0}"
            DataTextField="Title"></asp:HyperLinkField>
    </Columns>
</asp:GridView>
<asp:ObjectDataSource ID="ObjectDataSource1" Runat="server" TypeName="JobSiteStarterKit.BOL.JobPosting"
    SelectMethod="GetLatest">
</asp:ObjectDataSource>
