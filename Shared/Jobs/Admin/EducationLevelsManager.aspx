<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" CodeFile="EducationLevelsManager.aspx.cs" Inherits="EducationLevelsManager_aspx" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="server">
<div align="center">
    <asp:Label ID="Label14" Runat="server" SkinID="FormHeading" Text="Education Levels Manager"></asp:Label>
    <br />
    <br />
    <asp:DetailsView ID="DetailsView1" Runat="server" DataSourceID="ObjectDataSource1"
        AllowPaging="True" AutoGenerateRows="False" DataKeyNames="EducationLevelID" GridLines="None" CellPadding="5" SkinID="AdminEntry">
        <PagerSettings Mode="NumericFirstLast"></PagerSettings>
        <CommandRowStyle HorizontalAlign="Left"></CommandRowStyle>
        <Fields>
            <asp:CommandField ButtonType="Button" ShowDeleteButton="True" ShowInsertButton="True"
                ShowEditButton="True"></asp:CommandField>
            <asp:BoundField HeaderText="Education Level ID :" DataField="educationlevelid" ReadOnly="True" InsertVisible="False">
                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                <HeaderStyle CssClass="dataentryformlabel"></HeaderStyle>
            </asp:BoundField>
            <asp:BoundField HeaderText="Education Level Name :" DataField="educationlevelname">
                <HeaderStyle CssClass="dataentryformlabel"></HeaderStyle>
            </asp:BoundField>
        </Fields>
        <FieldHeaderStyle HorizontalAlign="Right"></FieldHeaderStyle>
        <HeaderStyle HorizontalAlign="Right"></HeaderStyle>
        <EditRowStyle HorizontalAlign="Left"></EditRowStyle>
    </asp:DetailsView><br />
    <asp:ObjectDataSource ID="ObjectDataSource1" Runat="server" TypeName="JobSiteStarterKit.BOL.EducationLevel" DataObjectTypeName="JobSiteStarterKit.BOL.EducationLevel"
        DeleteMethod="Delete" InsertMethod="Insert" SelectMethod="GetEducationLevels"
        UpdateMethod="Update">
    </asp:ObjectDataSource>
</div>
</asp:Content>
