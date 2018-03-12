<%@ Page Language="C#" MasterPageFile="~/TimeTracker/MasterPage.master" CodeFile="Project_List.aspx.cs"
    Inherits="Project_List_aspx" Title="My Company - Time Tracker - List of Projects"
    Culture="auto" UICulture="auto" %>
<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <div id="projectadministration">
        <a name="content_start" id="content_start"></a>
        <fieldset>
            <!-- add H2 here and hide it with css since you can not put h2 inside a legend tag -->
            <h2 class="none">
                Project List</h2>
            <legend>Project List</legend>
            <asp:ObjectDataSource ID="ProjectData" runat="server" TypeName="ASPNET.StarterKit.BusinessLogicLayer.Project"
                DeleteMethod="DeleteProject" OldValuesParameterFormatString="{0}">
                <DeleteParameters>
                    <asp:Parameter Name="Id" Type="Int32" />
                </DeleteParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="ListAllProjects" DataSourceID="ProjectData"  AutoGenerateColumns="False" DataKeyNames="Id"
                AllowSorting="true" BorderWidth="0" runat="server" BorderStyle="None" Width="90%"
                CellPadding="2" PageSize="25" BorderColor="White" OnRowDeleting="ListAllProjects_RowDeleting">
                <Columns>
                    <asp:BoundField DataField="Id" Visible=true />
                    <asp:BoundField DataField="Name" HeaderText="Project Name" SortExpression="Name" />
                    <asp:BoundField DataField="ManagerUserName" HeaderText="Project Manager" SortExpression="ManagerUserName" />
                    <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                    <asp:BoundField DataField="CompletionDate" HeaderText="Completion" DataFormatString="{0:d}"
                        SortExpression="CompletionDate" />
                    <asp:BoundField DataField="EstimateDuration" HeaderText="EstimateDuration" SortExpression="EstimateDuration" />
                    <asp:HyperLinkField HeaderText="Edit Project" DataNavigateUrlFields="Id" DataNavigateUrlFormatString="Project_Details.aspx?ProjectId={0}"
                        Text="Edit..." />
                    <asp:CommandField ShowDeleteButton="True" HeaderText="Delete" DeleteImageUrl="images/icon-delete.gif"
                        ButtonType="Image" />
                </Columns>
                <RowStyle HorizontalAlign="Left" CssClass="row1" />
                <HeaderStyle CssClass="grid-header" HorizontalAlign="Left" />
                <EmptyDataTemplate>
                    <asp:Label ID="Label10" runat="server" Text="Label">There are not projects assigned to you</asp:Label>
                </EmptyDataTemplate>
            </asp:GridView>
            <br />
            <asp:Button ID="CreateProject" runat="server" Text="Create new project" OnClick="Button_Click" />
        </fieldset>
    </div>
</asp:Content>
