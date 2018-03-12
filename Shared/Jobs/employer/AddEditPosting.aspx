<%@ Page Language="C#" CodeFile="AddEditPosting.aspx.cs" Inherits="AddEditPosting_aspx" MasterPageFile="~/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="server">
    <div style="text-align: center">
        <div align="center">
            <asp:Label ID="Label14" Runat="server" Text="Add / Edit Job Posting" SkinID="FormHeading"></asp:Label>
        </div>
        <br />
        <asp:HyperLink ID="HyperLink2" Runat="server" NavigateUrl="~/employer/jobpostings.aspx">Go to listing page</asp:HyperLink><br />
        <asp:Label ID="Label6" runat="server" Text="(All the fields are mandatory.)"></asp:Label>
        <br />
        <asp:DetailsView ID="DetailsView1" Runat="server" DataSourceID="ObjectDataSource1"
            AutoGenerateRows="False" HorizontalAlign="Center"
            OnItemInserting="DetailsView1_ItemInserting" OnItemUpdating="DetailsView1_ItemUpdating" Width="100%" DataKeyNames="JobPostingID" OnItemDeleted="DetailsView1_ItemDeleted" OnDataBound="DetailsView1_DataBound" GridLines="Horizontal" CellPadding="5" >
            <RowStyle HorizontalAlign="Left"></RowStyle>
            <Fields>
                <asp:BoundField HeaderText="Job Posting ID :" DataField="JobPostingID" SortExpression="JobPostingID" ReadOnly="True" InsertVisible="False">
                    <ItemStyle CssClass="dataentryformlabel" Wrap="False"></ItemStyle>
                    <HeaderStyle Wrap="False" CssClass="dataentryformlabel"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField HeaderText="Title :" DataField="Title" SortExpression="Title">
                    <HeaderStyle CssClass="dataentryformlabel"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField HeaderText="Contact Person :" DataField="ContactPerson" SortExpression="ContactPerson">
                    <ItemStyle Wrap="False"></ItemStyle>
                    <HeaderStyle Wrap="False" CssClass="dataentryformlabel"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField HeaderText="Department :" DataField="Department" SortExpression="Department">
                    <HeaderStyle CssClass="dataentryformlabel"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField HeaderText="Job Code :" DataField="JobCode" SortExpression="JobCode">
                    <HeaderStyle CssClass="dataentryformlabel"></HeaderStyle>
                </asp:BoundField>
                <asp:TemplateField SortExpression="CountryID" HeaderText="Country :">
                    <EditItemTemplate>
                        &nbsp;<asp:DropDownList ID="ddlCountryUpdate" Runat="server" DataSourceID="ObjectDataSource3"
                            DataTextField="CountryName" DataValueField="CountryID" AutoPostBack="True"  OnSelectedIndexChanged="ddlCountryUpdate_SelectedIndexChanged" SelectedValue='<%# Bind("CountryID") %>'>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label Runat="server" Text='<%# JobSiteStarterKit.BOL.Country.GetCountryName((int)Eval("CountryID")) %>' ID="Label2"></asp:Label>
                    </ItemTemplate>
                    <InsertItemTemplate>
                        &nbsp;<asp:DropDownList ID="ddlCountryInsert" Runat="server" DataSourceID="ObjectDataSource3"
                            DataTextField="CountryName" DataValueField="CountryID" AutoPostBack="True" OnSelectedIndexChanged="ddlCountryInsert_SelectedIndexChanged" SelectedValue='<%# Bind("CountryID") %>'>
                        </asp:DropDownList>
                    </InsertItemTemplate>
                    <HeaderStyle CssClass="dataentryformlabel"></HeaderStyle>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="StateID" HeaderText="State :">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlStateUpdate" Runat="server" DataSourceID="ObjectDataSource2" 
                            DataTextField="StateName" DataValueField="StateID" >
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" Runat="server" Text='<%# JobSiteStarterKit.BOL.State.GetStateName((int)Eval("StateID")) %>'></asp:Label>
                    </ItemTemplate>
                    <InsertItemTemplate>
                        <asp:DropDownList ID="ddlStateInsert" Runat="server" DataSourceID="ObjectDataSource2" 
                            DataTextField="StateName" DataValueField="StateID">
                        </asp:DropDownList>
                    </InsertItemTemplate>
                    <HeaderStyle CssClass="dataentryformlabel"></HeaderStyle>
                </asp:TemplateField>
                <asp:BoundField HeaderText="City :" DataField="City" SortExpression="City">
                    <HeaderStyle CssClass="dataentryformlabel"></HeaderStyle>
                </asp:BoundField>
                <asp:TemplateField SortExpression="EducationLevelID" HeaderText="Education Level :"><EditItemTemplate>
                    &nbsp;<asp:DropDownList ID="ddlEduLevelUpdate" Runat="server" DataSourceID="ObjectDataSource4" SelectedValue='<%# Bind("EducationLevelID") %>'
                        DataTextField="EducationLevelName" DataValueField="EducationLevelID">
                    </asp:DropDownList>
                </EditItemTemplate>
                    <ItemStyle Wrap="False"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label Runat="server" Text='<%# JobSiteStarterKit.BOL.EducationLevel.GetEducationLevelName((int)Eval("EducationLevelID")) %>' ID="Label3"></asp:Label>
                    </ItemTemplate>
                    <InsertItemTemplate>
                        &nbsp;<asp:DropDownList ID="ddlEduLevelInsert" Runat="server" DataSourceID="ObjectDataSource4" SelectedValue='<%# Bind("EducationLevelID") %>'
                            DataTextField="EducationLevelName" DataValueField="EducationLevelID">
                        </asp:DropDownList>
                    </InsertItemTemplate>
                    <HeaderStyle Wrap="False" CssClass="dataentryformlabel"></HeaderStyle>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="JobTypeID" HeaderText="Job Type :"><EditItemTemplate>
                    &nbsp;<asp:DropDownList ID="ddlJobTypeUpdate" Runat="server" DataSourceID="ObjectDataSource5" SelectedValue='<%# Bind("JobTypeID") %>'
                        DataTextField="JobTypeName" DataValueField="JobTypeID">
                    </asp:DropDownList>
                </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label Runat="server" Text='<%# JobSiteStarterKit.BOL.JobType.GetJobTypeName((int)Eval("JobTypeID")) %>' ID="Label4"></asp:Label>
                    </ItemTemplate>
                    <InsertItemTemplate>
                        &nbsp;<asp:DropDownList ID="ddlJobTypeInsert" Runat="server" DataSourceID="ObjectDataSource5" SelectedValue='<%# Bind("JobTypeID") %>'
                            DataTextField="JobTypeName" DataValueField="JobTypeID">
                        </asp:DropDownList>
                    </InsertItemTemplate>
                    <HeaderStyle CssClass="dataentryformlabel"></HeaderStyle>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Min Salary :" DataField="MinSalary" SortExpression="MinSalary">
                    <HeaderStyle CssClass="dataentryformlabel"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField HeaderText="Max Salary :" DataField="MaxSalary" SortExpression="MaxSalary">
                    <HeaderStyle CssClass="dataentryformlabel"></HeaderStyle>
                </asp:BoundField>
                <asp:TemplateField SortExpression="Description" HeaderText="Description :"><EditItemTemplate>
                    <asp:TextBox ID="TextBox1" Runat="server" Text='<%# Bind("Description") %>' Width="98%"
                        TextMode="MultiLine" Rows="5"></asp:TextBox>
                </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label Runat="server" Text='<%# Bind("Description") %>' ID="Label5"></asp:Label>
                    </ItemTemplate>
                    <InsertItemTemplate>
                        <asp:TextBox Runat="server" Text='<%# Bind("Description") %>' ID="TextBox1" TextMode="MultiLine" Rows="5"></asp:TextBox>
                    </InsertItemTemplate>
                    <HeaderStyle VerticalAlign="Top" CssClass="dataentryformlabel"></HeaderStyle>
                </asp:TemplateField>
                <asp:CommandField ShowDeleteButton="True" ShowInsertButton="True" ShowEditButton="True"></asp:CommandField>
            </Fields>
            <FieldHeaderStyle HorizontalAlign="Right"></FieldHeaderStyle>
            <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
            <InsertRowStyle HorizontalAlign="Left"></InsertRowStyle>
            <EditRowStyle HorizontalAlign="Left"></EditRowStyle>
        </asp:DetailsView><br />
        <asp:HyperLink ID="HyperLink1" Runat="server" NavigateUrl="~/employer/jobpostings.aspx">Go to listing page</asp:HyperLink>
        <br />
        <br />
        <asp:ObjectDataSource ID="ObjectDataSource1" Runat="server" TypeName="JobSiteStarterKit.BOL.JobPosting"
            DeleteMethod="Delete" InsertMethod="Insert" SelectMethod="GetPosting" UpdateMethod="Update"
            DataObjectTypeName="JobSiteStarterKit.BOL.JobPosting">
            <DeleteParameters>
                <asp:Parameter Name="original_JobPostingID" Type="Int32" />
            </DeleteParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="id" QueryStringField="id" Type="Int32"></asp:QueryStringParameter>
            </SelectParameters>
        </asp:ObjectDataSource><br />
        <asp:ObjectDataSource ID="ObjectDataSource2" Runat="server" TypeName="JobSiteStarterKit.BOL.State"
            SelectMethod="GetStates">
            <SelectParameters>
                <asp:Parameter Type="Int32" Name="countryid"></asp:Parameter>
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ObjectDataSource3" Runat="server" TypeName="JobSiteStarterKit.BOL.Country"
            SelectMethod="SelectCountries">
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ObjectDataSource4" Runat="server" SelectMethod="GetEducationLevels" TypeName="JobSiteStarterKit.BOL.EducationLevel">
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ObjectDataSource5" Runat="server" SelectMethod="GetJobTypes" TypeName="JobSiteStarterKit.BOL.JobType">
        </asp:ObjectDataSource>
    
    </div>
</asp:Content>


