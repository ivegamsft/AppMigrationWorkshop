<%@ Page Language="C#" MasterPageFile="~/TimeTracker/MasterPage.master" CodeFile="Report_Resources_Result.aspx.cs"
    Inherits="Report_Resources_Result_aspx" Title="My Company - Time Tracker - Resource Reports" Culture="auto" UICulture="auto"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <div id="projectreport">
        <a name="content_start" id="content_start"></a>
        <fieldset>
            <h2 class="none">
                Resources Report</h2>
            <legend>Resources Report</legend>
            <asp:ObjectDataSource ID="UserReportData" runat="server" TypeName="ASPNET.StarterKit.BusinessLogicLayer.UserTotalDurationReport"
                SelectMethod="GetUserReportsByUserNames">
                <SelectParameters>
                    <asp:SessionParameter Name="userNames" SessionField="SelectedUserNames" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <table class="tan-border" cellspacing="11" cellpadding="0" width="100%" border="0"
                height="526">
                <tr valign="top">
                    <td>
                        <asp:Label ID="NoData" runat="server" Visible="False" CssClass="header-gray">
      No Data Retrieved.
                        </asp:Label>
                        <asp:DataList ID="UserList" runat="server" Width="100%" DataSourceID="UserReportData"
                            OnItemCreated="OnListUserTimeEntriesItemCreated">
                            <HeaderTemplate>
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td colspan="3" class="header-gray">
                                            Resource Report</td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td width="150" class="report-header">
                                            Beginning Date</td>
                                        <td width="150" class="report-header">
                                            Ending Date</td>
                                        <td width="*" class="report-header">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="report-text">
                                            <asp:Label ID="StartDate" runat="server" Text='<%# Session["SelectedStartingDate"] %>' />
                                        </td>
                                        <td class="report-text">
                                            <asp:Label ID="EndDate" runat="server" Text='<%# Session["SelectedEndDate"]  %>' />
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            &nbsp;</td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                    <tr>
                                        <td width="150" class="report-main-header">
                                            Consultant</td>
                                        <td width="78" align="right" class="report-main-header">
                                            Total Hours</td>
                                        <td width="*" class="report-main-header">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="report-text">
                                            <%# Eval("UserName") %>
                                        </td>
                                        <td class="report-text" align="right">
                                            <%# Eval("TotalDuration") %>
                                        </td>
                                        <td class="report-text">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td height="24" colspan="3">
                                            &nbsp;</td>
                                    </tr>
                                </table>
                                <table cellspacing="0" cellpadding="3" width="100%" border="0">
                                    <tr>
                                        <td>
                                            <asp:ObjectDataSource ID="TimeEntryData" runat="server" TypeName="ASPNET.StarterKit.BusinessLogicLayer.TimeEntry"
                                                SelectMethod="GetTimeEntriesByUserNameAndDates">
                                                <SelectParameters>
                                                    <asp:Parameter Name="userName" Type="string" />
                                                    <asp:SessionParameter Name="startingDate" SessionField="SelectedStartingDate" Type="DateTime" />
                                                    <asp:SessionParameter Name="endDate" SessionField="SelectedEndDate" Type="DateTime" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                            <asp:GridView ID="ListUserTimeEntries" DataSourceID="TimeEntryData" DataKeyNames="Id"
                                                AutoGenerateColumns="False" AllowSorting="true" BorderWidth="0" runat="server"
                                                BorderStyle="None" Width="90%" CellPadding="2" PageSize="25">
                                                <HeaderStyle CssClass="grid-header" HorizontalAlign="Left" />
                                                <RowStyle HorizontalAlign="Left" BorderWidth="5" BorderStyle="solid" BorderColor="Blue"
                                                    ForeColor="Red" />
                                                <Columns>
                                                    <asp:BoundField DataField="ReportedDate" HeaderText="Reported Date" SortExpression="ReportedDate"
                                                        DataFormatString="{0:d}" />
                                                    <asp:BoundField DataField="ProjectName" HeaderText="Project Name" SortExpression="ProjectName" />
                                                    <asp:BoundField DataField="CategoryName" HeaderText="Category Name" SortExpression="CategoryName" />
                                                    <asp:BoundField DataField="Duration" HeaderText="Duration" SortExpression="Duration" />
                                                    <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="48">
                                            &nbsp;</td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:DataList>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
</asp:Content>
