<%@ Page Language="C#" MasterPageFile="~/TimeTracker/MasterPage.master" CodeFile="TimeEntry.aspx.cs"
    Inherits="TimeEntry_aspx" Title="My Company - Time Tracker - Log a Time Entry"
    Culture="auto" UICulture="auto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <div id="TimeEntryView" runat="server" visible="false">
        <div id="body">
            <div id="addhours">
                <a name="content_start" id="content_start"></a>
                <fieldset>
                    <h2 class="none">
                        Log your hours</h2>
                    <legend>Log your hours</legend>Week Ending:<br />
                    <br />
                    Project<br />
                    <asp:ObjectDataSource ID="ProjectData" runat="server" TypeName="ASPNET.StarterKit.BusinessLogicLayer.Project" />
                    <asp:DropDownList ID="ProjectList" runat="server" AutoPostBack="True" DataSourceID="ProjectData"
                        DataTextField="Name" DataValueField="Id" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ProjectList"
                        ErrorMessage="Project is a required value." Display="Dynamic" ValidationGroup="newEntry"></asp:RequiredFieldValidator>
                    <br />
                    Category<br />
                    <asp:ObjectDataSource ID="ProjectCategories" runat="server" TypeName="ASPNET.StarterKit.BusinessLogicLayer.Category"
                        SelectMethod="GetCategoriesByProjectId">
                        <SelectParameters>
                            <asp:ControlParameter Name="projectId" ControlID="ProjectList" PropertyName="SelectedValue"
                                DefaultValue="0" Type="Int32" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <asp:DropDownList ID="CategoryList" runat="server" DataSourceID="ProjectCategories"
                        DataTextField="Name" DataValueField="Id" />
                    <br />
                    Day<br />
                    <asp:TextBox ID="WeekEnding2" runat="server" Columns="12"  ValidationGroup="newEntry"></asp:TextBox>
                    <a href="javascript:OpenPopupPage('Calendar.aspx','<%= WeekEnding2.ClientID %>','<%= Page.IsPostBack %>');">
                        <img src="images/icon-calendar.gif" border="0" align="absBottom" width="24" height="16"></a>
                        <asp:CompareValidator id="CompletionDateCompareValidator" runat="server" Display="Dynamic" ErrorMessage="Date format is incorrect." ControlToValidate="WeekEnding2" Operator="DataTypeCheck" Type="Date">
                </asp:CompareValidator>
                    <br />
                    Hours<br />
                    <asp:TextBox ID="Hours" runat="server" Columns="5" CssClass="hours"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="Requiredfieldvalidator1" runat="server" ErrorMessage="Hours is a required value."
                        ControlToValidate="Hours" Display="Dynamic" ValidationGroup="newEntry"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Hours must be a decimal value."
                        ControlToValidate="Hours" Type="Currency" Operator="DataTypeCheck" Display="Dynamic"
                        ValidationGroup="newEntry"></asp:CompareValidator>
                    <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="Hours is out of range."
                        ControlToValidate="Hours" MaximumValue="24" MinimumValue="0" Type="Double" ValidationGroup="newEntry"></asp:RangeValidator>
                    <br />
                    Description<br>
                    <asp:TextBox ID="Description" runat="server" TextMode="MultiLine" Rows="9" Columns="20"
                        MaxLength="200"></asp:TextBox><asp:CustomValidator ID="CustomValidator1" runat="server"
                            ControlToValidate="Description" ErrorMessage="Description must be less than 200 characters"
                            OnServerValidate="CustomValidator1_ServerValidate"></asp:CustomValidator>
                    <br />
                    <asp:Button ID="AddEntry" runat="server" CssClass="submit" CausesValidation="False"
                        Text="Add Entry" OnClick="AddEntry_Click" ValidationGroup="newEntry" />
                    <asp:Button ID="Cancel" runat="server" CssClass="reset" CausesValidation="False"
                        Text="Cancel" OnClick="Cancel_Click" />
                </fieldset>
            </div>
            <div id="timesheet">
                <fieldset>
                    <!-- add H2 here and hide it with css since you can not put h2 inside a legend tag -->
                    <h2 class="none">
                        Time Sheet for:</h2>
                    <legend>Time Sheet For:
                        <asp:ObjectDataSource ID="ProjectMembers" runat="server" TypeName="ASPNET.StarterKit.BusinessLogicLayer.Project"
                            SelectMethod="GetProjectMembers">
                            <SelectParameters>
                                <asp:ControlParameter Name="Id" ControlID="ProjectList" PropertyName="SelectedValue"
                                    DefaultValue="0" Type="Int32" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:DropDownList ID="UserList" runat="server" AutoPostBack="True" CssClass="username" />
                    </legend>
                    <asp:ObjectDataSource ID="ProjectListDataSource" runat="server" TypeName="ASPNET.StarterKit.BusinessLogicLayer.TimeEntry"
                        SelectMethod="GetTimeEntries" DeleteMethod="DeleteTimeEntry" UpdateMethod="UpdateTimeEntry" OldValuesParameterFormatString="{0}">
                        <DeleteParameters>
                            <asp:Parameter Name="Id" Type="Int32" />
                        </DeleteParameters>
                        <SelectParameters>
                            <asp:ControlParameter Name="projectId" ControlID="ProjectList" PropertyName="SelectedValue"
                                DefaultValue="0" Type="Int32" />
                            <asp:ControlParameter Name="userName" ControlID="UserList" PropertyName="SelectedValue"
                                Type="String" />
                        </SelectParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="Id" Type="Int32" />
                            <asp:Parameter Name="CategoryName" Type="String" />
                            <asp:Parameter Name="Duration" Type="Decimal" />
                            <asp:Parameter Name="ReportedDate" Type="DateTime" />
                            <asp:ControlParameter Name="UserName" ControlID="UserList" PropertyName="SelectedValue"
                                Type="String" />
                        </UpdateParameters>
                    </asp:ObjectDataSource>
                    <asp:GridView ID="ProjectListGridView" runat="server" DataSourceID="ProjectListDataSource"
                        DataKeyNames="Id" AutoGenerateColumns="False" AllowSorting="true" BorderWidth="0"
                        BorderStyle="None" Width="100%" CellPadding="2" PageSize="25" OnRowDeleting="ProjectListGridView_RowDeleting" OnRowUpdated="ProjectListGridView_RowUpdated">
                        <Columns>
                        <asp:BoundField DataField="Id" HeaderText="Id" ReadOnly=true />
                            <asp:BoundField DataField="CategoryName" HeaderText="CategoryName"  />
                            <asp:BoundField DataField="Description" HeaderText="Description" />
                            <asp:BoundField DataField="Duration" HeaderText="Duration" />
                            <asp:BoundField DataField="ReportedDate" HeaderText="ReportedDate" DataFormatString="{0:d}" />
                            <asp:CommandField ShowEditButton="True" HeaderText="Edit" ButtonType="Image" EditImageUrl="images/icon-edit.gif"
                                UpdateImageUrl="images/icon-save.gif" CancelImageUrl="images/icon-cancel.gif" />
                            <asp:CommandField ShowDeleteButton="True" HeaderText="Delete" DeleteImageUrl="images/icon-delete.gif"
                                ButtonType="Image" />
                        </Columns>
                        <EmptyDataTemplate>
                            <asp:Label ID="Label10" runat="server" Text="Label">There are no time entries for this user</asp:Label>
                        </EmptyDataTemplate>
                        <RowStyle CssClass="row1" BorderStyle="None" />
                        <HeaderStyle CssClass="grid-header" HorizontalAlign="Left" />
                    </asp:GridView>
                </fieldset>
            </div>
        </div>
    </div>
    <div id="MessageView" runat="server" visible="false">
        <div id="projectadministration">
            <fieldset>
                <h2 class="none">
                    Time Sheet for:</h2>
                <legend>Time Sheet For:</legend>
                <center>
                    You do not have any projects assigned to you.
                </center>
            </fieldset>
        </div>
    </div>
</asp:Content>
