<%@ Control Language="C#" CodeFile="CategoryDropDown.ascx.cs" Inherits="CategoryDropDown_ascx" %>
<asp:DropDownList ID="CategoryList" Runat="server" CssClass="fixed-main" DataSourceID="CategoryDataSource" DataTextField="LevelIndentedName" DataValueField="IdString" OnSelectedIndexChanged="CategoryList_SelectedIndexChanged" OnDataBound="CategoryList_DataBound" OnDataBinding="CategoryList_DataBinding">
</asp:DropDownList>
<asp:ObjectDataSource ID="CategoryDataSource" TypeName="AspNet.StarterKits.Classifieds.Web.CategoryCache"
    SelectMethod="GetAllCategories" Runat="server">
</asp:ObjectDataSource>
