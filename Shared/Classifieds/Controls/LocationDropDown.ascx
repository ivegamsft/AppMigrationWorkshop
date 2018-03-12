<%@ Control Language="C#" CodeFile="LocationDropDown.ascx.cs" Inherits="LocationDropDown_ascx" %>
<asp:DropDownList ID="LocationList" Runat="server" DataSourceID="LocationsDataSource"
     DataTextField="Name" DataValueField="Name" OnDataBound="LocationList_DataBound" CssClass="fixed" 
     OnSelectedIndexChanged="LocationList_SelectedIndexChanged" AutoPostBack="True">
</asp:DropDownList>
<asp:PlaceHolder ID="OtherLocationPanel" Runat="server">
&nbsp;
<asp:Label ID="OtherLabel" Runat="server" Text="Other:"></asp:Label>
&nbsp;
<asp:TextBox ID="OtherLocationTextBox" Runat="server"></asp:TextBox>
</asp:PlaceHolder>
<asp:ObjectDataSource ID="LocationsDataSource" Runat="server" SelectMethod="GetAllLocations"
     TypeName="AspNet.StarterKits.Classifieds.Web.LocationCache" 
     OnSelected="LocationsDataSource_Selected">
</asp:ObjectDataSource>