<%@ Page Language="C#" CodeFile="jobpostings.aspx.cs" Inherits="postinglist_aspx" MasterPageFile="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="server">
    <div align="center">
        <asp:Label ID="Label14" Runat="server" Text="List of Job Postings" SkinID="FormHeading"></asp:Label>
        <br />
    <br />
        <asp:Button ID="Button1" Runat="server" Text="Add New Job Posting" OnClick="Button1_Click" /></div>

        <br />
        <asp:GridView ID="GridView1" Runat="server" DataSourceID="ObjectDataSource1" AllowPaging="True"
            AllowSorting="True" AutoGenerateColumns="False" OnRowCommand="GridView1_RowCommand" OnRowDataBound="GridView1_RowDataBound" DataKeyNames="PostingID" Width="100%">
            <Columns>
                <asp:BoundField HeaderText="Title" DataField="Title"></asp:BoundField>
                <asp:BoundField HeaderText="Job Code" DataField="JobCode"></asp:BoundField>
                <asp:BoundField HeaderText="Location" DataField="City"></asp:BoundField>
                <asp:BoundField HeaderText="Posted On" DataField="PostingDate" DataFormatString="{0:MM/dd/yyyy}"></asp:BoundField>
                <asp:ButtonField Text="View Details">
                    <ItemStyle Wrap="False"></ItemStyle>
                </asp:ButtonField>
            </Columns>
        </asp:GridView><br />
        &nbsp;<asp:ObjectDataSource ID="ObjectDataSource1" Runat="server" TypeName="JobSiteStarterKit.BOL.JobPosting"
            DeleteMethod="Delete" InsertMethod="Insert" SelectMethod="GetPostings" UpdateMethod="Update"
            DataObjectTypeName="JobSiteStarterKit.BOL.JobPosting">
            <DeleteParameters>
                <asp:Parameter Type="Int32" Name="id"></asp:Parameter>
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Type="Int32" Name="JobPostingID"></asp:Parameter>
                <asp:Parameter Type="Int32" Name="CompanyID"></asp:Parameter>
                <asp:Parameter Type="String" Name="ContactPerson"></asp:Parameter>
                <asp:Parameter Type="String" Name="Title"></asp:Parameter>
                <asp:Parameter Type="String" Name="Department"></asp:Parameter>
                <asp:Parameter Type="String" Name="JobCode"></asp:Parameter>
                <asp:Parameter Type="String" Name="City"></asp:Parameter>
                <asp:Parameter Type="Int32" Name="StateID"></asp:Parameter>
                <asp:Parameter Type="Int32" Name="CountryID"></asp:Parameter>
                <asp:Parameter Type="Int32" Name="EducationLevelID"></asp:Parameter>
                <asp:Parameter Type="Int32" Name="JobTypeID"></asp:Parameter>
                <asp:Parameter Type="Decimal" Name="MinSalary"></asp:Parameter>
                <asp:Parameter Type="Decimal" Name="MaxSalary"></asp:Parameter>
                <asp:Parameter Type="String" Name="Description"></asp:Parameter>
                <asp:Parameter Type="DateTime" Name="PostingDate"></asp:Parameter>
                <asp:Parameter Type="String" Name="PostedBy"></asp:Parameter>
            </UpdateParameters>
            <SelectParameters>
                <asp:ProfileParameter Name="username" Type="String" PropertyName="UserName"></asp:ProfileParameter>
            </SelectParameters>
            <InsertParameters>
                <asp:Parameter Type="Int32" Name="JobPostingID"></asp:Parameter>
                <asp:Parameter Type="Int32" Name="CompanyID"></asp:Parameter>
                <asp:Parameter Type="String" Name="ContactPerson"></asp:Parameter>
                <asp:Parameter Type="String" Name="Title"></asp:Parameter>
                <asp:Parameter Type="String" Name="Department"></asp:Parameter>
                <asp:Parameter Type="String" Name="JobCode"></asp:Parameter>
                <asp:Parameter Type="String" Name="City"></asp:Parameter>
                <asp:Parameter Type="Int32" Name="StateID"></asp:Parameter>
                <asp:Parameter Type="Int32" Name="CountryID"></asp:Parameter>
                <asp:Parameter Type="Int32" Name="EducationLevelID"></asp:Parameter>
                <asp:Parameter Type="Int32" Name="JobTypeID"></asp:Parameter>
                <asp:Parameter Type="Decimal" Name="MinSalary"></asp:Parameter>
                <asp:Parameter Type="Decimal" Name="MaxSalary"></asp:Parameter>
                <asp:Parameter Type="String" Name="Description"></asp:Parameter>
                <asp:Parameter Type="DateTime" Name="PostingDate"></asp:Parameter>
                <asp:Parameter Type="String" Name="PostedBy"></asp:Parameter>
            </InsertParameters>
        </asp:ObjectDataSource></asp:Content>

