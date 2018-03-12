<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeFile="Categories.aspx.cs"
    Inherits="Categories_aspx" Title="Manage Categories" %>
<%@ Register TagPrefix="uc2" TagName="CategoryDropDown" Src="../Controls/CategoryDropDown.ascx" %>
<%@ Register TagPrefix="uc1" TagName="CategoryPath" Src="../Controls/CategoryPath.ascx" %>
<asp:Content ID="MainContent" ContentPlaceHolderID="Main" Runat="server">
    <div id="body">
        <div id="col_main_left">
            <div id="user_assistance">
                <a id="content_start"></a>
                <h3>
                    Actions</h3>
                <p>
                    <asp:HyperLink ID="BackToAdminLink" Runat="server" NavigateUrl="~/Admin/Default.aspx">back to Administration</asp:HyperLink></p>
                <p>
                    <asp:Label ID="RemoveSuccessLabel" Runat="server" Text="The Category was successfully removed."
                        Visible="False" EnableViewState="False"></asp:Label><br />
                    <asp:Label ID="RemoveFailedLabel" Runat="server" Text='The Category could not be removed. Please make sure that there no Ads and no Sub-Categories within it. You can use the "Move" Feature below to clear the category first.'
                        Visible="False" EnableViewState="False" Font-Bold="True"></asp:Label></p>
            </div>
        </div>
        <div id="col_main_right">
            <h2 class="section">
                Categories</h2>
            <div class="content_right">
                <fieldset>
                    <p>
                        <uc1:CategoryPath ID="CategoryPath" Runat="server" OnCategorySelectionChanged="CategoryPath_CategorySelectionChanged"/>
                    </p>
                    <p>
                        Current Selection:<br />
                        <asp:Label ID="CurrentCategoryLabel" Runat="server" Font-Size="Large"></asp:Label>
                    </p>
                    <legend>Sub-Categories:</legend>
                    <asp:ListBox ID="SubcategoriesList" Runat="server" DataSourceID="SubcategoryDataSource"
                        DataValueField="IdString" DataTextField="Name" AutoPostBack="True" OnSelectedIndexChanged="SubcategoriesList_SelectedIndexChanged"
                        OnDataBound="SubcategoriesList_DataBound" Width="350px" Rows="5">
                    </asp:ListBox>
                    <h3 class="section">
                        Actions for 
                        <asp:Label ID="CurrentCategoryActionLabel" Runat="server" Font-Underline="true"></asp:Label>:</h3>
                    <p>
                    </p>
                    <legend>Add Sub-Category</legend><span>
                    <asp:TextBox ID="NewCategoryTextBox" Runat="server" ValidationGroup="NewCategory"
                        CssClass="post_title"></asp:TextBox></span>
                    <asp:Button ID="AddCategoryButton" Runat="server" Text="Add Sub-Category" ValidationGroup="NewCategory"
                        OnClick="AddCategoryButton_Click" />
                    <p>
                        <asp:RequiredFieldValidator ID="RequiredCategoryValidator" Runat="server" ValidationGroup="NewCategory"
                            ControlToValidate="NewCategoryTextBox" ErrorMessage="*">
                        </asp:RequiredFieldValidator>
                    </p>
                    <legend>Rename</legend><span>
                    <asp:TextBox ID="RenameCategoryTextBox" Runat="server" ValidationGroup="CategoryRename"
                        CssClass="post_title"></asp:TextBox></span>
                    <asp:Button ID="RenameCategoryButton" Runat="server" Text="Rename Category" OnClick="RenameCategoryButton_Click"
                        ValidationGroup="CategoryRename" />
                    <p>
                        <asp:RequiredFieldValidator ID="RequiredCategoryNameValidator" Runat="server" ValidationGroup="CategoryRename"
                            ErrorMessage="*" ControlToValidate="RenameCategoryTextBox">
                        </asp:RequiredFieldValidator></p>
                    <p>
                    </p>
                    <legend>Move</legend>
                    <asp:RadioButtonList ID="MoveAction" Runat="server" ValidationGroup="MoveCategory">
                        <asp:ListItem Value="Category" Selected="True">Move Category (incl. any sub-categories)</asp:ListItem>
                        <asp:ListItem Value="Ads">Move all Ads in this Category</asp:ListItem>
                    </asp:RadioButtonList>---------- moving to: &nbsp;<uc2:CategoryDropDown ID="CategoryDropDown"
                        Runat="server" AllCategoriesOptionText="[Top-Level]"></uc2:CategoryDropDown>
                    <asp:Button ID="MoveButton" Runat="server" Text="   Move   " OnClick="MoveButton_Click"
                        ValidationGroup="MoveCategory" />
                    <p>&nbsp;
                    </p>
                    <legend>Delete</legend>
                    <div>
                        To delete the Category, it be must be empty - with no sub-categories
                        or ads within it.</div>
                        <p><asp:Button ID="RemoveCategoryButton" Runat="server" Text="Remove" OnClick="RemoveCategoryButton_Click" /></p>
                    <asp:ObjectDataSource ID="SubcategoryDataSource" Runat="server" TypeName="AspNet.StarterKits.Classifieds.Web.CategoryCache"
                        SelectMethod="GetCategoriesByParentId">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="CategoryPath" PropertyName="CurrentCategoryId"
                                Type="Int32" DefaultValue="0" Name="parentCategoryId" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </fieldset>
            </div>
        </div>
    </div>
</asp:Content>