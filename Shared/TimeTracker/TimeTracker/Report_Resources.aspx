<%@ Page Language="C#" MasterPageFile="~/TimeTracker/MasterPage.master" CodeFile="Report_Resources.aspx.cs"
    Inherits="Report_Resources_aspx" Title="My Company - Time Tracker - Create Consultants Reports" Culture="auto" UICulture="auto"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <div id="projectreport">
        <a name="content_start" id="content_start"></a>
        <fieldset>
            <h2 class="none">
                Project and Resource Report</h2>
            <legend>Project and Resource Report</legend>
            <div class="formsection">
                STEP 1 - Select project(s)</div>
            <p>
                <asp:Label ID="Label2" runat="server" Text="Label" AssociatedControlID="ProjectList">Select a project. Use ctrl+click to select multiple resources at once:</asp:Label>
                <br />
                <asp:ObjectDataSource ID="ProjectData" runat="server" TypeName="ASPNET.StarterKit.BusinessLogicLayer.Project" />
                <asp:ListBox ID="ProjectList" runat="server" SelectionMode="Multiple" CssClass="projectlist"
                    Width="270px" Height="330px" Rows="28" DataSourceID="ProjectData" DataValueField="Id"
                    DataTextField="Name" />
                <br>
            </p>
            <div class="formsection">
                STEP 2 - Select resource(s)</div>
            <p>
                <asp:Label ID="Label1" runat="server" Text="Label" AssociatedControlID="UserList">select a resource. Use ctrl+click to select multiple resources at once:</asp:Label>
                
                <br />
                <asp:ObjectDataSource ID="UserData" runat="server" TypeName="ASPNET.StarterKit.BusinessLogicLayer.Project" />
                <asp:ListBox ID="UserList" runat="server" SelectionMode="Multiple" CssClass="resourcelist"
                    Width="226px" Height="302px" Rows="22" DataSourceID="UserData" />
                <br>
                <asp:RequiredFieldValidator ID="UserListRequiredFieldValidator" runat="server" Width="223px"
                    ErrorMessage="At least one consultant must be selected." ControlToValidate="UserList"
                    Display="Dynamic"></asp:RequiredFieldValidator>
            </p>
            <div class="formsection">
                STEP 3 - Select a date range</div>
            <p>
                Dates from:<br />
                <asp:TextBox ID="StartDate" runat="server" 
                    BorderStyle="None"></asp:TextBox>
                &nbsp; <a href="javascript:OpenPopupPage('Calendar.aspx','<%= StartDate.ClientID %>','<%= Page.IsPostBack %>');">
                    <img src="images/icon-calendar.gif"  ></a>
                <asp:CompareValidator ID="CompletionDateCompareValidator" runat="server" ControlToValidate="StartDate"
                    Display="Dynamic" ErrorMessage="Date format is incorrect." Operator="DataTypeCheck"
                    Type="Date"></asp:CompareValidator>
                <br />
                to:<br />
                <asp:TextBox ID="EndDate" runat="server" BorderStyle="None"></asp:TextBox>
                &nbsp; <a href="javascript:OpenPopupPage('Calendar.aspx','<%= EndDate.ClientID %>','<%= Page.IsPostBack %>');">
                    <img src="images/icon-calendar.gif"  ></a>
                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="EndDate"
                    Display="Dynamic" ErrorMessage="Date format is incorrect." Operator="DataTypeCheck"
                    Type="Date"></asp:CompareValidator>
                <br />
            </p>
            <div class="formsection">
                <asp:Button ID="GenResourceRpt" runat="server" CssClass="submit" Text="Generate Report"
                    CausesValidation="False" OnClick="GenResourceRpt_Click" />
            </div>
        </fieldset>
    </div>
</asp:Content>
