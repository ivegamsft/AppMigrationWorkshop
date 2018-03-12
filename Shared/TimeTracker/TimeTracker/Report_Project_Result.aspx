<%@ Page Language="C#" MasterPageFile="~/TimeTracker/MasterPage.master" CodeFile="Report_Project_Result.aspx.cs"
    Inherits="Report_Project_Result_aspx" Title="My Company - Time Tracker - Project Report" Culture="auto" UICulture="auto" %>
<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <div id="projectreport">
        <a name="content_start" id="content_start"></a>
        <fieldset>
            <h2 class="none">
                Project Report</h2>
            <legend>Project Report</legend>
            <asp:Label ID="NoData" runat="server" CssClass="header-gray" Visible="False">
      No Data Retrieved.
            </asp:Label>
            <asp:ObjectDataSource ID="ProjectReportData" runat="server" TypeName="ASPNET.StarterKit.BusinessLogicLayer.Project"
                SelectMethod="GetProjectByIds">
                <SelectParameters>
                    <asp:SessionParameter Name="ProjectIds" SessionField="SelectedProjectIds" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:DataList ID="ProjectList" RepeatColumns="1" RepeatDirection="Vertical" runat="server"
                DataSourceID="projectReportData" OnItemCreated="OnProjectListItemCreated">
                <HeaderStyle CssClass="header-gray" />
                <HeaderTemplate>
                    Project Report
                </HeaderTemplate>
                <ItemTemplate>
                    <table border="0" cellpadding="0" cellspacing="0" class="Content" width="100%">
                        <tr>
                            <td valign="top">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0" class="Content" width="100%">
                                    <tr>
                                        <td width="180" class="report-main-header">
                                            Project Name</td>
                                        <td width="70" align="right" class="report-main-header">
                                            Est. Hours</td>
                                        <td width="100" align="right" class="report-main-header">
                                            Actual Hours</td>
                                        <td width="100" align="right" class="report-main-header">
                                            Est. Completion</td>
                                    </tr>
                                    <tr>
                                        <td class="report-text">
                                            <%# Eval("Name") %>
                                        </td>
                                        <td class="report-text" align="right">
                                            <%# Eval("EstimateDuration") %>
                                        </td>
                                        <td class="report-text" align="right">
                                            <%# Eval("ActualDuration") %>
                                        </td>
                                        <td class="report-text" align="right">
                                            <%# Eval("CompletionDate", "{0:d}") %>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:ObjectDataSource ID="CategorReportData" runat="server" TypeName="ASPNET.StarterKit.BusinessLogicLayer.Category"
                                    SelectMethod="GetCategoriesByProjectId">
                                    <SelectParameters>
                                        <asp:Parameter Name="ProjectId" Type="Int32" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                                <asp:DataList ID="CategoryList" RepeatColumns="1" RepeatDirection="Vertical" DataSourceID="CategorReportData"
                                    runat="server" OnItemCreated="OnCategoryListItemCreated">
                                    <ItemTemplate>
                                        <table border="0" cellpadding="0" cellspacing="0" class="Content" width="100%">
                                            <tr>
                                                <td height="12" width="105">
                                                    <img height="1" src="images/spacer.gif" width="1"></td>
                                                <td valign="top" width="72" class="report-header">
                                                    Category</td>
                                                <td valign="top" width="70" align="right" class="report-header">
                                                    Est. Hours</td>
                                                <td valign="top" width="100" align="right" class="report-header">
                                                    Actual Hours</td>
                                            </tr>
                                            <tr>
                                                <td height="12" width="105">
                                                    <img height="1" src="images/spacer.gif" width="1"></td>
                                                <td valign="top" class="report-text">
                                                    <%# Eval("Abbreviation") %>
                                                </td>
                                                <td valign="top" class="report-text" align="right">
                                                    <%# Eval("EstimateDuration") %>
                                                </td>
                                                <td valign="top" class="report-text" align="right">
                                                    <%# Eval("ActualDuration") %>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" colspan="4">
                                                    <img height="15" src="images/spacer.gif" width="1"></td>
                                            </tr>
                                            <tr>
                                                <td valign="top" colspan="2">
                                                    &nbsp;</td>
                                                <td colspan="2">
                                                    <asp:ObjectDataSource ID="UserReportData" runat="server" TypeName="ASPNET.StarterKit.BusinessLogicLayer.UserReport"
                                                        SelectMethod="GetUserReportsByCategoryId">
                                                        <SelectParameters>
                                                            <asp:QueryStringParameter Name="CategoryId" Type="Int32" />
                                                        </SelectParameters>
                                                    </asp:ObjectDataSource>
                                                    <asp:DataList ID="EntryList" Width="100%" RepeatColumns="1" RepeatDirection="Vertical"
                                                        DataSourceID="UserReportData" runat="server" OnDataBinding="EntryListDataBinding">
                                                        <HeaderTemplate>
                                                            <table border="0" cellpadding="0" cellspacing="0" class="Content" width="100%">
                                                                <tr>
                                                                    <td valign="top" align="left" class="report-header">
                                                                        Consultant</td>
                                                                    <td valign="top" align="right" class="report-header">
                                                                        Hours</td>
                                                                </tr>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td valign="top" class="report-text">
                                                                    <!-- Add Hyperlink here00 -->
                                                                    <%# Eval("UserName") %>
                                                                </td>
                                                                <td valign="top" class="report-text" align="right">
                                                                    <%# Eval("ActualDuration") %>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            </table>
                                                        </FooterTemplate>
                                                    </asp:DataList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="25" valign="top" colspan="4">
                                                    <img height="1" src="images/spacer.gif" width="1"></td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:DataList>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:DataList>
        </fieldset>
    </div>
</asp:Content>
