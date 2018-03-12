<%@ Page Language="C#" MasterPageFile="~/TimeTracker/MasterPage.master" CodeFile="Project_Details.aspx.cs"
    Inherits="Project_Details_aspx" Title="My Company - Time Tracker - Manage Projects" Culture="auto" UICulture="auto" %>
<asp:Content ID="Content1" ContentPlaceHolderID="maincontent" runat="Server">
    <div id="adminedit">
        <a name="content_start" id="content_start"></a>
        <fieldset>
            <!-- add H2 here and hide it with css since you can not put h2 inside a legend tag -->
          
           <h2 class="none">
                Project configuration</h2>
            
            <legend>Project configuration</legend>Define the project and specify which users
            will be part of the project. Then add categories to the project to help keep track
            of specific areas of product. Press the SAVE button at the bottom for your configuration
            to take effect.
            <table>
            <tr valign=top>
            <td width="30%">
            <div class="formsection">
                Project Information
            </div>
            <p>
                <asp:Label ID="Label1" runat="server" Text="Label" AssociatedControlID="ProjectName">Project Name</asp:Label>
                <br>
                <asp:TextBox ID="ProjectName" runat="server" Width="194px"
                    MaxLength="50"></asp:TextBox>
                <asp:RequiredFieldValidator ID="ProjectNameRequiredfieldvalidator" runat="server"
                    Display="Dynamic" ControlToValidate="ProjectName" ErrorMessage="Project Name is a required value.">
                </asp:RequiredFieldValidator>
                <br />
                <asp:Label ID="Label2" runat="server" Text="Label" AssociatedControlID="Managers">Project Manager</asp:Label>
                <br>
                <asp:ObjectDataSource ID="ManagerData" runat="server" TypeName="System.Web.Security.Roles"
                    SelectMethod="GetUsersInRole"></asp:ObjectDataSource>
                <asp:DropDownList ID="Managers" runat="server" Width="193px"
                    DataSourceID="ManagerData" />
                <asp:RequiredFieldValidator ID="ManagerRequiredFieldValidator" runat="server" Display="Dynamic"
                    ControlToValidate="Managers" ErrorMessage="You must select a manager.">
                </asp:RequiredFieldValidator>
                <br />
                <asp:Label ID="Label3" runat="server" Text="Label" AssociatedControlID="CompletionDate">Estimated complete date:</asp:Label>
                <br />
                <asp:TextBox ID="CompletionDate" runat="server" Width="76px" Columns="12"></asp:TextBox>
                <a href="javascript:OpenPopupPage('Calendar.aspx','<%= CompletionDate.ClientID %>','<%= Page.IsPostBack %>');">
                    <img src="images/icon-calendar.gif"  ></a>
                <asp:CompareValidator ID="CompletionDateCompareValidator" runat="server" Display="Dynamic"
                    ErrorMessage="Date format is incorrect." Operator="DataTypeCheck" Type="Date"
                    ControlToValidate="CompletionDate">
                </asp:CompareValidator>
                <asp:RequiredFieldValidator ID="CompletionDateRequiredFieldValidator" runat="server"
                    ControlToValidate="CompletionDate" ErrorMessage="Est. Comletion Date is required."
                    Display="Dynamic">
                </asp:RequiredFieldValidator>
                <br />
                <asp:Label ID="Label4" runat="server" Text="Label" AssociatedControlID="Duration">Estimated Duration (in hours):</asp:Label>
                <br />
                <asp:TextBox ID="Duration" runat="server" Width="49px" Columns="12"></asp:TextBox>
                <asp:CompareValidator ID="DurationCompareValidator" runat="server" ControlToValidate="Duration"
                    ErrorMessage="Duration must be integer value." Display="Dynamic" Operator="DataTypeCheck"
                    Type="Integer">
                </asp:CompareValidator>
                <asp:RequiredFieldValidator ID="DurationRequiredFieldValidator" runat="server" ControlToValidate="Duration"
                    ErrorMessage="Duration is required." Display="Dynamic">
                </asp:RequiredFieldValidator>
                <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="Duration is out of range"
                    ControlToValidate="Duration" MaximumValue="99999" MinimumValue="0" Type="Integer">
                </asp:RangeValidator>
                <br />
                <asp:Label ID="Label5" runat="server" Text="Label" AssociatedControlID="Description">Description:</asp:Label>
                <br />
                <asp:TextBox ID="Description" runat="server" TextMode="MultiLine"
                    Width="204px" Columns="20" Rows="8" MaxLength="200"></asp:TextBox>
                <br />
                <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="Description"
                    ErrorMessage="Description must be less than 200 characters" OnServerValidate="CustomValidator1_ServerValidate"></asp:CustomValidator>
            </p>
            </td>
            <td width="30%">
            <div class="formsection">
                Specify Project Members
            </div>
            <p>
                <asp:Label ID="Label6" runat="server" Text="Label" AssociatedControlID="Consultants">Select a resource. Use ctrl+click to select multiple resources at once:</asp:Label>
                <br>
                <asp:ObjectDataSource ID="ProjectConsultantData" runat="server" TypeName="System.Web.Security.Roles"
                    SelectMethod="GetUsersInRole">
                    
                </asp:ObjectDataSource>
                <asp:ListBox ID="Consultants" runat="server" SelectionMode="Multiple" Rows="9" DataSourceID="ProjectConsultantData" CssClass="resourcelist" />
            </p>
            </td>
              
            <td width="40%" runat=server id=ProjectCategoryColumn >           
            <div class="formsection">
                Define Project Categories for Project Management
            </div>
            <p>
                Categories can be added in two ways. You can <b>ADD</b> a category by specifying
                name, abbreviation (4 characters max), and duration - the amount of hours that may
                be charged under the category. Or, You can <b>COPY</b> categories that already have
                been defined in another project to this project.
            </p>
            <p>
               
                <asp:Label ID="Label7" runat="server" Text="Label" AssociatedControlID="CategoryName">Category name:</asp:Label>
                <br />
                <asp:TextBox ID="CategoryName" runat="server" Width="166px" EnableViewState="False"
                    MaxLength="50" ValidationGroup=CategoryValidation></asp:TextBox><asp:RequiredFieldValidator
                        ID="RequiredFieldValidator1" runat="server" ControlToValidate="CategoryName"
                        Display="Dynamic" ErrorMessage="Category Name is a required value." ValidationGroup="CategoryValidation"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="CategoryNameValidator" runat="server" ErrorMessage="Category name must be alphanumeric."
                    ControlToValidate="CategoryName" Display="Dynamic" ValidationExpression="[a-zA-Z0-9 ]*" ValidationGroup=CategoryValidation>
                </asp:RegularExpressionValidator>
                <br />
                <asp:Label ID="Label8" runat="server" Text="Label" AssociatedControlID="Abbrev">Category abbreviation:</asp:Label>
                <br />
                <asp:TextBox ID="Abbrev" runat="server" Width="70px" EnableViewState="False" ValidationGroup=CategoryValidation></asp:TextBox>
                <br />
                <asp:RequiredFieldValidator
                    ID="RequiredFieldValidator2" runat="server" ControlToValidate="Abbrev"
                    Display="Dynamic" ErrorMessage="Category abbreviation  is a required value." ValidationGroup="CategoryValidation"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Category abbreviation must be alphanumeric."
                    ControlToValidate="Abbrev" Display="Dynamic" ValidationExpression="[a-zA-Z0-9 ]*" ValidationGroup=CategoryValidation>
                </asp:RegularExpressionValidator>
                <br />
                <asp:Label ID="Label9" runat="server" Text="Label" AssociatedControlID="CatDuration">Duration:</asp:Label>
                <br />
                <asp:TextBox ID="CatDuration" runat="server" Width="70px"
                    EnableViewState="False" ValidationGroup=CategoryValidation></asp:TextBox>
                    <br />
                    <asp:RequiredFieldValidator
                        ID="RequiredFieldValidator3" runat="server" ControlToValidate="CatDuration"
                        Display="Dynamic" ErrorMessage="Duration  is a required value." ValidationGroup="CategoryValidation"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="CatDurationValidator" runat="server" ControlToValidate="CatDuration"
                    ErrorMessage="Duration for category must be integer value." Display="Dynamic"
                    Operator="DataTypeCheck" Type="Integer" ValidationGroup=CategoryValidation>
                </asp:CompareValidator>
                <asp:RangeValidator ID="RangeValidator2" runat="server" ErrorMessage="Duration is out of range."
                    ControlToValidate="CatDuration" MaximumValue="99999" MinimumValue="0" Type="Integer" ValidationGroup=CategoryValidation>
                </asp:RangeValidator>
                <br />
                <asp:Button ID="AddButton" runat="server" Width="46px" CssClass="submit" Text="Add"
                    OnClick="AddButton_Click" ValidationGroup=CategoryValidation></asp:Button><br />
                <asp:ObjectDataSource ID="CategoryData" runat="server" TypeName="ASPNET.StarterKit.BusinessLogicLayer.Category"
                    SelectMethod="GetCategoriesByProjectId" DeleteMethod="DeleteCategory" UpdateMethod="UpdateCategory">
                    <DeleteParameters>
                        <asp:Parameter Name="Id" Type="Int32" />
                    </DeleteParameters>
                    <SelectParameters>
                        <asp:QueryStringParameter Name="projectId" QueryStringField="ProjectId" Type="Int32" />
                    </SelectParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="Abbreviation" Type="String" />
                        <asp:Parameter Name="Id" Type="Int32" />
                        <asp:Parameter Name="EstimateDuration" Type="decimal" />
                        <asp:Parameter Name="Name" Type="string" />
                    </UpdateParameters>
                </asp:ObjectDataSource>
                <asp:GridView ID="ListAllCategories" DataSourceID="CategoryData" DataKeyNames="Id"
                    AutoGenerateColumns="False" AllowSorting="true" AllowPaging="true" runat="server"
                    BorderStyle="None" Width="90%" CellPadding="2" BorderWidth="0">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="Identification" ReadOnly="true" Visible=false/>
                        <asp:TemplateField HeaderText="Name">
                            <ItemTemplate>
                                <asp:Label ID="Label11" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="CategoryName2" runat="server" CssClass="catname" Text='<%# Bind("Name") %>'></asp:TextBox>
                                <asp:RegularExpressionValidator ID="CategoryNameValidator2" runat="server" ErrorMessage="Category name must be alphanumeric."
                                    ControlToValidate="CategoryName2" Display="Dynamic" ValidationExpression="[a-zA-Z0-9 ]*">
                                </asp:RegularExpressionValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Abbreviation">
                            <ItemTemplate>
                                <asp:Label ID="Label12" runat="server" Text='<%# Eval("Abbreviation") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="Abbrev2" runat="server" CssClass="catabbr" Text='<%# Bind("Abbreviation") %>'></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator12" runat="server"
                                    ErrorMessage="Category abbreviation must be alphanumeric." ControlToValidate="Abbrev2"
                                    Display="Dynamic" ValidationExpression="[a-zA-Z0-9 ]*">
                                </asp:RegularExpressionValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Estimate Duration">
                            <ItemTemplate>
                                <asp:Label ID="Label13" runat="server" Text='<%# Eval("EstimateDuration") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="CatDuration2" runat="server" CssClass="catduration" Text='<%# Bind("EstimateDuration") %>'></asp:TextBox>
                                <asp:CompareValidator ID="CatDurationValidator2" runat="server" ControlToValidate="CatDuration2"
                                    ErrorMessage="Duration for category must be integer value." Display="Dynamic"
                                    Operator="DataTypeCheck" Type="Integer">
                                </asp:CompareValidator>
                                <asp:RangeValidator ID="RangeValidator22" runat="server" ErrorMessage="Duration is out of range."
                                    ControlToValidate="CatDuration2" MaximumValue="99999" MinimumValue="0" Type="Integer">
                                </asp:RangeValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowEditButton="True" HeaderText="Edit" ButtonType="Image" EditImageUrl="images/icon-edit.gif"
                            UpdateImageUrl="images/icon-save.gif" CancelImageUrl="images/icon-cancel.gif" />
                        <asp:CommandField ShowDeleteButton="True" HeaderText="Delete" DeleteImageUrl="images/icon-delete.gif"
                            ButtonType="Image" />
                    </Columns>
                    <RowStyle BorderStyle="None" CssClass="row1" />
                   
                    <HeaderStyle CssClass="grid-header"></HeaderStyle>
                    <EmptyDataTemplate>
                        <asp:Label ID="Label10" runat="server" Text="Label">There are no categories associated to this project</asp:Label>
                    </EmptyDataTemplate>
                </asp:GridView>
            </p>
            <p>
                OR</p>
            <p>
                Add categories from another project:<br />
                <asp:ObjectDataSource ID="ProjectData" runat="server" TypeName="ASPNET.StarterKit.BusinessLogicLayer.Project" />
                <asp:DropDownList ID="ProjectList" runat="server" Width="190px"
                    DataSourceID="ProjectData" DataValueField="Id" DataTextField="Name" OnPreRender="ProjectList_PreRender" />
                <br />
                <asp:Button ID="CopyButton" runat="server" Width="46" CausesValidation="False" CssClass="standard-text"
                    Text="Copy" OnClick="CopyButton_Click" Enabled="false"></asp:Button>
            </p>
            <p>
                <asp:Label ID="ErrorMessage" runat="server" CssClass="standard-text" EnableViewState="False"
                    ForeColor="Red"></asp:Label>
            </p>
            </td>

            </tr>
            </table>
            <div class="formsection">
                <asp:Button ID="SaveButton2" runat="server" CssClass="submit" Text="Save" OnClick="SaveButton_Click"></asp:Button>
                &nbsp;
                <asp:Button ID="CancelButton2" runat="server" CausesValidation="False" CssClass="reset"
                    Text="Cancel" OnClick="CancelButton_Click"></asp:Button>
                &nbsp;
                <asp:Button ID="DeleteButton2" runat="server" Text="Delete" CssClass="delete" CausesValidation="False"
                    OnClick="DeleteButton_Click"></asp:Button>
            </div>
        </fieldset>
    </div>
</asp:Content>
