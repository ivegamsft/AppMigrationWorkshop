<%@ Control Language="C#" CodeFile="MySearches.ascx.cs" Inherits="MySearches_ascx" %>
<asp:GridView ID="GridView1" Runat="server" DataSourceID="ObjectDataSource1" AllowPaging="True"
    AutoGenerateColumns="False" Width="100%" 
    DataKeyNames="MySearchID">
    <Columns>
        <asp:HyperLinkField Text="View" DataNavigateUrlFields="mysearchid" DataNavigateUrlFormatString="~/jobseeker/jobsearch.aspx?mysearchid={0}"></asp:HyperLinkField>
        <asp:BoundField HeaderText="Criteria" DataField="SearchCriteria"></asp:BoundField>
        <asp:BoundField HeaderText="Country" DataField="countryname"></asp:BoundField>
        <asp:BoundField HeaderText="State" DataField="statename"></asp:BoundField>
        <asp:BoundField HeaderText="City" DataField="city"></asp:BoundField>
        <asp:CommandField ShowDeleteButton="True"></asp:CommandField>
    </Columns>
</asp:GridView><br />
<asp:ObjectDataSource ID="ObjectDataSource1" 
    Runat="server" 
    TypeName="JobSiteStarterKit.BOL.MySearch"
    DeleteMethod="Delete" 
    SelectMethod="GetMySearches" DataObjectTypeName="JobSiteStarterKit.BOL.MySearch">
    <SelectParameters>
        <asp:ProfileParameter Name="username" Type="String" PropertyName="UserName"></asp:ProfileParameter>
    </SelectParameters>
</asp:ObjectDataSource>
